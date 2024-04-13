using Dapper;
using LogisticsCompany.Data;
using LogisticsCompany.Data.Helpers;
using LogisticsCompany.Dto;
using LogisticsCompany.Entity;
using LogisticsCompany.Services.Contracts;
using LogisticsCompany.Services.Dto;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static LogisticsCompany.Data.Helpers.SqlQueryHelper;

namespace LogisticsCompany.Services.Users
{
    public class UserService : IUserService
    {
        private readonly string _connectionString;
        private readonly LogisticsCompanyContext _dbContext;
        private readonly IRoleService _roleService;

        public UserService(LogisticsCompanyContext dbContext, IRoleService roleService)
        {
            _dbContext = dbContext;
            _connectionString = dbContext.GetConnectionString();
            _roleService = roleService;
        }

        public async Task Update(UserDto userDto)
        {
            var roleId = await _roleService.GetIdByName(userDto.RoleName);

            Dictionary<string, string> keyValuePairs = new Dictionary<string, string>()
            {
                { "Username", userDto.Username },
                { "Email", userDto.Email },
                { "RoleId", roleId.ToString()},
            };

            var updateCommand = SqlCommandHelper.UpdateCommand("Users", typeof(User), keyValuePairs, userDto.Id);

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.ExecuteAsync(updateCommand);
            }
        }

        public async Task Delete(int id)
        {
            var user = GetById(id);

            if (user == null) return;

            using (var connection = new SqlConnection(_connectionString))
            {
                var query = SqlCommandHelper.DeleteCommand("Users", "Id");
                await connection.ExecuteAsync(query, new {criteriaValue = id});
            }
        }

        public async Task<LoginDto?> GetById(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = SelectEntityBySingleCriteria("Users", "Id");
                var user = await connection.QuerySingleOrDefaultAsync<LoginDto?>(query, new { criteriaValue = id });
                return user;
            }
        }

        public async Task<IEnumerable<UserDto>> GetUsers()
        {
            using (var connection = new SqlConnection(this._connectionString))
            {
                var query = """
                    SELECT u.Id, u.Email, u.Username, r.Name as RoleName FROM Users AS u
                        INNER JOIN Roles as r ON r.Id = u.RoleId
                    """;
                var users = await connection.QueryAsync<UserDto>(query);

                return users;
            }
        }

        public async Task<LoginDto?> GetUserByEmailAndPassword(string email, string password)
        {
            using (var connection = new SqlConnection(this._connectionString))
            {
                var query = SelectEntityBySingleCriteria("Users", "Email");
                var user = await connection.QuerySingleOrDefaultAsync<LoginDto?>(query, new { criteriaValue = email });

                return user;
            }
        }
        public async Task<string> GetRegisterEmail(string email)
        {
            using (var sqlConnection = new SqlConnection(_dbContext.GetConnectionString()))
            {
                var query = SelectSingleColumnBySingleCriteria("Users", "Email");
                var queryResult = await sqlConnection
                   .QueryFirstOrDefaultAsync<string>
                   (
                       query,
                       new { criteriaValue = email }
                   );

                return queryResult ?? string.Empty;
            }
        }

        public async Task<string> Login(LoginDto dto, string issuer, string key)
        {
            var user = await GetUserByEmailAndPassword(dto.Email, dto.PasswordHash);

            if (user != null && PasswordHasher.VerifyPassword(dto.PasswordHash, user.PasswordHash))
            {
                var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

                var role = await _roleService.GetRoleNameById(user.RoleId) ?? string.Empty;

                var claims = new Claim[]
                {
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
            var roleId = await _roleService.GetIdByName(dto.Role);

            if (roleId != 0)
            {
                var registerEmail = await GetRegisterEmail(dto.Email);

                if (string.IsNullOrEmpty(registerEmail))
                {
                    using (var sqlConnection = new SqlConnection(_connectionString))
                    {
                        var insertCommand = SqlCommandHelper.InsertCommand("Users", $"'{dto.Username}'", $"'{dto.Email}'", $"{roleId}", $"'{dto.Password}'");
                        await sqlConnection.ExecuteAsync(insertCommand);
                    }
                }

                // TO DO: Add descriptive exception.
                return;
            }
        }
    }
}
