using Domain.DataTransferObjects.Employees;
using Domain.DataTransferObjects.Tokens;
using Domain.DataTransferObjects.Users;
using Domain.Entities;

namespace Business.Services
{
    public interface IAuthenticationService
    {
        Task<UserDto> RegisterUser(UserRegistrationDto userRegistration);
        Task<bool> ValidateUser(UserAuthenticationDto userAuthentication);
        Task<TokenDto> CreateToken(bool extendExpire);
        Task<TokenDto> RefreshToken(TokenDto tokenDto);
        Task<User> GetUserByIdForAdminAsync(string id);
        Task<EmployeeDto> CreateEmployeeAsync(EmployeeCreationDto employeeCreation);
    }
}
