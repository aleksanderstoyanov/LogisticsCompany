using Autofac;
using Autofac.Core;
using Autofac.Extras.Moq;
using LogisticsCompany.Data;
using LogisticsCompany.Data.Contracts;
using LogisticsCompany.Services.Offices.Dto;
using LogisticsCompany.Services.Offices.Queries;
using LogisticsCompany.Tests.Common;
using Moq;
using System.Net;
using Xunit;

namespace LogisticsCompany.Tests.Services.Offices
{
    public class OfficeQueryServiceTests : BaseServiceTest
    {
        public OfficeQueryServiceTests()
            : base()
        {
        }

        [Theory]
        [InlineData("ul.Geo Milev")]
        [InlineData("ul.Shipchenski Prohod")]
        public async void GetIdByName_should_compose_valid_query_and_return_id(string name)
        {
            using (var mock = AutoMock.GetLoose())
            {
                // Arrange
                var id = MockDataRepository.GetOffices()
                    .SingleOrDefault(office => office.Address == name)
                    .Id;

                var task = Task.FromResult<int?>(id);

                _dbAdapter
                    .Setup(method => method
                        .QuerySingle<int?>($"SELECT Id FROM Offices\r\n WHERE Address = '{name}'")
                    )
                    .Returns(task);

                var dbContext = mock.Create<LogisticsCompanyContext>(_dbParameters);

                var serviceParamaters = new Parameter[]
                {
                    new TypedParameter(typeof(LogisticsCompanyContext), dbContext),
                    new TypedParameter(typeof(IDbAdapter), _dbAdapter.Object)
                };

                var service = mock.Create<OfficeQueryService>(serviceParamaters);

                // Act
                var result = await service.GetIdByName(name);

                // Arrange
                _dbAdapter
                    .Verify(method => method
                        .QuerySingle<int?>(It.Is<string>(query => query == $"SELECT Id FROM Offices\r\n WHERE Address = '{name}'")),
                        Times.Once
                    );

                Assert.NotNull(result);
            }
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async void GetById_should_compose_valid_query_and_return_entity(int identifier)
        {
            using (var mock = AutoMock.GetLoose())
            {
                // Arrange
                var office = MockDataRepository.GetOffices()
                    .SingleOrDefault(office => office.Id == identifier);

                var task = Task.FromResult<OfficeDto>(office);

                _dbAdapter
                    .Setup(method => method
                        .QuerySingle<OfficeDto>($"SELECT * FROM Offices\r\n WHERE Id = '{identifier}'")
                    )
                    .Returns(task);

                var dbContext = mock.Create<LogisticsCompanyContext>(_dbParameters);

                var serviceParamaters = new Parameter[]
                {
                    new TypedParameter(typeof(LogisticsCompanyContext), dbContext),
                    new TypedParameter(typeof(IDbAdapter), _dbAdapter.Object)
                };

                var service = mock.Create<OfficeQueryService>(serviceParamaters);

                // Act
                var result = await service.GetById(identifier);

                // Arrange
                _dbAdapter
                    .Verify(method => method
                        .QuerySingle<OfficeDto>(It.Is<string>(query => query == $"SELECT * FROM Offices\r\n WHERE Id = '{identifier}'")),
                        Times.Once
                    );

                Assert.NotNull(result);
            }
        }

        [Theory]
        [InlineData("ul.Geo Milev")]
        [InlineData("ul.Shipchenski Prohod")]
        public async void GetByAddress_should_compose_valid_query_and_return_entity(string address)
        {
            using (var mock = AutoMock.GetLoose())
            {
                // Arrange
                var office = MockDataRepository.GetOffices()
                    .SingleOrDefault(office => office.Address == address);

                var task = Task.FromResult<OfficeDto>(office);

                _dbAdapter
                    .Setup(method => method
                        .QuerySingle<OfficeDto>($"SELECT * FROM Offices\r\n WHERE Address = '{address}'")
                    )
                    .Returns(task);

                var dbContext = mock.Create<LogisticsCompanyContext>(_dbParameters);

                var serviceParamaters = new Parameter[]
                {
                    new TypedParameter(typeof(LogisticsCompanyContext), dbContext),
                    new TypedParameter(typeof(IDbAdapter), _dbAdapter.Object)
                };

                var service = mock.Create<OfficeQueryService>(serviceParamaters);

                // Act
                var result = await service.GetByAddress(address);

                // Assert
                _dbAdapter
                    .Verify(method => method
                        .QuerySingle<OfficeDto>(It.Is<string>(query => query == $"SELECT * FROM Offices\r\n WHERE Address = '{address}'")),
                        Times.Once
                    );

                Assert.NotNull(result);
            }
        }

        [Fact]
        public async void GetAll_should_compose_valid_query_and_return_entities()
        {
            using (var mock = AutoMock.GetLoose())
            {
                // Arrange
                var offices = MockDataRepository.GetOffices();

                var task = Task.FromResult(offices);

                _dbAdapter
                    .Setup(method => method
                        .QueryAll<OfficeDto>($"SELECT * FROM Offices")
                    )
                    .Returns(task);

                var dbContext = mock.Create<LogisticsCompanyContext>(_dbParameters);

                var serviceParamaters = new Parameter[]
                {
                    new TypedParameter(typeof(LogisticsCompanyContext), dbContext),
                    new TypedParameter(typeof(IDbAdapter), _dbAdapter.Object)
                };

                var service = mock.Create<OfficeQueryService>(serviceParamaters);

                // Act
                var result = await service.GetAll();

                // Assert
                _dbAdapter
                    .Verify(method => method
                        .QueryAll<OfficeDto>(It.Is<string>(query => query == $"SELECT * FROM Offices")),
                        Times.Once
                    );

                Assert.NotEmpty(result);
            }
        }
    }
}
