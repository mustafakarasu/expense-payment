using AutoMapper;
using Domain.DataTransferObjects.Categories;
using Domain.DataTransferObjects.Documents;
using Domain.DataTransferObjects.Employees;
using Domain.DataTransferObjects.Payments;
using Domain.DataTransferObjects.Expenses;
using Domain.DataTransferObjects.PaymentMethods;
using Domain.DataTransferObjects.Users;
using Domain.Entities;

namespace Business.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UserRegistrationDto, User>();
            CreateMap<User, UserDto>();

            CreateMap<EmployeeCreationDto, UserRegistrationDto>();
            CreateMap<EmployeeCreationDto, EmployeeDto>();

            CreateMap<UserDto, EmployeeDto>();

            CreateMap<CategoryCreationDto, Category>();
            CreateMap<CategoryUpdateDto, Category>();
            CreateMap<Category, CategoryDto>();

            CreateMap<PaymentMethodCreationDto, PaymentMethod>();
            CreateMap<PaymentMethodUpdateDto, PaymentMethod>();
            CreateMap<PaymentMethod, PaymentMethodDto>();

            CreateMap<DocumentCreationDto, Document>();

            CreateMap<ExpenseCreationDto, Expense>()
                .ForMember(source => source.Documents,
                    options => options.Ignore());

            CreateMap<ExpenseUpdateDto, Expense>()
                .ForMember(source => source.Documents,
                    options => options.Ignore());

            CreateMap<Document, DocumentDto>();
            CreateMap<Expense, ExpenseDto>();

            CreateMap<PaymentCreationDto, Payment>();
            CreateMap<PaymentUpdateDto, Payment>();
            CreateMap<Payment, PaymentDto>();

            CreateProjection<User, EmployeeDto>();
        }
    }
}
