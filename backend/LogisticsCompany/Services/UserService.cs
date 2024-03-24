using Dapper;
using LogisticsCompany.Data;
using LogisticsCompany.Data.Helpers;
using LogisticsCompany.Dto;
using LogisticsCompany.Services.Contracts;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.Data.SqlClient;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;

namespace LogisticsCompany.Services
{
    public class UserService : IUserService
    {
        private readonly LogisticsCompanyContext _dbContext;
        private readonly IRoleService _roleService;

        public UserService(LogisticsCompanyContext dbContext, IRoleService roleService)
        {
            this._dbContext = dbContext;
            this._roleService = roleService;
        }
        public async Task Login()
        {
            throw new NotImplementedException();
        }

        private string HashPassword(string password)
        {
            // cryptographically strong random bytes.
            byte[] salt = RandomNumberGenerator.GetBytes(128 / 8); // divide by 8 to convert bits to bytes
            Console.WriteLine($"Salt: {Convert.ToBase64String(salt)}");

            // derive a 256-bit subkey (use HMACSHA256 with 100,000 iterations)
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password!,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8));

            return hashed;
        }

        public async Task Register(RegisterDto dto)
        {
            // TODO: Add Arguments Validation

            var connectionString = _dbContext.GetConnectionString();

            dto.Password = HashPassword(dto.Password);
            var roleId = await _roleService.GetIdByName(dto.Role);
            
            if (roleId != 0)
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    var insertCommand = SqlCommandHelper.InsertCommand("Users", $"'{dto.Username}'", $"'{dto.Email}'", $"{roleId}" ,$"'{dto.Password}'");
                    await sqlConnection.ExecuteAsync(insertCommand);
                }
            }
        }
    }
}
