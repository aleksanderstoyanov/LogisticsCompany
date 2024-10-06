using Dapper;
using LogisticsCompany.Data;
using LogisticsCompany.Data.Builders;
using LogisticsCompany.Data.Common.Descriptors;
using LogisticsCompany.Data.Common.Operators;
using LogisticsCompany.Data.Contracts;
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
        /// with the injected <paramref name="dbContext"/> and <paramref name="dbAdapter"/>
        /// arguments.
        /// </summary>
        /// <param name="dbContext">The Database context.</param>
        /// <param name="dbAdapter">The Database adapter that will instantiate a connection and execute the constructed query.</param>
        public DeliveryQueryService(LogisticsCompanyContext dbContext
            , IDbAdapter dbAdapter)
            : base(dbContext, dbAdapter)
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

            var result = await this._dbAdapter
                .QueryAll<DeliveryDto>(query);

            return result;
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

            var query = new SqlQueryBuilder()
                .Select(columns: "*")
                .From(table: "Deliveries")
                .Where(clauseDescriptorContainer)
                .ToQuery();

            var result = await this._dbAdapter.QuerySingle<DeliveryDto>(query);

            return result;
        }
    }
}
