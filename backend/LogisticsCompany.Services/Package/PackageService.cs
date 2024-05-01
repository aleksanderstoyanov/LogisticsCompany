using Dapper;
using LogisticsCompany.Data;
using LogisticsCompany.Data.Builders;
using LogisticsCompany.Data.Common;
using LogisticsCompany.Data.Helpers;
using LogisticsCompany.Services.Contracts;
using LogisticsCompany.Services.Dto;
using Microsoft.Data.SqlClient;
using System.Data;
using static LogisticsCompany.Data.Helpers.SqlCommandHelper;

namespace LogisticsCompany.Services.Packages
{
    public class PackageService : BaseService, IPackageService
    {
        private readonly IPackageStatusService _packageStatusService;
        private readonly IOfficeService _officeService;

        public PackageService(LogisticsCompanyContext dbContext, IPackageStatusService packageStatusService, IOfficeService officeService) :
            base(dbContext)
        {
            this._packageStatusService = packageStatusService;
            this._officeService = officeService;
        }

        public async Task Create(PackageDto dto)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = SqlCommandHelper.InsertCommand(
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
            var packageStatusId = await _packageStatusService.GetIdByName(dto.PackageStatusName);

            var keyValuePairs = new Dictionary<string, string>()
            {
                {"FromId", dto.FromId != null ?  dto.FromId.ToString() : "NULL" },
                {"ToId", dto.ToId != null ?  dto.ToId.ToString() : "NULL"},
                {"DeliveryId", dto.DeliveryId != null ?  dto.DeliveryId.ToString() : "NULL"},
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
            var package = GetById(id);

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


        public async Task<IEnumerable<PackageDto>> GetPackagesByUserId(int id)
        {
            var clauseDescriptorContainer = new ClauseDescriptorContainer();

            clauseDescriptorContainer
                .Descriptors(descriptors =>
                {
                    descriptors.Add(descriptor => descriptor
                        .Field("FromId")
                        .FieldValue(id)
                        .EqualityOperator(EqualityOperator.EQUALS)
                        .LogicalOperator(LogicalOperator.OR)
                    );

                    descriptors.Add(descriptor => descriptor
                        .Field("ToId")
                        .FieldValue(id)
                        .EqualityOperator(EqualityOperator.EQUALS)
                    );
                });

            using (var connection = new SqlConnection(_connectionString))
            {
                var query = new SqlQueryBuilder()
                    .Select(columns: "*")
                    .From(table: "Packages")
                    .Where(clauseDescriptorContainer)
                    .ToQuery();

                var result = await connection.QueryAsync<PackageDto>(query);

                return result;
            }


        }

        public async Task<PackageDto?> GetById(int id)
        {
            var clauseDescriptorContainer = new ClauseDescriptorContainer();

            clauseDescriptorContainer
                .Descriptors(descriptors =>
                {
                    descriptors.Add(descriptor => descriptor
                        .Field("package.Id")
                        .FieldValue(id)
                        .EqualityOperator(EqualityOperator.EQUALS)
                    );
                });

            var joinClauseDescriptorContainer = new ClauseDescriptorContainer();

            joinClauseDescriptorContainer.Descriptors(descriptors =>
            {
                descriptors.Add(descriptor => descriptor
                    .Field("status.Id")
                    .FieldValue("package.PackageStatusId")
                    .EqualityOperator(EqualityOperator.EQUALS)
                );
            });

            using (var connection = new SqlConnection(_connectionString))
            {
                var query = new SqlQueryBuilder()
                   .Select(
                        columns:
                        new string[]
                        {
                            "package.Id",
                            "package.Address",
                            "package.FromId",
                            "package.ToId",
                            "package.DeliveryId",
                            "package.OfficeId",
                            "package.Weight",
                            "package.PackageStatusId",
                            "status.Name as PackageStatusName"
                        }
                     )
                    .From(table: "Packages", @as: "package")
                    .Join(
                        joinOperator: JoinOperator.INNER,
                        table: "PackageStatuses",
                        container: joinClauseDescriptorContainer,
                        @as: "status"
                    )
                    .Where(clauseDescriptorContainer)
                    .ToQuery();

                var result = await connection.QuerySingleOrDefaultAsync<PackageDto>(query);

                return result;
            }

        }

        public async Task<IEnumerable<PackageDto>> GetAll()
        {
            var clauseContainerDescriptor = new ClauseDescriptorContainer();

            clauseContainerDescriptor
                .Descriptors(descriptors =>
                {
                    descriptors.Add(descriptor => descriptor
                            .Field("package.PackageStatusId")
                            .FieldValue("status.Id")
                            .EqualityOperator(EqualityOperator.EQUALS)
                    );
                });

            var query = new SqlQueryBuilder()
                .Select(
                    columns:
                    new string[]
                    {
                        "package.Id",
                        "package.Address",
                        "package.FromId",
                        "package.DeliveryId",
                        "package.OfficeId",
                        "package.ToId",
                        "package.Weight",
                        "package.PackageStatusId",
                        "status.Name as PackageStatusName"
                    }
                )
                .From(table: "Packages", @as: "package")
                .Join
                (
                    joinOperator: JoinOperator.INNER,
                    table: "PackageStatuses",
                    container: clauseContainerDescriptor,
                    @as: "status"
                )
                .ToQuery();

            using (var connection = new SqlConnection(_connectionString))
            {
                var result = await connection.QueryAsync<PackageDto>(query);
                return result;
            }
        }

        public async Task<IEnumerable<PackageDto>> GetReceivedPackages(int id)
        {
            var clauseDescriptorContainer = new ClauseDescriptorContainer();

            clauseDescriptorContainer
                .Descriptors(descriptors =>
                {
                    descriptors.Add(descriptor => descriptor
                        .Field("ToId")
                        .FieldValue(id)
                        .EqualityOperator(EqualityOperator.EQUALS)
                    );
                });

            var joinClauseDescriptorContainer = new ClauseDescriptorContainer();

            joinClauseDescriptorContainer
                .Descriptors(descriptors =>
                {
                    descriptors.Add(descriptor => descriptor
                        .Field("status.Id")
                        .FieldValue("package.PackageStatusId")
                        .EqualityOperator(EqualityOperator.EQUALS)
                    );
                });

            using (var connection = new SqlConnection(_connectionString))
            {
                var query = new SqlQueryBuilder()
                .Select(
                    columns:
                    new string[]
                    {
                        "package.Id",
                        "package.Address",
                        "package.Weight",
                        "package.ToOffice",
                        "status.Name as PackageStatusName"
                    }
                )
                .From(table: "Packages", @as: "package")
                .Join
                (
                    joinOperator: JoinOperator.INNER,
                    table: "PackageStatuses",
                    container: joinClauseDescriptorContainer,
                    @as: "status"
                )
                .Where(clauseDescriptorContainer)
                .ToQuery();

                var result = await connection.QueryAsync<PackageDto>(query);

                return result;
            }
        }

        public async Task<IEnumerable<PackageDto>> GetSentPackages(int id)
        {
            var clauseDescriptorContainer = new ClauseDescriptorContainer();

            clauseDescriptorContainer.Descriptors(descriptors =>
            {
                descriptors.Add(descriptor => descriptor
                    .Field("FromId")
                    .FieldValue(id)
                    .EqualityOperator(EqualityOperator.EQUALS)
                );
            });

            var joinClauseDescriptorContainer = new ClauseDescriptorContainer();

            joinClauseDescriptorContainer.Descriptors(descriptors =>
            {
                descriptors.Add(descriptor => descriptor
                    .Field("status.Id")
                    .FieldValue("package.PackageStatusId")
                    .EqualityOperator(EqualityOperator.EQUALS)
                );
            });

            using (var connection = new SqlConnection(_connectionString))
            {
                var query = new SqlQueryBuilder()
                .Select(
                    columns:
                    new string[]
                    {
                        "package.Id",
                        "package.Address",
                        "package.Weight",
                        "package.ToOffice",
                        "status.Name as PackageStatusName"
                    }
                )
                .From(table: "Packages", @as: "package")
                .Join
                (
                    joinOperator: JoinOperator.INNER,
                    table: "PackageStatuses",
                    container: joinClauseDescriptorContainer,
                    @as: "status"
                )
                .Where(clauseDescriptorContainer)
                .ToQuery();

                var result = await connection.QueryAsync<PackageDto>(query);

                return result;
            }
        }

        public async Task<int> GetPackageCountByFromAndTo(int from, int to)
        {
            var clauseDescriptorContainer = new ClauseDescriptorContainer();

            clauseDescriptorContainer.Descriptors(descriptors =>
            {
                descriptors.Add(descriptor => descriptor
                    .Field("ToId")
                    .FieldValue(to)
                    .EqualityOperator(EqualityOperator.EQUALS)
                    .LogicalOperator(LogicalOperator.AND)
                );

                descriptors.Add(descriptor => descriptor
                    .Field("FromId")
                    .FieldValue(from)
                    .EqualityOperator(EqualityOperator.EQUALS)
                );
            });

            var query = new SqlQueryBuilder()
                .Select(columns: "COUNT(Id)")
                .From(table: "Packages")
                .Where(clauseDescriptorContainer)
                .ToQuery();

            using (var connection = new SqlConnection(_connectionString))
            {
                var result = await connection.QuerySingleAsync<int>(query);

                return result;
            }
        }

    }
}
