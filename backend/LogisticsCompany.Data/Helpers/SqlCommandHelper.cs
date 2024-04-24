using System.Linq;
using System.Reflection;
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
        public static string InsertCommand(string table, params string[] values)
        {
            var sb = new StringBuilder();

            sb.AppendLine(string.Format("INSERT INTO {0} VALUES", table))
              .AppendLine("(")
              .Append(string.Join(", ", values))
              .AppendLine(")");

            return sb.ToString().Trim();
        }

        /// <summary>
        /// Constructs a SQL Delete command which removes an identity based on its unique identifier.
        /// </summary>
        /// <param name="table"></param>
        /// <param name="primaryKey"></param>
        /// <returns></returns>
        public static string DeleteCommand(string table, string primaryKey)
        {
            var sb = new StringBuilder();

            sb.AppendLine(string.Format("DELETE FROM {0} WHERE {1} = @criteriaValue", table, primaryKey));

            return sb.ToString().Trim();
        }

        /// <summary>
        /// Constructs an Update SQL command with an arbitrary amount of parameters.
        /// </summary>
        /// <param name="table"></param>
        /// <param name="entityType"></param>
        /// <param name="entityValues"></param>
        /// <param name="primaryKey"></param>
        public static string UpdateCommand(string table, Type entityType, Dictionary<string, string> entityValues, int primaryKey)
        {
            var sb = new StringBuilder();

            sb.AppendLine(string.Format("UPDATE {0}", table));
            sb.AppendLine(Environment.NewLine);
            sb.Append(string.Format("SET "));

            Func<PropertyInfo, bool> predicate = (property) => entityValues.ContainsKey(property.Name);

            var fields = entityType
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Where(predicate)
                .ToList();

            var lastField = fields.Last();

            foreach (var field in fields)
            {
                string entityfieldValue = String.Empty;

                if (field.Name.Equals(lastField.Name))
                {
                    if (entityValues.TryGetValue(field.Name, out entityfieldValue))
                    {
                        if(entityfieldValue == "NULL")
                        {
                            sb.Append(string.Format("{0} = {1}", field.Name, entityfieldValue));
                        }
                        else
                        {
                            sb.Append(string.Format("{0} = '{1}'", field.Name, entityfieldValue));
                        }

                    }
                }
                else
                {
                    if (entityValues.TryGetValue(field.Name, out entityfieldValue))
                    {
                        if (entityfieldValue == "NULL")
                        {
                            sb.Append(string.Format("{0} = {1}", field.Name, entityfieldValue));
                        }
                        else
                        {
                            sb.Append(string.Format("{0} = '{1}', ", field.Name, entityfieldValue));
                        }
                    }
                }

            }

            sb.AppendLine(Environment.NewLine);
            sb.Append(string.Format("WHERE Id = {0}", primaryKey));

            return sb.ToString();
        }
    }

}
