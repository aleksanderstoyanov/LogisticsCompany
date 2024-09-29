using LogisticsCompany.Data.Common.Descriptors;

namespace LogisticsCompany.Data.Builders
{
    /// <summary>
    /// Class used for creaging ClauseDescriptors via the <see cref="ClauseDescriptorBuilder"/>
    /// </summary>
    public class ClauseDescriptorFactory
    {
        public ICollection<ClauseDescriptor> Clauses = new HashSet<ClauseDescriptor>();

        /// <summary>
        /// Method which exposes creation of a <see cref="ClauseDescriptor"/> Clause 
        /// by using its underlying <see cref="ClauseDescriptorBuilder"/> Builder.
        /// </summary>
        /// <param name="action">
        /// The Action which will construct the <see cref="ClauseDescriptor"/> instance.
        /// </param>
        public void Add(Action<ClauseDescriptorBuilder> action)
        {
            var builder = new ClauseDescriptorBuilder();

            action.Invoke(builder);

            this.Clauses.Add(builder.Build());

            builder.Reset();
        }
    }
}
