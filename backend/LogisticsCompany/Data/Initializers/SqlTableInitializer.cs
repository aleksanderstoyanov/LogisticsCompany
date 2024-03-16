using Dapper;
using LogisticsCompany.Data.Contracts;
using Microsoft.Data.SqlClient;

namespace LogisticsCompany.Data.Initializers
{
    public class SqlTableInitializer : IInitializer
    {
        internal string _connectionString { get; set; }

        internal SqlConnection _sqlConnection { get; set; }

        public SqlTableInitializer(string connectionString)
        {
            _connectionString = connectionString;
            _sqlConnection = new SqlConnection(_connectionString);
        }
        public async Task Init()
        {
            await InitUsers();
            await InitRoles();
            await InitUserRoles();

            _sqlConnection.Dispose();
        }

        private async Task InitUsers()
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

            await _sqlConnection.ExecuteAsync(sql);
        }


        private async Task InitRoles()
        {

            using var connection = new SqlConnection(_connectionString);
            var sql = """
                 IF OBJECT_ID('Roles', 'U') IS NULL
                 CREATE TABLE Roles(
                    Id INT NOT NULL PRIMARY KEY IDENTITY (1, 1),
                    Name NVARCHAR(MAX),
                 )
                 """;

            await _sqlConnection.ExecuteAsync(sql);
        }

        private async Task InitUserRoles()
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

            await _sqlConnection.ExecuteAsync(sql);
        }
    }
}
