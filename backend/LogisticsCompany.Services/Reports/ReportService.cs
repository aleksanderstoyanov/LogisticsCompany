using Dapper;
using LogisticsCompany.Data;
using LogisticsCompany.Data.Builders;
using LogisticsCompany.Data.Common;
using LogisticsCompany.Services.Contracts;
using LogisticsCompany.Services.Dto;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;

namespace LogisticsCompany.Services.Reports
{
    public class ReportService : BaseService, IReportService
    {
        private readonly IUserService _userService;
        private readonly IPackageService _packageService;

        public ReportService(LogisticsCompanyContext dbContext,
            IUserService userService,
            IPackageService packageService)
            : base(dbContext)
        {
            this._userService = userService;
            this._packageService = packageService;
        }

        public async Task<IEnumerable<UserDto>> GetAllClients()
        {
            var users = await _userService.GetUsers();

            return users.Where(user => user.RoleName == "Client");
        }


        public async Task<IEnumerable<UserDto>> GetAllEmployees()
        {
            var users = await _userService.GetUsers();

            return users.Where(user => user.RoleName == "OfficeEmployee" || user.RoleName == "Courier");
        }

        public async Task<IEnumerable<PackageReportDto>> GetAllRegisteredPackages()
        {
            var fromUserClauseDescriptorContainer = new ClauseDescriptorContainer();

            fromUserClauseDescriptorContainer
                .Descriptors(descriptor =>
                {
                    descriptor.Add(descriptor => descriptor
                        .Field("fromUser.Id")
                        .FieldValue("package.FromId")
                        .EqualityOperator(EqualityOperator.EQUALS)
                    );
                });

            var toUserClauseDescriptorContainer = new ClauseDescriptorContainer();

            toUserClauseDescriptorContainer
                .Descriptors(descriptors =>
                {
                    descriptors.Add(descriptor => descriptor
                        .Field("toUser.Id")
                        .FieldValue("package.ToId")
                        .EqualityOperator(EqualityOperator.EQUALS)
                    );
                });

            var packageStatusClauseContainer = new ClauseDescriptorContainer();

            packageStatusClauseContainer
                .Descriptors(descriptors =>
                {
                    descriptors.Add(descriptor => descriptor
                        .Field("packageStatus.Id")
                        .FieldValue("package.PackageStatusId")
                        .EqualityOperator(EqualityOperator.EQUALS)
                    );
                });



            var query = new SqlQueryBuilder()
                .Select
                (
                    columns: new string[]
                    {
                        "package.Address",
                        "package.Weight",
                        "package.ToOffice",
                        "packageStatus.Name AS PackageStatusName",
                        "CONCAT(fromUser.FirstName, ' ', fromUser.LastName) AS FromUser",
                        "CONCAT(toUser.FirstName, ' ', toUser.LastName) AS toUser"
                    }
                )
                .From(table: "Packages", @as: "package")
                .Join
                (
                    table: "Users",
                    joinOperator: JoinOperator.INNER,
                    container: fromUserClauseDescriptorContainer,
                    @as: "fromUser"
                )
                .Join
                (
                    table: "Users",
                    joinOperator: JoinOperator.INNER,
                    container: toUserClauseDescriptorContainer,
                    @as: "toUser"
                )
                .Join
                (
                    table: "PackageStatuses",
                    joinOperator: JoinOperator.INNER,
                    container: packageStatusClauseContainer,
                    @as: "packageStatus"
                )
                .ToQuery();

            using (var connection = new SqlConnection(_connectionString))
            {
                var packages = await connection.QueryAsync<PackageReportDto>(query);

                return packages
                    .Where(package => package.PackageStatusName != "NonRegistered")
                    .ToList();
            }

        }

        public async Task<IEnumerable<PackageReportDto>> GetAllInDeliveryPackages()
        {
            var packages = await GetAllRegisteredPackages();

            return packages.Where(package => package.PackageStatusName == "InDelivery");
        }
    }
}
