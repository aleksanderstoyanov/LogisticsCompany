using Dapper;
using LogisticsCompany.Data;
using LogisticsCompany.Data.Builders;
using LogisticsCompany.Data.Common.Descriptors;
using LogisticsCompany.Data.Common.Operators;
using Microsoft.Data.SqlClient;

namespace LogisticsCompany.Services.PackageStatuses.Queries
{
    /// <summary>
    /// A <see cref="BaseService"/> class that will perform Database Query operations for PackageStatuses
    /// </summary>
    public class PackageStatusQueryService : BaseService, IPackageStatusQueryService
    {
        /// <summary>
        /// Creates a <see cref="PackageStatusQueryService"/> instance 
        /// with the injected <paramref name="dbContext"/>
        /// </summary>
        /// <param name="dbContext">The Database context</param>
        public PackageStatusQueryService(LogisticsCompanyContext dbContext)
            : base(dbContext)
        {

        }

        /// <summary>
        /// Performs a SQL Query for retrieving the Id field
        /// of a PackageStatus based on the passed <paramref name="name"/>.
        /// </summary>
        /// <param name="name">The name of the package status.</param>
        /// <returns>
        /// The Id field of the gathered PackageStatus entity.
        /// </returns>
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
