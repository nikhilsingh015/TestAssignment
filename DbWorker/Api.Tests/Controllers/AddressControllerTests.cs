using Api.Controllers;
using Api.DTOs;
using Api.Libraries.AddressLibrary.Models;
using Api.Libraries.AddressLibrary.Repositories;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Api.Tests.Controllers
{
    public class AddressControllerTests
    {
        private readonly Mock<IAddressRepository> _mockRepo;
        private readonly AddressController _controller;

        public AddressControllerTests()
        {
            _mockRepo = new Mock<IAddressRepository>();
            _controller = new AddressController(_mockRepo.Object);
        }

        [Fact]
        public async Task GetAddresses_ReturnsOkResult_WithAddresses()
        {
            // Arrange
            var addresses = new List<Address>
            {
                new()
                {
                    Id = 1,
                    Name = "Test1",
                    City = "City1",
                    Country = "Country1",
                    Email = "Test1@gmail.com", // Mandatory field
                    PhoneNumber = "1234567890" // Optional field
                },
                new()
                {
                    Id = 2,
                    Name = "Test2",
                    City = "City2",
                    Country = "Country2",
                    Email = "Test2@gmail.com", // Mandatory field
                    PhoneNumber = null // Optional field can be null
                },
            };
            _mockRepo.Setup(repo => repo.GetAllAddressesAsync()).ReturnsAsync(addresses);
            
            // Act
            var result = await _controller.GetAddresses();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedAddresses = Assert.IsAssignableFrom<IEnumerable<Address>>(okResult.Value);
            Assert.Equal(2, returnedAddresses.Count());
        }

        [Fact]
        public async Task GetAddress_ReturnsOkResult_WhenAddressExists()
        {
            // Arrange
            var address = new Address
            {
                Id = 1,
                Name = "Test",
                City = "City",
                Country = "Country",
                Email = "Test2@gmail.com",
            };
            _mockRepo.Setup(repo => repo.GetAddressByIdAsync(1)).ReturnsAsync(address);

            // Act
            var result = await _controller.GetAddress(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedAddress = Assert.IsType<Address>(okResult.Value);
            Assert.Equal(1, returnedAddress.Id);
        }

        [Fact]
        public async Task GetAddress_ReturnsNotFound_WhenAddressDoesNotExist()
        {
            // Arrange
            _mockRepo
                .Setup(repo => repo.GetAddressByIdAsync(1))
                .ThrowsAsync(new KeyNotFoundException());

            // Act
            var result = await _controller.GetAddress(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task CreateAddress_ReturnsOkResult_WithId()
        {
            // Arrange
            var requestAddress = new RequestAddress
            {
                Name = "Test",
                City = "City",
                Country = "Country",
                Email = "Test2@gmail.com",
            };
            _mockRepo.Setup(repo => repo.AddAddressAsync(requestAddress)).ReturnsAsync(1);

            // Act
            var result = await _controller.CreateAddress(requestAddress);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(1, okResult.Value);
        }

        [Fact]
        public async Task UpdateAddress_ReturnsNoContent_WhenSuccessful()
        {
            // Arrange
            var requestAddress = new RequestAddress
            {
                Name = "Test",
                City = "City",
                Country = "Country",
                Email = "Test2@gmail.com",
            };

            // Act
            var result = await _controller.UpdateAddress(1, requestAddress);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteAddress_ReturnsNoContent_WhenAddressExists()
        {
            // Arrange
            var address = new Address
            {
                Id = 1,
                Name = "Test",
                City = "City",
                Country = "Country",
                Email = "Test2@gmail.com",
            };
            _mockRepo.Setup(repo => repo.GetAddressByIdAsync(1)).ReturnsAsync(address);

            // Act
            var result = await _controller.DeleteAddress(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteAddress_ReturnsNotFound_WhenAddressDoesNotExist()
        {
            // Arrange
            _mockRepo
                .Setup(repo => repo.GetAddressByIdAsync(1))
                .ThrowsAsync(new KeyNotFoundException());

            // Act
            var result = await _controller.DeleteAddress(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task GetAddresses_WhenRepositoryThrowsException_ReturnStatusCode500()
        {
            // Arrange
            var mockRepo = new Mock<IAddressRepository>();
            mockRepo
                .Setup(repo => repo.GetAllAddressesAsync())
                .ThrowsAsync(new Exception("Test exception"));

            var controller = new AddressController(mockRepo.Object);

            // Act
            var result = await controller.GetAddresses();

            // Assert
            var statusCodeResult = Assert.IsType<StatusCodeResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
        }

        [Fact]
        public async Task GetAddress_WhenRepositoryThrowsException_ReturnStatusCode500()
        {
            // Arrange
            _mockRepo
                .Setup(repo => repo.GetAddressByIdAsync(1))
                .ThrowsAsync(new Exception("Test exception"));

            // Act
            var result = await _controller.GetAddress(1);

            // Assert
            var statusCodeResult = Assert.IsType<StatusCodeResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
        }

        [Fact]
        public async Task CreateAddress_WhenExceptionOccurs_ReturnsFiveHundredStatusCode()
        {
            // Arrange
            var mockAddressRepository = new Mock<IAddressRepository>();
            var controller = new AddressController(mockAddressRepository.Object);
            var address = new RequestAddress()
            {
                Name = "Test",
                City = "City",
                Country = "Country",
                Email = "Test2@gmail.com",
            };
            mockAddressRepository
                .Setup(repo => repo.AddAddressAsync(address))
                .ThrowsAsync(new Exception("Test exception"));

            // Act
            var result = await controller.CreateAddress(address);

            // Assert
            var statusCodeResult = Assert.IsType<StatusCodeResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
        }

        [Fact]
        public async Task DeleteAddress_WhenRepositoryThrowsException_ReturnStatusCode500()
        {
            // Arrange
            _mockRepo
                .Setup(repo => repo.GetAddressByIdAsync(1))
                .ReturnsAsync(
                    new Address
                    {
                        Id = 1,
                        Name = "Test Name",
                        City = "Dublin",
                        Country = "Ireland",
                        Email = "Test2@gmail.com",
                    }
                );
            _mockRepo
                .Setup(repo => repo.DeleteAddressAsync(1))
                .ThrowsAsync(new Exception("Test exception"));

            // Act
            var result = await _controller.DeleteAddress(1);

            // Assert
            var statusCodeResult = Assert.IsType<StatusCodeResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
        }

        [Fact]
        public async Task GetAddress_ReturnsOkResult_WithAllAddressFields()
        {
            // Arrange
            var expectedAddress = new Address
            {
                Id = 1,
                Name = "Test Name",
                City = "Test City",
                Country = "Test Country",
                Email = "Test2@gmail.com",
            };
            var expectedStrings = new[]
            {
                expectedAddress.Name,
                expectedAddress.City,
                expectedAddress.Country,
            };

            _mockRepo.Setup(repo => repo.GetAddressByIdAsync(1)).ReturnsAsync(expectedAddress);

            // Act
            var result = await _controller.GetAddress(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedAddress = Assert.IsType<Address>(okResult.Value);
            Assert.Equal(expectedAddress.Id, returnedAddress.Id);
            Assert.All(
                new[] { returnedAddress.Name, returnedAddress.City, returnedAddress.Country },
                str => Assert.Contains(str, expectedStrings)
            );
        }
    }
}
