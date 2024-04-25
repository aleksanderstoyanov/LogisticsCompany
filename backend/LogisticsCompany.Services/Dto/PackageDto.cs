using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogisticsCompany.Services.Dto
{
    public class PackageDto
    {
        public int Id { get; set; }
        public string Address { get; set; }
        public int FromId { get; set; }
        public int ToId { get; set; }
        public string PackageStatusName { get; set; }
        public double Weight { get; set; }
        public bool ToOffice { get; set; }
    }
}
