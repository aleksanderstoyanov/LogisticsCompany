using LogisticsCompany.Services.Dto;

namespace LogisticsCompany.Services.Contracts
{
    public interface IOfficeService
    {
        Task<OfficeDto?> GetById(int id);
        Task<IEnumerable<OfficeDto>> GetAll();
        Task Update(OfficeDto dto);
        Task Delete(int id);
    }
}
