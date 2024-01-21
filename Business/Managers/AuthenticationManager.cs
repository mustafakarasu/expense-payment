using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using Business.Services;
using Domain.ConfigurationModels;
using Domain.Constants;
using Domain.DataTransferObjects.Employees;
using Domain.DataTransferObjects.Tokens;
using Domain.DataTransferObjects.Users;
using Domain.Entities;
using Domain.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Business.Managers
{
    public class AuthenticationManager : IAuthenticationService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly IMapper _mapper;
        private readonly JwtConfiguration _jwtConfiguration;
        private readonly IOptions<JwtConfiguration> _configuration;
        private User _user;

        public AuthenticationManager(UserManager<User> userManager, IMapper mapper, IOptions<JwtConfiguration> configuration, RoleManager<Role> roleManager)
        {
            _userManager = userManager;
            _mapper = mapper;
            _configuration = configuration;
            _roleManager = roleManager;
            _jwtConfiguration = _configuration.Value;
        }

        public async Task<UserDto> RegisterUser(UserRegistrationDto userRegistration)
        {
            var validEmail = await _userManager.FindByEmailAsync(userRegistration.Email);
            if ( validEmail != null )
            {
                throw new BadRequestException(Messages.AlreadyEmail);
            }

            var user = _mapper.Map<User>(userRegistration);
            user.UserName = userRegistration.Email;

            var result = await _userManager.CreateAsync(user, userRegistration.Password);
            if ( !result.Succeeded )
            {
                var message = string.Join('\n', result.Errors.Select(x => x.Description));
                throw new BadRequestException(message);
            }

            var roleResult = await _roleManager.RoleExistsAsync(userRegistration.Role);
            if ( !roleResult )
            {
                throw new BadRequestException(Messages.NoRoleDefinition);
            }

            await _userManager.AddToRoleAsync(user, userRegistration.Role);

            return _mapper.Map<UserDto>(user);
        }

        public async Task<bool> ValidateUser(UserAuthenticationDto userAuthentication)
        {
            if ( userAuthentication == null )
            {
                throw new ArgumentNullException(nameof(userAuthentication));
            }

            _user = await _userManager.FindByEmailAsync(userAuthentication.Email);

            var checkPassword = await _userManager.CheckPasswordAsync(_user, userAuthentication.Password);

            if ( _user == null || checkPassword == false )
            {
                throw new BadRequestException(Messages.AuthenticationFailed);
            }

            var result = ( _user != null && await _userManager.CheckPasswordAsync(_user, userAuthentication.Password) );

            //if ( !result )
            //    _logger.LogWarn($"{nameof(ValidateUser)}: Authentication failed. Wrong user name or password.");

            return result;
        }

        public async Task<TokenDto> CreateToken(bool populateExp)
        {
            var signingCredentials = GetSigningCredentials();
            var claims = await GetClaims();
            var tokenOptions = GenerateTokenOptions(signingCredentials, claims);

            var refreshToken = GenerateRefreshToken();

            _user.RefreshToken = refreshToken;

            if ( populateExp )
                _user.RefreshTokenExpiryTime = DateTime.Now.AddDays(14);

            await _userManager.UpdateAsync(_user);

            var accessToken = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

            return new TokenDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }

        public async Task<TokenDto> RefreshToken(TokenDto tokenDto)
        {
            var principal = GetPrincipalFromExpiredToken(tokenDto.AccessToken);

            var user = await _userManager.FindByIdAsync(principal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value);
            if ( user == null || user.RefreshToken != tokenDto.RefreshToken ||
                 user.RefreshTokenExpiryTime <= DateTime.Now )
                throw new BadRequestException(Messages.RefreshTokenBadRequest);

            _user = user;

            return await CreateToken(populateExp: false);
        }

        public async Task<User> GetUserByIdForAdminAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if ( user == null )
            {
                throw new NotFoundException($"The user with id: {user.Id} doesn't exist in the database.");
            }
            return user;
        }

        public async Task<EmployeeDto> CreateEmployeeAsync(EmployeeCreationDto employeeCreation)
        {
            var userRegistration = _mapper.Map<UserRegistrationDto>(employeeCreation);
            userRegistration.Role = RoleSettings.Employee;

            var userDto = await RegisterUser(userRegistration);

            return _mapper.Map<EmployeeDto>(userDto);
        }

        private SigningCredentials GetSigningCredentials()
        {
            var key = Encoding.UTF8.GetBytes(_jwtConfiguration.SecretKey);
            var secret = new SymmetricSecurityKey(key);

            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }

        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfiguration.SecretKey)),
                ValidateLifetime = true,
                ValidIssuer = _jwtConfiguration.ValidIssuer,
                ValidAudience = _jwtConfiguration.ValidAudience
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);

            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if ( jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                    StringComparison.InvariantCultureIgnoreCase) )
            {
                throw new SecurityTokenException(Messages.InvalidToken);
            }

            return principal;
        }

        private async Task<List<Claim>> GetClaims()
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, _user.Email),
                new Claim(ClaimTypes.NameIdentifier, _user.Id.ToString())
            };

            var roles = await _userManager.GetRolesAsync(_user);
            foreach ( var role in roles )
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            return claims;
        }

        private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
        {
            var tokenOptions = new JwtSecurityToken
            (
                issuer: _jwtConfiguration.ValidIssuer,
                audience: _jwtConfiguration.ValidAudience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(_jwtConfiguration.Expires)),
                signingCredentials: signingCredentials
            );

            return tokenOptions;
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using ( var rng = RandomNumberGenerator.Create() )
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }
    }
}
