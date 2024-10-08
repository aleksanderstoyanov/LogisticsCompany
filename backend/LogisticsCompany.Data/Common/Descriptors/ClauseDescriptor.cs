﻿using System.Runtime.CompilerServices;
using LogisticsCompany.Data.Common.Operators;

namespace LogisticsCompany.Data.Common.Descriptors
{
    /// <summary>
    /// Class for composing a raw WHERE Clause in the Database.
    /// </summary>
    public class ClauseDescriptor
    {
        public string? Field { get; set; }

        public object? FieldValue { get; set; }

        public EqualityOperator EqualityOperator { get; set; }

        public LogicalOperator? LogicalOperator { get; set; }

        /// <summary>
        /// Creates an empty <see cref="ClauseDescriptor"/>.instance.
        /// </summary>
        public ClauseDescriptor()
        {
        }

        /// <summary>
        /// Creates a <see cref="ClauseDescriptor"/> instance.
        /// with the passed <paramref name="field"/> and <paramref name="fieldValue"/>.
        /// </summary>
        /// <param name="field">The Field which will be used in the WHERE clause.</param>
        /// <param name="fieldValue">The Field Value which will be used in the WHERE clause.</param>
        public ClauseDescriptor(string field, object fieldValue)
        {
            Field = field;
            FieldValue = fieldValue;
        }

        /// <summary>
        /// Creates a <see cref="ClauseDescriptor"> instance.</see>
        /// </summary>
        /// <param name="field">The Field which will be used in the WHERE clause.</param>
        /// <param name="fieldValue">The Field Value which will be used in the WHERE clause</param>
        /// <param name="logicalOperator">The Logical Operator which will be used in the WHERE clause.</param>
        /// <param name="equalityOperator">The Equality Operator which will be used in the WHERE clause.</param>
        public ClauseDescriptor(string field, object fieldValue, LogicalOperator logicalOperator, EqualityOperator equalityOperator)
        {
            Field = field;
            FieldValue = fieldValue;
            LogicalOperator = logicalOperator;
            EqualityOperator = equalityOperator;
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

        /// <summary>
        /// Method which will compose the raw WHERE clause.
        /// </summary>
        /// <param name="isJoinClause">Determines whether the clause will be used in a JOIN Statement.</param>
        /// <returns>
        /// The raw WHERE clause.
        /// </returns>
        public string ToString(bool isJoinClause)
        {
            var result = "";
            var serializedEqualityOperator = SerializeEqualityOperator();

            if (LogicalOperator == null)
            {
                result = $"{Field} {serializedEqualityOperator} '{FieldValue}' {LogicalOperator}";
            }
            else
            {
                result = $"{Field} {serializedEqualityOperator} '{FieldValue}' {LogicalOperator}";
            }

            if (isJoinClause)
            {
                result = result.Replace("'", string.Empty);
            }

            return result;
        }
    }
}
