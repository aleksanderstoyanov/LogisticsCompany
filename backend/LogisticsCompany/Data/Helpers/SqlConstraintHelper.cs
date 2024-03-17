using System.Globalization;

namespace LogisticsCompany.Data.Helpers
{
    public static class SqlConstraintHelper
    {
        /// <summary>
        /// Composes a SQL Foreign Key Constraint with the
        /// name of constraint, name of the column, name of the referenced table and its containing field.
        /// </summary>
        /// <param name="name">Name of the ForeignKey constraint.</param>
        /// <param name="column">Name of the foreign key column.</param>
        /// <param name="referencedTable">The table which would be referenced.</param>
        /// <param name="referencedColumn">The field which would be referenced.</param>
        /// <returns></returns>
        public static string ForeignKeyConstraint(string name, string column, string referencedTable, string referencedColumn)
            => string.Format("CONSTRAINT  {0} FOREIGN KEY ({1}) REFERENCES {2}({3})", name, column, referencedTable, referencedColumn);

        /// <summary>
        /// Composes a SQL Primary Key Constraint with the name of the constraint and the two columns which will be composite.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="compositeColumn1"></param>
        /// <param name="compositeColumn2"></param>
        /// <returns></returns>
        public static string CompositePrimaryКeyConstraint(string name, string compositeColumn1, string compositeColumn2)
            => string.Format("CONSTRAINT  {0} PRIMARY KEY ({1}, {2})", name, compositeColumn1, compositeColumn2);

    }
}
