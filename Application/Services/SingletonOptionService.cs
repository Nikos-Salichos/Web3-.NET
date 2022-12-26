using Application.Interfaces;
using Microsoft.Extensions.Options;

namespace Application.Services
{
    internal class SingletonOptionService : ISingletonOptionService
    {
        private readonly IOptionsMonitor<UnitOptions> _unitOptions;

        public SingletonService(IOptionsMonitor<UnitOptions> unitOptions)
        {
            _unitOptions = unitOptions;
        }
        public UnitOptions GetUnits()
        {
            return _unitOptions.CurrentValue;
        }
    }
}
