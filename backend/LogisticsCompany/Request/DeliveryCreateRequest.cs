using System.ComponentModel.DataAnnotations;

namespace LogisticsCompany.Request
{
    public class DeliveryCreateRequest
    {
        [Required]
        public int[] SelectedIds { get; set; }
    }
}
