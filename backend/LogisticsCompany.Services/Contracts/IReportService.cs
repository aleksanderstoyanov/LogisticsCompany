using LogisticsCompany.Services.Dto;

namespace LogisticsCompany.Services.Contracts
{
    public interface IReportService
    {
        Task<IEnumerable<UserDto>> GetAllClients();
        Task<IEnumerable<UserDto>> GetAllEmployees();
        Task<IEnumerable<PackageReportDto>> GetAllRegisteredPackages();
        Task<IEnumerable<PackageReportDto>> GetAllInDeliveryPackages();
        Task<decimal> GetIncomeForPeriod(DateTime startPeriod, DateTime endPeriod);

    }
}
