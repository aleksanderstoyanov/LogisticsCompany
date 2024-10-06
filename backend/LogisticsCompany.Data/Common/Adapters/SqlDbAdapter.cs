using Dapper;
using LogisticsCompany.Data.Contracts;
using Microsoft.Data.SqlClient;

namespace LogisticsCompany.Data.Common.Adapters
{
    public class SqlDbAdapter : IDbAdapter
    {
        private readonly string _connectionString;

        /// <summary>
        /// Constructs the SqlAdapter with the passed <paramref name="connectionString"/>.
        /// </summary>
        /// <param name="connectionString">
        /// The connection string that will make connectivity to the database.
        /// </param>
        public SqlDbAdapter(string connectionString)
        {
            this._connectionString = connectionString;
        }

        /// <summary>
        /// Executes a single row query.
        /// </summary>
        /// <typeparam name="T">The return type that the query will be mapped to.</typeparam>
        /// <param name="query">The constructed query.</param>
        public async Task<T> QuerySingle<T>(string query)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var result = await connection.QuerySingleOrDefaultAsync<T>(query);
                return result;
            }
        }

        /// <summary>
        /// Execute a multiple row query.
        /// </summary>
        /// <typeparam name="T">The return type that the query will be mapped to.</typeparam>
        /// <param name="query">The constructed query.</param>
        public async Task<IEnumerable<T>> QueryAll<T>(string query)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var result = await connection.QueryAsync<T>(query);
                return result;
            }
        }

        /// <summary>
        /// Executes a single command.
        /// </summary>
        /// <param name="command">The command that will be executed.</param>
        /// <param name="param">The additional parameters that will be passed to the command.</param>
        public async Task ExecuteCommand(string command, object? param = null)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.ExecuteAsync(command, param);
            }
        }
      
    }
}
