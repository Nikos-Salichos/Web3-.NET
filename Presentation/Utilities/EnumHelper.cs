﻿using Nethereum.Signer;

namespace WebApi.Utilities
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
                case Chain.Goerli:
                    return _configuration["User:BlockchainProviderGoerli"];
                case Chain.Ropsten:
                    return _configuration["User:BlockchainProviderRopsten"];
                default:
                    return "NO NETWORK GIVEN";
            }
        }
    }
}
