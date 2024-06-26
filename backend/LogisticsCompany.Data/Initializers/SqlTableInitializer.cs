﻿using Dapper;
using LogisticsCompany.Data.Contracts;
using LogisticsCompany.Data.Helpers;
using Microsoft.Data.SqlClient;

using static LogisticsCompany.Data.Helpers.SqlConstraintHelper;

namespace LogisticsCompany.Data.Initializers
{
    /// <summary>
    /// Initializer Class used for constructing SQL Table Schemas
    /// </summary>
    public class SqlTableInitializer : IInitializer
    {
        internal string _connectionString { get; set; }

        public SqlTableInitializer(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// Method used for creating SQL Tables.
        /// </summary>
        /// <returns></returns>
        public async Task Init()
        {
            await InitRoles();
            await InitPackageStatuses();
            await InitDeliveries();
            await InitOffices();
            await InitUsers();
            await InitPackages();
        }

        private async Task InitPackageStatuses()
        {
            using (var connection = new SqlConnection(_connectionString))
            {

                var sql = """
                      IF OBJECT_ID('PackageStatuses', 'U') IS NULL
                      CREATE TABLE PackageStatuses(
                         Id INT NOT NULL PRIMARY KEY IDENTITY (1, 1),
                         Name NVARCHAR(MAX)
                      )
                      """;

                await connection.ExecuteAsync(sql);
            }
        }

        private async Task InitPackages()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var sql = $"""
                IF OBJECT_ID('Packages', 'U') IS NULL
                CREATE TABLE Packages (
                    Id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
                    FromId INT NULL,
                    ToId INT NULL,
                    PackageStatusId INT NULL,
                    OfficeId INT NULL,
                    DeliveryId INT NULL,
                    Address NVARCHAR(MAX) NOT NULL,
                    ToOffice BIT,
                    Weight INT,
                    {ForeignKeyConstraint("fk_package_delivery", "DeliveryId", "dbo.Deliveries", "Id")} ON DELETE SET NULL,
                    {ForeignKeyConstraint("fk_package_office", "OfficeId", "dbo.Offices", "Id")} ON DELETE SET NULL,
                    {ForeignKeyConstraint("fk_from", "FromId", "dbo.Users", "Id")} ON DELETE NO ACTION,
                    {ForeignKeyConstraint("fk_to", "ToId", "dbo.Users", "Id")} ON DELETE NO ACTION,
                    {ForeignKeyConstraint("fk_packageStatus", "PackageStatusId", "dbo.PackageStatuses", "Id")} ON DELETE SET NULL
                )
                """;

                await connection.ExecuteAsync(sql);
            }
        }
        private async Task InitUsers()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var sql = $"""
                IF OBJECT_ID('Users', 'U') IS NULL
                CREATE TABLE Users (
                    Id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
                    Username NVARCHAR(MAX) NOT NULL,
                    FirstName NVARCHAR(MAX) NOT NULL,
                    LastName NVARCHAR(MAX) NOT NULL,
                    Email NVARCHAR(MAX) NOT NULL,
                    RoleId INT,
                    OfficeId INT NULL,
                    PasswordHash NVARCHAR(MAX) NOT NULL,
                    {ForeignKeyConstraint("fk_role", "RoleId", "dbo.Roles", "Id")},
                    {ForeignKeyConstraint("fk_office", "OfficeId", "dbo.Offices", "Id")} ON DELETE SET NULL
                )
                """;

                await connection.ExecuteAsync(sql);
            }
        }

        private async Task InitRoles()
        {

            using (var connection = new SqlConnection(_connectionString))
            {

                var sql = """
                      IF OBJECT_ID('Roles', 'U') IS NULL
                      CREATE TABLE Roles(
                         Id INT NOT NULL PRIMARY KEY IDENTITY (1, 1),
                         Name NVARCHAR(MAX)
                      )
                      """;

                await connection.ExecuteAsync(sql);
            }
        }

        private async Task InitOffices()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var sql = """
                      IF OBJECT_ID('Offices', 'U') IS NULL
                      CREATE TABLE Offices(
                         Id INT NOT NULL PRIMARY KEY IDENTITY (1, 1),
                         Address NVARCHAR(MAX),
                         PricePerWeight DECIMAL(10,2)
                      )
                      """;

                await connection.ExecuteAsync(sql);
            }
        }

        private async Task InitDeliveries()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var sql = """
                      IF OBJECT_ID('Deliveries', 'U') IS NULL
                      CREATE TABLE Deliveries(
                         Id INT NOT NULL PRIMARY KEY IDENTITY (1, 1),
                         StartDate DATE NOT NULL,
                         EndDate DATE NULL
                      )
                      """;

                await connection.ExecuteAsync(sql);
            }
        }
    }
}
