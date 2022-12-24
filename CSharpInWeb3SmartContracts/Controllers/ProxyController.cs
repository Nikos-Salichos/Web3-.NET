using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    // [Route("api/[controller]")]
    [Route("[action]")]
    [ApiController]
    public class ProxyController : ControllerBase
    {
        private readonly HttpClient _httpClient;
    }
}
