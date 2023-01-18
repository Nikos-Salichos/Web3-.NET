using Application.Interfaces;
using Domain.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using System.Net;
using WebApi.Controllers;
using Xunit;

namespace Tests.UnitTests
{
    public class SmartContractControllerUnitTests
    {
        [Fact]
        public async Task GetAllSmartContractsAsync_ReturnsOkResult()
        {
            // Arrange
            var smartContracts = new List<SmartContractDTO> {
            new SmartContractDTO { Id = 1, Address = "Smart Contract Address 1" },
            new SmartContractDTO { Id = 2, Address = "Smart Contract Address 2" } };

            var mockSmartContractService = new Mock<ISmartContractService>();

            mockSmartContractService.Setup(x => x.GetSmartContractsAsync()).ReturnsAsync(smartContracts);

            Mock<IConfigurationSection> mockSection = new Mock<IConfigurationSection>();
            mockSection.Setup(x => x.Value).Returns("User");

            Mock<IConfiguration> mockConfig = new Mock<IConfiguration>();
            mockConfig.Setup(x => x.GetSection(It.Is<string>(k => k == "User"))).Returns(mockSection.Object);

            var mockLogger = new Mock<ILogger<SmartContractController>>();

            var controller = new SmartContractController(mockConfig.Object,
                                    mockSmartContractService.Object, mockLogger.Object);

            // Act
            var result = await controller.GetAllSmartContractsAsync();

            // Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(HttpStatusCode.OK, (HttpStatusCode)okObjectResult.StatusCode!);
            var returnedSmartContracts = Assert.IsAssignableFrom<IEnumerable<SmartContractDTO>>(okObjectResult.Value);
            Assert.Equal(smartContracts, returnedSmartContracts);
            mockLogger.Verify(x => x.Log(It.IsAny<LogLevel>(),
                                         It.IsAny<EventId>(),
                                         It.IsAny<It.IsAnyType>(),
                                         It.IsAny<Exception>(),
                                         (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()));
        }

        [Fact]
        public async Task GetAllSmartContractsAsync_ReturnsEmptyResult()
        {
            // Arrange
            var smartContracts = new List<SmartContractDTO>();

            var mockSmartContractService = new Mock<ISmartContractService>();

            mockSmartContractService.Setup(x => x.GetSmartContractsAsync()).ReturnsAsync(smartContracts);

            Mock<IConfigurationSection> mockSection = new Mock<IConfigurationSection>();
            mockSection.Setup(x => x.Value).Returns("User");

            Mock<IConfiguration> mockConfig = new Mock<IConfiguration>();
            mockConfig.Setup(x => x.GetSection(It.Is<string>(k => k == "User"))).Returns(mockSection.Object);

            var mockLogger = new Mock<ILogger<SmartContractController>>();

            var controller = new SmartContractController(mockConfig.Object,
                                    mockSmartContractService.Object, mockLogger.Object);

            // Act
            var result = await controller.GetAllSmartContractsAsync();

            // Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(HttpStatusCode.OK, (HttpStatusCode)okObjectResult.StatusCode!);
            var returnedSmartContracts = Assert.IsAssignableFrom<IEnumerable<SmartContractDTO>>(okObjectResult.Value);
            Assert.Equal(smartContracts, returnedSmartContracts);
            mockLogger.Verify(x => x.Log(It.IsAny<LogLevel>(),
                                         It.IsAny<EventId>(),
                                         It.IsAny<It.IsAnyType>(),
                                         It.IsAny<Exception>(),
                                         (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()));
        }

        [Fact]
        public async Task GetSmartContractByIdAsync_ReturnsEmptyResult()
        {
            var smartContracts = new List<SmartContractDTO> {
            new SmartContractDTO { Id = 1, Address = "Smart Contract Address 2" } };

            var mockSmartContractService = new Mock<ISmartContractService>();

            mockSmartContractService.Setup(x => x.GetSmartContractByIdAsync(1))
                .ReturnsAsync(smartContracts.FirstOrDefault());

            Mock<IConfigurationSection> mockSection = new Mock<IConfigurationSection>();
            mockSection.Setup(x => x.Value).Returns("User");

        }

    }
}