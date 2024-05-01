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
            await SeedPackageStatuses();
            await SeedOffices();
            await SeedUsers();
            await SeedPackages();
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
                    .ToQuery();

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
                    await sqlConnection.ExecuteAsync(InsertCommand(table, "'ul.Geo Milev'", "'12.52'"));
                    await sqlConnection.ExecuteAsync(InsertCommand(table, "'ul.Shipchenski Prohod'", "'12.58'"));
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
                    await sqlConnection.ExecuteAsync(InsertCommand(table, "'admin'", "'Admin'", "'Adminov'", "'admin@gmail.com'", "5", "NULL", $"'{password}'"));
                    await sqlConnection.ExecuteAsync(InsertCommand(table, "'test'", "'Test'", "'Testov'", "'test@gmail.com'", "4", "1", $"'{password}'"));
                    await sqlConnection.ExecuteAsync(InsertCommand(table, "'client'", "'Client'", "'Clientov'", "'client@gmail.com'", "4", "1", $"'{password}'"));
                    await sqlConnection.ExecuteAsync(InsertCommand(table, "'pesho'", "'Pesho'", "'Peshov'", "'office@gmail.com'", "2", "1", $"'{password}'"));
                    await sqlConnection.ExecuteAsync(InsertCommand(table, "'gosho'", "'Gosho'", "'Goshov'", "'courier@gmail.com'", "3", "1", $"'{password}'"));
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

        private async Task SeedPackageStatuses()
        {
            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                var table = "PackageStatuses";

                if (!Exists(table))
                {
                    await sqlConnection.ExecuteAsync(InsertCommand(table, "'NonRegistered'"));
                    await sqlConnection.ExecuteAsync(InsertCommand(table, "'Registered'"));
                    await sqlConnection.ExecuteAsync(InsertCommand(table, "'InDelivery'"));
                    await sqlConnection.ExecuteAsync(InsertCommand(table, "'Delivered'"));
                }
            }
        }

        private async Task SeedPackages()
        {
            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                var table = "Packages";

                if (!Exists(table))
                {
                    await sqlConnection.ExecuteAsync(InsertCommand(table, "2", "3", "1", "1", "'Pesholandiq 12'", "0", "12.1"));
                }
            }

        }
    }
}
