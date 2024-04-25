using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogisticsCompany.Services.Dto
{
    public class PackageDto
    {
        public string Address { get; set; }
        public int From { get; set; }

        public int To { get; set; }
        public double Weight { get; set; }

        public bool ToOffice { get; set; }
    }
}
