using System.Data;
using System.Data.SqlClient;

namespace Server.Repositories
{
    public abstract class SqlConnectionProvider
    {
        private readonly string _connectionString;

        protected SqlConnectionProvider(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IDbConnection GetOpenConnection()
        {
            return new SqlConnection(_connectionString);
        }

        public SqlBulkCopy GetOpenBulkInsertConnection()
        {
            return new SqlBulkCopy(_connectionString);
        }
    }
}