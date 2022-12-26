using Application.Interfaces;
using Domain.Models;
using Microsoft.Extensions.Options;

namespace Application.Services
{
    internal class SingletonOptionService : ISingletonOptionService
    {
        private readonly IOptionsMonitor<User> _userOptions;

        public SingletonOptionService(IOptionsMonitor<User> userOptions)
        {
            _userOptions = userOptions;
        }

        public User GetUserSettings()
        {
            return _userOptions.CurrentValue;
        }
    }
}
