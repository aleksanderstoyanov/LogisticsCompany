using System.Linq;
using System.Text;

namespace LogisticsCompany.Data.Helpers
{
    public static class SqlCommandHelper
    {
        /// <summary>
        /// Constructs SQL Insert command with an arbitrary amount of parameters. 
        /// </summary>
        /// <param name="table">The table to which values will be added.</param>
        /// <param name="values">Values for the table.</param>
        public static string InsertCommand(string table, params string [] values)
        {
            var sb = new StringBuilder();

            sb.AppendLine(string.Format("INSERT INTO {0} VALUES", table))
              .AppendLine("(")
              .Append(string.Join(", ", values))
              .AppendLine(")");

            return sb.ToString().Trim();
        }
    }

}
