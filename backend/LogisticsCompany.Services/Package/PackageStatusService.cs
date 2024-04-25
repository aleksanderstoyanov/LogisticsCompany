using Dapper;
using LogisticsCompany.Data;
using LogisticsCompany.Data.Builders;
using LogisticsCompany.Data.Common;
using LogisticsCompany.Services.Contracts;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogisticsCompany.Services.Package
{
    public class PackageStatusService: BaseService, IPackageStatusService
    {
        public PackageStatusService(LogisticsCompanyContext dbContext):
            base(dbContext)
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
                    .GetQuery();


                var result = await connection.QuerySingleOrDefaultAsync<int>(query);

                return result;
            }            
        }
    }
}
