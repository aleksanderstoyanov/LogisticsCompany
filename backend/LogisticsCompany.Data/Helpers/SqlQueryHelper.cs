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


        /// <summary>
        /// Constructs an SQL query for retrieving an table record based on single criteria.
        /// </summary>
        /// <param name="table"></param>
        /// <param name="criteriaField"></param>
        /// <returns></returns>
        public static string SelectBySingleCriteria(string table, string criteriaField)
            => string.Format("SELECT * FROM {0} WHERE {1} = @criteriaValue", table, criteriaField);

        /// <summary>
        /// Constructs an SQL query for retrieving id based on single criteria.
        /// </summary>
        /// <param name="table"></param>
        /// <param name="criteriaField"></param>
        /// <param name="criteriaValue"></param>
        /// <returns></returns>
        public static string SelectIdBySingleCriteria(string table, string criteriaField)
            => string.Format("SELECT (Id) FROM {0} WHERE {1} = @criteriaValue", table, criteriaField);
    }
}
