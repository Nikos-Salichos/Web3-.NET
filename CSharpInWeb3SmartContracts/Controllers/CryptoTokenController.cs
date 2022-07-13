using CSharpInWeb3SmartContracts.Models;
using CSharpInWeb3SmartContracts.Utilities;
using Microsoft.AspNetCore.Mvc;
using Nethereum.Contracts;
using Nethereum.Contracts.Standards.ERC20.ContractDefinition;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Signer;
using Nethereum.Util;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using System.Numerics;

namespace CSharpInWeb3SmartContracts.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CryptoTokenController : ControllerBase
    {
        private readonly string _byteCode = @"60a06040526000805461ffff60a01b1916600160a81b17905560126003553480156200002a57600080fd5b5060405162001623380380620016238339810160408190526200004d91620001fb565b600080546001600160a01b0319163317905580620000a35760405162461bcd60e51b815260206004820152600f60248201526e0546f6b656e3a20636170206973203608c1b604482015260640160405180910390fd5b600354620000b39060646200038c565b620000bf9082620003a1565b608052600354620000d290600a6200038c565b620000de9085620003a1565b600481905533600090815260056020526040902055600162000101848262000452565b50600262000110838262000452565b5050600080546001600160a01b03191633179055506200051e915050565b634e487b7160e01b600052604160045260246000fd5b600082601f8301126200015657600080fd5b81516001600160401b03808211156200017357620001736200012e565b604051601f8301601f19908116603f011681019082821181831017156200019e576200019e6200012e565b81604052838152602092508683858801011115620001bb57600080fd5b600091505b83821015620001df5785820183015181830184015290820190620001c0565b83821115620001f15760008385830101525b9695505050505050565b600080600080608085870312156200021257600080fd5b845160208601519094506001600160401b03808211156200023257600080fd5b620002408883890162000144565b945060408701519150808211156200025757600080fd5b50620002668782880162000144565b606096909601519497939650505050565b634e487b7160e01b600052601160045260246000fd5b600181815b80851115620002ce578160001904821115620002b257620002b262000277565b80851615620002c057918102915b93841c939080029062000292565b509250929050565b600082620002e75750600162000386565b81620002f65750600062000386565b81600181146200030f57600281146200031a576200033a565b600191505062000386565b60ff8411156200032e576200032e62000277565b50506001821b62000386565b5060208310610133831016604e8410600b84101617156200035f575081810a62000386565b6200036b83836200028d565b806000190482111562000382576200038262000277565b0290505b92915050565b60006200039a8383620002d6565b9392505050565b6000816000190483118215151615620003be57620003be62000277565b500290565b600181811c90821680620003d857607f821691505b602082108103620003f957634e487b7160e01b600052602260045260246000fd5b50919050565b601f8211156200044d57600081815260208120601f850160051c81016020861015620004285750805b601f850160051c820191505b81811015620004495782815560010162000434565b5050505b505050565b81516001600160401b038111156200046e576200046e6200012e565b62000486816200047f8454620003c3565b84620003ff565b602080601f831160018114620004be5760008415620004a55750858301515b600019600386901b1c1916600185901b17855562000449565b600085815260208120601f198616915b82811015620004ef57888601518255948401946001909101908401620004ce565b50858210156200050e5787850151600019600388901b60f8161c191681555b5050505050600190811b01905550565b6080516110e26200054160003960008181610220015261075c01526110e26000f3fe608060405234801561001057600080fd5b506004361061018e5760003560e01c80635c975abb116100de5780638da5cb5b11610097578063d73dd62311610071578063d73dd62314610368578063dd62ed3e1461037b578063f2fde38b1461038e578063f8b2cb4f146103a157600080fd5b80638da5cb5b1461032257806395d89b411461034d578063a9059cbb1461035557600080fd5b80635c975abb146102b857806366188463146102cc57806370a08231146102df578063715018a6146102ff57806379cc6790146103075780638456cb591461031a57600080fd5b8063355274ea1161014b57806340c10f191161012557806340c10f191461025f57806342966c68146102725780634be8b05e146102855780635c6581651461028d57600080fd5b8063355274ea1461021b57806339df43ff146102425780633f4ba83a1461025757600080fd5b806306fdde0314610193578063095ea7b3146101b157806318160ddd146101d457806323b872dd146101eb578063313ce567146101fe578063323be1c514610207575b600080fd5b61019b6103ca565b6040516101a89190610edd565b60405180910390f35b6101c46101bf366004610f4a565b610458565b60405190151581526020016101a8565b6101dd60045481565b6040519081526020016101a8565b6101c46101f9366004610f76565b6104ea565b6101dd60035481565b6000546101c490600160a81b900460ff1681565b6101dd7f000000000000000000000000000000000000000000000000000000000000000081565b610255610250366004610fb7565b610608565b005b610255610682565b61025561026d366004610f4a565b610700565b6101c4610280366004610fdb565b610817565b6102556108f5565b6101dd61029b366004610ff4565b600660209081526000928352604080842090915290825290205481565b6000546101c490600160a01b900460ff1681565b6101c46102da366004610f4a565b610943565b6101dd6102ed366004610fb7565b60056020526000908152604090205481565b610255610a3e565b6101c4610315366004610f4a565b610a9d565b610255610bf1565b600054610335906001600160a01b031681565b6040516001600160a01b0390911681526020016101a8565b61019b610c8b565b6101c4610363366004610f4a565b610c98565b6101c4610376366004610f4a565b610d7a565b6101dd610389366004610ff4565b610dff565b61025561039c366004610fb7565b610e58565b6101dd6103af366004610fb7565b6001600160a01b031660009081526005602052604090205490565b600180546103d79061102d565b80601f01602080910402602001604051908101604052809291908181526020018280546104039061102d565b80156104505780601f1061042557610100808354040283529160200191610450565b820191906000526020600020905b81548152906001019060200180831161043357829003601f168201915b505050505081565b60008054600160a01b900460ff16158061047c57506000546001600160a01b031633145b61048557600080fd5b3360008181526006602090815260408083206001600160a01b03881680855290835292819020869055518581529192917f8c5be1e5ebec7d5bd14f71427d1e84f3dd0314c0f7b2291e5b200ac8c7c3b92591015b60405180910390a350600192915050565b60008054600160a01b900460ff16158061050e57506000546001600160a01b031633145b61051757600080fd5b6001600160a01b03841660009081526005602052604090205482111561053c57600080fd5b6001600160a01b038416600090815260066020908152604080832033845290915290205482111561056c57600080fd5b6001600160a01b0384166000908152600560205260408120805484929061059490849061107d565b90915550506001600160a01b038316600090815260056020526040812080548492906105c1908490611094565b90915550506001600160a01b0384166000908152600660209081526040808320338452909152812080548492906105f990849061107d565b90915550600195945050505050565b6000546001600160a01b0316331461061f57600080fd5b6000546001600160a01b031633146106765760405162461bcd60e51b81526020600482015260156024820152742cb7ba9030b932903737ba103a34329037bbb732b960591b60448201526064015b60405180910390fd5b806001600160a01b0316ff5b6000546001600160a01b0316331461069957600080fd5b600054600160a01b900460ff166106af57600080fd5b600054600160a01b900460ff1615156001146106ca57600080fd5b6000805460ff60a01b191681556040517f7805862f689e2f13df9f062ff482ad3ad112aca9e0847911ed832e158c525b339190a1565b6000546001600160a01b0316331461071757600080fd5b600054600160a01b900460ff16158061073a57506000546001600160a01b031633145b61074357600080fd5b6000546001600160a01b0316331461075a57600080fd5b7f0000000000000000000000000000000000000000000000000000000000000000816004546107899190611094565b11156107cd5760405162461bcd60e51b8152602060048201526013602482015272151bdad95b8e8818d85c08195e18d959591959606a1b604482015260640161066d565b6001600160a01b038216600090815260056020526040812080548392906107f5908490611094565b92505081905550806004600082825461080e9190611094565b90915550505050565b600080546001600160a01b0316331461082f57600080fd5b600054600160a01b900460ff16158061085257506000546001600160a01b031633145b61085b57600080fd5b3360009081526005602052604090205482111561087757600080fd5b336000908152600560205260408120805484929061089690849061107d565b9250508190555081600460008282546108af919061107d565b909155505060408051338152602081018490527fcc16f5dbb4873280815c1ee09dbd06736cffcc184412cf7a71a0fdb75d397ca5910160405180910390a1506001919050565b6000546001600160a01b0316331461090c57600080fd5b6000805461ffff60a01b191681556040517faff39f66825d4448497d384dee3f4a3adf00a622960add00806503ae4ccee01c9190a1565b60008054600160a01b900460ff16158061096757506000546001600160a01b031633145b61097057600080fd5b3360009081526006602090815260408083206001600160a01b0387168452909152902054808311156109c5573360009081526006602090815260408083206001600160a01b03881684529091528120556109f4565b6109cf838261107d565b3360009081526006602090815260408083206001600160a01b03891684529091529020555b6040518381526001600160a01b0385169033907ffbfd7cb641969b4296b582f72d2fbd6cf16d7fb1b333233a66e1091671d5d0219060200160405180910390a35060019392505050565b6000546001600160a01b03163314610a5557600080fd5b600080546040516001600160a01b03909116917ff8df31144d9c2f0f6b59d69b8b98abd5459d07f2742c4df920b25aae33c6482091a2600080546001600160a01b0319169055565b600080546001600160a01b03163314610ab557600080fd5b600054600160a01b900460ff161580610ad857506000546001600160a01b031633145b610ae157600080fd5b6000546001600160a01b03163314610af857600080fd5b6001600160a01b0383166000908152600660209081526040808320338452909152902054821115610b2857600080fd5b6001600160a01b03831660009081526005602052604081208054849290610b5090849061107d565b90915550506001600160a01b038316600090815260066020908152604080832033845290915281208054849290610b8890849061107d565b925050819055508160046000828254610ba1919061107d565b9091555050604080516001600160a01b0385168152602081018490527fcc16f5dbb4873280815c1ee09dbd06736cffcc184412cf7a71a0fdb75d397ca5910160405180910390a150600192915050565b6000546001600160a01b03163314610c0857600080fd5b600054600160a01b900460ff161580610c2b57506000546001600160a01b031633145b610c3457600080fd5b600054600160a81b900460ff161515600114610c4f57600080fd5b6000805460ff60a01b1916600160a01b1781556040517f6985a02210a168e66602d3235cb6db0e70f92b3ba4d376a33c0f3d9434bff6259190a1565b600280546103d79061102d565b60008054600160a01b900460ff161580610cbc57506000546001600160a01b031633145b610cc557600080fd5b33600090815260056020526040902054821115610ce157600080fd5b33600090815260056020526040902054610cfc90839061107d565b33600090815260056020526040808220929092556001600160a01b03851681522054610d29908390611094565b6001600160a01b0384166000818152600560205260409081902092909255905133907fddf252ad1be2c89b69c2b068fc378daa952ba7f163c4a11628f55a4df523b3ef906104d99086815260200190565b60008054600160a01b900460ff161580610d9e57506000546001600160a01b031633145b610da757600080fd5b3360008181526006602090815260408083206001600160a01b03881680855290835292819020869055518581529192917fa869c940e7ea6b9adb1299938ace0dabad3377352eb5e086ff21192ce4ca524b91016104d9565b60008054600160a01b900460ff161580610e2357506000546001600160a01b031633145b610e2c57600080fd5b506001600160a01b03918216600090815260066020908152604080832093909416825291909152205490565b6000546001600160a01b03163314610e6f57600080fd5b6001600160a01b038116610e8257600080fd5b600080546040516001600160a01b03808516939216917f8be0079c531659141344cd1fd0a4f28419497f9722a3daafe3b4186f6b6457e091a3600080546001600160a01b0319166001600160a01b0392909216919091179055565b600060208083528351808285015260005b81811015610f0a57858101830151858201604001528201610eee565b81811115610f1c576000604083870101525b50601f01601f1916929092016040019392505050565b6001600160a01b0381168114610f4757600080fd5b50565b60008060408385031215610f5d57600080fd5b8235610f6881610f32565b946020939093013593505050565b600080600060608486031215610f8b57600080fd5b8335610f9681610f32565b92506020840135610fa681610f32565b929592945050506040919091013590565b600060208284031215610fc957600080fd5b8135610fd481610f32565b9392505050565b600060208284031215610fed57600080fd5b5035919050565b6000806040838503121561100757600080fd5b823561101281610f32565b9150602083013561102281610f32565b809150509250929050565b600181811c9082168061104157607f821691505b60208210810361106157634e487b7160e01b600052602260045260246000fd5b50919050565b634e487b7160e01b600052601160045260246000fd5b60008282101561108f5761108f611067565b500390565b600082198211156110a7576110a7611067565b50019056fea2646970667358221220209e39557465513f9928b0b38b84c3178892a50afd4f463fd4ec930aaa3b809d64736f6c634300080f0033";

        private readonly string _abi = @" [{""inputs"":[{""internalType"":""uint256"",""name"":""initialSupply"",""type"":""uint256""},{""internalType"":""string"",""name"":""tokenName"",""type"":""string""},{""internalType"":""string"",""name"":""tokenSymbol"",""type"":""string""},{""internalType"":""uint256"",""name"":""tokenCap"",""type"":""uint256""}],""stateMutability"":""nonpayable"",""type"":""constructor""},{""anonymous"":false,""inputs"":[{""indexed"":true,""internalType"":""address"",""name"":""owner"",""type"":""address""},{""indexed"":true,""internalType"":""address"",""name"":""spender"",""type"":""address""},{""indexed"":false,""internalType"":""uint256"",""name"":""value"",""type"":""uint256""}],""name"":""Approval"",""type"":""event""},{""anonymous"":false,""inputs"":[{""indexed"":false,""internalType"":""address"",""name"":""from"",""type"":""address""},{""indexed"":false,""internalType"":""uint256"",""name"":""amount"",""type"":""uint256""}],""name"":""Burn"",""type"":""event""},{""anonymous"":false,""inputs"":[{""indexed"":true,""internalType"":""address"",""name"":""owner"",""type"":""address""},{""indexed"":true,""internalType"":""address"",""name"":""spender"",""type"":""address""},{""indexed"":false,""internalType"":""uint256"",""name"":""value"",""type"":""uint256""}],""name"":""DecreaseApproval"",""type"":""event""},{""anonymous"":false,""inputs"":[{""indexed"":true,""internalType"":""address"",""name"":""owner"",""type"":""address""},{""indexed"":true,""internalType"":""address"",""name"":""spender"",""type"":""address""},{""indexed"":false,""internalType"":""uint256"",""name"":""value"",""type"":""uint256""}],""name"":""IncreaseApproval"",""type"":""event""},{""anonymous"":false,""inputs"":[],""name"":""NotPausable"",""type"":""event""},{""anonymous"":false,""inputs"":[{""indexed"":true,""internalType"":""address"",""name"":""previousOwner"",""type"":""address""}],""name"":""OwnershipRenounced"",""type"":""event""},{""anonymous"":false,""inputs"":[{""indexed"":true,""internalType"":""address"",""name"":""previousOwner"",""type"":""address""},{""indexed"":true,""internalType"":""address"",""name"":""newOwner"",""type"":""address""}],""name"":""OwnershipTransferred"",""type"":""event""},{""anonymous"":false,""inputs"":[],""name"":""Pause"",""type"":""event""},{""anonymous"":false,""inputs"":[{""indexed"":false,""internalType"":""address"",""name"":""from"",""type"":""address""},{""indexed"":false,""internalType"":""address"",""name"":""to"",""type"":""address""},{""indexed"":false,""internalType"":""uint256"",""name"":""amount"",""type"":""uint256""}],""name"":""Sent"",""type"":""event""},{""anonymous"":false,""inputs"":[{""indexed"":true,""internalType"":""address"",""name"":""from"",""type"":""address""},{""indexed"":true,""internalType"":""address"",""name"":""to"",""type"":""address""},{""indexed"":false,""internalType"":""uint256"",""name"":""value"",""type"":""uint256""}],""name"":""Transfer"",""type"":""event""},{""anonymous"":false,""inputs"":[],""name"":""Unpause"",""type"":""event""},{""inputs"":[{""internalType"":""address"",""name"":""owner"",""type"":""address""},{""internalType"":""address"",""name"":""delegate"",""type"":""address""}],""name"":""allowance"",""outputs"":[{""internalType"":""uint256"",""name"":"""",""type"":""uint256""}],""stateMutability"":""view"",""type"":""function""},{""inputs"":[{""internalType"":""address"",""name"":"""",""type"":""address""},{""internalType"":""address"",""name"":"""",""type"":""address""}],""name"":""allowed"",""outputs"":[{""internalType"":""uint256"",""name"":"""",""type"":""uint256""}],""stateMutability"":""view"",""type"":""function""},{""inputs"":[{""internalType"":""address"",""name"":""delegate"",""type"":""address""},{""internalType"":""uint256"",""name"":""amount"",""type"":""uint256""}],""name"":""approve"",""outputs"":[{""internalType"":""bool"",""name"":"""",""type"":""bool""}],""stateMutability"":""nonpayable"",""type"":""function""},{""inputs"":[{""internalType"":""address"",""name"":"""",""type"":""address""}],""name"":""balanceOf"",""outputs"":[{""internalType"":""uint256"",""name"":"""",""type"":""uint256""}],""stateMutability"":""view"",""type"":""function""},{""inputs"":[{""internalType"":""uint256"",""name"":""amount"",""type"":""uint256""}],""name"":""burn"",""outputs"":[{""internalType"":""bool"",""name"":""success"",""type"":""bool""}],""stateMutability"":""nonpayable"",""type"":""function""},{""inputs"":[{""internalType"":""address"",""name"":""from"",""type"":""address""},{""internalType"":""uint256"",""name"":""amount"",""type"":""uint256""}],""name"":""burnFrom"",""outputs"":[{""internalType"":""bool"",""name"":""success"",""type"":""bool""}],""stateMutability"":""nonpayable"",""type"":""function""},{""inputs"":[],""name"":""canPause"",""outputs"":[{""internalType"":""bool"",""name"":"""",""type"":""bool""}],""stateMutability"":""view"",""type"":""function""},{""inputs"":[],""name"":""cap"",""outputs"":[{""internalType"":""uint256"",""name"":"""",""type"":""uint256""}],""stateMutability"":""view"",""type"":""function""},{""inputs"":[],""name"":""decimals"",""outputs"":[{""internalType"":""uint256"",""name"":"""",""type"":""uint256""}],""stateMutability"":""view"",""type"":""function""},{""inputs"":[{""internalType"":""address"",""name"":""spender"",""type"":""address""},{""internalType"":""uint256"",""name"":""subtractedAmount"",""type"":""uint256""}],""name"":""decreaseApproval"",""outputs"":[{""internalType"":""bool"",""name"":"""",""type"":""bool""}],""stateMutability"":""nonpayable"",""type"":""function""},{""inputs"":[{""internalType"":""address payable"",""name"":""_to"",""type"":""address""}],""name"":""destroySmartContract"",""outputs"":[],""stateMutability"":""nonpayable"",""type"":""function""},{""inputs"":[{""internalType"":""address"",""name"":""tokenOwner"",""type"":""address""}],""name"":""getBalance"",""outputs"":[{""internalType"":""uint256"",""name"":"""",""type"":""uint256""}],""stateMutability"":""view"",""type"":""function""},{""inputs"":[{""internalType"":""address"",""name"":""spender"",""type"":""address""},{""internalType"":""uint256"",""name"":""addedAmount"",""type"":""uint256""}],""name"":""increaseApproval"",""outputs"":[{""internalType"":""bool"",""name"":""success"",""type"":""bool""}],""stateMutability"":""nonpayable"",""type"":""function""},{""inputs"":[{""internalType"":""address"",""name"":""receiver"",""type"":""address""},{""internalType"":""uint256"",""name"":""amount"",""type"":""uint256""}],""name"":""mint"",""outputs"":[],""stateMutability"":""nonpayable"",""type"":""function""},{""inputs"":[],""name"":""name"",""outputs"":[{""internalType"":""string"",""name"":"""",""type"":""string""}],""stateMutability"":""view"",""type"":""function""},{""inputs"":[],""name"":""notPausable"",""outputs"":[],""stateMutability"":""nonpayable"",""type"":""function""},{""inputs"":[],""name"":""owner"",""outputs"":[{""internalType"":""address"",""name"":"""",""type"":""address""}],""stateMutability"":""view"",""type"":""function""},{""inputs"":[],""name"":""pause"",""outputs"":[],""stateMutability"":""nonpayable"",""type"":""function""},{""inputs"":[],""name"":""paused"",""outputs"":[{""internalType"":""bool"",""name"":"""",""type"":""bool""}],""stateMutability"":""view"",""type"":""function""},{""inputs"":[],""name"":""renounceOwnership"",""outputs"":[],""stateMutability"":""nonpayable"",""type"":""function""},{""inputs"":[],""name"":""symbol"",""outputs"":[{""internalType"":""string"",""name"":"""",""type"":""string""}],""stateMutability"":""view"",""type"":""function""},{""inputs"":[],""name"":""totalSupply"",""outputs"":[{""internalType"":""uint256"",""name"":"""",""type"":""uint256""}],""stateMutability"":""view"",""type"":""function""},{""inputs"":[{""internalType"":""address"",""name"":""receiver"",""type"":""address""},{""internalType"":""uint256"",""name"":""amount"",""type"":""uint256""}],""name"":""transfer"",""outputs"":[{""internalType"":""bool"",""name"":"""",""type"":""bool""}],""stateMutability"":""nonpayable"",""type"":""function""},{""inputs"":[{""internalType"":""address"",""name"":""from"",""type"":""address""},{""internalType"":""address"",""name"":""to"",""type"":""address""},{""internalType"":""uint256"",""name"":""value"",""type"":""uint256""}],""name"":""transferFrom"",""outputs"":[{""internalType"":""bool"",""name"":""success"",""type"":""bool""}],""stateMutability"":""nonpayable"",""type"":""function""},{""inputs"":[{""internalType"":""address"",""name"":""newOwner"",""type"":""address""}],""name"":""transferOwnership"",""outputs"":[],""stateMutability"":""nonpayable"",""type"":""function""},{""inputs"":[],""name"":""unpause"",""outputs"":[],""stateMutability"":""nonpayable"",""type"":""function""}] ";

        private readonly string _smartContractAddress = "0x0d26f523c24feda020c293185ac7a032814aedcf";

        private readonly User _user = new User();

        public EnumHelper EnumHelper { get; set; }

        public CryptoTokenController(IConfiguration configuration)
        {
            EnumHelper = new EnumHelper(configuration);
            _user = configuration.GetSection("User").Get<User>();
        }

        [HttpGet("Deploy")]
        public async Task<ActionResult> DeployContract(Chain chain, long initialSupply, string tokenName, string tokenSymbol, long tokenCap)
        {
            try
            {
                object[]? parameters = new object[4] { initialSupply, tokenName, tokenSymbol, tokenCap };

                Account? account = new Account(_user.PrivateKey, chain);
                Web3? web3 = new Web3(account, EnumHelper.GetStringBasedOnEnum(chain));

                HexBigInteger estimatedGas = await web3.Eth.DeployContract.EstimateGasAsync(_abi,
                                                                                            _byteCode,
                                                                                            _user.MetamaskAddress,
                                                                                            parameters);

                TransactionReceipt? deployContract = await web3.Eth.DeployContract.SendRequestAndWaitForReceiptAsync(_abi,
                                                                                                                     _byteCode,
                                                                                                                     _user.MetamaskAddress,
                                                                                                                     estimatedGas,
                                                                                                                     null,
                                                                                                                     parameters);

                return Ok($"Contract deployed successfully, transaction Hash {deployContract.TransactionHash} , smart contract address {deployContract.ContractAddress}");

            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }

        [HttpGet("GetBalance")]
        public async Task<ActionResult> GetBalance(Chain chain, string address)
        {
            try
            {
                Account? account = new Account(_user.PrivateKey, chain);
                Web3? web3 = new Web3(account, EnumHelper.GetStringBasedOnEnum(chain));

                object[]? parameters = new object[1] { address };
                Contract? smartContract = web3.Eth.GetContract(_abi, _smartContractAddress);
                Function? getBalance = smartContract.GetFunction("getBalance");

                BigInteger getBalanceResult = await getBalance.CallAsync<BigInteger>(parameters);

                BigDecimal balanceInEth = Web3.Convert.FromWeiToBigDecimal(getBalanceResult);

                return Ok($"{address} has {balanceInEth} tokens");
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }

        [HttpGet("Mint")]
        public async Task<ActionResult> Mint(Chain chain, string receiverAddress, long amount)
        {
            try
            {
                Account? account = new Account(_user.PrivateKey, chain);
                Web3? web3 = new Web3(account, EnumHelper.GetStringBasedOnEnum(chain));

                object[]? parameters = new object[2] { receiverAddress, amount };
                Contract? smartContract = web3.Eth.GetContract(_abi, _smartContractAddress);
                Function? mint = smartContract.GetFunction("mint");

                HexBigInteger? estimatedGas = await mint.EstimateGasAsync(account.Address, null, null, parameters);

                TransactionReceipt? mintResult = await mint.SendTransactionAndWaitForReceiptAsync(account.Address, estimatedGas, null, null, parameters);

                return Ok();
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }

        [HttpGet("Approve")]
        public async Task<ActionResult> Approve(Chain chain, string spenderAddress, long amount)
        {
            try
            {
                Account? account = new Account(_user.PrivateKey, chain);
                Web3? web3 = new Web3(account, EnumHelper.GetStringBasedOnEnum(chain));

                object[]? parameters = new object[2] { spenderAddress, amount };
                Contract? smartContract = web3.Eth.GetContract(_abi, _smartContractAddress);
                Function? approve = smartContract.GetFunction("approve");

                HexBigInteger? estimatedGas = await approve.EstimateGasAsync(account.Address, null, null, parameters);

                TransactionReceipt? approveResult = await approve.SendTransactionAndWaitForReceiptAsync(account.Address, estimatedGas, null, null, parameters);

                return Ok();
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }

        [HttpGet("Allowance")]
        public async Task<ActionResult> Allowance(Chain chain, string ownerAddress, string spenderAddress)
        {
            try
            {
                Account? account = new Account(_user.PrivateKey, chain);
                Web3? web3 = new Web3(account, EnumHelper.GetStringBasedOnEnum(chain));

                object[]? parameters = new object[2] { ownerAddress, spenderAddress };
                Contract? smartContract = web3.Eth.GetContract(_abi, _smartContractAddress);
                Function? allowance = smartContract.GetFunction("allowance");

                BigInteger allowanceAmount = await allowance.CallAsync<BigInteger>(parameters);
                BigDecimal balanceInEth = Web3.Convert.FromWeiToBigDecimal(allowanceAmount);

                return Ok($"{spenderAddress} allow to spend {balanceInEth} ETH of {ownerAddress}");
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }

        [HttpGet("DestroySmartContract")]
        public async Task<ActionResult> DestroySmartContract(Chain chain, string withdrawalAddress)
        {
            try
            {
                Account? account = new Account(_user.PrivateKey, chain);
                Web3? web3 = new Web3(account, EnumHelper.GetStringBasedOnEnum(chain));

                object[]? parameters = new object[1] { withdrawalAddress };
                Contract? smartContract = web3.Eth.GetContract(_abi, _smartContractAddress);
                Function? destroySmartContract = smartContract.GetFunction("destroySmartContract");

                HexBigInteger? estimatedGas = await destroySmartContract.EstimateGasAsync(account.Address, null, null, parameters);

                TransactionReceipt? destroySmartContractResult = await destroySmartContract.SendTransactionAndWaitForReceiptAsync(account.Address, estimatedGas, null, null, parameters);

                return Ok($"Smart contract destroyed {destroySmartContractResult.TransactionHash}");
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }

        [HttpGet("Transfer")]
        public async Task<ActionResult> Transfer(Chain chain, string receiver, long amountOfTokens)
        {
            try
            {
                Account? account = new Account(_user.PrivateKey, chain);
                Web3? web3 = new Web3(account, EnumHelper.GetStringBasedOnEnum(chain));

                object[]? parameters = new object[2] { receiver, amountOfTokens };
                Contract? smartContract = web3.Eth.GetContract(_abi, _smartContractAddress);
                Function? transfer = smartContract.GetFunction("transfer");

                TransferFunction? transferFunction = new TransferFunction()
                {
                    FromAddress = account.Address,
                    To = receiver,
                    Gas = 50000,
                    Value = amountOfTokens,
                };

                HexBigInteger? estimatedGas = await transfer.EstimateGasAsync(account.Address, null, null, parameters);

                TransactionReceipt? transferResult = await transfer.SendTransactionAndWaitForReceiptAsync(account.Address, estimatedGas, null, null, parameters);

                return Ok($"Tokens transfer was successful {transferResult.TransactionHash}");
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }

        [HttpGet("IncreaseApproval")]
        public async Task<ActionResult> IncreaseApproval(Chain chain, string spender, long addedAmount)
        {
            try
            {
                Account? account = new Account(_user.PrivateKey, chain);
                Web3? web3 = new Web3(account, EnumHelper.GetStringBasedOnEnum(chain));

                object[]? parameters = new object[2] { spender, addedAmount };
                Contract? smartContract = web3.Eth.GetContract(_abi, _smartContractAddress);
                Function? transfer = smartContract.GetFunction("increaseApproval");

                HexBigInteger? estimatedGas = await transfer.EstimateGasAsync(account.Address, null, null, parameters);

                TransactionReceipt? transferResult = await transfer.SendTransactionAndWaitForReceiptAsync(account.Address, estimatedGas, null, null, parameters);
                return Ok($"Increase approval was successful {transferResult.TransactionHash}");
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }

        [HttpGet("DecreaseApproval")]
        public async Task<ActionResult> DecreaseApproval(Chain chain, string spender, long subtractedAmount)
        {
            try
            {
                Account? account = new Account(_user.PrivateKey, chain);
                Web3? web3 = new Web3(account, EnumHelper.GetStringBasedOnEnum(chain));

                object[]? parameters = new object[2] { spender, subtractedAmount };
                Contract? smartContract = web3.Eth.GetContract(_abi, _smartContractAddress);
                Function? transfer = smartContract.GetFunction("decreaseApproval");

                HexBigInteger? estimatedGas = await transfer.EstimateGasAsync(account.Address, null, null, parameters);

                TransactionReceipt? transferResult = await transfer.SendTransactionAndWaitForReceiptAsync(account.Address, estimatedGas, null, null, parameters);

                return Ok($"Decrease approval was successful {transferResult.TransactionHash}");
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }

        [HttpGet("Burn")]
        public async Task<ActionResult> Burn(Chain chain, long amount)
        {
            try
            {
                Account? account = new Account(_user.PrivateKey, chain);
                Web3? web3 = new Web3(account, EnumHelper.GetStringBasedOnEnum(chain));

                object[]? parameters = new object[1] { amount };
                Contract? smartContract = web3.Eth.GetContract(_abi, _smartContractAddress);
                Function? transfer = smartContract.GetFunction("burn");

                HexBigInteger? estimatedGas = await transfer.EstimateGasAsync(account.Address, null, null, parameters);

                TransactionReceipt? transferResult = await transfer.SendTransactionAndWaitForReceiptAsync(account.Address, estimatedGas, null, null, parameters);

                return Ok($"{amount} tokens was burned {transferResult.TransactionHash}");
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }

        [HttpGet("TransferFrom")]
        public async Task<ActionResult> TransferFrom(Chain chain, string from, string to, long amount)
        {
            try
            {
                Account? account = new Account(_user.PrivateKey, chain);
                Web3? web3 = new Web3(account, EnumHelper.GetStringBasedOnEnum(chain));

                object[]? parameters = new object[3] { from, to, amount };

            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }
    }
}
