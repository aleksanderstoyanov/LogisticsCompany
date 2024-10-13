using Autofac;
using Autofac.Core;
using Autofac.Extras.Moq;
using LogisticsCompany.Data;
using LogisticsCompany.Data.Contracts;
using LogisticsCompany.Services.Package.Commands;
using LogisticsCompany.Services.Roles.Queries;
using LogisticsCompany.Services.Users.Commands;
using LogisticsCompany.Services.Users.Queries;
using LogisticsCompany.Tests.Common;
using Microsoft.Identity.Client;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace LogisticsCompany.Tests.Services.Users
{
    public class UserCommandServiceTests : BaseServiceTest
    {
        public UserCommandServiceTests()
            : base()
        {
        }

        [Fact]
        public async void Update_should_compose_valid_query()
        {
            using (var mock = AutoMock.GetLoose())
            {
                // Arrange 
                var command = "UPDATE Users\r\n\r\n\r\nSET Username = 'admin123', FirstName = 'FirstName1', LastName = 'LastName1', Email = 'admin@gmail.com', RoleId = '0', OfficeId = NULL\r\n\r\nWHERE Id = 1";

                _dbAdapter
                    .Setup(method => method
                        .ExecuteCommand(command, null)
                    );

                var dto = MockDataRepository.GetAllUsers()
                    .First();


                var task = Task.FromResult(1);
                var roleQueryService = mock.Mock<IRoleQueryService>();

                roleQueryService
                        .Setup(method => method
                            .GetIdByName(dto.OfficeName)
                        )
                        .Returns(task);

                var dbContext = mock.Create<LogisticsCompanyContext>(_dbParameters);

                var serviceParameters = new Parameter[]
                {
                    new TypedParameter(typeof(LogisticsCompanyContext), dbContext),
                    new TypedParameter(typeof(IRoleQueryService), roleQueryService.Object),
                    new TypedParameter(typeof(IDbAdapter), _dbAdapter.Object),
                };

                var service = mock.Create<UserCommandService>(serviceParameters);

                // Act
                await service.Update(dto);

                // Assert
                _dbAdapter
                    .Verify(method => method
                        .ExecuteCommand(It.Is<string>(argument => argument == command), null)
                    );
            }
        }

        [Fact]
        public async void Delete_should_compose_valid_query()
        {
            using (var mock = AutoMock.GetLoose())
            {
                // Arrange
                var command = "DELETE FROM Users WHERE Id = @criteriaValue";

                var dto = MockDataRepository.GetAllLogins()
                    .First();

                _dbAdapter
                    .Setup(method => method
                        .ExecuteCommand(command, new { criteriaValue = 1 })
                    );

                var task = Task.FromResult(dto);

                var queryService = mock.Mock<IUserQueryService>();

                queryService
                    .Setup(method => method.GetById(1))
                    .Returns(task);

                var dbContext = mock.Create<LogisticsCompanyContext>(_dbParameters);

                var serviceParameters = new Parameter[]
                {
                    new TypedParameter(typeof(LogisticsCompanyContext), dbContext),
                    new TypedParameter(typeof(IUserQueryService), queryService.Object),
                    new TypedParameter(typeof(IDbAdapter), _dbAdapter.Object),
                };

                var service = mock.Create<UserCommandService>(serviceParameters);

                // Act
                await service.Delete(dto.Id);

                // Assert
                _dbAdapter
                    .Verify(method => method
                        .ExecuteCommand(It.Is<string>(argument => argument == command), It.IsAny<object>()),
                        Times.Once
                    );
            }

        }
    }
}
