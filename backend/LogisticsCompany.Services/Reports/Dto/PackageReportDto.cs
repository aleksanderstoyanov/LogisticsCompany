using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogisticsCompany.Services.Reports.Dto
{
    public class PackageReportDto
    {
        public string Address { get; set; }
        public bool ToOffice { get; set; }
        public int Weight { get; set; }
        public string FromUser { get; set; }
        public string ToUser { get; set; }
        public string PackageStatusName { get; set; }
    }
}
