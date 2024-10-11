using Autofac;
using Autofac.Core;
using Autofac.Extras.Moq;
using LogisticsCompany.Data;
using LogisticsCompany.Data.Contracts;
using LogisticsCompany.Services.Authorization.Dto;
using LogisticsCompany.Services.Users.Dto;
using LogisticsCompany.Services.Users.Queries;
using LogisticsCompany.Tests.Common;
using Moq;
using Xunit;

namespace LogisticsCompany.Tests.Services.Users
{
    public class UserQueryServiceTests : BaseServiceTest
    {
        public UserQueryServiceTests() :
            base()
        {
        }

        [Fact]
        public async void GetUsers_should_compose_valid_query_and_return_entities()
        {
            using (var mock = AutoMock.GetLoose())
            {
                // Arrange
                var query = "SELECT u.Id, u.Username, u.FirstName, u.LastName, u.Email, r.Name AS RoleName, o.Address AS OfficeName FROM Users AS u \r\n\r\nINNER JOIN Roles AS r ON r.Id = u.RoleId  \r\n\r\nLEFT JOIN Offices AS o ON o.Id = u.OfficeId";

                var users = MockDataRepository.GetAllUsers()
                    .Where(user => user.RoleName != "Admin");

                var task = Task.FromResult(users);

                _dbAdapter
                    .Setup(method => method
                        .QueryAll<UserDto>(query)
                    )
                    .Returns(task);

                var dbContext = mock.Create<LogisticsCompanyContext>(_dbParameters);

                var serviceParameters = new Parameter[]
                {
                    new TypedParameter(typeof(LogisticsCompanyContext), dbContext),
                    new TypedParameter(typeof(IDbAdapter), _dbAdapter.Object)
                };

                var service = mock.Create<UserQueryService>(serviceParameters);

                // Act
                var result = await service.GetUsers();

                // Assert
                _dbAdapter
                    .Verify(method => method
                        .QueryAll<UserDto>(It.Is<string>(argument => argument == query)),
                        Times.Once
                    );

                Assert.NotNull(result);
                Assert.NotEmpty(result);
            }
        }

        [Theory]
        [InlineData(1, "Client")]
        public async void GetDifferentUsersFromCurrent_should_compose_valid_query_and_return_entities(int id, string role)
        {
            using (var mock = AutoMock.GetLoose())
            {
                // Arrange
                var query = $"SELECT u.Id, u.FirstName, u.LastName, r.Name as RoleName, u.Username, u.Email FROM Users AS u \r\n\r\nINNER JOIN Roles AS r ON r.Id = u.RoleId  \r\n WHERE u.Id != '1' AND u.Username != 'admin' AND r.Name = '{role}'";

                var users = MockDataRepository.GetAllUsers()
                    .Where(user => user.Id != id && user.RoleName == role);

                var task = Task.FromResult(users);

                _dbAdapter
                    .Setup(method => method
                        .QueryAll<UserDto>(query)
                    )
                    .Returns(task);

                var dbContext = mock.Create<LogisticsCompanyContext>(_dbParameters);

                var serviceParameters = new Parameter[]
                {
                    new TypedParameter(typeof(LogisticsCompanyContext), dbContext),
                    new TypedParameter(typeof(IDbAdapter), _dbAdapter.Object)
                };

                var service = mock.Create<UserQueryService>(serviceParameters);

                // Act
                var result = await service.GetDifferentUsersFromCurrent(id, role);

                // Assert
                _dbAdapter
                    .Verify(method => method
                        .QueryAll<UserDto>(It.Is<string>(argument => argument == query)),
                        Times.Once
                    );

                Assert.NotNull(result);
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
                var query = $"SELECT * FROM Users\r\n WHERE Id = '{id}'";

                var login = MockDataRepository.GetAllLogins()
                    .SingleOrDefault(login => login.Id == id);

                var task = Task.FromResult(login);

                _dbAdapter
                    .Setup(method => method.QuerySingle<LoginDto>(query))
                    .Returns(task);

                var dbContext = mock.Create<LogisticsCompanyContext>(_dbParameters);

                var serviceParameters = new Parameter[]
                {
                      new TypedParameter(typeof(LogisticsCompanyContext), dbContext),
                      new TypedParameter(typeof(IDbAdapter), _dbAdapter.Object)
                };

                var service = mock.Create<UserQueryService>(serviceParameters);

                // Act
                var result = await service.GetById(id);

                // Assert
                _dbAdapter
                    .Verify(method => method
                        .QuerySingle<LoginDto>(It.Is<string>(argument => argument == query)),
                        Times.Once
                    );

                Assert.NotNull(result);
                Assert.Equal(login, result);
            }
        }

        [Theory]
        [InlineData("admin@gmail.com", "HASH123")]
        [InlineData("test@gmail.com", "HASH123")]
        public async void GetUserByEmailAndPassword(string email, string password)
        {
            using (var mock = AutoMock.GetLoose())
            {
                // Arrange
                var query = $"SELECT * FROM Users\r\n WHERE Email = '{email}'";

                var login = MockDataRepository.GetAllLogins()
                    .SingleOrDefault(login => login.Email == email);

                var task = Task.FromResult(login);

                _dbAdapter
                    .Setup(method => method.QuerySingle<LoginDto>(query))
                    .Returns(task);

                var dbContext = mock.Create<LogisticsCompanyContext>(_dbParameters);

                var serviceParameters = new Parameter[]
                {
                      new TypedParameter(typeof(LogisticsCompanyContext), dbContext),
                      new TypedParameter(typeof(IDbAdapter), _dbAdapter.Object)
                };

                var service = mock.Create<UserQueryService>(serviceParameters);

                // Act
                var result = await service.GetUserByEmailAndPassword(email, password);

                // Assert
                _dbAdapter
                    .Verify(method => method
                        .QuerySingle<LoginDto>(It.Is<string>(argument => argument == query)),
                        Times.Once
                    );

                Assert.NotNull(result);
                Assert.Equal(login, result);
            }
        }

        [Theory]
        [InlineData("admin@gmail.com")]
        [InlineData("test@gmail.com")]
        public async void GetRegisterEmail_should_compose_valid_query_and_return_entity(string email)
        {
            using (var mock = AutoMock.GetLoose())
            {
                // Arrange
                var query = $"SELECT Email FROM Users\r\n WHERE Email = '{email}'";

                var mail = MockDataRepository.GetAllLogins()
                    .SingleOrDefault(login => login.Email == email)
                    .Email;

                var task = Task.FromResult(mail);

                _dbAdapter.Setup(mail => mail
                    .QuerySingle<string>(query)
                )
                .Returns(task);

                var dbContext = mock.Create<LogisticsCompanyContext>(_dbParameters);

                var serviceParameters = new Parameter[]
                {
                      new TypedParameter(typeof(LogisticsCompanyContext), dbContext),
                      new TypedParameter(typeof(IDbAdapter), _dbAdapter.Object)
                };

                var service = mock.Create<UserQueryService>(serviceParameters);

                // Act
                var result = await service.GetRegisterEmail(email);

                // Assert
                _dbAdapter
                    .Verify(method => method
                        .QuerySingle<string>(It.Is<string>(argument => argument == query))
                    );

                Assert.NotNull(result);
                Assert.Equal(email, result);
            }
        }

    }
}
