namespace LogisticsCompany.Data.Entity
{
    public class Package
    {
        public int Id { get; set; }
        public int FromId { get; set; }
        public int ToId { get; set; }
        public int DeliveryId { get; set; }
        public int PackageStatusId { get; set; }
        public int OfficeId { get; set; }
        public decimal Price { get; set; }
        public string Address { get; set; }
        public bool ToOffice { get; set; }
        public double Weight { get; set; }
    }
}
