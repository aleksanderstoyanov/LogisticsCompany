using Dapper;
using LogisticsCompany.Data;
using LogisticsCompany.Data.Builders;
using LogisticsCompany.Services.Contracts;
using LogisticsCompany.Services.Dto;
using Microsoft.Data.SqlClient;

namespace LogisticsCompany.Services.Office
{
    public class OfficeService : BaseService, IOfficeService
    {
        public OfficeService(LogisticsCompanyContext dbContext)
            :base(dbContext)
        {
        }

        public async Task<IEnumerable<OfficeDto>> GetAll()
        {
            using (var connection = new SqlConnection(this._connectionString))
            {
                var query = new SqlQueryBuilder()
                    .Select(columns: "*")
                    .From(table: "Offices")
                    .GetQuery();

                var offices = await connection.QueryAsync<OfficeDto>(query);
                return offices;
            }
        }
    }
}
