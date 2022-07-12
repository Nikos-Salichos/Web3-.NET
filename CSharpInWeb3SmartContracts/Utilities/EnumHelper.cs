using Nethereum.Signer;

namespace CSharpInWeb3SmartContracts.Utilities
{
    public class EnumHelper
    {
        private readonly IConfiguration _configuration;

        public EnumHelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GetStringBasedOnEnum(Chain chain)
        {
            switch (chain)
            {
                case Chain.MainNet:
                    return _configuration["User:BlockchainProviderMainnet"];
                case Chain.Kovan:
                    return _configuration["User:BlockchainProviderKovan"];
                case Chain.Ropsten:
                    return _configuration["User:BlockchainProviderRopsten"];
                default:
                    return "NO NETWORK GIVEN";
            }
        }
    }
}
