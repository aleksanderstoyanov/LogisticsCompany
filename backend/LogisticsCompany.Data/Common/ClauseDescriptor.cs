using System.Runtime.CompilerServices;

namespace LogisticsCompany.Data.Common
{
    public class ClauseDescriptor
    {
        public string? Field { get; set; }

        public object? FieldValue { get; set; }

        public EqualityOperator EqualityOperator { get; set; }

        public LogicalOperator? LogicalOperator { get; set; }

        public ClauseDescriptor()
        {
        }

        public ClauseDescriptor(string field, object fieldValue)
        {
            this.Field = field;
            this.FieldValue = fieldValue;
        }

        public ClauseDescriptor(string field, object fieldValue, LogicalOperator logicalOperator, EqualityOperator equalityOperator)
        {
            this.Field = field;
            this.FieldValue = fieldValue;
            this.LogicalOperator = logicalOperator;
            this.EqualityOperator = equalityOperator;
        }

        private string SerializeEqualityOperator()
        {
            switch (EqualityOperator)
            {
                case EqualityOperator.EQUALS:
                    return "=";
                case EqualityOperator.NOT_EQUALS:
                    return "!=";
                case EqualityOperator.CONTAINS:
                    return "LIKE";
                case EqualityOperator.GREATER_THAN:
                    return ">";
                case EqualityOperator.GREATER_THAN_AND_EQUALS:
                    return ">=";
                case EqualityOperator.LESSER_THAN:
                    return "<";
                case EqualityOperator.LESSER_THAN_AND_EQUALS:
                    return "<=";

                default:
                    return string.Empty;
            }
        }

        public string ToString(bool isJoinClause)
        {
            var result = "";
            var serializedEqualityOperator = SerializeEqualityOperator();

            if (this.LogicalOperator == null)
            {
                result = $"{this.Field} {serializedEqualityOperator} '{this.FieldValue}' {this.LogicalOperator}";
            }
            else
            {
                result = $"{this.Field} {serializedEqualityOperator} '{this.FieldValue}' {this.LogicalOperator}";
            }

            if (isJoinClause)
            {
                result = result.Replace("'", string.Empty);
            }

            return result;
        }
    }
}
