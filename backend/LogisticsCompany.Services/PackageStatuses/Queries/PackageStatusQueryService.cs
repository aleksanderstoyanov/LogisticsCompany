using Dapper;
using LogisticsCompany.Data;
using LogisticsCompany.Data.Builders;
using LogisticsCompany.Data.Common;
using Microsoft.Data.SqlClient;

namespace LogisticsCompany.Services.PackageStatuses.Queries
{
    public class PackageStatusQueryService : BaseService, IPackageStatusQueryService
    {
        public PackageStatusQueryService(LogisticsCompanyContext dbContext)
            : base(dbContext)
        {

        }

        public async Task<int?> GetIdByName(string name)
        {
            var clauseDescriptorContainer = new ClauseDescriptorContainer()
            {
                ClauseDescriptors = new HashSet<ClauseDescriptor>()
                {
                    new ClauseDescriptor
                    {
                        Field = "Name",
                        FieldValue = name,
                        EqualityOperator = EqualityOperator.EQUALS

                    }
                }
            };

            using (var connection = new SqlConnection(_connectionString))
            {
                var query = new SqlQueryBuilder()
                    .Select(columns: "Id")
                    .From("PackageStatuses")
                    .Where(clauseDescriptorContainer)
                    .ToQuery();

                var result = await connection.QuerySingleOrDefaultAsync<int>(query);

                return result;
            }
        }
    }
}
