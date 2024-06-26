﻿using System.ComponentModel.DataAnnotations;

namespace LogisticsCompany.Request.Package
{
    public class PackageRequestModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Address { get; set; }
        public int? FromId { get; set; }

        public int? DeliveryId { get; set; }

        [Required]
        public int OfficeId { get; set; }
        public string? PackageStatusName { get; set; }
        public int? ToId { get; set; }

        [Required]

        public double Weight { get; set; }

        [Required]
        public bool ToOffice { get; set; }
    }
}
