using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    public class HealthController : ControllerBase
    {
        private readonly ILogger<HealthController> _logger;
    }
}
