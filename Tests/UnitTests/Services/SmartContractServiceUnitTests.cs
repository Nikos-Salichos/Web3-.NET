using Application.CQRS.Queries;
using Application.Interfaces;
using Application.Services;
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
        public async Task GetSmartContractsAsync_ShouldReturnEmptyList()
        {
            // Arrange
            var smartContracts = new List<SmartContract>();
            var smartContractsDtos = new List<SmartContractDTO>();

            var query = new GetSmartContractsListQuery(IsAny<int>(), IsAny<int>());
            _mockMediator.Setup(m => m.Send(query, default)).ReturnsAsync(smartContracts);
            _mockMapper.Setup(m => m.Map<List<SmartContract>, List<SmartContractDTO>>(IsAny<List<SmartContract>>())).Returns(smartContractsDtos);
            _mockSmartContractService.Setup(m => m.GetSmartContractsAsync(IsAny<int>(), IsAny<int>())).ReturnsAsync(smartContractsDtos);

            var service = new SmartContractService(_mockMapper.Object, _mockMediator.Object, _mockOptionsService.Object);

            // Act
            var result = await service.GetSmartContractsAsync(sc => sc.Address == "0x1");

            // Assert
            Assert.NotNull(result);
            Assert.True(smartContracts.Select(x => new { x.Id, x.Address }).SequenceEqual(result.Select(x => new { x.Id, x.Address })));
        }

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

            var smartContractService = new SmartContractService(_mockMapper.Object, _mockMediator.Object, _mockOptionsService.Object);

            // Act
            var result = await smartContractService.GetSmartContractsAsync(IsAny<int>(), IsAny<int>());

            // Assert
            Assert.NotNull(result);
            Assert.True(smartContracts.Select(x => new { x.Id, x.Address }).SequenceEqual(result.Select(x => new { x.Id, x.Address })));
        }

        [Fact]
        public async Task GetSmartContractByIdAsync_ShouldReturnSmartContracts()
        {
            // Arrange
            var smartContracts = new List<SmartContract> { new SmartContract { Id = 1, Address = "SmartContract1" } };

            var smartContractsDtos = new List<SmartContractDTO> {
                new SmartContractDTO { Id = smartContracts.FirstOrDefault().Id, Address = smartContracts.First().Address }
            };

            var query = new GetSmartContractQuery(IsAny<int>());
            _mockMediator.Setup(m => m.Send(query, default)).ReturnsAsync(smartContracts.FirstOrDefault());
            _mockMapper.Setup(m => m.Map<SmartContract, SmartContractDTO>(IsAny<SmartContract>())).Returns(smartContractsDtos.FirstOrDefault());
            _mockSmartContractService.Setup(m => m.GetSmartContractAsync(1)).ReturnsAsync(smartContractsDtos.FirstOrDefault(x => x.Id == 1));

            var smartContractService = new SmartContractService(_mockMapper.Object, _mockMediator.Object, _mockOptionsService.Object);

            // Act
            var actualSmartContractDTO = await smartContractService.GetSmartContractAsync(1);

            // Assert
            Assert.NotNull(actualSmartContractDTO);
            Assert.IsType<SmartContractDTO>(actualSmartContractDTO);
            Assert.Equal(actualSmartContractDTO.Id, smartContractsDtos.FirstOrDefault().Id);
            Assert.Equal(actualSmartContractDTO.Address, smartContractsDtos.FirstOrDefault().Address);
        }

        [Fact]
        public async Task GetSmartContractByIdAsync_ShouldReturnEmptySmartContract()
        {
            // Arrange
            var smartContracts = new List<SmartContract>();

            var smartContractsDtos = new List<SmartContractDTO>();

            var query = new GetSmartContractQuery(IsAny<int>());
            _mockMediator.Setup(m => m.Send(query, default)).ReturnsAsync(new SmartContract());
            _mockMapper.Setup(m => m.Map<SmartContract, SmartContractDTO>(IsAny<SmartContract>())).Returns(new SmartContractDTO());
            _mockSmartContractService.Setup(m => m.GetSmartContractAsync(100)).ReturnsAsync(smartContractsDtos.FirstOrDefault(x => x.Id == 100));

            var smartContractService = new SmartContractService(_mockMapper.Object, _mockMediator.Object, _mockOptionsService.Object);

            // Act
            var actualSmartContractDTO = await smartContractService.GetSmartContractAsync(1);

            // Assert
            Assert.NotNull(actualSmartContractDTO);
            Assert.IsType<SmartContractDTO>(actualSmartContractDTO);
            Assert.Null(actualSmartContractDTO.Address);
        }
    }
}
