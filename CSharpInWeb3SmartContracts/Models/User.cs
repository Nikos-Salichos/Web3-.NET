namespace CSharpInWeb3SmartContracts.Models
{
    public class User
    {
        public string BlockchainProviderMainnet { get; set; }
        public string BlockchainProviderKovan { get; set; }
        public string BlockchainProviderRopsten { get; set; }
        public string MetamaskAddress { get; set; }
        public string PrivateKey { get; set; }
    }
}
