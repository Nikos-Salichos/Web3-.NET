using Nethereum.Signer;

namespace CSharpInWeb3SmartContracts.Utilities
{
    public class EnumHelper
    {
        private IConfiguration _configuration;

        public EnumHelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GetStringBasedOnEnum(Chain chain)
        {
            switch (chain)
            {
                case Chain.MainNet:
                    return _configuration["BlockchainProviderMainnet"];
                case Chain.Kovan:
                    return _configuration["BlockchainProviderKovan"];
                case Chain.Ropsten:
                    return _configuration["BlockchainProviderRopsten"];
                default:
                    return "NO NETWORK GIVEN";
            }
        }
    }
}
