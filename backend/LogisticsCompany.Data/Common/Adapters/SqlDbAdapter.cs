using Dapper;
using LogisticsCompany.Data.Contracts;
using Microsoft.Data.SqlClient;

namespace LogisticsCompany.Data.Common.Adapters
{
    public class SqlDbAdapter : IDbAdapter
    {
        private readonly string _connectionString;

        public SqlDbAdapter(string connectionString)
        {
            this._connectionString = connectionString;
        }

        public async Task<T> QuerySingle<T>(string query)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var result = await connection.QuerySingleOrDefaultAsync<T>(query);
                return result;
            }
        }

        public async Task<IEnumerable<T>> QueryAll<T>(string query)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var result = await connection.QueryAsync<T>(query);
                return result;
            }
        }

        public async Task ExecuteCommand(string command, object? param = null)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.ExecuteAsync(command, param);
            }
        }
      
    }
}
