using Dapper;
using LogisticsCompany.Data;
using LogisticsCompany.Data.Helpers;
using LogisticsCompany.Services.Contracts;
using Microsoft.Data.SqlClient;

namespace LogisticsCompany.Services
{
    public class RoleService : IRoleService
    {
        private readonly LogisticsCompanyContext _dbContext;
        private readonly string _connectionString;

        public RoleService(LogisticsCompanyContext _dbContext)
        {
            this._dbContext = _dbContext;
            this._connectionString = this._dbContext.GetConnectionString();
        }
        
        public async Task<int> GetIdByName(string name)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = SqlQueryHelper.SelectIdBySingleCriteria("Roles", "Name");
                var id = await connection.QuerySingleAsync<int>(query, new { criteriaValue = name });

                return id;
            }
        }

        public Task Create(string roleName)
        {
            throw new NotImplementedException();
        }

        public async Task<string?> GetRoleNameById(int id)
        {
            using (var connection = new SqlConnection(this._connectionString))
            {
                var query = SqlQueryHelper.SelectSingleColumnById("Roles", "Name");

                var roleName = await connection.QueryFirstOrDefaultAsync<string>(query, new { criteriaValue = id });

                return roleName;
            }
        }
    }
}
