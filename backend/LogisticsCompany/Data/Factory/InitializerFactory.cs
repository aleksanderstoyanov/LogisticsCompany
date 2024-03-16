using LogisticsCompany.Data.Contracts;

namespace LogisticsCompany.Data.Factory
{
    public abstract class InitializerFactory
    {
        public abstract IInitializer CreateInitializer(string connectionString);
    }
}
