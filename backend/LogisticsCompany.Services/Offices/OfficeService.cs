using Dapper;
using LogisticsCompany.Data;
using LogisticsCompany.Data.Builders;
using LogisticsCompany.Data.Common;
using LogisticsCompany.Data.Helpers;
using LogisticsCompany.Entity;
using LogisticsCompany.Services.Contracts;
using LogisticsCompany.Services.Dto;
using Microsoft.Data.SqlClient;
using System.Data;

namespace LogisticsCompany.Services.Offices
{
    public class OfficeService : BaseService, IOfficeService
    {
        public OfficeService(LogisticsCompanyContext dbContext)
            : base(dbContext)
        {
        }

        public async Task<OfficeDto?> GetById(int id)
        {
            var clauseContainer = new ClauseDescriptorContainer()
            {
                ClauseDescriptors = new HashSet<ClauseDescriptor>()
                {
                    new ClauseDescriptor
                    {
                        Field = "Id",
                        EqualityOperator = EqualityOperator.EQUALS,
                        FieldValue = id,
                    }
                }
            };

            using (var connection = new SqlConnection(this._connectionString))
            {
                var query = new SqlQueryBuilder()
                        .Select(columns: "*")
                        .Where(clauseContainer)
                        .From(table: "Offices")
                        .GetQuery();

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
                    .GetQuery();

                var offices = await connection.QueryAsync<OfficeDto>(query);
                return offices;
            }
        }

        public async Task Update(OfficeDto dto)
        {
            var entityValues = new Dictionary<string, string>()
            {
                { "Address", dto.Address }
            };

            using (var connection = new SqlConnection(this._connectionString))
            {
                var command = SqlCommandHelper.UpdateCommand(
                    table: "Offices",
                    entityType: typeof(Office),
                    entityValues: entityValues,
                    dto.Id
                );

                await connection.ExecuteAsync(command);
            }
        }

        public async Task Delete(int id)
        {
            var office = GetById(id);

            if (office == null)
                return;

            using (var connection = new SqlConnection(_connectionString))
            {
                var command = SqlCommandHelper.DeleteCommand("Offices", "Id");

                await connection.ExecuteAsync(command, new { criteriaValue = id });
            }
        }

    }
}
