using Microsoft.Data.Sqlite;

namespace Api.Libraries.AddressLibrary.Db
{
    public class SqliteConnectionFactory(string connectionString) : IDbConnectionFactory
    {
        private readonly string? _connectionString = connectionString;
        private SqliteConnection? _connection;

        public SqliteConnection CreateConnection()
        {
            if (_connection == null)
            {
                _connection = new SqliteConnection(_connectionString);
                _connection.Open();
            }
            return _connection;
        }

        public void Dispose()
        {
            if (_connection != null)
            {
                _connection.Close();
                _connection.Dispose();
                _connection = null;
            }
        }
    }
}
