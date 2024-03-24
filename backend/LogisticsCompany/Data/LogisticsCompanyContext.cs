using Dapper;
using LogisticsCompany.Data.Factory;
using Microsoft.Data.SqlClient;
using System.Data;

namespace LogisticsCompany.Data
{
    public class LogisticsCompanyContext
    {
        internal IConfiguration _configuration { get; set; }

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

        private string GetConnectionString()
                => _configuration.GetConnectionString("DefaultConnectionString");
    }
}
