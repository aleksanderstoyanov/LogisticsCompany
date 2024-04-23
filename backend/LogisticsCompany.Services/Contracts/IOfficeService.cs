using LogisticsCompany.Services.Dto;

namespace LogisticsCompany.Services.Contracts
{
    public interface IOfficeService
    {
        Task<OfficeDto?> GetById(int id);
        Task<OfficeDto?> GetByAddress(string name);
        Task<IEnumerable<OfficeDto>> GetAll();
        Task<OfficeDto?> Create(OfficeDto dto);
        Task Update(OfficeDto dto);
        Task Delete(int id);
    }
}
