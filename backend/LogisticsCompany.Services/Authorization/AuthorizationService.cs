using Dapper;
using System.Text;
using LogisticsCompany.Data;
using LogisticsCompany.Data.Helpers;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using LogisticsCompany.Services.Roles.Queries;
using LogisticsCompany.Services.Users.Queries;
using LogisticsCompany.Services.Authorization.Dto;

namespace LogisticsCompany.Services.Authorization
{
    public class AuthorizationService : BaseService, IAuthorizationService
    {
        private readonly IUserQueryService _userQueryService;
        private readonly IRoleQueryService _roleQueryService;
        public AuthorizationService(
            LogisticsCompanyContext dbContext, 
            IUserQueryService userQueryService,
            IRoleQueryService roleQueryService)
            : base(dbContext)
        {
            _userQueryService = userQueryService;
            _roleQueryService = roleQueryService;
        }

        public async Task<string> Login(LoginDto dto, string issuer, string key)
        {
            var user = await _userQueryService.GetUserByEmailAndPassword(dto.Email, dto.PasswordHash);

            if (user != null && PasswordHasher.VerifyPassword(dto.PasswordHash, user.PasswordHash))
            {
                var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

                var role = await _roleQueryService.GetRoleNameById(user.RoleId) ?? string.Empty;

                var claims = new Claim[]
                {
                    new Claim("Id", user.Id.ToString()),
                    new Claim("Email", user.Email),
                    new Claim("Role", role),
                };

                var Sectoken = new JwtSecurityToken(issuer,
                 issuer,
                 claims,
                 expires: DateTime.Now.AddMinutes(120),
                 signingCredentials: credentials);

                var token = new JwtSecurityTokenHandler().WriteToken(Sectoken);

                return token;
            }

            return string.Empty;
        }

        public async Task Register(RegisterDto dto)
        {
            dto.Password = PasswordHasher.HashPassword(dto.Password);
            var roleId = await _roleQueryService.GetIdByName(dto.Role);

            if (roleId != 0)
            {
                var registerEmail = await _userQueryService.GetRegisterEmail(dto.Email);

                if (string.IsNullOrEmpty(registerEmail))
                {
                    using (var sqlConnection = new SqlConnection(_connectionString))
                    {
                        var insertCommand = SqlCommandHelper.InsertCommand("Users", $"'{dto.Username}'", $"'{dto.FirstName}'", $"'{dto.LastName}'", $"'{dto.Email}'", $"{roleId}", "NULL", $"'{dto.Password}'");
                        await sqlConnection.ExecuteAsync(insertCommand);
                    }
                }

                // TO DO: Add descriptive exception.
                return;
            }
        }
    }
}
