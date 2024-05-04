using LogisticsCompany.Data.Factory;
using Microsoft.Extensions.Configuration;

namespace LogisticsCompany.Data
{
    /// <summary>
    /// Class used for creating the Database Context.
    /// </summary>
    public class LogisticsCompanyContext
    {
        internal IConfiguration _configuration { get; set; }

        /// <summary>
        /// Creates a <see cref="LogisticsCompanyContext" /> instance with the passed <paramref name="configuration"/>
        /// </summary>
        /// <param name="configuration"></param>
        public LogisticsCompanyContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Database constructor for context for an SQL Server.
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="dbFactory"></param>
        /// <param name="tableFactory"></param>
        public LogisticsCompanyContext(IConfiguration configuration, SqlDbFactory dbFactory)
        {
            _configuration = configuration;
            var connectionString = GetConnectionString();

            ConstructDatabase(dbFactory, connectionString);
        }

        private static void ConstructDatabase(SqlDbFactory dbFactory, string connectionString)
        {
            dbFactory
                .CreateDbInitializer(connectionString)
                .Init()
                .GetAwaiter()
                .GetResult();

            dbFactory
                .CreateTableInitializer(connectionString)
                .Init()
                .GetAwaiter()
                .GetResult();

            dbFactory
                .CreateDbSeeder(connectionString)
                .Seed()
                .GetAwaiter()
                .GetResult();
        }

        /// <summary>
        /// Method that retrieves the connection string from the Application Settings.
        /// </summary>
        public string GetConnectionString()
                => _configuration.GetConnectionString("DefaultConnectionString");
    }
}
