using Dapper;
using LogisticsCompany.Data;
using LogisticsCompany.Data.Builders;
using LogisticsCompany.Data.Common;
using LogisticsCompany.Services.Offices.Dto;
using Microsoft.Data.SqlClient;

namespace LogisticsCompany.Services.Offices.Queries
{
    public class OfficeQueryService : BaseService, IOfficeQueryService
    {
        public OfficeQueryService(LogisticsCompanyContext dbContext)
            : base(dbContext)
        {

        }

        public async Task<int?> GetIdByName(string name)
        {
            var clauseContainer = new ClauseDescriptorContainer();

            clauseContainer.Descriptors(descriptors =>
            {
                descriptors.Add(descriptor => descriptor
                    .Field("Address")
                    .EqualityOperator(EqualityOperator.EQUALS)
                    .FieldValue(name)
                );
            });

            var query = new SqlQueryBuilder()
                .Select(columns: "Id")
                .From(table: "Offices")
                .Where(clauseContainer)
                .ToQuery();

            using (var connection = new SqlConnection(_connectionString))
            {
                var result = await connection.QueryFirstAsync<int?>(query);

                return result ?? null;
            }
        }

        public async Task<OfficeDto?> GetById(int id)
        {
            var clauseContainer = new ClauseDescriptorContainer();

            clauseContainer
                .Descriptors(descriptors =>
                {
                    descriptors.Add(descriptor => descriptor
                        .Field("Id")
                        .FieldValue(id)
                        .EqualityOperator(EqualityOperator.EQUALS)
                    );
                });

            using (var connection = new SqlConnection(this._connectionString))
            {
                var query = new SqlQueryBuilder()
                        .Select(columns: "*")
                        .Where(clauseContainer)
                        .From(table: "Offices")
                        .ToQuery();

                var office = await connection.QuerySingleOrDefaultAsync<OfficeDto>(query);

                return office;
            }
        }

        public async Task<OfficeDto?> GetByAddress(string name)
        {
            var clauseDescriptorContainer = new ClauseDescriptorContainer();

            clauseDescriptorContainer
               .Descriptors(descriptors =>
               {
                   descriptors.Add(descriptor => descriptor
                       .Field("Address")
                       .FieldValue(name)
                       .EqualityOperator(EqualityOperator.EQUALS)
                   );
               });

            var query = new SqlQueryBuilder()
                .Select(columns: "*")
                .From(table: "Offices")
                .Where(clauseDescriptorContainer)
                .ToQuery();

            using (var connection = new SqlConnection(_connectionString))
            {
                var office = await connection.QuerySingleOrDefaultAsync<OfficeDto>(query);
                return office;
            }
        }

        public async Task<IEnumerable<OfficeDto>> GetAll()
        {
            using (var connection = new SqlConnection(this._connectionString))
            {
                var query = new SqlQueryBuilder()
                    .Select(columns: "*")
                    .From(table: "Offices")
                    .ToQuery();

                var offices = await connection.QueryAsync<OfficeDto>(query);
                return offices;
            }
        }
    }
}
