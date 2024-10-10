using Autofac;
using Autofac.Core;
using LogisticsCompany.Data.Contracts;
using Microsoft.Extensions.Configuration;
using Moq;

namespace LogisticsCompany.Tests.Services
{
    public abstract class BaseServiceTest
    {
        protected readonly Mock<IDbAdapter> _dbAdapter;
        protected readonly Parameter[] _dbParameters;

        public BaseServiceTest()
        {
            var inMemorySettings = new Dictionary<string, string> {
                    {"ConnectionStrings:DefaultConnectionString", "Test"},
                };

            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            _dbAdapter = new Mock<IDbAdapter>();
            _dbParameters = new Parameter[1]
            {
                new TypedParameter(typeof(IConfiguration), configuration)
            };
        }
    }
}
