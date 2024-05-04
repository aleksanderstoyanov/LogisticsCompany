using LogisticsCompany.Services.Dto;

namespace LogisticsCompany.Services.Deliveries.Commands
{
    public interface IDeliveryCommandService
    {
        public Task<string> Create(DeliveryDto dto);
        public Task Update(DeliveryDto dto);
    }
}
