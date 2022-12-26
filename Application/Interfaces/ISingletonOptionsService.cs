using Domain.Models;

namespace Application.Interfaces
{
    public interface ISingletonOptionsService
    {
        User GetUserSettings();
    }
}
