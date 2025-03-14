using Api.DTOs;
using Api.Libraries.AddressLibrary.Models;

namespace Api.Libraries.AddressLibrary.Repositories
{
    public interface IAddressRepository
    {
        Task<Address> GetAddressByIdAsync(int id);
        Task<IEnumerable<Address>> GetAllAddressesAsync();
        Task<int> AddAddressAsync(RequestAddress address);
        Task UpdateAddressAsync(int id, RequestAddress address);
        Task DeleteAddressAsync(int id);
    }
}
