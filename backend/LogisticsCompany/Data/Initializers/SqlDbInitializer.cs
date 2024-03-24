using Dapper;
using LogisticsCompany.Data.Contracts;
using Microsoft.Data.SqlClient;

namespace LogisticsCompany.Data.Initializers
{
    public class SqlDbInitializer : IInitializer
    {
        internal string _connectionString;

        public SqlDbInitializer(string connectionString)
        {
            _connectionString = connectionString;
        }

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
