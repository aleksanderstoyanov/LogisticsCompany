using LogisticsCompany.Data.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace LogisticsCompany.Data.Builders
{
    public class ClauseDescriptorBuilder
    {
        private ClauseDescriptor container;

        public ClauseDescriptorBuilder()
        {
            this.container = new ClauseDescriptor();   
        }
        public ClauseDescriptorBuilder FieldValue(object fieldValue)
        {
            this.container.FieldValue = fieldValue;
            return this;
        }

        public ClauseDescriptorBuilder Field(string field)
        {
            this.container.Field = field;
            return this;
        }

        public ClauseDescriptorBuilder EqualityOperator(EqualityOperator equalityOperator)
        {
            this.container.EqualityOperator = equalityOperator;
            return this;
        }
        public ClauseDescriptorBuilder LogicalOperator(LogicalOperator logicalOperator)
        {
            this.container.LogicalOperator = logicalOperator;
            return this;
        }

        public ClauseDescriptor Build() {
            return container;
        }

        public void Reset()
            => container = new ClauseDescriptor();
    }
}
