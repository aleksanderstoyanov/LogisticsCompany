using Autofac;
using Autofac.Core;
using Autofac.Extras.Moq;
using LogisticsCompany.Data;
using LogisticsCompany.Data.Contracts;
using LogisticsCompany.Services.Deliveries.Queries;
using LogisticsCompany.Services.Dto;
using LogisticsCompany.Tests.Common;
using Moq;
using Xunit;

namespace LogisticsCompany.Tests.Services.Deliveries
{
    public class DeliveryQueryServiceTests : BaseServiceTest
    {
        public DeliveryQueryServiceTests()
            : base()
        {
        }

        [Fact]
        public async void Get_All_Method_Should_Return_Entries()
        {
            using (var mock = AutoMock.GetLoose())
            {
                // Arrange 
                var task = Task.FromResult(MockDataRepository.GetAllDeliveries());

                var dbContext = mock.Create<LogisticsCompanyContext>(_dbParameters);

                _dbAdapter
                    .Setup(method => method
                        .QueryAll<DeliveryDto>("SELECT * FROM Deliveries")
                    )
                    .Returns(task);

                var serviceParameters = new Parameter[2]{
                    new TypedParameter(typeof(LogisticsCompanyContext), dbContext),
                    new TypedParameter(typeof(IDbAdapter), _dbAdapter.Object)
                };

                var service = mock.Create<DeliveryQueryService>(serviceParameters);

                // Act 
                var result = await service.GetAll();

                // Assert
                _dbAdapter
                   .Verify(x => x
                        .QueryAll<DeliveryDto>(It.Is<string>(sql => sql == "SELECT * FROM Deliveries"))
                        , Times.Exactly(1)
                   );

                Assert.NotEmpty(result);
            }
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public async void Get_By_Id_Should_Return_Entries(int id)
        {
            using (var mock = AutoMock.GetLoose())
            {
                // Arrange 
                var delivery = MockDataRepository.GetAllDeliveries().SingleOrDefault(delivery => delivery.Id == id);
                var task = Task.FromResult<DeliveryDto>(delivery);

                var dbContext = mock.Create<LogisticsCompanyContext>(_dbParameters);
                var serviceParameters = new Parameter[2]{
                    new TypedParameter(typeof(LogisticsCompanyContext), dbContext),
                    new TypedParameter(typeof(IDbAdapter), _dbAdapter.Object)
                };

                _dbAdapter
                    .Setup(method => method
                        .QuerySingle<DeliveryDto>($"SELECT * FROM Deliveries\r\n WHERE Id = '{id}'")
                     )
                    .Returns(task);

                var service = mock.Create<DeliveryQueryService>(serviceParameters);

                // Act 
                var result = await service.GetById(id);

                // Assert
                _dbAdapter
                   .Verify(x => x
                        .QuerySingle<DeliveryDto>(
                            It.Is<string>(query => query == $"SELECT * FROM Deliveries\r\n WHERE Id = '{id}'")
                        )
                        , Times.Exactly(1)
                   );

                Assert.NotNull(result);
                Assert.Equal(result, delivery);
            }
        }
    }
}