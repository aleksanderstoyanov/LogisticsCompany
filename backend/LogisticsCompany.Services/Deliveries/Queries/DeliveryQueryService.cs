using Dapper;
using LogisticsCompany.Data;
using LogisticsCompany.Data.Builders;
using LogisticsCompany.Data.Common;
using LogisticsCompany.Services.Dto;
using Microsoft.Data.SqlClient;

namespace LogisticsCompany.Services.Deliveries.Queries
{
    public class DeliveryQueryService : BaseService, IDeliveryQueryService
    {
        public DeliveryQueryService(LogisticsCompanyContext dbContext)
            : base(dbContext)
        {
        }

        public async Task<IEnumerable<DeliveryDto>> GetAll()
        {
            var query = new SqlQueryBuilder()
                .Select("*")
                .From(table: "Deliveries")
                .ToQuery();

            using (var connection = new SqlConnection(_connectionString))
            {
                var result = await connection.QueryAsync<DeliveryDto>(query);

                return result;
            }
        }

        public async Task<DeliveryDto?> GetById(int id)
        {
            var clauseDescriptorContainer = new ClauseDescriptorContainer()
                .Descriptors(descriptors =>
                {
                    descriptors.Add(descriptor => descriptor
                        .Field("Id")
                        .FieldValue(id)
                        .EqualityOperator(EqualityOperator.EQUALS)
                    );
                });

            using (var connection = new SqlConnection(_connectionString))
            {
                var query = new SqlQueryBuilder()
                    .Select(columns: "*")
                    .From(table: "Deliveries")
                    .Where(clauseDescriptorContainer)
                    .ToQuery();

                var result = await connection.QuerySingleOrDefaultAsync<DeliveryDto>(query);

                return result;
            }
        }
    }
}
