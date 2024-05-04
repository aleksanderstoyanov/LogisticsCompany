using LogisticsCompany.Services.Package.Dto;

namespace LogisticsCompany.Services.Package.Commands
{
    public interface IPackageCommandService
    {
        Task Update(PackageDto dto);
        Task Create(PackageDto dto);
        Task Delete(int id);
    }
}
