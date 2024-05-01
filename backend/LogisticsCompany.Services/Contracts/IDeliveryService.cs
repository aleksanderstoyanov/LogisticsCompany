using LogisticsCompany.Services.Dto;

namespace LogisticsCompany.Services.Contracts
{
    public interface IDeliveryService
    {
        public Task<IEnumerable<DeliveryDto>> GetAll();
        public Task<DeliveryDto?> GetById(int id);
        public Task<string> Create(DeliveryDto dto);
        public Task Update(DeliveryDto dto);
    }
}
