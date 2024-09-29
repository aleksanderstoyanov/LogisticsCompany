using Dapper;
using LogisticsCompany.Data;
using LogisticsCompany.Data.Builders;
using LogisticsCompany.Data.Common.Descriptors;
using LogisticsCompany.Data.Common.Operators;
using LogisticsCompany.Services.Dto;
using Microsoft.Data.SqlClient;

namespace LogisticsCompany.Services.Deliveries.Queries
{
    /// <summary>
    /// A <see cref="BaseService"/> class used for performing DataBase query operations for Deliveries.
    /// </summary>
    public class DeliveryQueryService : BaseService, IDeliveryQueryService
    {
        /// <summary>
        /// Creates a <see cref="DeliveryQueryService"/> instance 
        /// with the injected <paramref name="dbContext"/>
        /// </summary>
        /// <param name="dbContext">The Database context.</param>
        public DeliveryQueryService(LogisticsCompanyContext dbContext)
            : base(dbContext)
        {
        }

        /// <summary>
        /// Performs a SQL Query for Retrieving All Deliveries.
        /// </summary>
        /// <returns>
        /// <see cref="IEnumerable{DeliveryDto}"/> collection of deliveries.
        /// </returns>
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

        /// <summary>
        /// Performs a SQL Query for Retrieving a Delivery Entity
        /// based on the passed <paramref name="id"/>.
        /// </summary>
        /// <returns>
        /// <see cref="DeliveryDto"/>
        /// </returns>
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
