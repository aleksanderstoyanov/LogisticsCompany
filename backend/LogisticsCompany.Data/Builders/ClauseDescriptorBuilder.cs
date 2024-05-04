using LogisticsCompany.Data.Common;

namespace LogisticsCompany.Data.Builders
{
    /// <summary>
    /// Builder class used for composing a ClauseDescriptor instance.
    /// </summary>
    public class ClauseDescriptorBuilder
    {
        private ClauseDescriptor container;

        /// <summary>
        /// Creates an <see cref="ClauseDescriptorBuilder"/> instance 
        /// with a newly instantiate <see cref="ClauseDescriptor"/>.
        /// </summary>
        public ClauseDescriptorBuilder()
        {
            this.container = new ClauseDescriptor();   
        }

        /// <summary>
        /// Method which will set the passed <paramref name="fieldValue"/> to the container of the <see cref="ClauseDescriptorBuilder"/>.
        /// </summary>
        /// <code>
        ///     var clause = new ClausedDescriptorContainer()
        ///         .Descriptors(descriptors => {
        ///             descriptors.Add(descriptor => descriptor
        ///                 .FieldValue("1")    
        ///             )
        ///         });
        /// </code>
        /// <param name="fieldValue">The field value which will be passed to the builder.</param>
        /// <returns>
        /// The <see cref="ClauseDescriptorBuilder"/> Builder instance.
        /// </returns>
        public ClauseDescriptorBuilder FieldValue(object fieldValue)
        {
            this.container.FieldValue = fieldValue;
            return this;
        }

        /// <summary>
        /// Method which will set the passed <paramref name="field"/> to the container of the <see cref="ClauseDescriptorBuilder"/>.
        /// </summary>
        /// <code>
        ///     var clause = new ClausedDescriptorContainer()
        ///         .Descriptors(descriptors => {
        ///             descriptors.Add(descriptor => descriptor
        ///                 .Field("Id")    
        ///             )
        ///         });
        /// </code>
        /// <param name="field">The field which will be passed to the builder.</param>
        /// <returns>
        /// The <see cref="ClauseDescriptorBuilder"/> Builder instance.
        /// </returns>
        public ClauseDescriptorBuilder Field(string field)
        {
            this.container.Field = field;
            return this;
        }

        /// <summary>
        /// Method which will set the passed <paramref name="equalityOperator"/> to the container of the <see cref="ClauseDescriptorBuilder"/>.
        /// </summary>
        /// <code>
        ///     var clause = new ClausedDescriptorContainer()
        ///         .Descriptors(descriptors => {
        ///             descriptors.Add(descriptor => descriptor
        ///                 .EqualityOperator(EqualityOperator.EQUALS)    
        ///             )
        ///         });
        /// </code>
        /// <param name="equalityOperator">The equality operator which will be passed to the builder.</param>
        /// <returns>
        /// The <see cref="ClauseDescriptorBuilder"/> Builder instance.
        /// </returns>
        public ClauseDescriptorBuilder EqualityOperator(EqualityOperator equalityOperator)
        {
            this.container.EqualityOperator = equalityOperator;
            return this;
        }

        /// <summary>
        /// Method which will set the passed <paramref name="logicalOperator"/> to the container of the <see cref="ClauseDescriptorBuilder"/>.
        /// </summary>
        /// <code>
        ///     var clause = new ClausedDescriptorContainer()
        ///         .Descriptors(descriptors => {
        ///             descriptors.Add(descriptor => descriptor
        ///                 .EqualityOperator(EqualityOperator.EQUALS)    
        ///             )
        ///         });
        /// </code>
        /// <param name="logicalOperator">The logical operator which will be passed to the builder.</param>
        /// <returns>
        /// The <see cref="ClauseDescriptorBuilder"/> Builder instance.
        /// </returns>
        public ClauseDescriptorBuilder LogicalOperator(LogicalOperator logicalOperator)
        {
            this.container.LogicalOperator = logicalOperator;
            return this;
        }

        /// <summary>
        /// Method used for building the <see cref="ClauseDescriptor"/>.
        /// </summary>
        /// <returns>
        /// The constructed <see cref="ClauseDescriptor"/> instance.
        /// </returns>
        public ClauseDescriptor Build() {
            return container;
        }

        /// <summary>
        /// Resets the existing <see cref="ClauseDescriptor"/> to a new instance.
        /// </summary>
        public void Reset()
            => container = new ClauseDescriptor();
    }
}
