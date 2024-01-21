using AutoMapper;
using AutoMapper.QueryableExtensions;
using Business.Services;
using DataAccess.Repositories.Abstractions;
using Domain.Constants;
using Domain.DataTransferObjects.PaymentMethods;
using Domain.Entities;
using Domain.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Business.Managers
{
    public class PaymentMethodManager : IPaymentMethodService
    {
        private readonly IRepositoryService _repository;
        private readonly IMapper _mapper;

        public PaymentMethodManager(IRepositoryService repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<PaymentMethodDto> CreateAsync(PaymentMethodCreationDto paymentMethodCreationDto)
        {
            var entityPaymentMethod = _mapper.Map<PaymentMethod>(paymentMethodCreationDto);
            await _repository.PaymentMethod.CreateAsync(entityPaymentMethod);
            await _repository.SaveAsync();
            return _mapper.Map<PaymentMethodDto>(entityPaymentMethod);
        }

        public async Task UpdateAsync(int id, PaymentMethodUpdateDto paymentMethodUpdateDto)
        {
            var paymentMethodEntity = await CheckPaymentMethodAndGetByIdAsync(id);

            _mapper.Map(paymentMethodUpdateDto, paymentMethodEntity);

            await _repository.SaveAsync();
        }

        public async Task DeleteAsync(int id)
        {
            await CheckPaymentMethodRelationships(id);
            var paymentMethodEntity = await CheckPaymentMethodAndGetByIdAsync(id);
            _repository.PaymentMethod.Delete(paymentMethodEntity);
            await _repository.SaveAsync();
        }

        public async Task<PaymentMethodDto> GetByIdAsync(int id)
        {
            await CheckPaymentMethodAndGetByIdAsync(id);
            return await _repository.Context.PaymentMethods.Where(x => x.Id == id)
                                                                            .ProjectTo<PaymentMethodDto>(_mapper.ConfigurationProvider)
                                                                            .FirstOrDefaultAsync();
        }

        public async Task<List<PaymentMethodDto>> GetAllAsync()
        {
            return await _repository.Context.PaymentMethods.ProjectTo<PaymentMethodDto>(_mapper.ConfigurationProvider).ToListAsync();
        }

        public async Task<PaymentMethod> CheckPaymentMethodAndGetByIdAsync(int id)
        {
            var result = await _repository.PaymentMethod.AnyAsync(x => x.Id == id);

            if ( !result )
                throw new NotFoundException(Messages.PaymentMethodNotFound);

            return await _repository.PaymentMethod.GetAsync(x => x.Id == id);
        }

        private async Task CheckPaymentMethodRelationships(int id)
        {
            var result = await _repository.Expense.AnyAsync(x => x.PaymentMethodId == id);
            if ( result )
                throw new BadRequestException(Messages.PaymentMethodDeletionError);
        }
    }
}