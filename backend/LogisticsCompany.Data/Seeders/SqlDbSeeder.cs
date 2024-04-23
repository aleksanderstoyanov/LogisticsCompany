using Dapper;
using LogisticsCompany.Data.Builders;
using LogisticsCompany.Data.Contracts;
using LogisticsCompany.Data.Helpers;
using Microsoft.Data.SqlClient;

namespace LogisticsCompany.Data.Seeders
{
    public class SqlDbSeeder : ISeeder
    {
        private readonly string _connectionString;

        public SqlDbSeeder(string connectionString)
        {
            this._connectionString = connectionString;
        }
        public async Task Seed()
        {
            await SeedRoles();
            await SeedOffices();
            await SeedUsers();
        }

        private string InsertCommand(string table, params string[] values)
            => SqlCommandHelper.InsertCommand(table, values);

        private bool Exists(string table)
        {
            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                var query = new SqlQueryBuilder()
                    .Select(columns: "COUNT(Id)")
                    .From(table)
                    .GetQuery();

                var count = sqlConnection.QuerySingle<int>(query);

                return count > 0;
            }
        }

        private async Task SeedOffices()
        {
            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                var table = "Offices";

                if (!Exists(table))
                {
                    await sqlConnection.ExecuteAsync(InsertCommand(table, "'ul.Geo Milev'"));
                }
            }
        }

        private async Task SeedUsers()
        {

            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                var table = "Users";

                if (!Exists(table))
                {
                    var password = PasswordHasher.HashPassword("123123");
                    await sqlConnection.ExecuteAsync(InsertCommand(table, "'admin'", "'Admin'", "'Adminov'" ,"'admin@gmail.com'", "5", "NULL" , $"'{password}'"));
                    await sqlConnection.ExecuteAsync(InsertCommand(table, "'test'","'Test'", "'Testov'" ,"'test@gmail.com'", "1" ,"1" , $"'{password}'"));
                }
            }
        }

        private async Task SeedRoles()
        {

            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                var table = "Roles";

                if (!Exists(table))
                {
                    await sqlConnection.ExecuteAsync(InsertCommand(table, "'None'"));
                    await sqlConnection.ExecuteAsync(InsertCommand(table, "'OfficeEmployee'"));
                    await sqlConnection.ExecuteAsync(InsertCommand(table, "'Courier'"));
                    await sqlConnection.ExecuteAsync(InsertCommand(table, "'Client'"));
                    await sqlConnection.ExecuteAsync(InsertCommand(table, "'Admin'"));
                }
            }
        }
    }
}
