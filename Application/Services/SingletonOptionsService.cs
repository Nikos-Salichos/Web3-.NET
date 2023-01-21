using Application.Interfaces;
using Domain.Models;
using Microsoft.Extensions.Options;

namespace Application.Services
{
    public class SingletonOptionsService : ISingletonOptionsService
    {
        private readonly IOptionsMonitor<User> _userOptions;
        private readonly IOptionsMonitor<NetworkProvider> _networkConfig;

        public SingletonOptionsService(IOptionsMonitor<User> userOptions, IOptionsMonitor<NetworkProvider> networkConfig)
        {
            _userOptions = userOptions;
            _networkConfig = networkConfig;
        }

        public NetworkProvider GetNetworkConfig()
        {
            return _networkConfig.CurrentValue ?? new NetworkProvider();
        }

        public User GetUserSettings()
        {
            return _userOptions.CurrentValue ?? new User();
        }
    }
}
