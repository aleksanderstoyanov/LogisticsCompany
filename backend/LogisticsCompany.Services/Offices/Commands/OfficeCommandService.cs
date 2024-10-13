using Dapper;
using LogisticsCompany.Data;
using LogisticsCompany.Data.Common.Adapters;
using LogisticsCompany.Data.Contracts;
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
        /// <paramref name="dbContext"/>, <paramref name="dbAdapter"/> and <paramref name="officeQueryService"/>
        /// arguments.
        /// </summary>
        /// <param name="dbContext">The Database Context.</param>
        /// <param name="officeQueryService">The Service used for performing Query operations for Offices.</param>
        /// <param name="dbAdapter">The DataBase adapter that will instantiate a connection and process the constructed command.</param>
        public OfficeCommandService(LogisticsCompanyContext dbContext,
            IOfficeQueryService officeQueryService,
            IDbAdapter adapter)
            : base(dbContext, adapter)
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
            var entityValues = new[] { $"'{dto.Address}'", $"'{dto.PricePerWeight.ToString().Replace(",", ".")}'" };

            var command = SqlCommandHelper.InsertCommand("Offices", entityValues);

            await _dbAdapter
                .ExecuteCommand(command);

            var office = await _officeQueryService
                .GetByAddress(dto.Address);

            return office ?? null;
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

            var office = await _officeQueryService.GetById(dto.Id);

            if (office == null)
                return;

            var command = SqlCommandHelper.UpdateCommand(
                   table: "Offices",
                   entityType: typeof(Office),
                   entityValues: entityValues,
                   dto.Id
               );

            await _dbAdapter
                .ExecuteCommand(command);
        }

        /// <summary>
        /// Deletes an existing Office entity from the Database
        /// based on the passed <paramref name="id"/>
        /// </summary>
        /// <param name="id"></param>
        public async Task Delete(int id)
        {
            var office = await _officeQueryService.GetById(id);

            if (office == null)
                return;

            var command = SqlCommandHelper.DeleteCommand("Offices", "Id");
            
            await _dbAdapter
                .ExecuteCommand(command, new { criteriaValue = id });
        }
    }
}
