using Dapper;
using LogisticsCompany.Data;
using LogisticsCompany.Data.Builders;
using LogisticsCompany.Data.Common;
using Microsoft.Data.SqlClient;

namespace LogisticsCompany.Services.Roles.Queries
{
    public class RoleQueryService: BaseService, IRoleQueryService
    {
        public RoleQueryService(LogisticsCompanyContext dbContext)
            :base(dbContext)
        {
            
        }

        public async Task<int> GetIdByName(string name)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var clauseContainer = new ClauseDescriptorContainer();

                clauseContainer.Descriptors(descriptors =>
                {
                    descriptors.Add(descriptor => descriptor
                        .Field("Name")
                        .EqualityOperator(EqualityOperator.EQUALS)
                        .FieldValue(name)
                    );
                });

                var query = new SqlQueryBuilder()
                    .Select(columns: "Id")
                    .From(table: "Roles")
                    .Where(clauseContainer)
                    .ToQuery();

                var id = await connection.QuerySingleAsync<int>(query);

                return id;
            }
        }

        public async Task<string?> GetRoleNameById(int id)
        {
            using (var connection = new SqlConnection(this._connectionString))
            {
                var clauseContainer = new ClauseDescriptorContainer();

                clauseContainer.Descriptors(descriptors =>
                {
                    descriptors.Add(descriptor => descriptor
                        .Field("Id")
                        .EqualityOperator(EqualityOperator.EQUALS)
                        .FieldValue(id)
                    );
                });

                var query = new SqlQueryBuilder()
                   .Select(columns: "Name")
                   .From(table: "Roles")
                   .Where(clauseContainer)
                   .ToQuery();

                var roleName = await connection.QueryFirstOrDefaultAsync<string>(query);

                return roleName;
            }
        }
    }
}
