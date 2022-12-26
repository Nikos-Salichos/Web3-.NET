using Application.Interfaces;
using Domain.Models;
using Microsoft.Extensions.Options;

namespace Application.Services
{
    public class SingletonOptionsService : ISingletonOptionsService
    {
        private readonly IOptionsMonitor<User> _userOptions;

        public SingletonOptionsService(IOptionsMonitor<User> userOptions)
        {
            _userOptions = userOptions;
        }

        public User GetUserSettings()
        {
            return _userOptions.CurrentValue;
        }
    }
}
