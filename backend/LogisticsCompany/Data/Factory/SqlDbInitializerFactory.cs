using LogisticsCompany.Data.Contracts;
using LogisticsCompany.Data.Initializers;

namespace LogisticsCompany.Data.Factory
{
    public class SqlDbInitializerFactory : InitializerFactory
    {
        public override IInitializer CreateInitializer(string connectionString)
           => new SqlDbInitializer(connectionString);
    }
}
