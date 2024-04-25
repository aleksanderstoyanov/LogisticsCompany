using System.ComponentModel.DataAnnotations;

namespace LogisticsCompany.Request
{
    public class PackageRequestModel
    {
        [Required]
        public string Address { get; set; }
        public int? From { get; set; }

        public int? To { get; set; }

        [Required]

        public double Weight { get; set; }

        [Required]
        public bool ToOffice { get; set; }
    }
}
