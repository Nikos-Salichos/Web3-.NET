using CSharpInWeb3SmartContracts.Enumerations;
using CSharpInWeb3SmartContracts.Utilities;
using Microsoft.AspNetCore.Mvc;
using Nethereum.Signer;

namespace CSharpInWeb3SmartContracts.Controllers
{
    public class SmartContractController : ControllerBase
    {
        public EnumHelper EnumHelper { get; set; }

        public SmartContractController(IConfiguration configuration)
        {
            EnumHelper = new EnumHelper(configuration);
        }

        [Consumes("application/json")]
        [HttpPost("DeployWithoutParameters")]
        public async Task<ActionResult> DeployWithoutParameters(Chain chain, BlockchainNetwork blockchainNetwork, string privateKey, string metamaskAddress, string byteCode, [FromBody] object abi)
        {
            try
            {
                Account? account = new Account(privateKey, chain);


                return Ok(deployContract);
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }

    }
}
