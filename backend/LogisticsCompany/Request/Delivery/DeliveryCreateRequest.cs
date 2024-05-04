using System.ComponentModel.DataAnnotations;

namespace LogisticsCompany.Request.Delivery
{
    public class DeliveryCreateRequest
    {
        [Required]
        public int[] SelectedIds { get; set; }
    }
}
