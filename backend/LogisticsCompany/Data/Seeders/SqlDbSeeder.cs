using LogisticsCompany.Data.Contracts;

namespace LogisticsCompany.Data.Seeders
{
    public class SqlDbSeeder : ISeeder
    {
        private readonly string _connectionString;

        public SqlDbSeeder(string connectionString)
        {
            this._connectionString = connectionString;
        }
        public Task Seed()
        {
            // TODO: Implement Roles Seeding
            throw new NotImplementedException();
        }
    }
}
