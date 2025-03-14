using Xunit;
using Moq;
using Moq.Protected;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Json;
using System.Collections.Generic;
using Api.Controllers;
using Microsoft.Extensions.Configuration;
using System.Threading;
using Api.Models;
using System.Net;

namespace RestApi.Api.Tests
{
    public class DbWorkerProxyControllerTests
    {
        private readonly Mock<IHttpClientFactory> _httpClientFactoryMock;
        private readonly Mock<HttpMessageHandler> _httpMessageHandlerMock;
        private readonly HttpClient _httpClient;
        private readonly Mock<IConfiguration> _configurationMock;

        public DbWorkerProxyControllerTests()
        {
            _httpMessageHandlerMock = new Mock<HttpMessageHandler>();
            _httpClient = new HttpClient(_httpMessageHandlerMock.Object);
            _httpClientFactoryMock = new Mock<IHttpClientFactory>();
            _httpClientFactoryMock.Setup(f => f.CreateClient(It.IsAny<string>())).Returns(_httpClient);
            _configurationMock = new Mock<IConfiguration>();
            _configurationMock.Setup(x => x["DbWorkerUrl"]).Returns("http://localhost:5000");
        }

        private void SetupHttpMessageHandlerMock(HttpStatusCode statusCode, HttpContent content = null)
        {
            _httpMessageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = statusCode,
                    Content = content
                });
        }

        [Fact]
        public async Task GetAddresses_ReturnsOkResult_WithListOfAddresses()
        {
            // Arrange
            var expectedAddresses = new List<Address>
            {
                new Address
                {
                    Id = 1,
                    Name = "Test Name",
                    City = "Test City",
                    Country = "Test Country",
                    Email = "test@example.com"
                }
            };

            SetupHttpMessageHandlerMock(HttpStatusCode.OK, JsonContent.Create(expectedAddresses));

            var controller = new DbWorkerProxyController(_httpClientFactoryMock.Object, _configurationMock.Object);

            // Act
            var result = await controller.GetAddresses();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedAddresses = Assert.IsType<List<Address>>(okResult.Value);
            Assert.Single(returnedAddresses);

            var expectedAddress = expectedAddresses[0];
            var returnedAddress = returnedAddresses[0];

            Assert.Equal(expectedAddress.Id, returnedAddress.Id);
            Assert.Equal(expectedAddress.Name, returnedAddress.Name);
            Assert.Equal(expectedAddress.City, returnedAddress.City);
            Assert.Equal(expectedAddress.Country, returnedAddress.Country);
            Assert.Equal(expectedAddress.Email, returnedAddress.Email);
        }

        [Fact]
        public async Task GetAddresses_ReturnsNotFoundResult_WhenNoAddresses()
        {
            // Arrange
            SetupHttpMessageHandlerMock(HttpStatusCode.NotFound);

            var controller = new DbWorkerProxyController(_httpClientFactoryMock.Object, _configurationMock.Object);

            // Act
            var result = await controller.GetAddresses();

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task GetAddresses_ThrowsException_WhenHttpRequestFails()
        {
            // Arrange
            _httpMessageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .Throws(new HttpRequestException());

            var controller = new DbWorkerProxyController(_httpClientFactoryMock.Object, _configurationMock.Object);

            // Act and Assert
            await Assert.ThrowsAsync<HttpRequestException>(() => controller.GetAddresses());
        }

        [Fact]
        public async Task GetAddress_ReturnsOkResult_WithAddress()
        {
            // Arrange
            var expectedAddress = new Address
            {
                Id = 1,
                Name = "Test Name",
                City = "Test City",
                Country = "Test Country",
                Email = "test@example.com"
            };

            SetupHttpMessageHandlerMock(HttpStatusCode.OK, JsonContent.Create(expectedAddress));

            var controller = new DbWorkerProxyController(_httpClientFactoryMock.Object, _configurationMock.Object);

            // Act
            var result = await controller.GetAddress(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedAddress = Assert.IsType<Address>(okResult.Value);

            Assert.Equal(expectedAddress.Id, returnedAddress.Id);
            Assert.Equal(expectedAddress.Name, returnedAddress.Name);
            Assert.Equal(expectedAddress.City, returnedAddress.City);
            Assert.Equal(expectedAddress.Country, returnedAddress.Country);
            Assert.Equal(expectedAddress.Email, returnedAddress.Email);
        }

        [Fact]
        public async Task CreateAddress_ReturnsCreatedResult_WhenAddressIsValid()
        {
            // Arrange
            var newAddress = new Address
            {
                Name = "Test Name",
                City = "Test City",
                Country = "Test Country",
                Email = "test@example.com"
            };

            var createdAddress = new Address
            {
                Id = 1,
                Name = newAddress.Name,
                City = newAddress.City,
                Country = newAddress.Country,
                Email = newAddress.Email
            };

            SetupHttpMessageHandlerMock(HttpStatusCode.Created, JsonContent.Create(createdAddress));

            var controller = new DbWorkerProxyController(_httpClientFactoryMock.Object, _configurationMock.Object);

            // Act
            var result = await controller.CreateAddress(newAddress);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            var returnedAddress = Assert.IsType<Address>(createdAtActionResult.Value);

            Assert.Equal(1, returnedAddress.Id);
            Assert.Equal(newAddress.Name, returnedAddress.Name);
            Assert.Equal(newAddress.City, returnedAddress.City);
            Assert.Equal(newAddress.Country, returnedAddress.Country);
            Assert.Equal(newAddress.Email, returnedAddress.Email);
        }

        [Fact]
        public async Task CreateAddress_ReturnsBadRequest_WhenModelStateIsInvalid()
        {
            // Arrange
            var controller = new DbWorkerProxyController(_httpClientFactoryMock.Object, _configurationMock.Object);

            // Initialize required properties
            var newAddress = new Address
            {
                Name = "Test Name",
                City = "Test City",
                Country = "Test Country",
                Email = "test@example.com"
            };

            controller.ModelState.AddModelError("Email", "Required");

            // Act
            var result = await controller.CreateAddress(newAddress);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task UpdateAddress_ReturnsOkResult_WhenAddressExists()
        {
            // Arrange
            var addressToUpdate = new Address
            {
                Id = 1,
                Name = "Updated Name",
                City = "Updated City",
                Country = "Updated Country",
                Email = "updated@example.com"
            };

            SetupHttpMessageHandlerMock(HttpStatusCode.OK, JsonContent.Create(addressToUpdate));

            var controller = new DbWorkerProxyController(_httpClientFactoryMock.Object, _configurationMock.Object);

            // Act
            var result = await controller.UpdateAddress(1, addressToUpdate);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedAddress = Assert.IsType<Address>(okResult.Value);

            Assert.Equal(addressToUpdate.Name, returnedAddress.Name);
            Assert.Equal(addressToUpdate.City, returnedAddress.City);
            Assert.Equal(addressToUpdate.Country, returnedAddress.Country);
            Assert.Equal(addressToUpdate.Email, returnedAddress.Email);
        }

        [Fact]
        public async Task UpdateAddress_ReturnsNotFound_WhenAddressDoesNotExist()
        {
            // Arrange
            SetupHttpMessageHandlerMock(HttpStatusCode.NotFound);

            var controller = new DbWorkerProxyController(_httpClientFactoryMock.Object, _configurationMock.Object);

            // Initialize required properties
            var addressToUpdate = new Address
            {
                Name = "Test Name",
                City = "Test City",
                Country = "Test Country",
                Email = "test@example.com"
            };

            // Act
            var result = await controller.UpdateAddress(1, addressToUpdate);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeleteAddress_ReturnsNoContent_WhenAddressExists()
        {
            // Arrange
            SetupHttpMessageHandlerMock(HttpStatusCode.OK);

            var controller = new DbWorkerProxyController(_httpClientFactoryMock.Object, _configurationMock.Object);

            // Act
            var result = await controller.DeleteAddress(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteAddress_ReturnsNotFound_WhenAddressDoesNotExist()
        {
            // Arrange
            SetupHttpMessageHandlerMock(HttpStatusCode.NotFound);

            var controller = new DbWorkerProxyController(_httpClientFactoryMock.Object, _configurationMock.Object);

            // Act
            var result = await controller.DeleteAddress(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
        [Fact]
        public async Task GetAddress_ReturnsNotFoundResult_WhenAddressDoesNotExist()
        {
            // Arrange
            SetupHttpMessageHandlerMock(HttpStatusCode.NotFound);

            var controller = new DbWorkerProxyController(_httpClientFactoryMock.Object, _configurationMock.Object);

            // Act
            var result = await controller.GetAddress(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

    }
}
