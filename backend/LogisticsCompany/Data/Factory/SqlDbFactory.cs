using LogisticsCompany.Data.Contracts;
using LogisticsCompany.Data.Initializers;
using LogisticsCompany.Data.Seeders;

namespace LogisticsCompany.Data.Factory
{
    public class SqlDbFactory : IDbFactory
    {
        public IInitializer CreateDbInitializer(string connectionString)
            => new SqlDbInitializer(connectionString);

        public ISeeder CreateDbSeeder(string connectionString)
            => new SqlDbSeeder(connectionString);
            
        public IInitializer CreateTableInitializer(string connectionString)
            => new SqlTableInitializer(connectionString);
    }
}
