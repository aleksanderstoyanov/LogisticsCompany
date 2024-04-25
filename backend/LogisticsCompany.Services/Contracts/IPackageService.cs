using LogisticsCompany.Services.Dto;

namespace LogisticsCompany.Services.Contracts
{
    public interface IPackageService
    {
        Task<int> GetPackageCountByFromAndTo(int from, int to);
        Task Create(PackageDto dto);
    }
}
