namespace LogisticsCompany.Data.Contracts
{
    public interface IDbFactory
    {
        IInitializer CreateDbInitializer(string connectionString);
        IInitializer CreateTableInitializer(string connectionString);
        ISeeder CreateDbSeeder(string connectionString);
    }
}
