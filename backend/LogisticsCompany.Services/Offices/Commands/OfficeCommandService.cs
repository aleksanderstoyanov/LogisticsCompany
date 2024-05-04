using Dapper;
using LogisticsCompany.Data;
using LogisticsCompany.Data.Helpers;
using LogisticsCompany.Entity;
using LogisticsCompany.Services.Offices.Dto;
using LogisticsCompany.Services.Offices.Queries;
using Microsoft.Data.SqlClient;

namespace LogisticsCompany.Services.Offices.Commands
{
    /// <summary>
    /// A <see cref="BaseService"/> class used for performing DataBase command operations for Offices.
    /// </summary>
    public class OfficeCommandService : BaseService, IOfficeCommandService
    {
        private readonly IOfficeQueryService _officeQueryService;

        /// <summary>
        /// Creates an <see cref="OfficeCommandService"/> with the injected
        /// <paramref name="dbContext"/> and <paramref name="officeQueryService"/>
        /// arguments.
        /// </summary>
        /// <param name="dbContext">The Database Context.</param>
        /// <param name="officeQueryService">The Service used for performing Query operations for Offices.</param>
        public OfficeCommandService(LogisticsCompanyContext dbContext, IOfficeQueryService officeQueryService)
            : base(dbContext)
        {
            _officeQueryService = officeQueryService;
        }

        /// <summary>
        /// Creates a Office entity in the Database
        /// based on the passed <paramref name="dto"/>
        /// </summary>
        /// <param name="dto"></param>
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

        /// <summary>
        /// Updates an existing Office entity in the Database
        /// based on the passed <paramref name="dto"/>.
        /// </summary>
        /// <param name="dto"></param>
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

        /// <summary>
        /// Deletes an existing Office entity from the Database
        /// based on the passed <paramref name="id"/>
        /// </summary>
        /// <param name="id"></param>
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
