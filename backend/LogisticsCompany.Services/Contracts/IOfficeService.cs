using LogisticsCompany.Services.Dto;

namespace LogisticsCompany.Services.Contracts
{
    public interface IOfficeService
    {
        Task<IEnumerable<OfficeDto>> GetAll();
    }
}
