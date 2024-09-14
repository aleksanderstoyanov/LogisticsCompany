using Autofac;
using Autofac.Core;
using Autofac.Extras.Moq;
using LogisticsCompany.Data;
using LogisticsCompany.Services.Deliveries.Queries;
using LogisticsCompany.Services.Dto;
using Microsoft.Extensions.Configuration;
using Moq;

namespace LogisticsCompany.Tests.Deliveries
{
    public class QueryTests
    {

        [Fact]
        public async void Get_All_Method_Should_Return_Entries()
        {
            using (var mock = AutoMock.GetLoose())
            {

                // Arrange 
                var inMemorySettings = new Dictionary<string, string> {
                    {"TopLevelKey", "TopLevelValue"},
                    {"ConnectionStrings:DefaultConnectionString", "Server=.;Database=master;User Id=logisticsUser; Password=123123;MultipleActiveResultSets=true;Trusted_Connection=true;TrustServerCertificate=true;"},
                };

                var configuration = new ConfigurationBuilder()
                    .AddInMemoryCollection(inMemorySettings)
                    .Build();

                var parameters = new Parameter[1]
                {
                    new TypedParameter(typeof(IConfiguration), configuration)
                };

                var dbContext = mock.Create<LogisticsCompanyContext>(parameters);

                parameters[0] = new TypedParameter(typeof(LogisticsCompanyContext), dbContext);

                var service = mock.Create<DeliveryQueryService>(parameters);

                // Act 
                var result = await service.GetAll();


                // Assert
                Assert.NotNull(result.ToList());
            }
        }

        [Fact]
        public async void Get_By_Id_Should_Return_Entries()
        {
            using (var mock = AutoMock.GetLoose())
            {
                // Arrange 

                var inMemorySettings = new Dictionary<string, string> {
                    {"TopLevelKey", "TopLevelValue"},
                    {"ConnectionStrings:DefaultConnectionString", "Server=.;Database=master;User Id=logisticsUser; Password=123123;MultipleActiveResultSets=true;Trusted_Connection=true;TrustServerCertificate=true;"},
                };

                var configuration = new ConfigurationBuilder()
                    .AddInMemoryCollection(inMemorySettings)
                    .Build();

                var parameters = new Parameter[1]
                {
                    new TypedParameter(typeof(IConfiguration), configuration)
                };

                var dbContext = mock.Create<LogisticsCompanyContext>(parameters);

                parameters[0] = new TypedParameter(typeof(LogisticsCompanyContext), dbContext);

                var service = mock.Create<DeliveryQueryService>(parameters);

                // Act 
                var result = await service.GetById(1);


                // Assert
                Assert.NotNull(result);
            }
        }
    }
}