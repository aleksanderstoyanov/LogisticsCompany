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

        public RoleService(LogisticsCompanyContext _dbContext)
        {
            this._dbContext = _dbContext;
        }
        
        public async Task<int> GetIdByName(string name)
        {
            var connectionString = _dbContext.GetConnectionString();

            using (var connection = new SqlConnection(connectionString))
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
    }
}
