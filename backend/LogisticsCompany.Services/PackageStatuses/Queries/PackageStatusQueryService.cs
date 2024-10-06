using Dapper;
using LogisticsCompany.Data;
using LogisticsCompany.Data.Builders;
using LogisticsCompany.Data.Common.Descriptors;
using LogisticsCompany.Data.Common.Operators;
using LogisticsCompany.Data.Contracts;
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
        /// with the injected <paramref name="dbContext"/> and <paramref name="dbAdapter"/>
        /// arguments
        /// </summary>
        /// <param name="dbContext">The Database context</param>
        /// <param name="dbAdapter">The Database adapter that will instantiate a connection and execute the constructed query.</param>
        public PackageStatusQueryService(LogisticsCompanyContext dbContext
            , IDbAdapter dbAdapter)
            : base(dbContext, dbAdapter)
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

            var query = new SqlQueryBuilder()
                   .Select(columns: "Id")
                   .From("PackageStatuses")
                   .Where(clauseDescriptorContainer)
                   .ToQuery();

            var result = await this._dbAdapter
                .QuerySingle<int>(query);

            return result;
        }
    }
}
