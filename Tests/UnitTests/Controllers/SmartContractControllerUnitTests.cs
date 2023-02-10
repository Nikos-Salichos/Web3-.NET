using Application.Interfaces;
using Domain.DTOs;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using System.Linq.Expressions;
using System.Net;
using WebApi.Controllers;
using Xunit;
using static Moq.It;

namespace Tests.UnitTests.Controllers
{
    [Trait("SmartContracts", "UnitTests")]
    public class SmartContractControllerUnitTests
    {
        private static readonly Mock<ISmartContractService> _mockSmartContractService = new Mock<ISmartContractService>();
        private static readonly Mock<IConfigurationSection> _mockConfigurationSection = new Mock<IConfigurationSection>();
        private static readonly Mock<IConfiguration> _mockConfiguration = new Mock<IConfiguration>();
        private static readonly Mock<ILogger<SmartContractController>> _mockLogger = new Mock<ILogger<SmartContractController>>();

        [Fact]
        public async Task GetAllSmartContractsAsync_ReturnsOkResult()
        {
            // Arrange
            var smartContracts = new List<SmartContractDTO> {
            new SmartContractDTO { Id = 1, Address = "Smart Contract Address 1" },
            new SmartContractDTO { Id = 2, Address = "Smart Contract Address 2" } };

            _mockSmartContractService.Setup(x => x.GetSmartContractsAsync(IsAny<int>(), IsAny<int>())).ReturnsAsync(smartContracts);

            _mockConfigurationSection.Setup(x => x.Value).Returns("User");

            _mockConfiguration.Setup(x => x.GetSection(Is<string>(k => k == "User"))).Returns(_mockConfigurationSection.Object);

            var mockLogger = new Mock<ILogger<SmartContractController>>();

            var controller = new SmartContractController(_mockConfiguration.Object,
                                    _mockSmartContractService.Object, mockLogger.Object);

            // Act
            var result = await controller.GetAllSmartContractsAsync(IsAny<int>(), IsAny<int>());

            // Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(HttpStatusCode.OK, (HttpStatusCode)okObjectResult.StatusCode);
            var returnedSmartContracts = Assert.IsAssignableFrom<IEnumerable<SmartContractDTO>>(okObjectResult.Value);
            Assert.Equal(smartContracts, returnedSmartContracts);
            mockLogger.Verify(x => x.Log(IsAny<LogLevel>(),
                                         IsAny<EventId>(),
                                         IsAny<IsAnyType>(),
                                         IsAny<Exception>(),
                                         IsAny<Func<IsAnyType, Exception?, string>>()));
        }

        [Fact]
        public async Task GetAllSmartContractsAsync_ReturnsEmptyResult()
        {
            // Arrange
            var smartContracts = new List<SmartContractDTO>();

            _mockSmartContractService.Setup(x => x.GetSmartContractsAsync(IsAny<int>(), IsAny<int>())).ReturnsAsync(smartContracts);

            _mockConfigurationSection.Setup(x => x.Value).Returns("User");

            _mockConfiguration.Setup(x => x.GetSection(Is<string>(k => k == "User"))).Returns(_mockConfigurationSection.Object);

            var mockLogger = new Mock<ILogger<SmartContractController>>();

            var controller = new SmartContractController(_mockConfiguration.Object,
                                    _mockSmartContractService.Object, mockLogger.Object);

            // Act
            var result = await controller.GetAllSmartContractsAsync(IsAny<int>(), IsAny<int>());

            // Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(HttpStatusCode.OK, (HttpStatusCode)okObjectResult.StatusCode);
            var returnedSmartContracts = Assert.IsAssignableFrom<IEnumerable<SmartContractDTO>>(okObjectResult.Value);
            Assert.Equal(smartContracts, returnedSmartContracts);
            mockLogger.Verify(x => x.Log(IsAny<LogLevel>(),
                                         IsAny<EventId>(),
                                         IsAny<IsAnyType>(),
                                         IsAny<Exception>(),
                                         IsAny<Func<IsAnyType, Exception?, string>>()));
        }

        [Fact]
        public async Task GetSmartContractByIdAsync_ReturnsOkResult()
        {
            //Arrange
            var smartContracts = new List<SmartContractDTO> {
            new SmartContractDTO { Id = 1, Address = "Smart Contract Address 2" } };

            _mockSmartContractService.Setup(x => x.GetSmartContractByIdAsync(1))
                .ReturnsAsync(smartContracts.FirstOrDefault());

            _mockConfigurationSection.Setup(x => x.Value).Returns("User");

            _mockConfiguration.Setup(x => x.GetSection(Is<string>(k => k == "User"))).Returns(_mockConfigurationSection.Object);

            var mockLogger = new Mock<ILogger<SmartContractController>>();

            var controller = new SmartContractController(_mockConfiguration.Object,
                                    _mockSmartContractService.Object, mockLogger.Object);
            // Act
            var result = await controller.GetSmartContractByIdAsync(1);

            // Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(HttpStatusCode.OK, (HttpStatusCode)okObjectResult.StatusCode);
            var returnedSmartContract = Assert.IsAssignableFrom<SmartContractDTO>(okObjectResult.Value);
            Assert.Equal(smartContracts.FirstOrDefault(), returnedSmartContract);
            mockLogger.Verify(x => x.Log(IsAny<LogLevel>(),
                                        IsAny<EventId>(),
                                        IsAny<IsAnyType>(),
                                        IsAny<Exception>(),
                                        IsAny<Func<IsAnyType, Exception?, string>>()));
        }

        [Fact]
        public async Task GetSmartContractByIdAsync_ReturnsEmptyResult()
        {
            //Arrange
            var smartContracts = new List<SmartContractDTO>();

            _mockSmartContractService.Setup(x => x.GetSmartContractByIdAsync(1))
                .ReturnsAsync(smartContracts.FirstOrDefault());

            _mockConfigurationSection.Setup(x => x.Value).Returns("User");

            _mockConfiguration.Setup(x => x.GetSection(Is<string>(k => k == "User"))).Returns(_mockConfigurationSection.Object);

            var mockLogger = new Mock<ILogger<SmartContractController>>();

            var controller = new SmartContractController(_mockConfiguration.Object,
                        _mockSmartContractService.Object, mockLogger.Object);

            // Act
            var result = await controller.GetSmartContractByIdAsync(1);

            // Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(HttpStatusCode.OK, (HttpStatusCode)okObjectResult.StatusCode);
            Assert.Null(okObjectResult.Value);
            Assert.Empty(smartContracts);
            mockLogger.Verify(x => x.Log(IsAny<LogLevel>(),
                                        IsAny<EventId>(),
                                        IsAny<IsAnyType>(),
                                        IsAny<Exception>(),
                                        IsAny<Func<IsAnyType, Exception?, string>>()));
        }

        [Fact]
        public async Task FindSmartContractByAddressAsync_ReturnsOkResult()
        {
            //Arrange
            var smartContracts = new List<SmartContractDTO>
            {
                new SmartContractDTO { Id = 1 , Address = "123"}
            };

            _mockSmartContractService.Setup(x => x.FindSmartContractsByAddressAsync(IsAny<Expression<Func<SmartContract, bool>>>()))
               .ReturnsAsync(smartContracts);

            _mockConfigurationSection.Setup(x => x.Value).Returns("User");

            _mockConfiguration.Setup(x => x.GetSection(Is<string>(k => k == "User"))).Returns(_mockConfigurationSection.Object);

            var _smartContractController = new SmartContractController(_mockConfiguration.Object, _mockSmartContractService.Object, _mockLogger.Object);

            // Act
            var result = await _smartContractController.FindSmartContractsByAddressAsync("123");

            // Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(HttpStatusCode.OK, (HttpStatusCode)okObjectResult.StatusCode);
            Assert.NotNull(okObjectResult.Value);
            Assert.NotEmpty((IEnumerable<SmartContractDTO>)okObjectResult.Value);

            _mockLogger.Verify(x => x.Log(IsAny<LogLevel>(),
                                        IsAny<EventId>(),
                                        IsAny<IsAnyType>(),
                                        IsAny<Exception>(),
                                        IsAny<Func<IsAnyType, Exception?, string>>()));
        }

        [Fact]
        public async Task FindSmartContractByAddressAsync_ReturnsEmptyResult()
        {
            //Arrange
            var smartContracts = new List<SmartContractDTO>();
            var mockSmartContractService = new Mock<ISmartContractService>();

            mockSmartContractService.Setup(x => x.FindSmartContractsByAddressAsync(IsAny<Expression<Func<SmartContract, bool>>>()))
               .ReturnsAsync(smartContracts);

            Mock<IConfigurationSection> mockSection = new Mock<IConfigurationSection>();
            mockSection.Setup(x => x.Value).Returns("User");

            Mock<IConfiguration> mockConfig = new Mock<IConfiguration>();
            mockConfig.Setup(x => x.GetSection(Is<string>(k => k == "User"))).Returns(mockSection.Object);

            var mockLogger = new Mock<ILogger<SmartContractController>>();

            var controller = new SmartContractController(mockConfig.Object,
            mockSmartContractService.Object, mockLogger.Object);

            // Act
            var result = await controller.FindSmartContractsByAddressAsync("123");

            // Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result);

            Assert.Equal(HttpStatusCode.OK, (HttpStatusCode)okObjectResult.StatusCode);
            Assert.Empty((IEnumerable<SmartContractDTO>)okObjectResult.Value);

            mockLogger.Verify(x => x.Log(IsAny<LogLevel>(),
                                        IsAny<EventId>(),
                                        IsAny<IsAnyType>(),
                                        IsAny<Exception>(),
                                        IsAny<Func<IsAnyType, Exception?, string>>()));
        }

    }
}