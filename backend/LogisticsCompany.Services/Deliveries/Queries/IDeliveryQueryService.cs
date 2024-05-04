using LogisticsCompany.Services.Dto;

namespace LogisticsCompany.Services.Deliveries.Queries
{
    public interface IDeliveryQueryService
    {
        public Task<IEnumerable<DeliveryDto>> GetAll();
        public Task<DeliveryDto?> GetById(int id);
    }
}
