using Application.Interfaces;
using Nethereum.Signer;

namespace Application.Helpers
{
    public class EnumHelper
    {
        private readonly ISingletonOptionsService _singletonOptionsService;

        public EnumHelper(ISingletonOptionsService singletonOptionsService)
        {
            _singletonOptionsService = singletonOptionsService;
        }

        public string GetStringBasedOnEnum(Chain chain)
        {
            return chain switch
            {
                Chain.MainNet => _singletonOptionsService.GetNetworkConfig().BlockchainProviderMainnet,
                Chain.Goerli => _singletonOptionsService.GetNetworkConfig().BlockchainProviderGoerli,
                Chain.Ropsten => _singletonOptionsService.GetNetworkConfig().BlockchainProviderSepolia,
                _ => "NO NETWORK GIVEN",
            };
        }
    }
}
