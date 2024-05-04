using LogisticsCompany.Data.Common;
using System.Text;

namespace LogisticsCompany.Data.Builders
{
    /// <summary>
    /// Builder Class used for constructing raw SQL queries.
    /// </summary>
    public class SqlQueryBuilder
    {
        StringBuilder sb { get; set; }

        /// <summary>
        /// Creates a <see cref="SqlQueryBuilder" /> instance with a newly instatiated <see cref="StringBuilder"/>.
        /// </summary>
        public SqlQueryBuilder()
        {
            sb = new StringBuilder();
        }

        /// <summary>
        /// Method used Constructing raw WHERE clauses for the SQL Query.
        /// </summary>
        /// <param name="container">Contains an arbitrary amount of WHERE clause.</param>
        /// <param name="isJoinClause">Used for determining whether the WHERE clause will used in a JOIN Statement.</param>
        private void ConstructClauses(ClauseDescriptorContainer container, bool isJoinClause = false)
        {
            foreach (var clauseDescriptor in container.ClauseDescriptors)
            {
                sb.Append(clauseDescriptor.ToString(isJoinClause));
                sb.Append(" ");
            }
        }

        /// <summary>
        /// Method used for composing a SQL SELECT statement with arbitrary amount of columns.
        /// </summary>
        /// <param name="columns">The columns which will be used in the SELECT statement.</param>
        /// <returns>
        /// The existing <see cref="SqlQueryBuilder" /> instance.
        /// </returns>
        public SqlQueryBuilder Select(params string[] columns)
        {
            sb.Append($"SELECT {string.Join(", ", columns)}");
            return this;
        }

        /// <summary>
        /// Method used for composing a FROM command which will added to the SELECT statement.
        /// </summary>
        /// <param name="table">The Table which will be used in the FROM command.</param>
        /// <param name="as">The name to which the table will be renamed.</param>
        /// <returns>
        /// The existing <see cref="SqlQueryBuilder" /> instance.
        /// </returns>
        public SqlQueryBuilder From(string table, string? @as = null)
        {
            sb.Append($" FROM {table}");

            if (@as != null)
                sb.Append($" AS {@as} ");

            return this;
        }

        /// <summary>
        /// Construcs a raw WHERE clause for the SQL Query.
        /// </summary>
        /// <param name="container">Contains an arbitrary amount of WHERE clause.</param>
        /// <returns>
        /// The existing <see cref="SqlQueryBuilder" /> instance.
        /// </returns>
        public SqlQueryBuilder Where(ClauseDescriptorContainer container)
        {
            sb.Append(Environment.NewLine);
            sb.Append(" WHERE ");

            ConstructClauses(container);

            return this;
        }

        /// <summary>
        /// Constructs a raw JOIN Statement for the SQL Query.
        /// </summary>
        /// <param name="joinOperator">The Operator which will be used for determining JOIN keyword in the JOIN statement.</param>
        /// <param name="table">The table which will referenced in the JOIN statement.</param>
        /// <param name="container">The clause container which will used in the JOIN statement.</param>
        /// <param name="as">The name to which the joined table will be renamed as.</param>
        /// <returns>
        /// The existing <see cref="SqlQueryBuilder" /> instance.
        /// </returns>
        public SqlQueryBuilder Join(JoinOperator joinOperator, string table, ClauseDescriptorContainer container, string? @as = null)
        {
            var parsedOperator = joinOperator
                .ToString()
                .Replace("_", " ");

            sb.AppendLine(Environment.NewLine);
            sb.Append($"{parsedOperator} JOIN {table}");

            if (@as != null)
            {
                sb.Append($" AS {@as}");
            }

            sb.Append(" ON ");

            ConstructClauses(container, true);

            return this;
        }

        /// <summary>
        /// Method for constructing the SQL Query
        /// </summary>
        /// <returns>
        /// The raw SQL Query.
        /// </returns>
        public string ToQuery()
            => sb.ToString().Trim();
    }
}
