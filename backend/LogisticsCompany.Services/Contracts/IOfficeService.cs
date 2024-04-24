using LogisticsCompany.Services.Dto;

namespace LogisticsCompany.Services.Contracts
{
    public interface IOfficeService
    {
        Task<int?> GetIdByName(string name);
        Task<OfficeDto?> GetByAddress(string name);
        Task<OfficeDto?> GetById(int id);
        Task<IEnumerable<OfficeDto>> GetAll();
        Task<OfficeDto?> Create(OfficeDto dto);
        Task Update(OfficeDto dto);
        Task Delete(int id);
    }
}
