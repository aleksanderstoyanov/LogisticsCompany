using LogisticsCompany.Data.Contracts;
using LogisticsCompany.Data.Initializers;

namespace LogisticsCompany.Data.Factory
{
    public class SqlTableInitializerFactory : InitializerFactory
    {
        public override IInitializer CreateInitializer(string connectionString)
           => new SqlTableInitializer(connectionString);
    }
}
