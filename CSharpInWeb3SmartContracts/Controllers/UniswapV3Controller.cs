using CSharpInWeb3SmartContracts.Models;
using Microsoft.AspNetCore.Mvc;

namespace CSharpInWeb3SmartContracts.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UniswapV3Controller : ControllerBase
    {
        private readonly IConfiguration _configuration;

        private readonly User _user = new User();



    }
}
