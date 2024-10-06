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
using LogisticsCompany.Data.Contracts;

namespace LogisticsCompany.Services.Authorization
{
    /// <summary>
    /// A <see cref="BaseService"/> used for performing Register and Login operations to the Database.
    /// </summary>
    public class AuthorizationService : BaseService, IAuthorizationService
    {
        private readonly IUserQueryService _userQueryService;
        private readonly IRoleQueryService _roleQueryService;

        /// <summary>
        /// Creates a <see cref="AuthorizationService"/> with the injected
        /// <paramref name="dbContext"/>, <paramref name="dbAdapter"/> ,<paramref name="roleQueryService"/>, and <paramref name="userQueryService"/>
        /// arguments.
        /// </summary>
        /// <param name="dbContext">The Database Context</param>
        /// <param name="dbAdapter">The Database adapter that will instantiate a connection and execute the constructed queries and commands.</param>
        /// <param name="userQueryService">Service used for User Query operations.</param>
        /// <param name="roleQueryService">Service used for Role Query operations.</param>
        public AuthorizationService(
            LogisticsCompanyContext dbContext,
            IDbAdapter dbAdapter,
            IUserQueryService userQueryService,
            IRoleQueryService roleQueryService)
            : base(dbContext, dbAdapter)
        {
            _userQueryService = userQueryService;
            _roleQueryService = roleQueryService;
        }


        /// <summary>
        /// Executes a Login operation to the Database 
        /// based on the passed <paramref name="dto"/>.
        /// And returns a JWT token from the <paramref name="issuer"/> and <paramref name="key"/> 
        /// arguments.
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="issuer"></param>
        /// <param name="key"></param>
        /// <returns>
        /// The composed JWT Security Token.
        /// </returns>
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

        /// <summary>
        /// Executes a Register operation by creating a new User entity to the Database
        /// from the passed <paramref name="dto"/>.
        /// </summary>
        /// <param name="dto"></param>
        public async Task Register(RegisterDto dto)
        {
            dto.Password = PasswordHasher.HashPassword(dto.Password);
            var roleId = await _roleQueryService.GetIdByName(dto.Role);

            if (roleId != 0)
            {
                var registerEmail = await _userQueryService.GetRegisterEmail(dto.Email);

                if (string.IsNullOrEmpty(registerEmail))
                {
                        var insertCommand = SqlCommandHelper
                            .InsertCommand(
                                table: "Users",
                                values: new string[] {
                                    $"'{dto.Username}'", 
                                    $"'{dto.FirstName}'", 
                                    $"'{dto.LastName}'", 
                                    $"'{dto.Email}'", 
                                    $"{roleId}", 
                                    "NULL", 
                                    $"'{dto.Password}'" 
                                }
                            );

                        await this._dbAdapter.ExecuteCommand(insertCommand);
                }

                // TO DO: Add descriptive exception.
                return;
            }
        }
    }
}
