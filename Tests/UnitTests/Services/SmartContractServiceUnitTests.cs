using Application.CQRS.Queries;
using Application.Interfaces;
using AutoMapper;
using Domain.DTOs;
using Domain.Models;
using MediatR;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Moq;
using Xunit;
using static Moq.It;

namespace Tests.UnitTests.Repositories
{
    [Trait("SmartContractsService", "UnitTests")]
    public class SmartContractServiceUnitTests
    {
        private static readonly Mock<ISmartContractService> _mockSmartContractService = new Mock<ISmartContractService>();
        private static readonly Mock<ISingletonOptionsService> _mockOptionsService = new Mock<ISingletonOptionsService>();
        private static readonly Mock<IMediator> _mockMediator = new Mock<IMediator>();
        private static readonly Mock<IMapper> _mockMapper = new Mock<IMapper>();

        [Fact]
        public async Task GetSmartContractsAsync_ShouldReturnSmartContracts()
        {
            // Arrange
            var smartContracts = new List<SmartContract> {
                                         new SmartContract { Id = 1, Address = "SmartContract1" },
                                         new SmartContract { Id = 2, Address = "SmartContract2" }};

            var smartContractsDtos = new List<SmartContractDTO> {
                                         new SmartContractDTO { Id = smartContracts.First().Id, Address = smartContracts.First().Address },
                                         new SmartContractDTO { Id = smartContracts.Last().Id, Address = smartContracts.Last().Address }};

            var query = new GetSmartContractsListQuery(IsAny<int>(), IsAny<int>());
            _mockMediator.Setup(m => m.Send(query, default)).ReturnsAsync(smartContracts);
            _mockMapper.Setup(m => m.Map<List<SmartContract>, List<SmartContractDTO>>(IsAny<List<SmartContract>>())).Returns(smartContractsDtos);
            _mockSmartContractService.Setup(m => m.GetSmartContractsAsync(IsAny<int>(), IsAny<int>())).ReturnsAsync(smartContractsDtos);
        }

    }
}
