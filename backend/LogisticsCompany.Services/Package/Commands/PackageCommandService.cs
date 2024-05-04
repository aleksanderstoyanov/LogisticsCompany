using Dapper;
using LogisticsCompany.Data;
using LogisticsCompany.Services.Package.Dto;
using LogisticsCompany.Services.Package.Queries;
using LogisticsCompany.Services.PackageStatuses.Queries;
using Microsoft.Data.SqlClient;
using static LogisticsCompany.Data.Helpers.SqlCommandHelper;


namespace LogisticsCompany.Services.Package.Commands
{
    public class PackageCommandService : BaseService, IPackageCommandService
    {
        private readonly IPackageStatusQueryService _packageStatusQueryService;
        private readonly IPackageQueryService _packageQueryService;
        public PackageCommandService(LogisticsCompanyContext dbContext,
            IPackageQueryService packageQueryService,
            IPackageStatusQueryService packageStatusQueryService)
            : base(dbContext)
        {
            _packageQueryService = packageQueryService;
            _packageStatusQueryService = packageStatusQueryService;
        }

        public async Task Create(PackageDto dto)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = InsertCommand(
                    table: "Packages",
                    values: new string[]
                    {
                        dto.FromId != null ? dto.FromId.ToString() : "NULL",
                        dto.ToId != null ? dto.ToId.ToString() : "NULL",
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

        public async Task Update(PackageDto dto)
        {
            var packageStatusId = await _packageStatusQueryService.GetIdByName(dto.PackageStatusName);

            var keyValuePairs = new Dictionary<string, string>()
            {
                {"FromId", dto.FromId != null ?  dto.FromId.ToString() : "NULL" },
                {"ToId", dto.ToId != null ?  dto.ToId.ToString() : "NULL"},
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
