﻿using Microsoft.Data.SqlClient;
using System.Data;

namespace BlogApi.Context
{
    /// <summary>
    /// Wrapper class for database context.
    /// </summary>
    public class DapperContext
    {
        private string _connectionString;

        /// <summary>
        /// Create new instance of DapperContext.
        /// </summary>
        /// <param name="configuration">Contains a connection string to the target database</param>
        public DapperContext(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("SqlServer");
        }

        /// <summary>
        /// Creates a new connection to the database.
        /// </summary>
        /// <returns>The db connection</returns>
        public IDbConnection CreateConnection() => new SqlConnection(_connectionString);
    }

}
