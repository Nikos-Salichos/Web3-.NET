using Application.Interfaces;
using MediatR;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Moq;
using Xunit;

namespace Tests.UnitTests.Repositories
{
    [Trait("SmartContractsService", "UnitTests")]
    public class SmartContractServiceUnitTests
    {
        private static readonly Mock<ISmartContractService> _mockSmartContractService = new Mock<ISmartContractService>();
        private static readonly Mock<ISingletonOptionsService> _mockOptionsService = new Mock<ISingletonOptionsService>();
        private static readonly Mock<IMediator> _mockMediator = new Mock<IMediator>();


    }
}
