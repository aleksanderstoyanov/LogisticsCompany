using Dapper;
using LogisticsCompany.Data;
using LogisticsCompany.Data.Builders;
using LogisticsCompany.Data.Common.Descriptors;
using LogisticsCompany.Data.Common.Operators;
using LogisticsCompany.Data.Contracts;
using Microsoft.Data.SqlClient;

namespace LogisticsCompany.Services.Roles.Queries
{
    /// <summary>
    /// A <see cref="BaseService"/> class which will perform Database Query operations for Roles.
    /// </summary>
    public class RoleQueryService : BaseService, IRoleQueryService
    {

        /// <summary>
        /// Creates a <see cref="RoleQueryService"/> instance 
        /// with the injected <paramref name="dbContext"/> and <paramref name="dbAdapter"/>
        /// arguments.
        /// </summary>
        /// <param name="dbContext">The Database context</param>
        public RoleQueryService(LogisticsCompanyContext dbContext,
            IDbAdapter dbAdapter)
            : base(dbContext, dbAdapter)
        {
        }

        /// <summary>
        /// Performs a SQL Query for retrieving the Id field 
        /// for a given Role entity based on the passed
        /// <paramref name="name"/> argument.
        /// </summary>
        /// <param name="name">The name of the Role.</param>
        /// <returns>
        ///   The Id field of the Role.
        /// </returns>
        public async Task<int> GetIdByName(string name)
        {
            var clauseContainer = new ClauseDescriptorContainer();

            clauseContainer.Descriptors(descriptors =>
            {
                descriptors.Add(descriptor => descriptor
                    .Field("Name")
                    .EqualityOperator(EqualityOperator.EQUALS)
                    .FieldValue(name)
                );
            });

            var query = new SqlQueryBuilder()
                .Select(columns: "Id")
                .From(table: "Roles")
                .Where(clauseContainer)
                .ToQuery();


            var id = await this._dbAdapter
                .QuerySingle<int>(query);

            return id;
        }

        /// <summary>
        /// Performs a SQL Query based for retrieving
        /// the Name field of an exiting Role entity
        /// based on the passed <paramref name="id"/>
        /// argument.
        /// </summary>
        /// <param name="id">The Id field of the Role</param>
        /// <returns></returns>
        public async Task<string?> GetRoleNameById(int id)
        {
            var clauseContainer = new ClauseDescriptorContainer();

            clauseContainer.Descriptors(descriptors =>
            {
                descriptors.Add(descriptor => descriptor
                    .Field("Id")
                    .EqualityOperator(EqualityOperator.EQUALS)
                    .FieldValue(id)
                );
            });

            var query = new SqlQueryBuilder()
               .Select(columns: "Name")
               .From(table: "Roles")
               .Where(clauseContainer)
               .ToQuery();

            var result = await this._dbAdapter
                .QuerySingle<string>(query);

            return result;
        }
    }
}
