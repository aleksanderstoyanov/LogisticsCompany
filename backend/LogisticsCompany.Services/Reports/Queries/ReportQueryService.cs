﻿using LogisticsCompany.Data.Builders;
using LogisticsCompany.Data.Common;
using LogisticsCompany.Data.Entity;
using LogisticsCompany.Data;
using Microsoft.Data.SqlClient;
using LogisticsCompany.Services.Users.Queries;
using Dapper;
using LogisticsCompany.Services.Reports.Dto;
using LogisticsCompany.Services.Users.Dto;

namespace LogisticsCompany.Services.Reports.Queries
{
    public class ReportQueryService : BaseService, IReportQueryService
    {
        private readonly IUserQueryService _userQueryService;
        public ReportQueryService(LogisticsCompanyContext dbContext, IUserQueryService userQueryService)
            : base(dbContext)
        {
            _userQueryService = userQueryService;
        }
        public async Task<IEnumerable<UserDto>> GetAllClients()
        {
            var users = await _userQueryService.GetUsers();

            return users.Where(user => user.RoleName == "Client");
        }

        public async Task<IEnumerable<UserDto>> GetAllEmployees()
        {
            var users = await _userQueryService.GetUsers();

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

        public async Task<decimal> GetIncomeForPeriod(DateTime startPeriod, DateTime endPeriod)
        {
            var startPeriodParsed = DateOnly.FromDateTime(startPeriod);
            var endPeriodParsed = DateOnly.FromDateTime(endPeriod);

            var officeClauseContainer = new ClauseDescriptorContainer()
                .Descriptors(descriptors =>
                {
                    descriptors.Add(descriptor => descriptor
                        .Field("package.OfficeId")
                        .FieldValue("office.Id")
                        .EqualityOperator(EqualityOperator.EQUALS)
                    );
                });

            var deliveryClauseContainer = new ClauseDescriptorContainer()
                .Descriptors(descriptors =>
                {
                    descriptors.Add(descriptor => descriptor
                        .Field("package.DeliveryId")
                        .FieldValue("delivery.Id")
                        .EqualityOperator(EqualityOperator.EQUALS)
                    );
                });

            var clauseDescriptorContaier = new ClauseDescriptorContainer()
                .Descriptors(descriptors =>
                {
                    descriptors.Add(descriptor => descriptor
                        .Field("delivery.EndDate")
                        .FieldValue($"{startPeriodParsed.Year}-{startPeriodParsed.Month}-{startPeriodParsed.Day}")
                        .EqualityOperator(EqualityOperator.GREATER_THAN_AND_EQUALS)
                        .LogicalOperator(LogicalOperator.AND)
                    );

                    descriptors.Add(descriptor => descriptor
                        .Field("delivery.EndDate")
                        .FieldValue($"{endPeriodParsed.Year}-{endPeriodParsed.Month}-{endPeriodParsed.Day}")
                        .EqualityOperator(EqualityOperator.LESSER_THAN_AND_EQUALS)
                    );

                });

            using (var connection = new SqlConnection(_connectionString))
            {
                var query = new SqlQueryBuilder()
                    .Select(columns: "SUM(office.PricePerWeight * package.Weight) AS TotalPrice")
                    .From(table: "Packages", @as: "package")
                    .Join
                    (
                        joinOperator: JoinOperator.INNER,
                        table: "Offices",
                        container: officeClauseContainer,
                        @as: "office"
                    )
                    .Join(
                        joinOperator: JoinOperator.INNER,
                        table: "Deliveries",
                        container: deliveryClauseContainer,
                        @as: "delivery"
                    )
                    .Where(clauseDescriptorContaier)
                    .ToQuery();


                var result = await connection.QuerySingleOrDefaultAsync<IncomeAggregateModel>(query);

                return result.TotalPrice;
            }
        }
    }
}