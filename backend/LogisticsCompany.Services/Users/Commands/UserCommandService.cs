using Dapper;
using LogisticsCompany.Data;
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
    public class UserCommandService : BaseService, IUserCommandService
    {
        private readonly IUserQueryService _userQueryService;
        private readonly IRoleQueryService _roleQueryService;
        private readonly IOfficeQueryService _officeQueryService;
        private readonly IPackageQueryService _packageQueryService;
        
        public UserCommandService(
            LogisticsCompanyContext dbContext,
            IUserQueryService userQueryService,
            IRoleQueryService roleQueryService,
            IOfficeQueryService officeQueryService,
            IPackageQueryService packageQueryService)
            : base(dbContext)
        {
            _userQueryService = userQueryService;
            _roleQueryService = roleQueryService;
            _officeQueryService = officeQueryService;
            _packageQueryService = packageQueryService;
        }

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
            var user = _userQueryService.GetById(id);

            if (user == null) return;

            using (var connection = new SqlConnection(_connectionString))
            {
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
    }
}
