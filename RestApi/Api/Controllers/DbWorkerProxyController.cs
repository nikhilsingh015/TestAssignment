namespace Api.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Collections.Generic;
    using System.Text.Json;/// <summary>
    /// Controller for proxying requests to the DbWorker service.
    /// </summary>
    /// <remarks>
    /// This controller uses an HTTP client factory to send requests to the DbWorker service
    /// and retrieve address data. It requires the 'DbWorkerUrl' configuration setting.
    /// </remarks>
    /// <param name="httpClientFactory">Factory for creating HTTP clients.</param>
    /// <param name="configuration">Configuration settings for the application.</param>
    /// <returns>A list of addresses if the request is successful; otherwise, a 404 response.</returns>
    using Microsoft.Extensions.Configuration;
    using System.ComponentModel.DataAnnotations;
    using Api.Models;

    [ApiController]
    [Route("api/[controller]")]
    public class DbWorkerProxyController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly string _baseUrl;

        public DbWorkerProxyController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _baseUrl = _configuration["DbWorkerUrl"] ?? throw new InvalidOperationException("DbWorkerUrl configuration is missing");
        }

        [HttpGet]
        /// <summary>
        /// Retrieves a list of addresses from the DbWorker service.
        /// </summary>
        /// <returns>
        /// An <see cref="IActionResult"/> containing a list of addresses if successful; otherwise, a 404 response.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the 'DbWorkerUrl' configuration is missing.
        /// </exception>
        public async Task<IActionResult> GetAddresses()
        {
            var httpClient = _httpClientFactory.CreateClient();
            var response = await httpClient.GetAsync($"{_baseUrl}/api/addresses");

            if (response.IsSuccessStatusCode)
            {
                var addresses = await response.Content.ReadFromJsonAsync<List<Address>>();
                return Ok(addresses);
            }
            return NotFound();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAddress(int id)
        {
            var httpClient = _httpClientFactory.CreateClient();
            var response = await httpClient.GetAsync($"{_baseUrl}/api/addresses/{id}");

            if (response.IsSuccessStatusCode)
            {
                var address = await response.Content.ReadFromJsonAsync<Address>();
                return Ok(address);
            }
            return NotFound();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> CreateAddress([FromBody] Address address)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var httpClient = _httpClientFactory.CreateClient();
            var response = await httpClient.PostAsJsonAsync($"{_baseUrl}/api/addresses", address);

            if (response.IsSuccessStatusCode)
            {
                var createdAddress = await response.Content.ReadFromJsonAsync<Address>();
                return CreatedAtAction(nameof(GetAddress), new { id = createdAddress.Id }, createdAddress);
            }
            return BadRequest();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAddress(int id, [FromBody] Address address)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var httpClient = _httpClientFactory.CreateClient();
            var response = await httpClient.PutAsJsonAsync($"{_baseUrl}/api/addresses/{id}", address);

            if (response.IsSuccessStatusCode)
            {
                var updatedAddress = await response.Content.ReadFromJsonAsync<Address>();
                return Ok(updatedAddress);
            }
            return response.StatusCode == System.Net.HttpStatusCode.NotFound ? NotFound() : BadRequest();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAddress(int id)
        {
            var httpClient = _httpClientFactory.CreateClient();
            var response = await httpClient.DeleteAsync($"{_baseUrl}/api/addresses/{id}");

            if (response.IsSuccessStatusCode)
                return NoContent();
            
            return response.StatusCode == System.Net.HttpStatusCode.NotFound ? NotFound() : BadRequest();
        }
    }
}
