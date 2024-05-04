using LogisticsCompany.Data.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogisticsCompany.Data.Common
{
    /// <summary>
    /// Class used for storing existing <see cref="ClauseDescriptor"/> instances.
    /// </summary>
    public class ClauseDescriptorContainer
    {
        public IReadOnlyCollection<ClauseDescriptor> ClauseDescriptors { get; set; }

        /// <summary>
        /// Creates an existing <see cref="ClauseDescriptorContainer"/> instance.
        /// </summary>
        public ClauseDescriptorContainer()
        {
            ClauseDescriptors = new HashSet<ClauseDescriptor>();
        }

        /// <summary>
        /// Method used for adding existing
        /// </summary>
        /// <param name="factoryOptions">The Action 
        /// which will allow creation of clauses through the <paramref name="factoryOptions"/>
        /// </param>
        /// <returns>
        /// The existing <see cref="ClauseDescriptorContainer"/> instance.
        /// </returns>
        public ClauseDescriptorContainer Descriptors(Action<ClauseDescriptorFactory> factoryOptions)
        {
            var factory = new ClauseDescriptorFactory();

            factoryOptions.Invoke(factory);

            this.ClauseDescriptors = (IReadOnlyCollection<ClauseDescriptor>)factory.Clauses;

            return this;
        }
    }
}
