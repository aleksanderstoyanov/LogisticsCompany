using Autofac;
using Autofac.Core;
using Autofac.Extras.Moq;
using LogisticsCompany.Data;
using LogisticsCompany.Data.Contracts;
using LogisticsCompany.Services.Package.Commands;
using LogisticsCompany.Services.Package.Queries;
using LogisticsCompany.Services.PackageStatuses.Queries;
using LogisticsCompany.Tests.Common;
using Moq;
using Xunit;

namespace LogisticsCompany.Tests.Services.Packages
{
    public class PackageCommandServiceTests : BaseServiceTest
    {
        public PackageCommandServiceTests()
            : base()
        {
        }

        [Fact]
        public async void Create_should_compose_valid_query()
        {
            using (var mock = AutoMock.GetLoose())
            {
                // Arrange
                var command = "INSERT INTO Packages VALUES\r\n(\r\n2, 2, 1, 1, NULL, 'ul.Geo Milev', 1, 125)";

                var dto = MockDataRepository.GetAllPackages()
                    .First();

                _dbAdapter
                    .Setup(method => method.ExecuteCommand(command, null));


                var dbContext = mock.Create<LogisticsCompanyContext>(_dbParameters);

                var serviceParameters = new Parameter[]
                {
                    new TypedParameter(typeof(LogisticsCompanyContext), dbContext),
                    new TypedParameter(typeof(IDbAdapter), _dbAdapter.Object),

                };

                var service = mock.Create<PackageCommandService>(serviceParameters);

                // Act
                await service.Create(dto);

                // Assert
                _dbAdapter
                    .Verify(method => method
                        .ExecuteCommand(It.Is<string>(argument => argument == command), null),
                        Times.Once
                    );
            }
        }

        [Fact]
        public async void Update_should_compose_valid_query()
        {
            using (var mock = AutoMock.GetLoose())
            {
                // Arrange
                var command = "UPDATE Packages\r\n\r\n\r\nSET FromId = '2', ToId = '2', DeliveryId = NULL, PackageStatusId = '1', Address = 'ul.Geo Milev', ToOffice = '1', Weight = '125'\r\n\r\nWHERE Id = 1";

                var dto = MockDataRepository.GetAllPackages()
                    .First();

                _dbAdapter
                    .Setup(method => method.ExecuteCommand(command, null));

                var queryService = mock.Mock<IPackageStatusQueryService>();

                int? id = 1;
                var task = Task.FromResult(id);

                queryService
                    .Setup(method => method.GetIdByName(dto.PackageStatusName))
                    .Returns(task);

                var dbContext = mock.Create<LogisticsCompanyContext>(_dbParameters);

                var serviceParameters = new Parameter[]
                {
                    new TypedParameter(typeof(LogisticsCompanyContext), dbContext),
                    new TypedParameter(typeof(IPackageStatusQueryService), queryService.Object),
                    new TypedParameter(typeof(IDbAdapter), _dbAdapter.Object),
                };

                var service = mock.Create<PackageCommandService>(serviceParameters);

                // Act
                await service.Update(dto);

                // Assert
                _dbAdapter
                    .Verify(method => method
                        .ExecuteCommand(It.Is<string>(argument => argument == command), null),
                        Times.Once
                    );
            }
        }

        [Fact]
        public async void Delete_should_compose_valid_query()
        {
            using (var mock = AutoMock.GetLoose())
            {
                // Arrange
                var command = "DELETE FROM Packages WHERE Id = @criteriaValue";

                var dto = MockDataRepository.GetAllPackages()
                    .First();

                var task = Task.FromResult(dto);

                _dbAdapter
                    .Setup(method => method.ExecuteCommand(command, null));

                var queryService = mock.Mock<IPackageQueryService>();

                queryService
                    .Setup(method => method.GetById(dto.Id))
                    .Returns(task);

                var dbContext = mock.Create<LogisticsCompanyContext>(_dbParameters);

                var serviceParameters = new Parameter[]
                {
                    new TypedParameter(typeof(LogisticsCompanyContext), dbContext),
                    new TypedParameter(typeof(IPackageQueryService), queryService.Object),
                    new TypedParameter(typeof(IDbAdapter), _dbAdapter.Object)
                };

                var service = mock.Create<PackageCommandService>(serviceParameters);

                // Act
                await service.Delete(dto.Id);

                // Assert
                _dbAdapter
                    .Verify(method => method
                        .ExecuteCommand(It.Is<string>(argument => argument == command), null),
                        Times.Once
                    );
            }
        }
    }
}
