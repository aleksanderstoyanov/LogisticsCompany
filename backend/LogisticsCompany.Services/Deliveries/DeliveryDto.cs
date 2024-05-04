namespace LogisticsCompany.Services.Dto
{
    public class DeliveryDto
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int[] SelectedIds { get; set; } 
    }
}
