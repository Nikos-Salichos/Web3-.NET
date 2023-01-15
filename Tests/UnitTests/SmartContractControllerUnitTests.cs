using Application.Interfaces;
using Domain.DTOs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
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

        }
    }
}