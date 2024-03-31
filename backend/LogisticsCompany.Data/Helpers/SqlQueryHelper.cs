namespace LogisticsCompany.Data.Helpers
{
    public class SqlQueryHelper
    {
        /// <summary>
        /// Constructs an SQL query for retrieving all records.
        /// </summary>
        /// <param name="table"></param>
        /// <returns>The constructer query string</returns>
        public static string SelectAll(string table)
            => string.Format("SELECT * FROM {0}", table);

        /// <summary>
        /// Constructs an SQL query for retrieving the count of all records.
        /// </summary>
        /// <param name="table"></param>
        /// <returns>The constructer query string</returns>
        public static string SelectAllCount(string table)
            => string.Format("SELECT COUNT(Id) FROM {0}", table);

        /// <summary>
        /// Constructs an SQL query for retrieving a table in its entirey based on a single criteria
        /// </summary>
        /// <param name="table"></param>
        /// <param name="criteriaField"></param>
        /// <returns>The constructer query string</returns>
        public static string SelectEntityBySingleCriteria(string table, string criteriaField)
            => string.Format("SELECT * FROM {0} WHERE {1} = @criteriaValue", table, criteriaField);


        /// <summary>
        /// Constructs an SQL query for retrieving an table record based on id and a single criteria.
        /// </summary>
        /// <param name="table"></param>
        /// <param name="criteriaField"></param>
        /// <returns>The constructor query string</returns>
        public static string SelectSingleColumnById(string table, string criteriaField)
            => string.Format("SELECT {1} FROM {0} WHERE Id = @criteriaValue", table, criteriaField);

        /// <summary>
        /// Constructs an SQL query for retrieving an table record based on single criteria.
        /// </summary>
        /// <param name="table"></param>
        /// <param name="criteriaField"></param>
        /// <returns>The constructor query string</returns>
        public static string SelectSingleColumnBySingleCriteria(string table, string criteriaField)
            => string.Format("SELECT {1} FROM {0} WHERE {1} = @criteriaValue", table, criteriaField);

        /// <summary>
        /// Constructs an SQL query for retrieving id based on single criteria.
        /// </summary>
        /// <param name="table"></param>
        /// <param name="criteriaField"></param>
        /// <param name="criteriaValue"></param>
        /// <returns>The constructor query string</returns>
        public static string SelectIdBySingleCriteria(string table, string criteriaField)
            => string.Format("SELECT (Id) FROM {0} WHERE {1} = @criteriaValue", table, criteriaField);
    }
}
