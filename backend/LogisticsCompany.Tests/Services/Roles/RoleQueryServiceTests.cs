using Autofac;
using Autofac.Core;
using Autofac.Extras.Moq;
using LogisticsCompany.Data;
using LogisticsCompany.Data.Contracts;
using LogisticsCompany.Entity;
using LogisticsCompany.Services.Roles.Queries;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace LogisticsCompany.Tests.Services.Roles
{
    public class RoleQueryServiceTests: BaseServiceTest
    {
        public RoleQueryServiceTests()
            :base()
        {
        }

        private IEnumerable<Role> Get_Roles()
        {
            return new List<Role>()
            {
                new Role
                {
                    Id = 1,
                    Name = "None"
                },
                new Role
                {
                    Id = 2,
                    Name = "OfficeEmployee"
                },
                new Role
                {
                    Id = 3,
                    Name = "Courier"
                },
                new Role
                {
                    Id = 4,
                    Name = "Client"
                },
                new Role
                {
                    Id = 5,
                    Name = "Admin"
                }
            };
        }

        [Theory]
        [InlineData("None")]
        [InlineData("OfficeEmployee")]
        [InlineData("Courier")]
        [InlineData("Client")]
        [InlineData("Admin")]
        public async void GetIdByName_should_produce_valid_query_and_return_entry(string name)
        {
            using (var mock = AutoMock.GetLoose())
            {
                // Arrange
                var role = Get_Roles()
                    .SingleOrDefault(role => role.Name == name)
                    .Id;

                var task = Task.FromResult(role);

                _dbAdapter
                    .Setup(method => method
                        .QuerySingle<int>($"SELECT Id FROM Roles\r\n WHERE Name = '{name}'")
                    )
                    .Returns(task);

                var dbContext = mock.Create<LogisticsCompanyContext>(_dbParameters);

                var serviceParamaters = new Parameter[2]
                {
                    new TypedParameter(typeof(LogisticsCompanyContext), dbContext),
                    new TypedParameter(typeof(IDbAdapter), _dbAdapter.Object)
                };

                var service = mock.Create<RoleQueryService>(serviceParamaters);

                // Act
                var id = await service.GetIdByName(name);

                // Assert
                _dbAdapter.Verify(method => method
                    .QuerySingle<int>(
                        It.Is<string>(query => query == $"SELECT Id FROM Roles\r\n WHERE Name = '{name}'")
                    ),
                    Times.Once
                );
            }
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        public async void GetRoleNameById_should_produce_valid_query_and_return_entry(int identifier)
        {
            using (var mock = AutoMock.GetLoose())
            {
                // Arrange
                var role = Get_Roles()
                    .SingleOrDefault(role => role.Id == identifier)
                    .Name;

                var task = Task.FromResult(role);

                _dbAdapter
                    .Setup(method => method
                        .QuerySingle<string>($"SELECT Name FROM Roles\r\n WHERE Id = '{identifier}'")
                    )
                    .Returns(task);

                var dbContext = mock.Create<LogisticsCompanyContext>(_dbParameters);

                var serviceParamaters = new Parameter[2]
                {
                    new TypedParameter(typeof(LogisticsCompanyContext), dbContext),
                    new TypedParameter(typeof(IDbAdapter), _dbAdapter.Object)
                };

                var service = mock.Create<RoleQueryService>(serviceParamaters);

                // Act
                var id = await service.GetRoleNameById(identifier);

                // Assert
                _dbAdapter.Verify(method => method
                    .QuerySingle<string>(
                        It.Is<string>(sql => sql == $"SELECT Name FROM Roles\r\n WHERE Id = '{identifier}'")
                    ),
                    Times.Once
                );
            }
        }
    }
}
