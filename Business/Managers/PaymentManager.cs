using AutoMapper;
using AutoMapper.QueryableExtensions;
using Business.Services;
using DataAccess.Repositories.Abstractions;
using Domain.Constants;
using Domain.DataTransferObjects.Payments;
using Domain.Entities;
using Domain.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Business.Managers
{
    public class PaymentManager : IPaymentService
    {
        private readonly IRepositoryService _repository;
        private readonly IExpenseService _expenseService;
        private readonly IMapper _mapper;

        public PaymentManager(IMapper mapper, IRepositoryService repository, IExpenseService expenseService)
        {
            _mapper = mapper;
            _repository = repository;
            _expenseService = expenseService;
        }

        public async Task<PaymentDto> CreateAsync(int expenseId, PaymentCreationDto paymentCreationDto)
        {
            await CheckPaymentByExpenseIdAsync(expenseId);

            var expense = await _expenseService.CheckAndGetExpenseByIdAsync(expenseId);

            if ( paymentCreationDto.IsApproved )
            {
                await MakeMoneyTransfer(expense);
            }

            var paymentEntity = _mapper.Map<Payment>(paymentCreationDto);
            paymentEntity.Expense = expense;
            await _repository.Payment.CreateAsync(paymentEntity);
            await _repository.SaveAsync();
            return _mapper.Map<PaymentDto>(paymentEntity);
        }

        public async Task<PaymentDto> GetByIdAsync(int id)
        {
            var payment = await CheckPaymentByIdAsync(id);
            return _mapper.Map<PaymentDto>(payment);
        }

        public async Task<PaymentDto> GetByExpenseIdAsync(int expenseId)
        {
            var payment = await _expenseService.CheckAndGetExpenseByIdAsync(expenseId);
            return _mapper.Map<PaymentDto>(payment);
        }

        public async Task<List<PaymentDto>> GetAllAsync()
        {
            return await _repository.Context.Payments.ProjectTo<PaymentDto>(_mapper.ConfigurationProvider).ToListAsync();
        }

        public async Task<PaymentDto> GetForEmployeeByIdAsync(int userId, int id)
        {
            await CheckPaymentByIdAsync(id);
            var payment = await _repository.Context.Payments.Include(x => x.Expense)
                .Where(x => x.Expense.UserId == userId && x.Id == id)
                                                                        .FirstOrDefaultAsync();

            return _mapper.Map<PaymentDto>(payment);
        }

        public async Task<List<PaymentDto>> GetAllForEmployeeAsync(int userId)
        {
            var payments = await _repository.Context.Payments.Include(x => x.Expense)
                                                                    .Where(x => x.Expense.UserId == userId)
                                                                    .ToListAsync();

            return _mapper.Map<List<PaymentDto>>(payments);
        }

        public async Task<Payment> CheckPaymentByIdAsync(int id)
        {
            var payment = await _repository.Context.Payments.Include(x => x.Id == id).FirstOrDefaultAsync();

            if ( payment == null )
                throw new NotFoundException(Messages.PaymentNotFound);

            return payment;
        }

        public async Task CheckPaymentByExpenseIdAsync(int expenseId)
        {
            var payment = await _repository.Payment.AnyAsync(x => x.ExpenseId == expenseId);
            if ( payment )
                throw new BadRequestException(Messages.ExpenseProcessTransactionError);
        }

        private async Task MakeMoneyTransfer(Expense expense)
        {
            var paymentActivity = new PaymentActivity()
            {
                Amount = expense.Amount,
                ExpenseId = expense.Id,
                UserId = expense.UserId,
                CurrencyType = expense.CurrencyType,
                PaidDate = DateTime.UtcNow
            };

            await _repository.Context.PaymentActivities.AddAsync(paymentActivity);
        }
    }
}
