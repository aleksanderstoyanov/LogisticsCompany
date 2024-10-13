using Autofac;
using Autofac.Core;
using Autofac.Extras.Moq;
using LogisticsCompany.Data;
using LogisticsCompany.Data.Contracts;
using LogisticsCompany.Services.Offices.Commands;
using LogisticsCompany.Services.Offices.Queries;
using LogisticsCompany.Tests.Common;
using Microsoft.Identity.Client;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace LogisticsCompany.Tests.Services.Offices
{
    public class OfficeCommandServiceTests: BaseServiceTest
    {
        public OfficeCommandServiceTests():
            base()
        {
        }

        [Fact]
        public async void Create_should_compose_a_valid_query_and_return_entity()
        {
            using (var mock = AutoMock.GetLoose())
            {
                // Arrange
                var command = "INSERT INTO Offices VALUES\r\n(\r\n'ul.Geo Milev', '0')";

                var dto = MockDataRepository
                    .GetOffices()
                    .First();

                var task = Task.FromResult(dto);

                _dbAdapter
                    .Setup(method => method
                        .ExecuteCommand(command, null)
                    );

                var dbContext = mock.Create<LogisticsCompanyContext>(_dbParameters);

                var queryService = mock.Mock<IOfficeQueryService>();

                    queryService.Setup(method => method.GetByAddress(dto.Address))
                    .Returns(task);

                var serviceParameters = new Parameter[]
                {
                    new TypedParameter(typeof(LogisticsCompanyContext), dbContext),
                    new TypedParameter(typeof(IOfficeQueryService), queryService.Object),
                    new TypedParameter(typeof(IDbAdapter), _dbAdapter.Object)
                };


                var service = mock.Create<OfficeCommandService>(serviceParameters);

                // Act
                var result = await service.Create(dto);

                // Assert
                _dbAdapter
                    .Verify(method => method
                        .ExecuteCommand(It.Is<string>(argument => argument == command), null),
                        Times.Once
                    );

                Assert.NotNull(result);
                Assert.Equal(dto, result);
            }
        }

        [Fact]
        public async void Update_should_compose_valid_query_and_return_entity()
        {
            using (var mock = AutoMock.GetLoose())
            {
                // Arrange
                var command = "UPDATE Offices\r\n\r\n\r\nSET Address = 'ul.Geo Milev', PricePerWeight = '0'\r\n\r\nWHERE Id = 1";

                var dto = MockDataRepository.GetOffices()
                    .First();

                _dbAdapter
                    .Setup(method => method
                        .ExecuteCommand(command, null)
                    );

                var task = Task.FromResult(dto);

                var dbContext = mock.Create<LogisticsCompanyContext>(_dbParameters);

                var queryService = mock.Mock<IOfficeQueryService>();

                queryService
                    .Setup(method => method
                        .GetById(dto.Id)
                    )
                    .Returns(task);

                var serviceParamaters = new Parameter[]
                {
                    new TypedParameter(typeof(LogisticsCompanyContext), dbContext),
                    new TypedParameter(typeof(IOfficeQueryService), queryService.Object),
                    new TypedParameter(typeof(IDbAdapter), _dbAdapter.Object),
                };

                var service = mock.Create<OfficeCommandService>(serviceParamaters);

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
        public async void Delete_should_compose_valid_query_and_return_entity()
        {
            using (var mock = AutoMock.GetLoose())
            {
                // Arrange
                var command = "DELETE FROM Offices WHERE Id = @criteriaValue";
                
                var dto = MockDataRepository.GetOffices()
                    .First();
                
                object? criteria = new { criteriaValue = 1 };

                var task = Task.FromResult(dto);

                _dbAdapter
                    .Setup(method => method
                        .ExecuteCommand(command, criteria)
                    );

                var queryService = mock.Mock<IOfficeQueryService>();

                queryService
                        .Setup(method => method
                            .GetById(1)
                        )
                        .Returns(task);

                var dbContext = mock.Create<LogisticsCompanyContext>(_dbParameters);

                var serviceParameters = new Parameter[]
                {
                    new TypedParameter(typeof(LogisticsCompanyContext), dbContext),
                    new TypedParameter(typeof(IOfficeQueryService), queryService.Object),
                    new TypedParameter(typeof(IDbAdapter), _dbAdapter.Object),
                };

                var service = mock.Create<OfficeCommandService>(serviceParameters);

                // Act
                await service.Delete(dto.Id);

                // Arrange
                _dbAdapter
                    .Verify(method => method
                        .ExecuteCommand(It.Is<string>(argument => argument == command), It.IsAny<object?>()),
                        Times.Once
                    );
            }
        }
    }
}
