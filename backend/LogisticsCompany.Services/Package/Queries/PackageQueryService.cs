using Dapper;
using LogisticsCompany.Data;
using LogisticsCompany.Data.Builders;
using LogisticsCompany.Data.Common;
using LogisticsCompany.Services.Package.Dto;
using Microsoft.Data.SqlClient;

namespace LogisticsCompany.Services.Package.Queries
{
    /// <summary>
    /// A <see cref="BaseService"/> class for performing Database query operations for Packages
    /// </summary>
    public class PackageQueryService : BaseService, IPackageQueryService
    {
        /// <summary>
        /// Creates a <see cref="PackageQueryService"/> with the 
        /// injected <paramref name="dbContext"/> argument.
        /// </summary>
        /// <param name="dbContext">The Database context.</param>
        public PackageQueryService(LogisticsCompanyContext dbContext) :
            base(dbContext)
        {

        }

        /// <summary>
        /// Performs a SQL Query for retrieving a package
        /// based on the passed <paramref name="id"/> argument.
        /// </summary>
        /// <param name="id">The User id which will be used for retrieving an existing entity.</param>
        /// <returns>
        /// <see cref="IEnumerable{PackageDto}"/> collection of packages.
        /// </returns>
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

        /// <summary>
        /// Performs a SQL Query for retrieving a package
        /// based on the passed <paramref name="id"/> argument.
        /// </summary>
        /// <param name="id">The id which will be used for retrieving an existing entity</param>
        /// <returns>
        /// <see cref="PackageDto"/>
        /// </returns>
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

        /// <summary>
        /// Performs an SQL Query for retrieving all packages.
        /// </summary>
        /// <returns>
        /// <see cref="IEnumerable{PackageDto}"/> collection of packages.
        /// </returns>
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

        /// <summary>
        /// Performs an SQL Query for retrieving received packages based on the 
        /// User <paramref name="id"/> argument.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>
        /// <see cref="IEnumerable{SentReceivedPackageDto}"/> collection of packages.
        /// </returns>
        public async Task<IEnumerable<SentReceivedPackageDto>> GetReceivedPackages(int id)
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

            var packageStatusClauseContainer = new ClauseDescriptorContainer();

            packageStatusClauseContainer
                .Descriptors(descriptors =>
                {
                    descriptors.Add(descriptor => descriptor
                        .Field("status.Id")
                        .FieldValue("package.PackageStatusId")
                        .EqualityOperator(EqualityOperator.EQUALS)
                    );
                });

            var toClauseContainer = new ClauseDescriptorContainer()
                .Descriptors(descriptors =>
                {
                    descriptors.Add(descriptor => descriptor
                        .Field("package.ToId")
                        .FieldValue("toUser.Id")
                        .EqualityOperator(EqualityOperator.EQUALS)
                    );
                });

            var fromClauseContainer = new ClauseDescriptorContainer()
                .Descriptors(descriptors =>
                {
                    descriptors.Add(descriptor => descriptor
                        .Field("package.FromId")
                        .FieldValue("fromUser.Id")
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
                        "CONCAT(toUser.FirstName,' ',toUser.LastName) AS ToUser",
                        "CONCAT(fromUser.FirstName, ' ', fromUser.LastName) AS FromUser",
                        "status.Name as PackageStatusName"
                    }
                )
                .From(table: "Packages", @as: "package")
                .Join
                (
                    joinOperator: JoinOperator.INNER,
                    table: "PackageStatuses",
                    container: packageStatusClauseContainer,
                    @as: "status"
                )
                .Join(
                    joinOperator: JoinOperator.INNER,
                    table: "Users",
                    container: toClauseContainer,
                    @as: "toUser"
                 )
                .Join(
                    joinOperator: JoinOperator.INNER,
                    table: "Users",
                    container: fromClauseContainer,
                    @as: "fromUser"
                )
                .Where(clauseDescriptorContainer)
                .ToQuery();

                var result = await connection.QueryAsync<SentReceivedPackageDto>(query);

                return result;
            }
        }

        /// <summary>
        /// Performs an SQL Query for retrieving sent packages based on the 
        /// User <paramref name="id"/> argument.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>
        /// <see cref="IEnumerable{SentReceivedPackageDto}"/> collection of packages.
        /// </returns>
        public async Task<IEnumerable<SentReceivedPackageDto>> GetSentPackages(int id)
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

            var packageStatusContainer = new ClauseDescriptorContainer();

            packageStatusContainer.Descriptors(descriptors =>
            {
                descriptors.Add(descriptor => descriptor
                    .Field("status.Id")
                    .FieldValue("package.PackageStatusId")
                    .EqualityOperator(EqualityOperator.EQUALS)
                );
            });

            var fromUserClauseContainer = new ClauseDescriptorContainer()
                .Descriptors(descriptors =>
                {
                    descriptors.Add(descriptor => descriptor
                        .Field("package.FromId")
                        .FieldValue("fromUser.Id")
                        .EqualityOperator(EqualityOperator.EQUALS)
                    );
                });

            var toUserClauseDescriptorContainer = new ClauseDescriptorContainer()
                .Descriptors(descriptors =>
                {
                    descriptors.Add(descriptor => descriptor
                        .Field("package.ToId")
                        .FieldValue("toUser.Id")
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
                        "CONCAT(fromUser.FirstName, ' ', fromUser.LastName) AS FromUser",
                        "CONCAT(toUser.FirstName, ' ', toUser.LastName) AS ToUser",
                        "status.Name as PackageStatusName"
                    }
                )
                .From(table: "Packages", @as: "package")
                .Join
                (
                    joinOperator: JoinOperator.INNER,
                    table: "PackageStatuses",
                    container: packageStatusContainer,
                    @as: "status"
                )
                .Join(
                    joinOperator: JoinOperator.INNER,
                    table: "Users",
                    container: fromUserClauseContainer,
                    @as: "fromUser"
                 )
                .Join(
                    joinOperator: JoinOperator.INNER,
                    table: "Users",
                    container: toUserClauseDescriptorContainer,
                    @as: "toUser"
                 )
                .Where(clauseDescriptorContainer)
                .ToQuery();

                var result = await connection.QueryAsync<SentReceivedPackageDto>(query);

                return result;
            }
        }

        /// <summary>
        /// Performs an SQL Query for getting the Count
        /// of Total Packages based on the <paramref name="from"/> and <paramref name="to"/>
        /// User ids.
        /// </summary>
        /// <param name="from">FromId Package argument.</param>
        /// <param name="to">ToId Package argument.</param>
        /// <returns>
        /// The Total Count of Packages.
        /// </returns>
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
