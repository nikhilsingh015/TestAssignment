using Api.DTOs;
using Api.Libraries.AddressLibrary.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/address")]
    public class AddressController(IAddressRepository addressRepository) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAddresses()
        {
            try
            {
                var addresses = await addressRepository.GetAllAddressesAsync();
                return Ok(addresses);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAddress(int id)
        {
            try
            {
                var address = await addressRepository.GetAddressByIdAsync(id);
                return Ok(address);
            }
            catch (KeyNotFoundException ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateAddress([FromBody] RequestAddress address)
        {
            try
            {
                if (string.IsNullOrEmpty(address.Email))
                {
                    return BadRequest("Email is required.");
                }

                var rowId = await addressRepository.AddAddressAsync(address);
                return Ok(rowId);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500);
            }
        }

        [HttpPut("{id:int}")]
public async Task<IActionResult> UpdateAddress(int id, [FromBody] RequestAddress address)
{
    try
    {
        if (string.IsNullOrEmpty(address.Email))
        {
            return BadRequest("Email is required.");
        }

        await addressRepository.UpdateAddressAsync(id, address); // Await repository call directly
        return NoContent(); // Return directly without wrapping in Task<>
    }
    catch (Exception e)
    {
        Console.WriteLine(e);
        return StatusCode(500);
    }
}


        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteAddress(int id)
        {
            try
            {
                _ = await addressRepository.GetAddressByIdAsync(id);

                await addressRepository.DeleteAddressAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500);
            }
        }
    }
}
