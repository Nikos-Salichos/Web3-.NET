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

        public string GetStringBasedOnEnum(BlockchainNetwork blockchainNetworks)
        {
            switch (blockchainNetworks)
            {
                case BlockchainNetwork.BlockchainNetworkMainnet:
                    return _configuration["BlockchainProviderMainnet"];
                case BlockchainNetwork.BlockchainNetworkKovan:
                    return _configuration["BlockchainProviderKovan"];
                case BlockchainNetwork.BlockchainNetworkRopsten:
                    return _configuration["BlockchainProviderRopsten"];
                default:
                    return "NO NETWORK GIVEN";
            }
        }
    }
}
