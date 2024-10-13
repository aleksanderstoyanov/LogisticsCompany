using Autofac;
using Autofac.Core;
using Autofac.Extras.Moq;
using LogisticsCompany.Data;
using LogisticsCompany.Data.Contracts;
using LogisticsCompany.Services.Deliveries.Commands;
using LogisticsCompany.Services.Deliveries.Queries;
using LogisticsCompany.Services.Dto;
using LogisticsCompany.Services.Package.Dto;
using LogisticsCompany.Services.Package.Queries;
using LogisticsCompany.Tests.Common;
using Microsoft.Identity.Client;
using Moq;
using Xunit;

namespace LogisticsCompany.Tests.Services.Deliveries
{
    public class DeliveryCommandServiceTests : BaseServiceTest
    {

        public DeliveryCommandServiceTests()
            : base()
        {
        }

        [Theory]
        [InlineData(new int[] { 1 })]
        public async void Create_should_compose_a_valid_command_and_return_success_message(int[] selectedIds)
        {
            using (var mock = AutoMock.GetLoose())
            {
                // Arrange 
                var command = "INSERT INTO Deliveries VALUES\r\n(\r\n'2024-10-13', NULL)SELECT CAST(SCOPE_IDENTITY() as int)";
                var query = $"SELECT package.Id, package.Address, package.FromId, package.ToId, package.DeliveryId, package.OfficeId, package.Weight, package.PackageStatusId, status.Name as PackageStatusName FROM Packages AS package \r\n\r\nINNER JOIN PackageStatuses AS status ON status.Id = package.PackageStatusId  \r\n WHERE package.Id = '1'";

                var dto = MockDataRepository.GetAllDeliveries()
                    .First();

                _dbAdapter
                    .Setup(method => method
                        .QuerySingle<PackageDto>(query)
                    );

                _dbAdapter.Setup(method => method
                        .QuerySingle<int>(command)
                    );

                var dbContext = mock.Create<LogisticsCompanyContext>(_dbParameters);
                var task = Task.FromResult(MockDataRepository.GetAllPackages().First());

                var packageQueryService = mock.Mock<IPackageQueryService>();

                packageQueryService.Setup(setup => setup.GetById(selectedIds[0]))
                    .Returns(task);

                var serviceParamaters = new Parameter[]
                {
                     new TypedParameter(typeof(LogisticsCompanyContext), dbContext),
                     new TypedParameter(typeof(IPackageQueryService), packageQueryService.Object),
                     new TypedParameter(typeof(IDbAdapter), _dbAdapter.Object),
                };

                var service = mock.Create<DeliveryCommandService>(serviceParamaters);

                // Act
                var result = await service.Create(dto);

                // Assert
                _dbAdapter
                    .Verify(method => method
                        .QuerySingle<int>(It.Is<string>(argument => argument == command)),
                        Times.Once
                    );

                var message = $"Delivery has begun for: {string.Join(", ", dto.SelectedIds)}\r\n";
                Assert.Equal(message, result);
            }
        }

        [Fact]
        public async void Update_should_compose_a_valid_command()
        {
            using (var mock = AutoMock.GetLoose())
            {
                // Arrange
                var command = "UPDATE Deliveries\r\n\r\n\r\nSET EndDate = '2024-10-13'\r\n\r\nWHERE Id = 1";

                var dto = MockDataRepository.GetAllDeliveries()
                    .First();

                _dbAdapter
                    .Setup(method => method
                        .ExecuteCommand(command, null)
                    );

                var dbContext = mock.Create<LogisticsCompanyContext>(_dbParameters);

                var task = Task.FromResult(dto);

                var queryService = mock.Mock<IDeliveryQueryService>();

                    queryService.Setup(method => method.GetById(1))
                    .Returns(task);

                var serviceParameters = new Parameter[]
                {
                     new TypedParameter(typeof(LogisticsCompanyContext), dbContext),
                     new TypedParameter(typeof(IDeliveryQueryService), queryService.Object),
                     new TypedParameter(typeof(IDbAdapter), _dbAdapter.Object),
                };

                var service = mock.Create<DeliveryCommandService>(serviceParameters);
                
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
    }
}
