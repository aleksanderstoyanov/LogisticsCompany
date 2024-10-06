using Dapper;
using System.Text;
using LogisticsCompany.Data;
using LogisticsCompany.Data.Entity;
using LogisticsCompany.Data.Helpers;
using LogisticsCompany.Services.Deliveries.Queries;
using LogisticsCompany.Services.Dto;
using LogisticsCompany.Services.Package.Commands;
using LogisticsCompany.Services.Package.Queries;
using Microsoft.Data.SqlClient;
using LogisticsCompany.Data.Contracts;

namespace LogisticsCompany.Services.Deliveries.Commands
{
    /// <summary>
    /// A <see cref="BaseService"/> class used for performing DataBase command operations for Deliveries.
    /// </summary>
    public class DeliveryCommandService : BaseService, IDeliveryCommandService
    {
        private readonly IPackageQueryService _packageQueryService;
        private readonly IDeliveryQueryService _deliveryQueryService;

        private readonly IPackageCommandService _packageCommandService;

        /// <summary>
        /// Creates a <see cref="DeliveryCommandService"/> instance
        /// with the injected <paramref name="dbContext"/>, <paramref name="dbAdapter"/>, <paramref name="packageQueryService"/>,
        /// <paramref name="deliveryQueryService"/>, and <paramref name="packageCommandService"/> 
        /// arguments.
        /// </summary>
        /// <param name="dbContext">The Database Context</param>
        /// <param name="packageQueryService">Service used for performing Package Query operations.</param>
        /// <param name="deliveryQueryService">Service use for performing Delivery Query operations.</param>
        /// <param name="packageCommandService">Service used for performing Package Command operations.</param>
        /// <param name="dbAdapter">The DataBase adapter that will instantiate a connection and process the constructed command.</param>
        public DeliveryCommandService(LogisticsCompanyContext dbContext,
            IDbAdapter dbAdapter,
            IPackageQueryService packageQueryService,
            IDeliveryQueryService deliveryQueryService,
            IPackageCommandService packageCommandService)
            : base(dbContext, dbAdapter)
        {
            _packageQueryService = packageQueryService;
            _deliveryQueryService = deliveryQueryService;
            _packageCommandService = packageCommandService;
        }

        /// <summary>
        /// Creates a Delivery Entity in the Database.
        /// based on the passed <paramref name="dto"/>
        /// </summary>
        /// <param name="dto">Model coming from the Controller API layer.</param>
        public async Task<string> Create(DeliveryDto dto)
        {
            var sb = new StringBuilder();
            var dateOnly = DateOnly.FromDateTime(DateTime.Now);
            var parsedDate = $"{dateOnly.Year}-{dateOnly.Month}-{dateOnly.Day}";
            var command = SqlCommandHelper.InsertCommand("Deliveries", $"'{parsedDate}'", "NULL");


            command = command += "SELECT CAST(SCOPE_IDENTITY() as int)";

            var isVerified = await VerifyIdsAsync(dto.SelectedIds);

            if (isVerified)
            {
                var identityCreated = await this._dbAdapter
                    .QuerySingle<int>(command);

                if (dto.SelectedIds.Length > 0)
                {
                    foreach (var selectedId in dto.SelectedIds)
                    {
                        var package = await _packageQueryService
                            .GetById(selectedId);

                        if (package != null)
                        {
                            if (package.DeliveryId != null && package.DeliveryId != 0)
                            {
                                dto.SelectedIds = dto.SelectedIds
                                    .Where(id => id != selectedId)
                                    .ToArray();
                            }
                            else
                            {
                                package.DeliveryId = identityCreated;
                                await _packageCommandService.Update(package);
                            }
                        }
                    }
                    sb.AppendLine($"Delivery has begun for: {string.Join(", ", dto.SelectedIds)}");
                }
            }
            else
            {
                sb.AppendLine($"Delivery has already began for records with: {string.Join(",", dto.SelectedIds)}");
            }

            return sb.ToString();
        }

        /// <summary>
        /// Updates an existing Delivery Entity
        /// based on the passed <paramref name="dto"/>
        /// </summary>
        /// <param name="dto">Model coming from the Controller API layer.</param>
        public async Task Update(DeliveryDto dto)
        {
            var delivery = await _deliveryQueryService
                .GetById(dto.Id);

            if (delivery == null)
                return;

            var dateOnly = DateOnly.FromDateTime(DateTime.Now);

            var keyValuePairs = new Dictionary<string, string>()
            {
                {"EndDate", $"{dateOnly.Year}-{dateOnly.Month}-{dateOnly.Day}" }
            };

            var command = SqlCommandHelper.UpdateCommand(
                table: "Deliveries",
                entityType: typeof(Delivery),
                entityValues: keyValuePairs,
                primaryKey: dto.Id);

            await this._dbAdapter
                .ExecuteCommand(command);
        }

        private async Task<bool> VerifyIdsAsync(int[] ids)
        {
            var result = false;

            if (ids.Length > 0)
            {
                foreach (var selectedId in ids)
                {
                    var package = await _packageQueryService.GetById(selectedId);
                    if (package != null)
                    {
                        if (package.DeliveryId != null && package.DeliveryId != 0)
                            result = false;
                        else
                            result = true;
                    }
                }
            }

            return result;
        }
    }
}
