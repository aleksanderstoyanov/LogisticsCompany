using Dapper;
using LogisticsCompany.Data;
using LogisticsCompany.Data.Builders;
using LogisticsCompany.Data.Common;
using LogisticsCompany.Data.Helpers;
using LogisticsCompany.Entity;
using LogisticsCompany.Services.Contracts;
using LogisticsCompany.Services.Dto;
using Microsoft.Data.SqlClient;
using System.Data;

namespace LogisticsCompany.Services.Offices
{
    public class OfficeService : BaseService, IOfficeService
    {
        public OfficeService(LogisticsCompanyContext dbContext)
            : base(dbContext)
        {
        }

        public async Task<int?> GetIdByName(string name)
        {
            var clauseContainer = new ClauseDescriptorContainer()
            {
                ClauseDescriptors = new HashSet<ClauseDescriptor>()
                {
                    new ClauseDescriptor
                    {
                        Field = "Address",
                        EqualityOperator = EqualityOperator.EQUALS,
                        FieldValue = name
                    }
                }
            };

            var query = new SqlQueryBuilder()
                .Select(columns: "Id")
                .From(table: "Offices")
                .Where(clauseContainer)
                .ToQuery();

            using (var connection = new SqlConnection(_connectionString))
            {
                var result = await connection.QueryFirstAsync<int?>(query);

                return result ?? null;
            }
        }
        public async Task<OfficeDto?> GetById(int id)
        {
            var clauseContainer = new ClauseDescriptorContainer()
            {
                ClauseDescriptors = new HashSet<ClauseDescriptor>()
                {
                    new ClauseDescriptor
                    {
                        Field = "Id",
                        EqualityOperator = EqualityOperator.EQUALS,
                        FieldValue = id,
                    }
                }
            };

            using (var connection = new SqlConnection(this._connectionString))
            {
                var query = new SqlQueryBuilder()
                        .Select(columns: "*")
                        .Where(clauseContainer)
                        .From(table: "Offices")
                        .ToQuery();

                var office = await connection.QuerySingleOrDefaultAsync<OfficeDto>(query);

                return office;
            }
        }

        public async Task<OfficeDto?> GetByAddress(string name)
        {
            var clauseDescriptorContainer = new ClauseDescriptorContainer()
            {
                ClauseDescriptors = new HashSet<ClauseDescriptor>
                {
                    new ClauseDescriptor
                    {
                        EqualityOperator = EqualityOperator.EQUALS,
                        Field = "Address",
                        FieldValue = name
                    }
                }
            };

            var query = new SqlQueryBuilder()
                .Select(columns: "*")
                .From(table: "Offices")
                .Where(clauseDescriptorContainer)
                .ToQuery();

            using (var connection = new SqlConnection(_connectionString))
            {
                var office = await connection.QuerySingleOrDefaultAsync<OfficeDto>(query);
                return office;
            }
        }
        public async Task<IEnumerable<OfficeDto>> GetAll()
        {
            using (var connection = new SqlConnection(this._connectionString))
            {
                var query = new SqlQueryBuilder()
                    .Select(columns: "*")
                    .From(table: "Offices")
                    .ToQuery();

                var offices = await connection.QueryAsync<OfficeDto>(query);
                return offices;
            }
        }

        public async Task<OfficeDto?> Create(OfficeDto dto)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = SqlCommandHelper.InsertCommand
                (
                    table: "Offices",
                    values: new[] { $"'{dto.Address}'" }
                );

                await connection.ExecuteAsync(command);

                var office = await GetByAddress(dto.Address);

                return office ?? null;
            }
        }

        public async Task Update(OfficeDto dto)
        {
            var entityValues = new Dictionary<string, string>()
            {
                { "Address", dto.Address }
            };

            using (var connection = new SqlConnection(this._connectionString))
            {
                var command = SqlCommandHelper.UpdateCommand(
                    table: "Offices",
                    entityType: typeof(Office),
                    entityValues: entityValues,
                    dto.Id
                );

                await connection.ExecuteAsync(command);
            }
        }

        public async Task Delete(int id)
        {
            var office = GetById(id);

            if (office == null)
                return;

            using (var connection = new SqlConnection(_connectionString))
            {
                var command = SqlCommandHelper.DeleteCommand("Offices", "Id");

                await connection.ExecuteAsync(command, new { criteriaValue = id });
            }
        }

    }
}
