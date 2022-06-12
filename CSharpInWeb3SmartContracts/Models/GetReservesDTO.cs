using Nethereum.ABI.FunctionEncoding.Attributes;
using System.Numerics;

namespace CSharpInWeb3SmartContracts.Models
{
    [FunctionOutput]
    public class GetReservesDTO : IFunctionOutputDTO
    {
        [Parameter("uint112", "_reserve0", 1)]
        public virtual BigInteger Reserve0 { get; set; }

        [Parameter("uint112", "_reserve1", 2)]
        public virtual BigInteger Reserve1 { get; set; }

        [Parameter("uint32", "_blockTimestampLast", 3)]
        public virtual int BlockTimeStampLast { get; set; }
    }
}
