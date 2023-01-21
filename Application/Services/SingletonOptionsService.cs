using Application.Interfaces;
using Domain.Models;
using Microsoft.Extensions.Options;

namespace Application.Services
{
    public class SingletonOptionsService : ISingletonOptionsService
    {
        private readonly IOptionsMonitor<WalletOwner> _userOptions;
        private readonly IOptionsMonitor<NetworkProvider> _networkConfig;

        public SingletonOptionsService(IOptionsMonitor<WalletOwner> userOptions, IOptionsMonitor<NetworkProvider> networkConfig)
        {
            _userOptions = userOptions;
            _networkConfig = networkConfig;
        }

        public NetworkProvider GetNetworkConfig()
        {
            return _networkConfig.CurrentValue ?? new NetworkProvider();
        }

        public WalletOwner GetUserSettings()
        {
            return _userOptions.CurrentValue ?? WalletOwner.Construct(string.Empty, string.Empty);
        }
    }
}
