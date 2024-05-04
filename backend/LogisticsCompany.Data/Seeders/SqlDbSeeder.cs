using Dapper;
using LogisticsCompany.Data.Builders;
using LogisticsCompany.Data.Contracts;
using LogisticsCompany.Data.Helpers;
using Microsoft.Data.SqlClient;

namespace LogisticsCompany.Data.Seeders
{
    /// <summary>
    /// Seeder class used for seeding data to the SQL Database.
    /// </summary>
    public class SqlDbSeeder : ISeeder
    {
        private readonly string _connectionString;

        public SqlDbSeeder(string connectionString)
        {
            this._connectionString = connectionString;
        }

        /// <summary>
        /// Method used for instatiating seeding operations to existing SQL Tables.
        /// </summary>
        public async Task Seed()
        {
            await SeedRoles();
            await SeedPackageStatuses();
            await SeedOffices();
            await SeedUsers();
            await SeedDeliveries();
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

        /// <summary>
        /// Table Structure
        /// <code>
        ///     <table>
        ///         <th> 
        ///             Address (NVARCHAR(MAX))
        ///         </th>
        ///         <th> 
        ///             PricePerWeight (DECIMAL(10, 2))
        ///         </th>
        ///     </table>
        /// </code>
        /// </summary>
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

        /// <summary>
        /// Table Structure
        /// <code>
        ///     <table>
        ///         <th> 
        ///             Username (NVARCHAR(MAX))
        ///         </th>
        ///         <th> 
        ///             FirstName (NVARCHAR(MAX))
        ///         </th>
        ///         <th> 
        ///             LastName (NVARCHAR(MAX))
        ///         </th>
        ///         <th>
        ///             Email (NVARCHAR(MAX))
        ///         </th>
        ///         <th> 
        ///             RoleId Fk (INT) NULL 
        ///             (1 - None; 2 - OfficeEmployee; 3 - Courier; 4 - Client 5 - Admin)
        ///         </th>
        ///     </table>
        /// </code>
        /// </summary>
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

        /// <summary>
        /// Table Structure
        /// <code>
        ///     <table>
        ///         <th> 
        ///             Name (NVARCHAR(MAX))
        ///         </th>
        ///     </table>
        /// </code>
        /// </summary>
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

        /// <summary>
        /// Table Structure
        /// <code>
        ///     <table>
        ///         <th> 
        ///             Name (NVARCHAR(MAX))
        ///         </th>
        ///     </table>
        /// </code>
        /// </summary>
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

        /// <summary>
        /// Table Structure
        /// <code>
        ///      <table>
        ///          <th> 
        ///              StartDate (DATE)
        ///          </th>
        ///          <th>
        ///              EndDate (DATE)    
        ///          </th>
        ///      </table>
        /// </code>
        /// </summary>
        private async Task SeedDeliveries()
        {
            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                var table = "Deliveries";

                if (!Exists(table))
                {
                    await sqlConnection.ExecuteAsync(InsertCommand(table, "'2024-05-01'", "NULL"));
                }
            }
        }


        /// <summary>
        /// Table Structure
        /// <code>
        ///      <table>
        ///          <th> 
        ///              FromId FK(INT) NULL
        ///          </th>
        ///          <th> 
        ///              ToId FK(INT) NULL
        ///          </th>
        ///          <th> 
        ///              PackageStatusId FK(INT) NULL
        ///              (1 - NonRegistered; 2 - Registered; 3 - InDelivery; 4 - Delivered)
        ///          </th>
        ///          <th>
        ///              OfficeId FK(INT) NULL
        ///          </th>
        ///          <th>
        ///              DeliveryId FK(INT) NULL
        ///          </th>
        ///          <th> 
        ///              Address (NVARCHAR(MAX))
        ///          </th>
        ///          <th>
        ///              ToOffice (BIT)
        ///          </th>
        ///           <th>
        ///              WEIGHT (INT)
        ///          </th>
        ///      </table>
        /// </code>
        /// </summary>
        private async Task SeedPackages()
        {
            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                var table = "Packages";

                if (!Exists(table))
                {
                    await sqlConnection.ExecuteAsync(InsertCommand(table, "2", "3", "1", "1", "1", "'Pesholandiq 12'", "0", "12.1"));
                }
            }

        }
    }
}
