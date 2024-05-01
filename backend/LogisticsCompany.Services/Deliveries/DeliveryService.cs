using Dapper;
using LogisticsCompany.Data;
using LogisticsCompany.Data.Builders;
using LogisticsCompany.Data.Common;
using LogisticsCompany.Data.Entity;
using LogisticsCompany.Data.Helpers;
using LogisticsCompany.Services.Contracts;
using LogisticsCompany.Services.Dto;
using Microsoft.Data.SqlClient;
using System.Text;

namespace LogisticsCompany.Services.Deliveries
{
    public class DeliveryService : BaseService, IDeliveryService
    {
        private readonly IPackageService _packageService;
        public DeliveryService(LogisticsCompanyContext dbContext, IPackageService packageService) :
            base(dbContext)
        {
            _packageService = packageService;
        }

        private async Task<bool> VerifyIdsAsync(int[] ids)
        {
            var result = false;
            if (ids.Length > 0)
            {
                foreach (var selectedId in ids)
                {
                    var package = await _packageService.GetById(selectedId);
                    if (package != null)
                    {
                        if (package.DeliveryId != null && package.DeliveryId != 0)
                        {
                            result = false;
                        }
                        else
                        {
                            result = true;
                        }
                    }
                }
            }

            return result;
        }

        public async Task<IEnumerable<DeliveryDto>> GetAll()
        {
            var query = new SqlQueryBuilder()
                .Select("*")
                .From(table: "Deliveries")
                .ToQuery();

            using (var connection = new SqlConnection(_connectionString))
            {
                var result = await connection.QueryAsync<DeliveryDto>(query);

                return result;
            }
        }


        public async Task<DeliveryDto?> GetById(int id)
        {
            var clauseDescriptorContainer = new ClauseDescriptorContainer()
                .Descriptors(descriptors =>
                {
                    descriptors.Add(descriptor => descriptor
                        .Field("Id")
                        .FieldValue(id)
                        .EqualityOperator(EqualityOperator.EQUALS)
                    );
                });

            using (var connection = new SqlConnection(_connectionString))
            {
                var query = new SqlQueryBuilder()
                    .Select(columns: "*")
                    .From(table: "Deliveries")
                    .Where(clauseDescriptorContainer)
                    .ToQuery();

                var result = await connection.QuerySingleOrDefaultAsync<DeliveryDto>(query);

                return result;
            }
        }

        public async Task<string> Create(DeliveryDto dto)
        {
            var sb = new StringBuilder();
            using (var connection = new SqlConnection(_connectionString))
            {
                var dateOnly = DateOnly.FromDateTime(DateTime.Now);
                var parsedDate = $"{dateOnly.Year}-{dateOnly.Month}-{dateOnly.Day}";
                var command = SqlCommandHelper.InsertCommand("Deliveries", $"'{parsedDate}'", "NULL");

                command = command += "SELECT CAST(SCOPE_IDENTITY() as int)";

                var isVerified = await VerifyIdsAsync(dto.SelectedIds);

                if (isVerified)
                {
                    var identityCreated = await connection.QuerySingleAsync<int>(command);

                    if (dto.SelectedIds.Length > 0)
                    {
                        foreach (var selectedId in dto.SelectedIds)
                        {
                            var package = await _packageService.GetById(selectedId);
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
                                    await _packageService.Update(package);
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

            }

            return sb.ToString();
        }

        public async Task Update(DeliveryDto dto)
        {

            var delivery = await GetById(dto.Id);
            
            if(delivery == null)
            {
                return;
            }

            var dateOnly = DateOnly.FromDateTime(DateTime.Now);

            var keyValuePairs = new Dictionary<string, string>()
            {
                {"EndDate", $"{dateOnly.Year}-{dateOnly.Month}-{dateOnly.Day}" }
            };

            using (var connection = new SqlConnection(_connectionString))
            {
                var command = SqlCommandHelper.UpdateCommand(
                    table: "Deliveries",
                    entityType: typeof(Delivery),
                    entityValues: keyValuePairs,
                    primaryKey: dto.Id);

                await connection.ExecuteAsync(command);
            }
        }
    }
}
