using LogisticsCompany.Data;

namespace LogisticsCompany.Services
{
    public class BaseService
    {
        protected readonly string _connectionString;
        protected readonly LogisticsCompanyContext _dbContext;

        public BaseService(LogisticsCompanyContext dbContext)
        {
            this._dbContext = dbContext;
            this._connectionString = this._dbContext.GetConnectionString();
        }
    }
}
