using System.Linq;
using System.Text;

namespace LogisticsCompany.Data.Helpers
{
    public static class SqlCommandHelper
    {
        /// <summary>
        /// Constructs SQL Insert command with an arbitrary amount of parameters. 
        /// </summary>
        /// <param name="table"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public static string InsertCommand(string table, params string [] values)
        {
            var sb = new StringBuilder();

            sb.AppendLine(string.Format("INSERT INTO {0} VALUES", table));
            sb.AppendLine("(");
            sb.Append(String.Join(", ", values));
            sb.AppendLine(")");

            return sb.ToString().Trim();
        }
    }

}
