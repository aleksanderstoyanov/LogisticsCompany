using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogisticsCompany.Data.Common
{
    public class ClauseDescriptorContainer
    {
        public ICollection<ClauseDescriptor> ClauseDescriptors { get; set; }

        public ClauseDescriptorContainer()
        {
            ClauseDescriptors = new HashSet<ClauseDescriptor>();
        }
    }
}
