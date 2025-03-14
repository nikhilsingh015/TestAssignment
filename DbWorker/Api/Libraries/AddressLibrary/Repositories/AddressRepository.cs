using Api.DTOs;
using Api.Libraries.AddressLibrary.Db;
using Api.Libraries.AddressLibrary.Models;
using Dapper;

namespace Api.Libraries.AddressLibrary.Repositories;

public class AddressRepository(IDbConnectionFactory db) : IAddressRepository
{
    public async Task<Address> GetAddressByIdAsync(int id)
    {
        await using var connection = db.CreateConnection();
        const string sql = """
            SELECT * 
            FROM Addresses 
            WHERE 
                Id = @Id;
            """;
        var address = await connection.QuerySingleOrDefaultAsync<Address>(sql, new { Id = id });
        if (address == null)
        {
            throw new KeyNotFoundException($"Address with ID {id} was not found");
        }

        return address;
    }

    public async Task<IEnumerable<Address>> GetAllAddressesAsync()
    {
        await using var connection = db.CreateConnection();
        const string sql = """
            SELECT * 
            FROM Addresses;
            """;
        return await connection.QueryAsync<Address>(sql);
    }

    public async Task<int> AddAddressAsync(RequestAddress address)
    {
        await using var connection = db.CreateConnection();
        const string sql = """
            INSERT INTO Addresses 
                (Street, City, State, ZipCode, Name, Country ) 
            VALUES 
                (@Street, @City, @State, @ZipCode ,@Name,@Country);
            SELECT last_insert_rowid();
            """;
        return await connection.ExecuteScalarAsync<int>(sql, address);
    }

    public async Task UpdateAddressAsync(int id, RequestAddress address)
    {
        await using var connection = db.CreateConnection();
        const string sql = """
            UPDATE Addresses 
            SET Street = @Street, 
                City = @City, 
                State = @State, 
                ZipCode = @ZipCode, 
                Name=@Name,
                Country=@Country
            WHERE 
                Id = @Id;
            """;

        await connection.ExecuteAsync(
            sql,
            new
            {
                Id = id,
                address.Street,
                address.City,
                address.State,
                address.ZipCode,
                address.Name,
                address.Country,
            }
        );
    }

    public async Task DeleteAddressAsync(int id)
    {
        await using var connection = db.CreateConnection();
        const string sql = """
                DELETE FROM Addresses 
                WHERE 
                    Id = @Id
            """;
        await connection.ExecuteAsync(sql, new { Id = id });
    }
}
