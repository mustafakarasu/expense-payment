using Business.Services;
using Domain.DataTransferObjects.Employees;
using Domain.DataTransferObjects.Tokens;
using Domain.DataTransferObjects.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/authentication")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Authenticate([FromBody] UserAuthenticationDto user)
        {
            if ( !await _authenticationService.ValidateUser(user) )
                return Unauthorized();

            return Ok(new
            {
                Token = await _authenticationService.CreateToken(true)
            });
        }

        [HttpPost("token/refresh")]
        public async Task<IActionResult> Refresh([FromBody] TokenDto tokenDto)
        {
            var tokenDtoToReturn = await _authenticationService.RefreshToken(tokenDto);
            return Ok(tokenDtoToReturn);
        }


        [HttpPost("employee")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateEmployee([FromBody] EmployeeCreationDto employeeCreationDto)
        {
            var employeeDto = await _authenticationService.CreateEmployeeAsync(employeeCreationDto);
            return Ok(employeeDto);
        }
    }
}
