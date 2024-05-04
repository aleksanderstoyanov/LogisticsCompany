namespace LogisticsCompany.Response.Package
{
    public class PackageClientResponseModel
    {
        public int Id { get; set; }
        public string Address { get; set; }
        public string PackageStatusName { get; set; }
        public string FromUser { get; set; }
        public string ToUser { get; set; }
        public int Weight { get; set; }
        public bool ToOffice { get; set; }
    }
}
