using Microsoft.Data.Sqlite;

namespace Api.Libraries.AddressLibrary.Db
{
    public interface IDbConnectionFactory : IDisposable
    {
        SqliteConnection CreateConnection();
    }
}
