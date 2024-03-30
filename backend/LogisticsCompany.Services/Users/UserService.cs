using Dapper;
using LogisticsCompany.Data;
using LogisticsCompany.Data.Helpers;
using LogisticsCompany.Dto;
using LogisticsCompany.Services.Contracts;
using Microsoft.Data.SqlClient;

using static LogisticsCompany.Data.Helpers.SqlQueryHelper;

namespace LogisticsCompany.Services.Users
{
    public class UserService : IUserService
    {
        private readonly LogisticsCompanyContext _dbContext;
        private readonly IRoleService _roleService;

        public UserService(LogisticsCompanyContext dbContext, IRoleService roleService)
        {
            _dbContext = dbContext;
            _roleService = roleService;
        }

        public async Task<RegisterDto> GetUserByEmail(string email)
        {
            using (var sqlConnection = new SqlConnection(_dbContext.GetConnectionString()))
            {
                var user = await sqlConnection
                   .QuerySingleAsync<RegisterDto>
                   (
                       SelectBySingleCriteria("Users", "Email"),
                       new { criteriaValue = email }
                   );

                return user;
            }
        }

        public async Task Login()
        {
            // TODO: Add Login Logic
            throw new NotImplementedException();
        }

        public async Task Register(RegisterDto dto)
        {
            var connectionString = _dbContext.GetConnectionString();

            dto.Password = PasswordHasher.HashPassword(dto.Password);
            var roleId = await _roleService.GetIdByName(dto.Role);

            if (roleId != 0)
            {
                if (GetUserByEmail(dto.Email) == null)
                {
                    using (var sqlConnection = new SqlConnection(connectionString))
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
