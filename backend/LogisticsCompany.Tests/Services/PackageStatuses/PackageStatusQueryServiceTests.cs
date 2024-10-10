using Autofac;
using Autofac.Core;
using Autofac.Extras.Moq;
using LogisticsCompany.Data;
using LogisticsCompany.Data.Contracts;
using LogisticsCompany.Data.Entity;
using LogisticsCompany.Services.PackageStatuses.Queries;
using Moq;
using Xunit;

namespace LogisticsCompany.Tests.Services.PackageStatuses
{
    public class PackageStatusQueryServiceTests : BaseServiceTest
    {
        public PackageStatusQueryServiceTests()
            : base()
        {
        }

        private IEnumerable<PackageStatus> GetPackageStatuses()
        {
            return new List<PackageStatus>()
            {
                new PackageStatus
                {
                    Id = 1,
                    Name = "NonRegistered"
                },
                new PackageStatus
                {
                    Id = 2,
                    Name = "Registered"
                },
                new PackageStatus
                {
                    Id = 3,
                    Name = "InDelivery"
                },
                new PackageStatus
                {
                    Id = 4,
                    Name = "Delivered"
                }

            };
        }

        [Theory]
        [InlineData("NonRegistered")]
        [InlineData("Registered")]
        [InlineData("InDelivery")]
        [InlineData("Delivered")]
        public async void GetIdByName_should_produce_valid_query_and_return_entity(string name)
        {
            using (var mock = AutoMock.GetLoose())
            {
                // Arrange
                var packageStatus = GetPackageStatuses()
                       .SingleOrDefault(package => package.Name == name)
                       .Id;

                var task = Task.FromResult(packageStatus);

                _dbAdapter
                    .Setup(method => method
                        .QuerySingle<int>($"SELECT Id FROM PackageStatuses \\r\\n WHERE Name = '{name}'")
                    )
                    .Returns(task);

                var dbContext = mock.Create<LogisticsCompanyContext>(_dbParameters);

                var serviceParameters = new Parameter[]
                {
                    new TypedParameter(typeof(LogisticsCompanyContext), dbContext),
                    new TypedParameter(typeof(IDbAdapter), _dbAdapter.Object)
                };

                var service = mock.Create<PackageStatusQueryService>(serviceParameters);

                // Act
                var result = await service.GetIdByName(name);

                // Assert
                _dbAdapter
                    .Verify(method => method
                        .QuerySingle<int>(
                            It.Is<string>(query => query == $"SELECT Id FROM PackageStatuses\r\n WHERE Name = '{name}'")
                        ),
                        Times.Once
                   );
            }
        }
    }
}
