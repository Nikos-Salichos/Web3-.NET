using Application.Interfaces;
using Domain.Models;
using Microsoft.Extensions.Options;

namespace Application.Services
{
    public class SingletonOptionsService : ISingletonOptionsService
    {
        private readonly IOptionsMonitor<User> _userOptions;
        private readonly IOptionsMonitor<NetworkConfig> _networkConfig;

        public SingletonOptionsService(IOptionsMonitor<User> userOptions)
        {
            _userOptions = userOptions;
        }

        public NetworkConfig GetNetworkConfig()
        {
            throw new NotImplementedException();
        }

        public User GetUserSettings()
        {
            return _userOptions.CurrentValue;
        }
    }
}
