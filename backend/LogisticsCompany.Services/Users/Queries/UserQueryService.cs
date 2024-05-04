using Dapper;
using LogisticsCompany.Data;
using LogisticsCompany.Data.Builders;
using LogisticsCompany.Data.Common;
using LogisticsCompany.Services.Authorization.Dto;
using LogisticsCompany.Services.Users.Dto;
using Microsoft.Data.SqlClient;

namespace LogisticsCompany.Services.Users.Queries
{
    public class UserQueryService: BaseService, IUserQueryService
    {
        public UserQueryService(LogisticsCompanyContext dbContext)
            : base(dbContext)
        {
        }

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
