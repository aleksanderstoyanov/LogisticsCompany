using LogisticsCompany.Services.Reports.Dto;
using LogisticsCompany.Services.Users.Dto;

namespace LogisticsCompany.Services.Reports.Queries
{
    public interface IReportQueryService
    {
        Task<IEnumerable<UserDto>> GetAllClients();
        Task<IEnumerable<UserDto>> GetAllEmployees();
        Task<IEnumerable<PackageReportDto>> GetAllRegisteredPackages();
        Task<IEnumerable<PackageReportDto>> GetAllInDeliveryPackages();
        Task<decimal> GetIncomeForPeriod(DateTime startPeriod, DateTime endPeriod);
    }
}
