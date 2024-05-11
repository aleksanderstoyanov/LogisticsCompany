using Dapper;
using LogisticsCompany.Data;
using LogisticsCompany.Services.Package.Dto;
using LogisticsCompany.Services.Package.Queries;
using LogisticsCompany.Services.PackageStatuses.Queries;
using Microsoft.Data.SqlClient;
using static LogisticsCompany.Data.Helpers.SqlCommandHelper;


namespace LogisticsCompany.Services.Package.Commands
{
    /// <summary>
    /// A <see cref="BaseService"/> class for performing Database command operations for Packages.
    /// </summary>
    public class PackageCommandService : BaseService, IPackageCommandService
    {
        private readonly IPackageStatusQueryService _packageStatusQueryService;
        private readonly IPackageQueryService _packageQueryService;

        /// <summary>
        /// Creates an <see cref="PackageCommandService"/> instance 
        /// with the injected <paramref name="dbContext"/>, <paramref name="packageQueryService"/>,
        /// and <paramref name="packageStatusQueryService"/> arguments.
        /// </summary>
        /// <param name="dbContext">The Database Context</param>
        /// <param name="packageQueryService">The Service used for performing Query operations for Packages.</param>
        /// <param name="packageStatusQueryService">The Service used for performing Query operations for PackageStatuses.</param>
        public PackageCommandService(LogisticsCompanyContext dbContext,
            IPackageQueryService packageQueryService,
            IPackageStatusQueryService packageStatusQueryService)
            : base(dbContext)
        {
            _packageQueryService = packageQueryService;
            _packageStatusQueryService = packageStatusQueryService;
        }

        /// <summary>
        /// Creates a new Package entity in the Database
        /// by using the <paramref name="dto"/> argument.
        /// </summary>
        /// <param name="dto">The DTO object containing the fields for creation.</param>
        public async Task Create(PackageDto dto)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = InsertCommand(
                    table: "Packages",
                    values: new string[]
                    {
                        dto.FromId != null && dto.FromId != 0 ? dto.FromId.ToString() : "NULL",
                        dto.ToId != null && dto.ToId != 0 ? dto.ToId.ToString() : "NULL",
                        "1", // PackageStatusID
                        dto.OfficeId != null ? dto.OfficeId.ToString() : "NULL",
                        "NULL", // DeliveryId
                        $"'{dto.Address}'",
                        dto.ToOffice ? "1" : "0",
                        dto.Weight.ToString().Replace(",", ".")
                    }
                );

                await connection.ExecuteAsync(command);
            }
        }

        /// <summary>
        /// Updates an existing Package entity
        /// by using the values passed from the <paramref name="dto"/>.
        /// </summary>
        /// <param name="dto">The DTO object containing the fields for update.</param>
        public async Task Update(PackageDto dto)
        {
            var packageStatusId = await _packageStatusQueryService.GetIdByName(dto.PackageStatusName);

            var keyValuePairs = new Dictionary<string, string>()
            {
                {"FromId", dto.FromId != null && dto.FromId != 0 ?  dto.FromId.ToString() : "NULL" },
                {"ToId", dto.ToId != null && dto.ToId != 0 ?  dto.ToId.ToString() : "NULL"},
                {"DeliveryId", dto.DeliveryId != null && dto.DeliveryId != 0 ?  dto.DeliveryId.ToString() : "NULL"},
                {"Address", $"{dto.Address}"},
                {"PackageStatusId", packageStatusId != null ? packageStatusId.Value.ToString() :  "NULL"},
                {"ToOffice", dto.ToOffice ? "1": "0"},
                {"Weight", dto.Weight.ToString()},
            };

            using (var connection = new SqlConnection(_connectionString))
            {
                var command = UpdateCommand
                (
                    table: "Packages",
                    entityType: typeof(Data.Entity.Package),
                    entityValues: keyValuePairs,
                    primaryKey: dto.Id
                );

                await connection.ExecuteAsync(command);
            }
        }

        /// <summary>
        /// Deletes an existing record 
        /// by using the passed <paramref name="id"/>
        /// </summary>
        /// <param name="id">The id argument which will be used for deleting an existing package.</param>
        public async Task Delete(int id)
        {
            var package = _packageQueryService.GetById(id);

            if (package == null)
            {
                return;
            }

            using (var connection = new SqlConnection(_connectionString))
            {
                var command = DeleteCommand(table: "Packages", primaryKey: "Id");
                await connection.ExecuteAsync(command, new { criteriaValue = id });
            }
        }
    }
}
