using AutoMapper;
using AutoMapper.QueryableExtensions;
using Business.QueryParameters;
using Business.Services;
using Business.Utilities;
using DataAccess.Repositories.Abstractions;
using Domain.Constants;
using Domain.DataTransferObjects.Documents;
using Domain.DataTransferObjects.Expenses;
using Domain.Entities;
using Domain.Exceptions;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Linq.Expressions;
using System.Net.Http.Headers;
using LinqKit;

namespace Business.Managers
{
    public class ExpenseManager : IExpenseService
    {
        private readonly IRepositoryService _repository;
        private readonly ICategoryService _categoryService;
        private readonly IPaymentMethodService _paymentMethodService;
        private readonly IMapper _mapper;
        private readonly IValidator<DocumentCreationDto> _documentValidator;
        private readonly IAuthenticationService _authenticationService;
        public ExpenseManager(IRepositoryService repository,
            IMapper mapper,
            ICategoryService categoryService,
            IPaymentMethodService paymentMethodService,
            IValidator<DocumentCreationDto> documentValidator, IAuthenticationService authenticationService)
        {
            _repository = repository;
            _mapper = mapper;
            _categoryService = categoryService;
            _paymentMethodService = paymentMethodService;
            _documentValidator = documentValidator;
            _authenticationService = authenticationService;
        }

        public async Task<ExpenseDto> CreateAsync(int userId, ExpenseCreationDto expenseCreationDto)
        {
            CheckDocumentLength(expenseCreationDto.Documents);

            var category = await _categoryService.CheckAndGetCategoryByIdAsync(expenseCreationDto.CategoryId);
            var paymentMethod = await _paymentMethodService.CheckPaymentMethodAndGetByIdAsync(expenseCreationDto.PaymentMethodId);
            var employeeUser =
                await _authenticationService.GetUserByIdForAdminAsync(userId.ToString());

            var expenseEntity = _mapper.Map<Expense>(expenseCreationDto);

            if ( expenseCreationDto.Documents != null && expenseCreationDto.Documents.Count > 0 )
            {
                foreach ( var document in expenseCreationDto.Documents )
                {
                    var documentEntity = SaveDocumentAndReturnEntity(document);
                    expenseEntity.Documents.Add(documentEntity);
                }
            }

            expenseEntity.Category = category;
            expenseEntity.PaymentMethod = paymentMethod;
            expenseEntity.User = employeeUser;

            expenseEntity.CurrencyType = "TRY";
            await _repository.Expense.CreateAsync(expenseEntity);
            await _repository.SaveAsync();

            return _mapper.Map<ExpenseDto>(expenseEntity);
        }

        private Document SaveDocumentAndReturnEntity(IFormFile document)
        {
            var fileName = ContentDispositionHeaderValue.Parse(document.ContentDisposition).FileName?.Trim('"');
            FileHelper.SaveToPath(document, fileName);
            var documentEntity = CreateDocument(FileHelper.FolderNamePath, fileName);
            return documentEntity;
        }

        public async Task UpdateAsync(int id, ExpenseUpdateDto expenseUpdateDto)
        {
            await CheckPaymentByExpenseIdAsync(id);
            var category = await _categoryService.CheckAndGetCategoryByIdAsync(expenseUpdateDto.CategoryId);
            var paymentMethod = await _paymentMethodService.CheckPaymentMethodAndGetByIdAsync(expenseUpdateDto.PaymentMethodId);

            var expense = await CheckAndGetExpenseByIdAsync(id);
            var documents = await _repository.Context.Documents.Where(x => x.ExpenseId == id).ToListAsync();
            _repository.Context.Documents.RemoveRange(documents);

            if ( expenseUpdateDto.Documents != null && expenseUpdateDto.Documents.Count > 0 )
            {
                foreach ( var document in expenseUpdateDto.Documents )
                {
                    var documentEntity = SaveDocumentAndReturnEntity(document);
                    expense.Documents.Add(documentEntity);
                }
            }
            expense.Category = category;
            expense.PaymentMethod = paymentMethod;

            _mapper.Map(expenseUpdateDto, expense);
            await _repository.SaveAsync();
        }

        public async Task DeleteAsync(int id)
        {
            await CheckPaymentByExpenseIdAsync(id);
            var expense = await CheckAndGetExpenseByIdAsync(id);
            _repository.Expense.Delete(expense);
            await _repository.SaveAsync();
        }

        public async Task<ExpenseDto> GetByIdAsync(int id)
        {
            Expression<Func<Expense, bool>> filter = x => x.Id == id;
            return await GetAllQueryable(filter).FirstOrDefaultAsync();
        }

        public async Task<List<ExpenseDto>> GetAllAsync(ExpenseQueryParameter parameter)
        {
            return await QueryableBuilder(parameter).ToListAsync();
        }

        public async Task<List<ExpenseDto>> GetAllByEmployeeIdAsync(int employeeId, ExpenseQueryParameter queryParameter)
        {
            //Expression<Func<Expense, bool>> filter = x => x.UserId == employeeId;
            return await QueryableBuilder(queryParameter, employeeId).ToListAsync();
        }

        public async Task<ExpenseDto> GetByEmployeeIdAsync(int employeeId, int expenseId)
        {
            Expression<Func<Expense, bool>> filter = x => x.UserId == employeeId && x.Id == expenseId;
            return await GetAllQueryable(filter).FirstOrDefaultAsync();
        }

        public async Task<Expense> CheckAndGetExpenseByIdAsync(int id)
        {
            var result = await _repository.Expense.AnyAsync(x => x.Id == id);

            if ( !result )
                throw new NotFoundException(Messages.ExpenseNotFound);

            return await _repository.Expense.GetAsync(x => x.Id == id);
        }

        private void CheckDocumentLength(List<IFormFile> documents)
        {
            if ( documents == null )
                return;

            if ( documents.Any(x => x.Length  == 0) )
            {
                throw new BadRequestException(Messages.IncorrectFiles);
            }
        }

        private Document CreateDocument(string folderName, string fileName)
        {
            var dbPath = Path.Combine(folderName, fileName);
            var documentCreationDto = new DocumentCreationDto()
            {
                Name = fileName,
                FolderPath = dbPath
            };

            ValidationHelper.Validate(_documentValidator, documentCreationDto);
            return _mapper.Map<Document>(documentCreationDto);
        }

        private IQueryable<ExpenseDto> QueryableBuilder(ExpenseQueryParameter parameter, int? userId = null)
        {
            var predicate = PredicateBuilder.New<Expense>();
            predicate = userId.HasValue ? predicate.Start(x => x.UserId == userId.Value) : predicate.Start(x => true);
           
            //Expression<Func<Expense, bool>> filter = null;

            //if ( !string.IsNullOrWhiteSpace(parameter.Status) )
            //{
            //    filter = parameter.Status?.ToLower(CultureInfo.InvariantCulture) switch
            //    {
            //        ApprovalStatus.Pending => x => !GetActiveExpenses().Contains(x.Id),
            //        ApprovalStatus.Completed => x => GetActiveExpenses().Contains(x.Id),
            //        ApprovalStatus.Approved => x => GetApprovedExpenses().Contains(x.Id),
            //        ApprovalStatus.Rejected => x => GetRejectedExpenses().Contains(x.Id),
            //        _ => x => true
            //    };
            //}
            //else
            //{
            //    filter = x => true;
            //}

            if(!string.IsNullOrWhiteSpace(parameter.Status))
            {
                predicate = parameter.Status?.ToLower(CultureInfo.InvariantCulture) switch
                {
                    ApprovalStatus.Pending => predicate.And(x => !GetActiveExpenses().Contains(x.Id)),
                    ApprovalStatus.Completed => predicate.And(x => GetActiveExpenses().Contains(x.Id)),
                    ApprovalStatus.Approved => predicate.And(x => GetApprovedExpenses().Contains(x.Id)),
                    ApprovalStatus.Rejected => predicate.And(x => GetRejectedExpenses().Contains(x.Id)),
                    _ => predicate
                };
            }

            return GetAllQueryable(predicate);
        }

        private IQueryable<ExpenseDto> GetAllQueryable(Expression<Func<Expense, bool>> filter)
        {
            var expenses = _repository.Expense.Queryable()
                .Include(x => x.Category)
                .Include(x => x.PaymentMethod)
                .Include(x => x.Documents)
                .Include(x => x.Payment)
                .AsNoTracking()
                .Where(filter)
                .ProjectTo<ExpenseDto>(_mapper.ConfigurationProvider);

            return expenses;

        }

        private IQueryable<int> GetActiveExpenses()
        {
            return _repository.Context.Payments.Select(x => x.ExpenseId);
        }

        private IQueryable<int> GetApprovedExpenses()
        {
            return _repository.Context.Payments.Where(x => x.IsApproved).Select(x => x.ExpenseId);
        }

        private IQueryable<int> GetRejectedExpenses()
        {
            return _repository.Context.Payments.Where(x => x.IsApproved == false).Select(x => x.ExpenseId);
        }

        private async Task CheckPaymentByExpenseIdAsync(int expenseId)
        {
            var payment = await _repository.Payment.AnyAsync(x => x.ExpenseId == expenseId);
            if ( payment )
                throw new BadRequestException(Messages.ExpenseProcessTransactionError);
        }
    }
}
