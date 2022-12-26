using Domain.Models;

namespace Application.Interfaces
{
    public interface ISingletonOptionService
    {
        User GetUserSettings();
    }
}
