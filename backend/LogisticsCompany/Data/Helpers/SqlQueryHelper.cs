namespace LogisticsCompany.Data.Helpers
{
    public class SqlQueryHelper
    {
        /// <summary>
        /// Constructs an SQL query for retrieving all records.
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public static string SelectAll(string table)
            => string.Format("SELECT * FROM {0}", table);

        /// <summary>
        /// Constructs an SQL query for retrieving the count of all records.
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public static string SelectAllCount(string table)
            => string.Format("SELECT COUNT(Id) FROM {0}", table);
    }
}
