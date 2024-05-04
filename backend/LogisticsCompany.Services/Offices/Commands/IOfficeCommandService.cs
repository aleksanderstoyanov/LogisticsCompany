using LogisticsCompany.Services.Offices.Dto;

namespace LogisticsCompany.Services.Offices.Commands
{
    public interface IOfficeCommandService
    {
        Task<OfficeDto?> Create(OfficeDto dto);
        Task Update(OfficeDto dto);
        Task Delete(int id);
    }
}
