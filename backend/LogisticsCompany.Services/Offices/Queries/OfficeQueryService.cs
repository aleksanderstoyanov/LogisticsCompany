﻿using Dapper;
using LogisticsCompany.Data;
using LogisticsCompany.Data.Builders;
using LogisticsCompany.Data.Common.Adapters;
using LogisticsCompany.Data.Common.Descriptors;
using LogisticsCompany.Data.Common.Operators;
using LogisticsCompany.Data.Contracts;
using LogisticsCompany.Services.Offices.Dto;
using Microsoft.Data.SqlClient;

namespace LogisticsCompany.Services.Offices.Queries
{
    /// <summary>
    /// A <see cref="BaseService"/> class used for performing DataBase query operations for Offices.
    /// </summary>
    public class OfficeQueryService : BaseService, IOfficeQueryService
    {
        /// <summary>
        /// Creates a <see cref="OfficeQueryService"/> instance 
        /// with the injected <paramref name="dbContext"/> and <paramref name="dbAdapter"/>
        /// arguments.
        /// </summary>
        /// <param name="dbContext">The Database context.</param>
        /// <param name="dbAdapter">The Database adapter that will instantiate a connection and execute the constructed query.</param>
        public OfficeQueryService(LogisticsCompanyContext dbContext, 
            IDbAdapter dbAdapter)
            : base(dbContext, dbAdapter)
        {
        }

        /// <summary>
        /// Performs an SQL Query for retrieving the Id based on the passed
        /// <paramref name="name"/>
        /// </summary>
        /// <param name="name">The Address which should be queried with.</param>
        /// <returns>
        ///  An id.
        /// </returns>
        public async Task<int?> GetIdByName(string name)
        {
            var clauseContainer = new ClauseDescriptorContainer();

            clauseContainer.Descriptors(descriptors =>
            {
                descriptors.Add(descriptor => descriptor
                    .Field("Address")
                    .EqualityOperator(EqualityOperator.EQUALS)
                    .FieldValue(name)
                );
            });

            var query = new SqlQueryBuilder()
                .Select(columns: "Id")
                .From(table: "Offices")
                .Where(clauseContainer)
                .ToQuery();


            var result = await _dbAdapter.QuerySingle<int?>(query);

            return result ?? null;
        }

        /// <summary>
        /// Performs an SQL Query for retrieving an existing Office entity
        /// based on the passed <paramref name="id"/>.
        /// </summary>
        /// <param name="id">The which should be queried with</param>
        /// <returns>
        ///  An <see cref="OfficeDto"/> model.
        /// </returns>
        public async Task<OfficeDto?> GetById(int id)
        {
            var clauseContainer = new ClauseDescriptorContainer();

            clauseContainer
                .Descriptors(descriptors =>
                {
                    descriptors.Add(descriptor => descriptor
                        .Field("Id")
                        .FieldValue(id)
                        .EqualityOperator(EqualityOperator.EQUALS)
                    );
                });

            var query = new SqlQueryBuilder()
                       .Select(columns: "*")
                       .From(table: "Offices")
                       .Where(clauseContainer)
                       .ToQuery();

            var office = await _dbAdapter
                .QuerySingle<OfficeDto>(query);

            return office;
        }

        /// <summary>
        /// Performs an SQL Query for retrieving an existing Office
        /// based on the passed <paramref name="name"/>
        /// </summary>
        /// <param name="name">The address which should be queried with.</param>
        /// <returns>
        ///  An <see cref="OfficeDto"/> model.
        /// </returns>
        public async Task<OfficeDto?> GetByAddress(string name)
        {
            var clauseDescriptorContainer = new ClauseDescriptorContainer();

            clauseDescriptorContainer
               .Descriptors(descriptors =>
               {
                   descriptors.Add(descriptor => descriptor
                       .Field("Address")
                       .FieldValue(name)
                       .EqualityOperator(EqualityOperator.EQUALS)
                   );
               });

            var query = new SqlQueryBuilder()
                .Select(columns: "*")
                .From(table: "Offices")
                .Where(clauseDescriptorContainer)
                .ToQuery();

            var office = await _dbAdapter.QuerySingle<OfficeDto>(query);

            return office;
        }

        /// <summary>
        /// Performs an SQL Query for retrieving all Office entites from the Database.
        /// </summary>
        /// <returns>
        /// <see cref="IEnumerable{OfficeDto}"/> collection of offices.
        /// </returns>
        public async Task<IEnumerable<OfficeDto>> GetAll()
        {
            var query = new SqlQueryBuilder()
                   .Select(columns: "*")
                   .From(table: "Offices")
                   .ToQuery();

            var offices = await _dbAdapter.QueryAll<OfficeDto>(query);
            return offices;
        }
    }
}
