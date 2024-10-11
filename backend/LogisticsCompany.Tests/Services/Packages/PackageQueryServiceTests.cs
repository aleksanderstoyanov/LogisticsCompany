using Autofac;
using Autofac.Core;
using Autofac.Extras.Moq;
using LogisticsCompany.Data;
using LogisticsCompany.Data.Contracts;
using LogisticsCompany.Services.Package.Dto;
using LogisticsCompany.Services.Package.Queries;
using LogisticsCompany.Tests.Common;
using Moq;
using Xunit;

namespace LogisticsCompany.Tests.Services.Packages
{
    public class PackageQueryServiceTests : BaseServiceTest
    {
        public PackageQueryServiceTests()
            : base()
        {
        }

        [Fact]
        public async void GetAll_should_produce_valid_query_and_return_entities()
        {
            using (var mock = AutoMock.GetLoose())
            {
                // Arrange
                var query = "SELECT package.Id, package.Address, package.FromId, package.DeliveryId, package.OfficeId, package.ToId, package.Weight, package.PackageStatusId, status.Name as PackageStatusName FROM Packages AS package \r\n\r\nINNER JOIN PackageStatuses AS status ON package.PackageStatusId = status.Id";
                var packages = MockDataRepository.GetAllPackages();
                var task = Task.FromResult(packages);

                _dbAdapter
                    .Setup(method => method
                        .QueryAll<PackageDto>(query)
                    )
                    .Returns(task);

                var dbContext = mock.Create<LogisticsCompanyContext>(_dbParameters);

                var serviceParameters = new Parameter[]
                {
                    new TypedParameter(typeof(LogisticsCompanyContext), dbContext),
                    new TypedParameter(typeof(IDbAdapter), _dbAdapter.Object),
                };

                var service = mock.Create<PackageQueryService>(serviceParameters);

                // Act
                var result = await service.GetAll();

                // Assert
                _dbAdapter
                    .Verify(method => method
                        .QueryAll<PackageDto>(It.Is<string>(argument => argument == query)),
                        Times.Once
                    );

                Assert.NotEmpty(result);
            }
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async void GetPackagesByUserId_should_compose_valid_query_and_return_entity(int id)
        {
            using (var mock = AutoMock.GetLoose())
            {
                // Arrange
                var query = $"SELECT * FROM Packages\r\n WHERE FromId = '{id}' OR ToId = '{id}'";

                var packages = MockDataRepository.GetAllPackages()
                       .Where(package => package.FromId == id || package.ToId == id)
                       .AsEnumerable();

                var task = Task.FromResult(packages);

                _dbAdapter
                    .Setup(method => method
                        .QueryAll<PackageDto>(query)
                    )
                    .Returns(task);

                var dbContext = mock.Create<LogisticsCompanyContext>(_dbParameters);

                var serviceParamaters = new Parameter[]
                {
                    new TypedParameter(typeof(LogisticsCompanyContext), dbContext),
                    new TypedParameter(typeof(IDbAdapter), _dbAdapter.Object)
                };

                var service = mock.Create<PackageQueryService>(serviceParamaters);

                // Act
                var result = await service.GetPackagesByUserId(id);

                // Assert
                _dbAdapter
                    .Verify(method => method
                        .QueryAll<PackageDto>(It.Is<string>(argument => argument == query)),
                        Times.Once
                    );

                Assert.NotEmpty(result);
                Assert.Equal(packages.Count(), result.Count());
            }
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async void GetReceivedPackages_should_compose_valid_query_and_return_entities(int id)
        {
            using (var mock = AutoMock.GetLoose())
            {
                // Arrange
                var query = $"SELECT package.Id, package.Address, package.Weight, package.ToOffice, CONCAT(toUser.FirstName,' ',toUser.LastName) AS ToUser, CONCAT(fromUser.FirstName, ' ', fromUser.LastName) AS FromUser, status.Name as PackageStatusName FROM Packages AS package \r\n\r\nINNER JOIN PackageStatuses AS status ON status.Id = package.PackageStatusId  \r\n\r\nINNER JOIN Users AS toUser ON package.ToId = toUser.Id  \r\n\r\nINNER JOIN Users AS fromUser ON package.FromId = fromUser.Id  \r\n WHERE ToId = '{id}'";

                var packages = MockDataRepository.GetAllSentReceivedPackages();
                
                var task = Task.FromResult(packages);

                _dbAdapter
                    .Setup(setup => setup
                        .QueryAll<SentReceivedPackageDto>(query)
                    )
                    .Returns(task);

                var dbContext = mock.Create<LogisticsCompanyContext>(_dbParameters);


                var serviceParamaters = new Parameter[]
                {
                    new TypedParameter(typeof(LogisticsCompanyContext), dbContext),
                    new TypedParameter(typeof(IDbAdapter), _dbAdapter.Object)
                };

                var service = mock.Create<PackageQueryService>(serviceParamaters);

                // Act
                var result = await service.GetReceivedPackages(id);

                // Assert
                _dbAdapter
                    .Verify(method => method
                        .QueryAll<SentReceivedPackageDto>(It.Is<string>(argument => argument == query))
                        , Times.Once
                    );

                Assert.NotEmpty(result);
            }
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async void GetSentPackages_should_compose_valid_query_and_return_entities(int id)
        {
            using (var mock = AutoMock.GetLoose())
            {
                // Arrange
                var query = $"SELECT package.Id, package.Address, package.Weight, package.ToOffice, CONCAT(fromUser.FirstName, ' ', fromUser.LastName) AS FromUser, CONCAT(toUser.FirstName, ' ', toUser.LastName) AS ToUser, status.Name as PackageStatusName FROM Packages AS package \r\n\r\nINNER JOIN PackageStatuses AS status ON status.Id = package.PackageStatusId  \r\n\r\nINNER JOIN Users AS fromUser ON package.FromId = fromUser.Id  \r\n\r\nINNER JOIN Users AS toUser ON package.ToId = toUser.Id  \r\n WHERE FromId = '{id}'";

                var packages = MockDataRepository.GetAllSentReceivedPackages();

                var task = Task.FromResult(packages);

                _dbAdapter
                    .Setup(setup => setup
                        .QueryAll<SentReceivedPackageDto>(query)
                    )
                    .Returns(task);

                var dbContext = mock.Create<LogisticsCompanyContext>(_dbParameters);


                var serviceParamaters = new Parameter[]
                {
                    new TypedParameter(typeof(LogisticsCompanyContext), dbContext),
                    new TypedParameter(typeof(IDbAdapter), _dbAdapter.Object)
                };

                var service = mock.Create<PackageQueryService>(serviceParamaters);

                // Act
                var result = await service.GetSentPackages(id);

                // Assert
                _dbAdapter
                    .Verify(method => method
                        .QueryAll<SentReceivedPackageDto>(It.Is<string>(argument => argument == query))
                        , Times.Once
                    );

                Assert.NotEmpty(result);
            }
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async void GetById_should_compose_valid_query_and_return_entity(int id)
        {
            using (var mock = AutoMock.GetLoose())
            {
                // Arrange
                var query = $"SELECT package.Id, package.Address, package.FromId, package.ToId, package.DeliveryId, package.OfficeId, package.Weight, package.PackageStatusId, status.Name as PackageStatusName FROM Packages AS package \r\n\r\nINNER JOIN PackageStatuses AS status ON status.Id = package.PackageStatusId  \r\n WHERE package.Id = '{id}'";

                var package = MockDataRepository.GetAllPackages()
                    .SingleOrDefault(package => package.Id == id);

                var task = Task.FromResult(package);

                _dbAdapter
                    .Setup(method => method
                        .QuerySingle<PackageDto>(query)
                    )
                    .Returns(task);

                var dbContext = mock.Create<LogisticsCompanyContext>(_dbParameters);

                var serviceParamaters = new Parameter[]
                {
                     new TypedParameter(typeof(LogisticsCompanyContext), dbContext),
                     new TypedParameter(typeof(IDbAdapter), _dbAdapter.Object)
                };

                var service = mock.Create<PackageQueryService>(serviceParamaters);

                // Act
                var result = await service.GetById(id);

                // Assert
                _dbAdapter
                    .Verify(method => method
                        .QuerySingle<PackageDto>(It.Is<string>(argument => argument == query))
                    );

                Assert.NotNull(package);
                Assert.Equal(package, result);
            }
        }

        [Theory]
        [InlineData(2, 1)]
        [InlineData(2, 2)]
        public async void GetPackageCount_should_compose_valid_query_and_return_count(int from, int to)
        {
            using (var mock = AutoMock.GetLoose())
            {
                // Arrange
                var query = $"SELECT COUNT(Id) FROM Packages\r\n WHERE ToId = '{to}' AND FromId = '{from}'";

                var count = MockDataRepository.GetAllPackages()
                    .Where(package => package.FromId == from && package.ToId == to)
                    .Count();

                var task = Task.FromResult(count);

                _dbAdapter
                   .Setup(method => method
                       .QuerySingle<int>(query)
                   )
                   .Returns(task);

                var dbContext = mock.Create<LogisticsCompanyContext>(_dbParameters);

                var serviceParamaters = new Parameter[]
                {
                    new TypedParameter(typeof(LogisticsCompanyContext), dbContext),
                    new TypedParameter(typeof(IDbAdapter), _dbAdapter.Object)
                };

                var service = mock.Create<PackageQueryService>(serviceParamaters);

                // Act
                var result = await service.GetPackageCountByFromAndTo(from, to);

                // Assert
                _dbAdapter
                    .Verify(method => method
                        .QuerySingle<int>(It.Is<string>(argument => argument == query)),
                        Times.Once
                    );

                Assert.Equal(count, result);
            }
        }
    }
}
