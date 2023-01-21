using Domain.Models;

namespace Application.Interfaces
{
    public interface ISingletonOptionsService
    {
        WalletOwner GetUserSettings();

        NetworkProvider GetNetworkConfig();
    }
}
