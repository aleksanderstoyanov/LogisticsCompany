using Dapper;
using LogisticsCompany.Data;
using LogisticsCompany.Data.Helpers;
using LogisticsCompany.Dto;
using LogisticsCompany.Services.Contracts;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.Data.SqlClient;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;

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
        public async Task Login()
        {
            throw new NotImplementedException();
        }

        public async Task Register(RegisterDto dto)
        {
            // TODO: Add Arguments Validation

            var connectionString = _dbContext.GetConnectionString();

            dto.Password = PasswordHasher.HashPassword(dto.Password);
            var roleId = await _roleService.GetIdByName(dto.Role);

            if (roleId != 0)
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    var insertCommand = SqlCommandHelper.InsertCommand("Users", $"'{dto.Username}'", $"'{dto.Email}'", $"{roleId}", $"'{dto.Password}'");
                    await sqlConnection.ExecuteAsync(insertCommand);
                }
            }
        }
    }
}
