using Dapper;
using LogisticsCompany.Data;
using LogisticsCompany.Data.Builders;
using LogisticsCompany.Data.Common;
using LogisticsCompany.Data.Helpers;
using LogisticsCompany.Dto;
using LogisticsCompany.Entity;
using LogisticsCompany.Services.Contracts;
using LogisticsCompany.Services.Dto;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LogisticsCompany.Services.Users
{
    public class UserService : BaseService, IUserService
    {
        private readonly IRoleService _roleService;
        private readonly IOfficeService _officeService;
        private readonly IPackageService _packageService;

        public UserService(LogisticsCompanyContext dbContext,
            IRoleService roleService,
            IOfficeService officeService,
            IPackageService packageService)
            : base(dbContext)
        {
            _roleService = roleService;
            _officeService = officeService;
            _packageService = packageService;
        }

        public async Task Update(UserDto userDto)
        {
            var roleId = await _roleService.GetIdByName(userDto.RoleName);

            int? officeId = null;
            if (userDto.OfficeName != null)
            {
                officeId = await _officeService.GetIdByName(userDto.OfficeName);
            }

            Dictionary<string, string> keyValuePairs = new Dictionary<string, string>()
            {
                { "Username", userDto.Username },
                { "FirstName", userDto.FirstName },
                { "LastName", userDto.LastName},
                { "Email", userDto.Email },
                { "RoleId", roleId != null ? roleId.ToString() : "NULL"},
                { "OfficeId", officeId != null ? officeId.ToString() : "NULL"},
            };

            var updateCommand = SqlCommandHelper.UpdateCommand(
                    table: "Users",
                    entityType: typeof(User),
                    entityValues: keyValuePairs,
                    primaryKey: userDto.Id
            );

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.ExecuteAsync(updateCommand);
            }
        }

        public async Task Delete(int id)
        {
            var user = GetById(id);

            if (user == null) return;

            using (var connection = new SqlConnection(_connectionString))
            {
                var keyValuePairs = new Dictionary<string, string>()
                {
                    {"FromId", "NULL"},
                    {"ToId", "NULL"}
                };


                var packages = await _packageService.GetPackagesByUserId(id);

                if (packages.Count() > 0)
                {
                    foreach (var package in packages)
                    {
                        var updatePackageCommand = SqlCommandHelper
                            .UpdateCommand
                            (
                                table: "Packages",
                                entityType: typeof(LogisticsCompany.Data.Entity.Package),
                                entityValues: keyValuePairs,
                                primaryKey: package.Id
                            );

                        await connection.ExecuteAsync(updatePackageCommand);
                    }
                }

                var query = SqlCommandHelper.DeleteCommand(table: "Users", primaryKey: "Id");
                await connection.ExecuteAsync(query, new { criteriaValue = id });
            }
        }

        public async Task<IEnumerable<UserDto>> GetDifferentUsersFromCurrent(int id, string role)
        {
            var user = GetById(id);

            if (user == null) new List<UserDto>();

            using (var connection = new SqlConnection(_connectionString))
            {

                var clauseDescriptorContainer = new ClauseDescriptorContainer()
                {
                    ClauseDescriptors = new HashSet<ClauseDescriptor>()
                {
                    new ClauseDescriptor
                    {
                        Field = "u.Id",
                        EqualityOperator = EqualityOperator.NOT_EQUALS,
                        FieldValue = id,
                        LogicalOperator = LogicalOperator.AND
                    },
                    new ClauseDescriptor
                    {
                        Field = "u.Username",
                        EqualityOperator = EqualityOperator.NOT_EQUALS,
                        FieldValue = "admin",
                        LogicalOperator = LogicalOperator.AND
                    },
                    new ClauseDescriptor
                    {
                        Field = "r.Name",
                        EqualityOperator = EqualityOperator.EQUALS,
                        FieldValue = role
                    }
                }
                };

                var roleClauseDescriptorContainer = new ClauseDescriptorContainer()
                {
                    ClauseDescriptors = new HashSet<ClauseDescriptor>()
                    {
                        new ClauseDescriptor
                        {
                            Field = "r.Id",
                            EqualityOperator = EqualityOperator.EQUALS,
                            FieldValue = "u.RoleId",
                        }
                    }
                };

                var officeClauseDescriptorContainer = new ClauseDescriptorContainer()
                {
                    ClauseDescriptors = new HashSet<ClauseDescriptor>()
                    {
                        new ClauseDescriptor
                        {
                            Field = "o.Id",
                            EqualityOperator = EqualityOperator.EQUALS,
                            FieldValue = "u.OfficeId"
                        }
                    }
                };

                var query = new SqlQueryBuilder()
                    .Select(columns: new string[]
                    {
                        "u.Id",
                        "u.FirstName",
                        "u.LastName",
                        "r.Name as RoleName",
                        "o.Address as OfficeName",
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
                    .Join(
                        joinOperator: JoinOperator.INNER,
                        table: "Offices",
                        container: officeClauseDescriptorContainer,
                        @as: "o"
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
                var clauseContainer = new ClauseDescriptorContainer()
                {
                    ClauseDescriptors = new HashSet<ClauseDescriptor>()
                    {
                        new ClauseDescriptor
                        {
                            Field = "Id",
                            EqualityOperator = EqualityOperator.EQUALS,
                            FieldValue = id
                        }
                    }
                };

                var query = new SqlQueryBuilder()
                        .Select(columns: "*")
                        .From("Users")
                        .Where(clauseContainer)
                        .ToQuery();

                var user = await connection.QuerySingleOrDefaultAsync<LoginDto?>(query);
                return user;
            }
        }

        public async Task<IEnumerable<UserDto>> GetUsers()
        {
            using (var connection = new SqlConnection(this._connectionString))
            {
                var rolesClauseContainer = new ClauseDescriptorContainer()
                {
                    ClauseDescriptors = new HashSet<ClauseDescriptor>()
                    {
                        new ClauseDescriptor
                        {
                            Field = "r.Id",
                            EqualityOperator = EqualityOperator.EQUALS,
                            FieldValue = "u.RoleId"
                        }
                    }
                };

                var officesClauseContainer = new ClauseDescriptorContainer()
                {
                    ClauseDescriptors = new HashSet<ClauseDescriptor>()
                    {
                        new ClauseDescriptor
                        {
                            Field = "o.Id",
                            EqualityOperator = EqualityOperator.EQUALS,
                            FieldValue = "u.OfficeId"
                        }
                    }
                };

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
                var clauseContainer = new ClauseDescriptorContainer()
                {
                    ClauseDescriptors = new HashSet<ClauseDescriptor>()
                    {
                        new ClauseDescriptor
                        {
                            Field = "Email",
                            EqualityOperator = EqualityOperator.EQUALS,
                            FieldValue = email
                        }
                    }
                };

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
                var clauseContainer = new ClauseDescriptorContainer()
                {
                    ClauseDescriptors = new HashSet<ClauseDescriptor>
                    {
                        new ClauseDescriptor
                        {
                            Field = "Email",
                            EqualityOperator = EqualityOperator.EQUALS,
                            FieldValue = email
                        }
                    }
                };

                var query = new SqlQueryBuilder()
                      .Select(columns: "Email")
                      .From(table: "Users")
                      .Where(clauseContainer)
                      .ToQuery();

                var queryResult = await sqlConnection.QueryFirstOrDefaultAsync<string>(query);

                return queryResult ?? string.Empty;
            }
        }

        public async Task<string> Login(LoginDto dto, string issuer, string key)
        {
            var user = await GetUserByEmailAndPassword(dto.Email, dto.PasswordHash);

            if (user != null && PasswordHasher.VerifyPassword(dto.PasswordHash, user.PasswordHash))
            {
                var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

                var role = await _roleService.GetRoleNameById(user.RoleId) ?? string.Empty;

                var claims = new Claim[]
                {
                    new Claim("Id", user.Id.ToString()),
                    new Claim("Email", user.Email),
                    new Claim("Role", role),
                };

                var Sectoken = new JwtSecurityToken(issuer,
                 issuer,
                 claims,
                 expires: DateTime.Now.AddMinutes(120),
                 signingCredentials: credentials);

                var token = new JwtSecurityTokenHandler().WriteToken(Sectoken);

                return token;
            }

            return string.Empty;
        }
        public async Task Register(RegisterDto dto)
        {
            dto.Password = PasswordHasher.HashPassword(dto.Password);
            var roleId = await _roleService.GetIdByName(dto.Role);

            if (roleId != 0)
            {
                var registerEmail = await GetRegisterEmail(dto.Email);

                if (string.IsNullOrEmpty(registerEmail))
                {
                    using (var sqlConnection = new SqlConnection(_connectionString))
                    {
                        var insertCommand = SqlCommandHelper.InsertCommand("Users", $"'{dto.Username}'", $"'{dto.FirstName}'", $"'{dto.LastName}'", $"'{dto.Email}'", $"{roleId}", "NULL", $"'{dto.Password}'");
                        await sqlConnection.ExecuteAsync(insertCommand);
                    }
                }

                // TO DO: Add descriptive exception.
                return;
            }
        }
    }
}
