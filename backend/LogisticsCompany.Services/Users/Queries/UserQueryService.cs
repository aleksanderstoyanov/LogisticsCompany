using Dapper;
using LogisticsCompany.Data;
using LogisticsCompany.Data.Builders;
using LogisticsCompany.Data.Common;
using LogisticsCompany.Data.Common.Descriptors;
using LogisticsCompany.Data.Common.Operators;
using LogisticsCompany.Services.Authorization.Dto;
using LogisticsCompany.Services.Users.Dto;
using Microsoft.Data.SqlClient;

namespace LogisticsCompany.Services.Users.Queries
{
    /// <summary>
    /// A <see cref="BaseService"/> class used for the performing Database Query operations for Users.
    /// </summary>
    public class UserQueryService: BaseService, IUserQueryService
    {
        /// <summary>
        /// Creates a <see cref="UserQueryService"/> instance
        /// with the injected <paramref name="dbContext"/> 
        /// argument.
        /// </summary>
        /// <param name="dbContext"></param>
        public UserQueryService(LogisticsCompanyContext dbContext)
            : base(dbContext)
        {
        }

        /// <summary>
        /// Performs a SQL query for retrieving 
        /// different user from the user that matches the <paramref name="id"/>
        /// and the <paramref name="role"/> arguments.
        /// </summary>
        /// <param name="id">The Id of the User</param>
        /// <param name="role">The Role of the User</param>
        /// <returns>
        /// <see cref="IEnumerable{UserDto}"/> collection of Users.
        /// </returns>
        public async Task<IEnumerable<UserDto>> GetDifferentUsersFromCurrent(int id, string role)
        {
            var user = GetById(id);

            if (user == null) new List<UserDto>();

            using (var connection = new SqlConnection(_connectionString))
            {

                var clauseDescriptorContainer = new ClauseDescriptorContainer();

                clauseDescriptorContainer.Descriptors(descriptors =>
                {
                    descriptors.Add(descriptor => descriptor
                        .Field("u.Id")
                        .EqualityOperator(EqualityOperator.NOT_EQUALS)
                        .FieldValue(id)
                        .LogicalOperator(LogicalOperator.AND)
                    );

                    descriptors.Add(descriptor => descriptor
                        .Field("u.Username")
                        .EqualityOperator(EqualityOperator.NOT_EQUALS)
                        .FieldValue("admin")
                        .LogicalOperator(LogicalOperator.AND)
                    );

                    descriptors.Add(descriptor => descriptor
                        .Field("r.Name")
                        .EqualityOperator(EqualityOperator.EQUALS)
                        .FieldValue(role)
                    );

                });

                var roleClauseDescriptorContainer = new ClauseDescriptorContainer();

                roleClauseDescriptorContainer.Descriptors(descriptors =>
                {
                    descriptors.Add(descriptor => descriptor
                        .Field("r.Id")
                        .EqualityOperator(EqualityOperator.EQUALS)
                        .FieldValue("u.RoleId")
                    );
                });

                var query = new SqlQueryBuilder()
                    .Select(columns: new string[]
                    {
                        "u.Id",
                        "u.FirstName",
                        "u.LastName",
                        "r.Name as RoleName",
                        "u.Username",
                        "u.Email"
                    })
                    .From(table: "Users", @as: "u")
                    .Join
                    (
                        joinOperator: JoinOperator.INNER,
                        table: "Roles",
                        container: roleClauseDescriptorContainer,
                        @as: "r"
                    )
                    .Where(clauseDescriptorContainer)
                    .ToQuery();

                var result = await connection.QueryAsync<UserDto>(query);

                return result;
            }
        }

        /// <summary>
        /// Performs a SQL Query for retrieving a 
        /// user based on the provided <paramref name="id"/>
        /// </summary>
        /// <param name="id">The Id field of the User</param>
        /// <returns>
        /// <see cref="LoginDto"/>
        /// </returns>
        public async Task<LoginDto?> GetById(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var clauseDescriptorContainer = new ClauseDescriptorContainer();

                clauseDescriptorContainer.Descriptors(descriptors =>
                {
                    descriptors.Add(descriptor => descriptor
                        .Field("Id")
                        .EqualityOperator(EqualityOperator.EQUALS)
                        .FieldValue(id)
                    );
                });

                var query = new SqlQueryBuilder()
                     .Select(columns: "*")
                     .From("Users")
                     .Where(clauseDescriptorContainer)
                     .ToQuery();

                var user = await connection.QuerySingleOrDefaultAsync<LoginDto?>(query);
                return user;
            }
        }

        /// <summary>
        /// Performs a SQL Query for retrieving 
        /// all users.
        /// </summary>
        /// <returns>
        /// <see cref="IEnumerable{UserDto}"/> collection of users.
        /// </returns>
        public async Task<IEnumerable<UserDto>> GetUsers()
        {
            using (var connection = new SqlConnection(this._connectionString))
            {
                var rolesClauseContainer = new ClauseDescriptorContainer();

                rolesClauseContainer
                    .Descriptors(descriptors =>
                    {
                        descriptors.Add(descriptor => descriptor
                            .Field("r.Id")
                            .EqualityOperator(EqualityOperator.EQUALS)
                            .FieldValue("u.RoleId")
                        );
                    });


                var officesClauseContainer = new ClauseDescriptorContainer();

                officesClauseContainer.Descriptors(descriptors =>
                {
                    descriptors.Add(descriptor => descriptor
                        .Field("o.Id")
                        .EqualityOperator(EqualityOperator.EQUALS)
                        .FieldValue("u.OfficeId")
                    );
                });


                var query = new SqlQueryBuilder()
                    .Select
                    (
                        columns: new[] { "u.Id", "u.Username", "u.FirstName", "u.LastName", "u.Email", "r.Name AS RoleName", "o.Address AS OfficeName" }
                    )
                    .From(
                        table: "Users",
                        @as: "u"
                    )
                    .Join(
                        table: "Roles",
                        @as: "r",
                        joinOperator: JoinOperator.INNER,
                        container: rolesClauseContainer
                     )
                    .Join(
                        table: "Offices",
                        @as: "o",
                        joinOperator: JoinOperator.LEFT,
                        container: officesClauseContainer
                    )
                    .ToQuery();

                var users = await connection.QueryAsync<UserDto>(query);
                users = users.Where(user => user.RoleName != "Admin");
                return users;
            }
        }

        /// <summary>
        /// Performs a SQL Query for retrieving a User entity
        /// that matches the <paramref name="email"/> and <paramref name="password"/>
        /// arguments.
        /// </summary>
        /// <param name="email">The Email field of the User</param>
        /// <param name="password">The Password field of the User</param>
        /// <returns>
        /// <see cref="LoginDto"/>
        /// </returns>
        public async Task<LoginDto?> GetUserByEmailAndPassword(string email, string password)
        {
            using (var connection = new SqlConnection(this._connectionString))
            {
                var clauseContainer = new ClauseDescriptorContainer();

                clauseContainer.Descriptors(descriptors =>
                {
                    descriptors.Add(descriptor => descriptor
                        .Field("Email")
                        .EqualityOperator(EqualityOperator.EQUALS)
                        .FieldValue(email)
                    );
                });


                var query = new SqlQueryBuilder()
                    .Select(columns: "*")
                    .From(table: "Users")
                    .Where(clauseContainer)
                    .ToQuery();

                var user = await connection.QuerySingleOrDefaultAsync<LoginDto?>(query);

                return user;
            }
        }

        /// <summary>
        /// Performs a SQL Query for retrieving a Email from an existing user entity
        /// based on the passed <paramref name="email"/>
        /// argument.
        /// </summary>
        /// <param name="email">The Email field of the User.</param>
        /// <returns>
        /// </returns>
        public async Task<string> GetRegisterEmail(string email)
        {
            using (var sqlConnection = new SqlConnection(_dbContext.GetConnectionString()))
            {
                var clauseContainer = new ClauseDescriptorContainer();

                clauseContainer.Descriptors(descriptors =>
                {
                    descriptors.Add(descriptor => descriptor
                        .Field("Email")
                        .EqualityOperator(EqualityOperator.EQUALS)
                        .FieldValue(email)
                    );
                });

                var query = new SqlQueryBuilder()
                      .Select(columns: "Email")
                      .From(table: "Users")
                      .Where(clauseContainer)
                      .ToQuery();

                var queryResult = await sqlConnection.QueryFirstOrDefaultAsync<string>(query);

                return queryResult ?? string.Empty;
            }
        }
    }
}
