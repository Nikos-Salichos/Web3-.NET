using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ErrorController : ControllerBase
    {
        // URL for this API - /api/error
        [Route("/error")]
        public IActionResult Error() => Problem();
    }
}
