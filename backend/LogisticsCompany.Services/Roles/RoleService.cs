using Dapper;
using LogisticsCompany.Data;
using LogisticsCompany.Data.Builders;
using LogisticsCompany.Data.Common;
using LogisticsCompany.Data.Helpers;
using LogisticsCompany.Services.Contracts;
using Microsoft.Data.SqlClient;

namespace LogisticsCompany.Services
{
    public class RoleService : BaseService, IRoleService
    {
        public RoleService(LogisticsCompanyContext dbContext)
            : base(dbContext)
        {
        }

        public async Task<int> GetIdByName(string name)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var clauseContainer = new ClauseDescriptorContainer()
                {
                    ClauseDescriptors = new HashSet<ClauseDescriptor>()
                    {
                       new ClauseDescriptor
                       {
                           Field = "Name",
                           EqualityOperator = EqualityOperator.EQUALS,
                           FieldValue = name
                       }
                    }
                };

                var query = new SqlQueryBuilder()
                    .Select(columns: "Id")
                    .From(table: "Roles")
                    .Where(clauseContainer)
                    .GetQuery();

                var id = await connection.QuerySingleAsync<int>(query);

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
                var clauseContainer = new ClauseDescriptorContainer()
                {
                    ClauseDescriptors = new List<ClauseDescriptor>()
                    {
                       new ClauseDescriptor
                       {
                           Field = "Id",
                           EqualityOperator = EqualityOperator.EQUALS,
                           FieldValue = id
                       }
                    }
                };

                var query = new SqlQueryBuilder()
                   .Select(columns: "Name")
                   .From(table: "Roles")
                   .Where(clauseContainer)
                   .GetQuery();

                var roleName = await connection.QueryFirstOrDefaultAsync<string>(query);

                return roleName;
            }
        }
    }
}
