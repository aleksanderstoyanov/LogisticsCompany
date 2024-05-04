using Dapper;
using LogisticsCompany.Data.Contracts;
using Microsoft.Data.SqlClient;

namespace LogisticsCompany.Data.Initializers
{
    /// <summary>
    /// Initializer Class used for construcing the the LogisticsCompany Database.
    /// </summary>
    public class SqlDbInitializer : IInitializer
    {
        internal string _connectionString;

        public SqlDbInitializer(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// Method used constructing the Database through a newly created SQL Connection.
        /// </summary>
        public async Task Init()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var sql = $"IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'LogisticsCompany') CREATE DATABASE LogisticsCompany";
                await connection.ExecuteAsync(sql);

                sql = "USE LogisticsCompany";
                await connection.ExecuteAsync(sql);
            }
        }
    }
}
