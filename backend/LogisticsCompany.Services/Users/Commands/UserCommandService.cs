using Dapper;
using LogisticsCompany.Data;
using LogisticsCompany.Data.Contracts;
using LogisticsCompany.Data.Helpers;
using LogisticsCompany.Entity;
using LogisticsCompany.Services.Offices.Queries;
using LogisticsCompany.Services.Package.Queries;
using LogisticsCompany.Services.Roles.Queries;
using LogisticsCompany.Services.Users.Dto;
using LogisticsCompany.Services.Users.Queries;
using Microsoft.Data.SqlClient;

namespace LogisticsCompany.Services.Users.Commands
{
    /// <summary>
    /// A <see cref="BaseService"/> class which will perform Database Command operations for Users
    /// </summary>
    public class UserCommandService : BaseService, IUserCommandService
    {
        private readonly IUserQueryService _userQueryService;
        private readonly IRoleQueryService _roleQueryService;
        private readonly IOfficeQueryService _officeQueryService;
        private readonly IPackageQueryService _packageQueryService;

        /// <summary>
        /// Creates a <see cref="UserCommandService"/> instance
        /// with the injected <paramref name="dbContext"/>, <paramref name="dbAdapter"/>, <paramref name="userQueryService"/>,
        /// <paramref name="roleQueryService"/>, <paramref name="officeQueryService"/>, and
        /// <paramref name="packageQueryService"/> arguments.
        /// </summary>
        /// <param name="dbContext">The Database context</param>
        /// <param name="dbAdapter">The DataBase adapter that will instantiate a connection and process the constructed command.</param>
        /// <param name="userQueryService">Service used for performing query operations for Users</param>
        /// <param name="roleQueryService">Service used for performing query operations for Roles</param>
        /// <param name="officeQueryService">Service used for performing query operations for Offices</param>
        /// <param name="packageQueryService">Service used for performing query operations for Packages</param>
        public UserCommandService(
            LogisticsCompanyContext dbContext,
            IDbAdapter dbAdapter,
            IUserQueryService userQueryService,
            IRoleQueryService roleQueryService,
            IOfficeQueryService officeQueryService,
            IPackageQueryService packageQueryService)
            : base(dbContext, dbAdapter)
        {
            _userQueryService = userQueryService;
            _roleQueryService = roleQueryService;
            _officeQueryService = officeQueryService;
            _packageQueryService = packageQueryService;
        }

        /// <summary>
        /// Updates an existing User entity
        /// based on the passed <paramref name="userDto"/>
        /// </summary>
        /// <param name="userDto">The Dto model user for updating an existing User entity.</param>
        public async Task Update(UserDto userDto)
        {
            var roleId = await _roleQueryService.GetIdByName(userDto.RoleName);

            int? officeId = null;
            if (userDto.OfficeName != null)
            {
                officeId = await _officeQueryService.GetIdByName(userDto.OfficeName);
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

            var command = SqlCommandHelper.UpdateCommand(
                    table: "Users",
                    entityType: typeof(User),
                    entityValues: keyValuePairs,
                    primaryKey: userDto.Id
            );

            await this._dbAdapter
                .ExecuteCommand(command);
        }

        /// <summary>
        /// Deletes an existing User entity
        /// based on the passed <paramref name="id"/>
        /// argument.
        /// </summary>
        /// <param name="id">The Id field of the User.</param>
        public async Task Delete(int id)
        {
            var user = _userQueryService.GetById(id);

            if (user == null) return;

            
            var keyValuePairs = new Dictionary<string, string>()
            {
                {"FromId", "NULL"},
                {"ToId", "NULL"}
            };

            var packages = await _packageQueryService.GetPackagesByUserId(id);

            if (packages.Count() > 0)
            {
                foreach (var package in packages)
                {
                    var updatePackageCommand = SqlCommandHelper
                        .UpdateCommand
                        (
                            table: "Packages",
                            entityType: typeof(Data.Entity.Package),
                            entityValues: keyValuePairs,
                            primaryKey: package.Id
                        );

                    await this._dbAdapter
                        .ExecuteCommand(updatePackageCommand);
                }
            }

            var command = SqlCommandHelper.DeleteCommand(table: "Users", primaryKey: "Id");
            
            await this._dbAdapter
                .ExecuteCommand(command, new { criteriaValue = id });
        }
    }
}
