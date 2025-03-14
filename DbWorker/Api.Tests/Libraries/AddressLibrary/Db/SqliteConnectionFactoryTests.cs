using Api.Libraries.AddressLibrary.Db;

namespace Api.Tests.Libraries.AddressLibrary.Db
{
    public class SqliteConnectionFactoryTests : IDisposable
    {
        private readonly SqliteConnectionFactory _factory;
        private const string TestConnectionString = "Data Source=:memory:";

        public SqliteConnectionFactoryTests()
        {
            _factory = new SqliteConnectionFactory(TestConnectionString);
        }

        [Fact]
        public void CreateConnection_ReturnsOpenConnection()
        {
            // Act
            var connection = _factory.CreateConnection();

            // Assert
            Assert.NotNull(connection);
            Assert.Equal(System.Data.ConnectionState.Open, connection.State);
        }

        [Fact]
        public void CreateConnection_ReturnsSameInstance_WhenCalledMultipleTimes()
        {
            // Act
            var connection1 = _factory.CreateConnection();
            var connection2 = _factory.CreateConnection();

            // Assert
            Assert.Same(connection1, connection2);
        }

        [Fact]
        public void Dispose_ClosesAndDisposesConnection()
        {
            // Arrange
            var connection = _factory.CreateConnection();

            // Act
            _factory.Dispose();

            // Assert
            Assert.Equal(System.Data.ConnectionState.Closed, connection.State);
        }

        public void Dispose()
        {
            _factory.Dispose();
        }
    }
}
