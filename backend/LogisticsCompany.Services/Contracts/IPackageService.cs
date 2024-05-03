using LogisticsCompany.Services.Dto;

namespace LogisticsCompany.Services.Contracts
{
    public interface IPackageService
    {
        Task<IEnumerable<PackageDto>> GetAll();
        Task<IEnumerable<PackageDto>> GetPackagesByUserId(int id);
        Task<IEnumerable<SentReceivedPackageDto>> GetReceivedPackages(int id);
        Task<IEnumerable<SentReceivedPackageDto>> GetSentPackages(int id);
        Task<PackageDto?> GetById(int id);
        Task<int> GetPackageCountByFromAndTo(int from, int to);
        Task Update(PackageDto dto);
        Task Create(PackageDto dto);
        Task Delete(int id);
    }
}
