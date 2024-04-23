using Dapper;
using LogisticsCompany.Data.Contracts;
using LogisticsCompany.Data.Helpers;
using Microsoft.Data.SqlClient;

using static LogisticsCompany.Data.Helpers.SqlConstraintHelper;

namespace LogisticsCompany.Data.Initializers
{
    public class SqlTableInitializer : IInitializer
    {
        internal string _connectionString { get; set; }

        public SqlTableInitializer(string connectionString)
        {
            _connectionString = connectionString;
        }
        public async Task Init()
        {
            await InitRoles();
            await InitOffices();
            await InitUsers();
        }

        private async Task InitUsers()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var sql = $"""
                IF OBJECT_ID('Users', 'U') IS NULL
                CREATE TABLE Users (
                    Id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
                    Username NVARCHAR(MAX) NOT NULL,
                    FirstName NVARCHAR(MAX) NOT NULL,
                    LastName NVARCHAR(MAX) NOT NULL,
                    Email NVARCHAR(MAX) NOT NULL,
                    RoleId INT,
                    OfficeId INT NULL,
                    PasswordHash NVARCHAR(MAX) NOT NULL,
                    {ForeignKeyConstraint("fk_role", "RoleId", "dbo.Roles", "Id")},
                    {ForeignKeyConstraint("fk_office", "OfficeId", "dbo.Offices", "Id")} ON DELETE SET NULL
                )
                """;

                await connection.ExecuteAsync(sql);
            }
        }

        private async Task InitRoles()
        {

            using (var connection = new SqlConnection(_connectionString))
            {

                var sql = """
                      IF OBJECT_ID('Roles', 'U') IS NULL
                      CREATE TABLE Roles(
                         Id INT NOT NULL PRIMARY KEY IDENTITY (1, 1),
                         Name NVARCHAR(MAX)
                      )
                      """;

                await connection.ExecuteAsync(sql);
            }
        }

        private async Task InitOffices()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var sql = """
                      IF OBJECT_ID('Offices', 'U') IS NULL
                      CREATE TABLE Offices(
                         Id INT NOT NULL PRIMARY KEY IDENTITY (1, 1),
                         Address NVARCHAR(MAX)
                      )
                      """;

                await connection.ExecuteAsync(sql);
            }
        }
    }
}
