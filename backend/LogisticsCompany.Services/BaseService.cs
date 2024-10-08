﻿using LogisticsCompany.Data;
using LogisticsCompany.Data.Contracts;

namespace LogisticsCompany.Services
{
    /// <summary>
    /// A class that contains basic service logic.
    /// </summary>
    public class BaseService
    {
        protected readonly string _connectionString;
        protected readonly LogisticsCompanyContext _dbContext;
        protected readonly IDbAdapter _dbAdapter;

        /// <summary>
        /// Creates a <see cref="BaseService"/> instance based on the passed
        /// <paramref name="dbContext"/> argument.
        /// </summary>
        /// <param name="dbContext">The Database context</param>
        public BaseService(LogisticsCompanyContext dbContext, IDbAdapter dbAdapter)
        {
            this._dbContext = dbContext;
            this._connectionString = this._dbContext.GetConnectionString();
            this._dbAdapter = dbAdapter;
        }
    }
}
