using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace LogisticsCompany.Data
{
    public class LogisticsCompanyContext
    {
        internal IConfiguration _configuration { get; set; }
        internal string _connectionString { get; set; }

        public LogisticsCompanyContext(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnectionString");

            Init().GetAwaiter().GetResult();
        }

        private IDbConnection CreateConnection()
            => new SqlConnection(_connectionString);

        private async Task InitDatabase()
        {
            // TO DO: Add DbSettings based on environment

            using var connection = CreateConnection();
            var sql = $"IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'LogisticsCompany') CREATE DATABASE [LogisticsCompany];";
            await connection.ExecuteAsync(sql);

            sql = "USE LogisticsCompany";
            await connection.ExecuteAsync(sql);
        }

        private async Task InitTables()
        {
            using var connection = CreateConnection();
            await _initUsers();
            await _initRoles();
            await _initUserRole();

            async Task _initUsers()
            {
                var sql = """
                IF OBJECT_ID('Users', 'U') IS NULL
                CREATE TABLE Users (
                    Id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
                    Username NVARCHAR(MAX) NOT NULL,
                    Email NVARCHAR(MAX) NOT NULL,
                    Role INT,
                    PasswordHash NVARCHAR(MAX) NOT NULL
                )
                """;

                await connection.ExecuteAsync(sql);
            }


            async Task _initRoles()
            {
                var sql = """
                 IF OBJECT_ID('Roles', 'U') IS NULL
                 CREATE TABLE Roles(
                    Id INT NOT NULL PRIMARY KEY IDENTITY (1, 1),
                    Name NVARCHAR(MAX),
                 )
                 """;

                await connection.ExecuteAsync(sql);
            }

            async Task _initUserRole()
            {
                var sql = """
                 IF OBJECT_ID('UserRoles', 'U') IS NULL
                      CREATE TABLE UserRoles(
                      UserId INT,
                 	 RoleId INT

                 	 CONSTRAINT user_role_pk PRIMARY KEY (UserId, RoleId),
                 	 CONSTRAINT fk_user FOREIGN KEY (UserId) REFERENCES dbo.Users(Id),
                 	 CONSTRAINT fk_role FOREIGN KEY (RoleId) REFERENCES dbo.Roles(Id)
                 )
                 """;

                await connection.ExecuteAsync(sql);
            }
        }
        public async Task Init()
        {
            await InitDatabase();
            await InitTables();
        }
    }
}
