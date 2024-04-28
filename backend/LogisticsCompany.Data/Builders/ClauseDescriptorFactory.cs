
using LogisticsCompany.Data.Common;

namespace LogisticsCompany.Data.Builders
{
    public class ClauseDescriptorFactory
    {
        public ICollection<ClauseDescriptor> Clauses = new HashSet<ClauseDescriptor>();

        public void Add(Action<ClauseDescriptorBuilder> action)
        {
            var builder = new ClauseDescriptorBuilder();

            action.Invoke(builder);

            this.Clauses.Add(builder.Build());

            builder.Reset();
        }
    }
}
