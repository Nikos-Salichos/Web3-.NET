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
                    return _singletonOptionsService.GetUserSettings();
                case Chain.Goerli:
                    return _configuration["User:BlockchainProviderGoerli"]!;
                case Chain.Ropsten:
                    return _configuration["User:BlockchainProviderRopsten"]!;
                default:
                    return "NO NETWORK GIVEN";
            }
        }
    }
}
