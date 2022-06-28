using CSharpInWeb3SmartContracts.Enumerations;

namespace CSharpInWeb3SmartContracts.Utilities
{
    public class EnumHelper
    {
        private IConfiguration _configuration;

        public EnumHelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GetStringBasedOnEnum(BlockchainNetworks blockchainNetworks)
        {
            switch (blockchainNetworks)
            {
                case BlockchainNetworks.BlockchainNetworkMainnet:
                    return _configuration["BlockchainProviderMainnet"];
                case BlockchainNetworks.BlockchainNetworkKovan:
                    return _configuration["BlockchainProviderKovan"];
                case BlockchainNetworks.BlockchainNetworkRopsten:
                    return _configuration["BlockchainProviderRopsten"];
                default:
                    return "NO NETWORK GIVEN";
            }
        }
    }
}
