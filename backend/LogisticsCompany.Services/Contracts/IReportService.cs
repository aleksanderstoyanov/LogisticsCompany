using LogisticsCompany.Services.Dto;

namespace LogisticsCompany.Services.Contracts
{
    public interface IReportService
    {
        Task<IEnumerable<UserDto>> GetAllClients();

        Task<IEnumerable<UserDto>> GetAllEmployees();

        Task<IEnumerable<PackageReportDto>> GetAllRegisteredPackages();
    }
}
