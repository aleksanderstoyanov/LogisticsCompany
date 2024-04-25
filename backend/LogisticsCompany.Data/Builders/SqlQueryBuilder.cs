using LogisticsCompany.Data.Common;
using System.Text;

namespace LogisticsCompany.Data.Builders
{
    public class SqlQueryBuilder
    {
        StringBuilder sb { get; set; }

        public SqlQueryBuilder()
        {
            sb = new StringBuilder();
        }

        private void ConstructClauses(ClauseDescriptorContainer container, bool isJoinClause = false)
        {
            foreach (var clauseDescriptor in container.ClauseDescriptors)
            {
                sb.Append(clauseDescriptor.ToString(isJoinClause));
                sb.Append(" ");
            }
        }

        public SqlQueryBuilder Select(params string[] columns)
        {
            sb.Append($"SELECT {string.Join(", ", columns)}");
            return this;
        }

        public SqlQueryBuilder From(string table, string? @as = null)
        {
            sb.Append($" FROM {table}");

            if (@as != null)
                sb.Append($" AS {@as} ");

            return this;
        }

        public SqlQueryBuilder Where(ClauseDescriptorContainer container)
        {
            sb.Append(Environment.NewLine);
            sb.Append(" WHERE ");

            ConstructClauses(container);

            return this;
        }

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

        public string ToQuery()
            => sb.ToString().Trim();
    }
}
