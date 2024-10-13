using Autofac;
using Autofac.Core;
using Autofac.Extras.Moq;
using LogisticsCompany.Data;
using LogisticsCompany.Data.Contracts;
using LogisticsCompany.Data.Entity;
using LogisticsCompany.Services.Reports.Dto;
using LogisticsCompany.Services.Reports.Queries;
using LogisticsCompany.Services.Users.Dto;
using LogisticsCompany.Services.Users.Queries;
using LogisticsCompany.Tests.Common;
using Moq;
using System.Reflection;
using Xunit;

namespace LogisticsCompany.Tests.Services.Reports
{
    public class ReportQueryServiceTests : BaseServiceTest
    {
        public ReportQueryServiceTests()
            : base()
        {
        }

        [Fact]
        public async void GetAllClient_should_compose_valid_query_and_return_entities()
        {
            using (var mock = AutoMock.GetLoose())
            {
                // Arrange
                var query = "SELECT u.Id, u.Username, u.FirstName, u.LastName, u.Email, r.Name AS RoleName, o.Address AS OfficeName FROM Users AS u \r\n\r\nINNER JOIN Roles AS r ON r.Id = u.RoleId  \r\n\r\nLEFT JOIN Offices AS o ON o.Id = u.OfficeId";

                var users = MockDataRepository.GetAllUsers()
                    .Where(user => user.RoleName == "Client");

                var task = Task.FromResult(users);

                _dbAdapter
                    .Setup(method => method
                        .QueryAll<UserDto>(query)
                    )
                    .Returns(task);

                var dbContext = mock.Create<LogisticsCompanyContext>(_dbParameters);

                var adapterParameter = new TypedParameter(typeof(LogisticsCompanyContext), _dbAdapter.Object);

                var serviceParameters = new Parameter[]
                {
                    new TypedParameter(typeof(LogisticsCompanyContext), dbContext),
                    adapterParameter
                };


                var userService = mock.Create<UserQueryService>(serviceParameters);


                serviceParameters = new Parameter[]
                {
                    new TypedParameter(typeof(LogisticsCompanyContext), dbContext),
                    new TypedParameter(typeof(IUserQueryService), userService),
                    adapterParameter
                };

                var service = mock.Create<ReportQueryService>(serviceParameters.ToArray());

                // Act

                var result = await service.GetAllClients();

                // Assert
                var mocks = mock.MockRepository.GetType().GetProperties(BindingFlags.Instance | BindingFlags.NonPublic)
                    .SingleOrDefault(property => property.Name == "Mocks");

                var value = mocks.GetValue(mock.MockRepository) as IEnumerable<Mock>;
               
                Mock<IDbAdapter> adapter = (Mock<IDbAdapter>)value
                    .SingleOrDefault(mock => mock.GetType() == typeof(Mock<IDbAdapter>));    
                
                adapter.Verify(method => method
                    .QueryAll<UserDto>(It.Is<string>(argument => argument == query))
                );

            }
        }

        [Fact]
        public async void GetAllEmployees_should_compose_valid_query_and_return_entities()
        {
            using (var mock = AutoMock.GetLoose())
            {
                // Arrange
                var query = "SELECT u.Id, u.Username, u.FirstName, u.LastName, u.Email, r.Name AS RoleName, o.Address AS OfficeName FROM Users AS u \r\n\r\nINNER JOIN Roles AS r ON r.Id = u.RoleId  \r\n\r\nLEFT JOIN Offices AS o ON o.Id = u.OfficeId";

                var users = MockDataRepository.GetAllUsers()
                    .Where(user => user.RoleName == "Client");

                var task = Task.FromResult(users);

                _dbAdapter
                    .Setup(method => method
                        .QueryAll<UserDto>(query)
                    )
                    .Returns(task);

                var dbContext = mock.Create<LogisticsCompanyContext>(_dbParameters);

                var adapterParameter = new TypedParameter(typeof(LogisticsCompanyContext), _dbAdapter.Object);

                var serviceParameters = new Parameter[]
                {
                    new TypedParameter(typeof(LogisticsCompanyContext), dbContext),
                    adapterParameter
                };


                var userService = mock.Create<UserQueryService>(serviceParameters);


                serviceParameters = new Parameter[]
                {
                    new TypedParameter(typeof(LogisticsCompanyContext), dbContext),
                    new TypedParameter(typeof(IUserQueryService), userService),
                    adapterParameter
                };

                var service = mock.Create<ReportQueryService>(serviceParameters.ToArray());

                // Act

                var result = await service.GetAllEmployees();

                // Assert
                var mocks = mock.MockRepository.GetType().GetProperties(BindingFlags.Instance | BindingFlags.NonPublic)
                    .SingleOrDefault(property => property.Name == "Mocks");

                var value = mocks.GetValue(mock.MockRepository) as IEnumerable<Mock>;

                Mock<IDbAdapter> adapter = (Mock<IDbAdapter>)value
                    .SingleOrDefault(mock => mock.GetType() == typeof(Mock<IDbAdapter>));

                adapter.Verify(method => method
                    .QueryAll<UserDto>(It.Is<string>(argument => argument == query))
                );

            }
        }

        [Fact]
        public async void GetAllRegisteredPackages_should_compose_valid_query_and_return_entities()
        {
            using (var mock = AutoMock.GetLoose())
            {
                // Arrange
                var query = "SELECT package.Address, package.Weight, package.ToOffice, packageStatus.Name AS PackageStatusName, CONCAT(fromUser.FirstName, ' ', fromUser.LastName) AS FromUser, CONCAT(toUser.FirstName, ' ', toUser.LastName) AS toUser FROM Packages AS package \r\n\r\nINNER JOIN Users AS fromUser ON fromUser.Id = package.FromId  \r\n\r\nINNER JOIN Users AS toUser ON toUser.Id = package.ToId  \r\n\r\nINNER JOIN PackageStatuses AS packageStatus ON packageStatus.Id = package.PackageStatusId";

                var registeredPackages = MockDataRepository.GetAllPackageReports()
                    .Where(report => report.PackageStatusName != "NonRegistered")
                    .AsEnumerable();

                var task = Task.FromResult(registeredPackages);

                _dbAdapter
                    .Setup(method => method
                        .QueryAll<PackageReportDto>(query)
                    )
                    .Returns(task);

                var dbContext = mock.Create<LogisticsCompanyContext>(_dbParameters);

                var serviceParameters = new Parameter[]
                {
                        new TypedParameter(typeof(LogisticsCompanyContext), dbContext),
                        new TypedParameter(typeof(IDbAdapter), _dbAdapter.Object)
                };

                var service = mock.Create<ReportQueryService>(serviceParameters.ToArray());

                // Act
                var result = await service.GetAllRegisteredPackages();

                // Assert
                _dbAdapter
                    .Verify(method => method
                        .QueryAll<PackageReportDto>(It.Is<string>(argument => argument == query))
                    );

                Assert.NotNull(result);
                Assert.NotEmpty(result);
                Assert.Equal(registeredPackages.Count(), result.Count());
            }
        }

        [Fact]
        public async void GetAllInDeliveryPackages_should_compose_valid_query_and_return_entities()
        {
            using (var mock = AutoMock.GetLoose())
            {
                // Arrange
                var query = "SELECT package.Address, package.Weight, package.ToOffice, packageStatus.Name AS PackageStatusName, CONCAT(fromUser.FirstName, ' ', fromUser.LastName) AS FromUser, CONCAT(toUser.FirstName, ' ', toUser.LastName) AS toUser FROM Packages AS package \r\n\r\nINNER JOIN Users AS fromUser ON fromUser.Id = package.FromId  \r\n\r\nINNER JOIN Users AS toUser ON toUser.Id = package.ToId  \r\n\r\nINNER JOIN PackageStatuses AS packageStatus ON packageStatus.Id = package.PackageStatusId";

                var registeredPackages = MockDataRepository.GetAllPackageReports()
                    .Where(report => report.PackageStatusName == "InDelivery")
                    .AsEnumerable();

                var task = Task.FromResult(registeredPackages);

                _dbAdapter
                    .Setup(method => method
                        .QueryAll<PackageReportDto>(query)
                    )
                    .Returns(task);

                var dbContext = mock.Create<LogisticsCompanyContext>(_dbParameters);

                var serviceParameters = new Parameter[]
                {
                        new TypedParameter(typeof(LogisticsCompanyContext), dbContext),
                        new TypedParameter(typeof(IDbAdapter), _dbAdapter.Object)
                };

                var service = mock.Create<ReportQueryService>(serviceParameters.ToArray());

                // Act
                var result = await service.GetAllRegisteredPackages();

                // Assert
                _dbAdapter
                    .Verify(method => method
                        .QueryAll<PackageReportDto>(It.Is<string>(argument => argument == query))
                    );

                Assert.NotNull(result);
                Assert.NotEmpty(result);
                Assert.Equal(registeredPackages.Count(), result.Count());
            }
        }

        [Fact]
        public async void GetIncomeForPeriod_should_compose_valid_query_and_return_total_price()
        {
            using (var mock = AutoMock.GetLoose())
            {
                // Arrange
                var startPeriodParsed = DateOnly.FromDateTime(DateTime.Now);
                var endPeriodParsed = DateOnly.FromDateTime(DateTime.Now.AddDays(1));

                var query = $"SELECT SUM(office.PricePerWeight * package.Weight) AS TotalPrice FROM Packages AS package \r\n\r\nINNER JOIN Offices AS office ON package.OfficeId = office.Id  \r\n\r\nINNER JOIN Deliveries AS delivery ON package.DeliveryId = delivery.Id  \r\n WHERE delivery.EndDate >= '{startPeriodParsed.Year}-{startPeriodParsed.Month}-{startPeriodParsed.Day}' AND delivery.EndDate <= '{endPeriodParsed.Year}-{endPeriodParsed.Month}-{endPeriodParsed.Day}'";

                var income = MockDataRepository.GetIncome();

                var task = Task.FromResult(income);

                _dbAdapter
                    .Setup(method => method
                        .QuerySingle<IncomeAggregateModel>(query)
                    )
                    .Returns(task);

                var dbContext = mock.Create<LogisticsCompanyContext>(_dbParameters);

                var serviceParameters = new Parameter[]
                {
                         new TypedParameter(typeof(LogisticsCompanyContext), dbContext),
                         new TypedParameter(typeof(IDbAdapter), _dbAdapter.Object)
                };

                var service = mock.Create<ReportQueryService>(serviceParameters.ToArray());

                // Act
                var result = await service.GetIncomeForPeriod(DateTime.Now, DateTime.Now.AddDays(1));

                // Assert

                _dbAdapter
                    .Verify(setup => setup
                        .QuerySingle<IncomeAggregateModel>(It.Is<string>(argument => argument == query))
                    );

                Assert.NotNull(result);
                Assert.Equal(income.TotalPrice, result);
            }
        }
    }
}
