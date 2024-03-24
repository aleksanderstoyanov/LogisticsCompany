using Dapper;
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
        }

        private string InsertCommand(string table, params string[] values)
            => SqlCommandHelper.InsertCommand(table, values);

        private bool Exists(string table)
        {
            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                var query = SqlQueryHelper.SelectAllCount(table);

                var count = sqlConnection.QuerySingle<int>(query);

                return count > 0;
            }
        }

        private async Task SeedRoles()
        {

            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                var table = "Roles";

                if (!Exists(table))
                {
                    await sqlConnection.ExecuteAsync(InsertCommand(table, "'Employee'"));
                    await sqlConnection.ExecuteAsync(InsertCommand(table, "'Client'"));
                    await sqlConnection.ExecuteAsync(InsertCommand(table, "'Admin'"));
                }
            }
        }
    }
}
