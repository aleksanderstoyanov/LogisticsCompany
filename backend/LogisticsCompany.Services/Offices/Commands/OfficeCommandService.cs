using Dapper;
using LogisticsCompany.Data;
using LogisticsCompany.Data.Helpers;
using LogisticsCompany.Entity;
using LogisticsCompany.Services.Offices.Dto;
using LogisticsCompany.Services.Offices.Queries;
using Microsoft.Data.SqlClient;

namespace LogisticsCompany.Services.Offices.Commands
{
    public class OfficeCommandService : BaseService, IOfficeCommandService
    {
        private readonly IOfficeQueryService _officeQueryService;

        public OfficeCommandService(LogisticsCompanyContext dbContext, IOfficeQueryService officeQueryService)
            : base(dbContext)
        {
            _officeQueryService = officeQueryService;
        }

        public async Task<OfficeDto?> Create(OfficeDto dto)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = SqlCommandHelper.InsertCommand
                (
                    table: "Offices",
                    values: new[] { $"'{dto.Address}'", $"'{dto.PricePerWeight.ToString().Replace(",", ".")}'" }
                );

                await connection.ExecuteAsync(command);

                var office = await _officeQueryService.GetByAddress(dto.Address);

                return office ?? null;
            }
        }

        public async Task Update(OfficeDto dto)
        {
            var entityValues = new Dictionary<string, string>()
            {
                { "Address", dto.Address },
                { "PricePerWeight", $"{dto.PricePerWeight.ToString().Replace(",", ".")}"},
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
            var office = _officeQueryService.GetById(id);

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
