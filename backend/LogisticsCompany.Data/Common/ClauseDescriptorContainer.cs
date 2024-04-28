using LogisticsCompany.Data.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogisticsCompany.Data.Common
{
    public class ClauseDescriptorContainer
    {
        public IReadOnlyCollection<ClauseDescriptor> ClauseDescriptors { get; set; }
        public ClauseDescriptorContainer()
        {
            ClauseDescriptors = new HashSet<ClauseDescriptor>();
        }
        public ClauseDescriptorContainer Descriptors(Action<ClauseDescriptorFactory> factoryOptions)
        {
            var factory = new ClauseDescriptorFactory();

            factoryOptions.Invoke(factory);

            this.ClauseDescriptors = (IReadOnlyCollection<ClauseDescriptor>)factory.Clauses;

            return this;
        }
    }
}
