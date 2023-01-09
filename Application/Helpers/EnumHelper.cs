using Application.Interfaces;
using Nethereum.Signer;

namespace Application.Utilities
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
            switch (chain)
            {
                case Chain.MainNet:
                    return _singletonOptionsService.GetNetworkConfig().BlockchainProviderMainnet;
                case Chain.Goerli:
                    return _singletonOptionsService.GetNetworkConfig().BlockchainProviderGoerli;
                case Chain.Ropsten:
                    return _singletonOptionsService.GetNetworkConfig().BlockchainProviderSepolia;
                default:
                    return "NO NETWORK GIVEN";
            }
        }
    }
}
