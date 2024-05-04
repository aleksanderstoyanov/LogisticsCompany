using LogisticsCompany.Services.Package.Dto;

namespace LogisticsCompany.Services.Package.Queries
{
    public interface IPackageQueryService
    {
        Task<IEnumerable<PackageDto>> GetAll();
        Task<IEnumerable<PackageDto>> GetPackagesByUserId(int id);
        Task<IEnumerable<SentReceivedPackageDto>> GetReceivedPackages(int id);
        Task<IEnumerable<SentReceivedPackageDto>> GetSentPackages(int id);
        Task<PackageDto?> GetById(int id);
        Task<int> GetPackageCountByFromAndTo(int from, int to);
    }
}
