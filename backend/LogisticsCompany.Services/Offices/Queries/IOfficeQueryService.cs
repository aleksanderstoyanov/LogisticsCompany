using LogisticsCompany.Services.Offices.Dto;

namespace LogisticsCompany.Services.Offices.Queries
{
    public interface IOfficeQueryService
    {
        Task<int?> GetIdByName(string name);
        Task<OfficeDto?> GetByAddress(string name);
        Task<OfficeDto?> GetById(int id);
        Task<IEnumerable<OfficeDto>> GetAll();
    }
}
