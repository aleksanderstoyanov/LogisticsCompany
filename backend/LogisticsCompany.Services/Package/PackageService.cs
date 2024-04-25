using Dapper;
using LogisticsCompany.Data;
using LogisticsCompany.Data.Builders;
using LogisticsCompany.Data.Common;
using LogisticsCompany.Data.Helpers;
using LogisticsCompany.Services.Contracts;
using LogisticsCompany.Services.Dto;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogisticsCompany.Services.Packages
{
    public class PackageService : BaseService, IPackageService
    {
        public PackageService(LogisticsCompanyContext dbContext) :
            base(dbContext)
        {
        }

        public async Task Create(PackageDto dto)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = SqlCommandHelper.InsertCommand(
                    table: "Packages",
                    values: new string[]
                    {
                        dto.From != null ? dto.From.ToString() : "NULL",
                        dto.To != null ? dto.To.ToString() : "NULL",
                        "1",
                        $"'{dto.Address}'",
                        dto.ToOffice ? "1" : "0",
                        dto.Weight.ToString().Replace(",", ".")
                    }
                );

                await connection.ExecuteAsync(command);
            }
        }

        public async Task<int> GetPackageCountByFromAndTo(int from, int to)
        {
            var clauseDescriptorContainer = new ClauseDescriptorContainer()
            {
                ClauseDescriptors = new HashSet<ClauseDescriptor>()
                {
                    new ClauseDescriptor
                    {
                        Field = "ToId",
                        FieldValue = to,
                        EqualityOperator = EqualityOperator.EQUALS,
                        LogicalOperator = LogicalOperator.AND
                    },
                    new ClauseDescriptor
                    {
                        Field = "FromId",
                        FieldValue = from,
                        EqualityOperator = EqualityOperator.EQUALS
                    }
                }
            };

            var query = new SqlQueryBuilder()
                .Select(columns: "COUNT(Id)")
                .From(table: "Packages")
                .Where(clauseDescriptorContainer)
                .GetQuery();

            using (var connection = new SqlConnection(_connectionString))
            {
                var result = await connection.QuerySingleAsync<int>(query);

                return result;
            }
        }
    }
}
