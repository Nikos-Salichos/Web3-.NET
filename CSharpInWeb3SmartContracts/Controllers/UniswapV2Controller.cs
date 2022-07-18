using CSharpInWeb3SmartContracts.DTOs;
using CSharpInWeb3SmartContracts.GraphQL;
using CSharpInWeb3SmartContracts.Models;
using CSharpInWeb3SmartContracts.Utilities;
using Microsoft.AspNetCore.Mvc;
using Nethereum.Contracts;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Signer;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using System.Numerics;

namespace CSharpInWeb3SmartContracts.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UniswapV2Controller : ControllerBase
    {
        private readonly User _user = new User();

        private readonly string WETH_KOVAN_V2 = "0xd0A1E359811322d97991E03f863a0C30C2cF029C";
        private readonly string DAI_KOVAN_V2 = "0x4F96Fe3b7A6Cf9725f59d353F723c1bDb64CA6Aa";

        private readonly string _uniswapV2FactoryAddress = "0x5C69bEe701ef814a2B6a3EDD4B1652CB9cc5aA6f";
        private readonly string _uniswapv2FactoryAbi = @" [{""inputs"":[{""internalType"":""address"",""name"":""_feeToSetter"",""type"":""address""}],""payable"":false,""stateMutability"":""nonpayable"",""type"":""constructor""},{""anonymous"":false,""inputs"":[{""indexed"":true,""internalType"":""address"",""name"":""token0"",""type"":""address""},{""indexed"":true,""internalType"":""address"",""name"":""token1"",""type"":""address""},{""indexed"":false,""internalType"":""address"",""name"":""pair"",""type"":""address""},{""indexed"":false,""internalType"":""uint256"",""name"":"""",""type"":""uint256""}],""name"":""PairCreated"",""type"":""event""},{""constant"":true,""inputs"":[{""internalType"":""uint256"",""name"":"""",""type"":""uint256""}],""name"":""allPairs"",""outputs"":[{""internalType"":""address"",""name"":"""",""type"":""address""}],""payable"":false,""stateMutability"":""view"",""type"":""function""},{""constant"":true,""inputs"":[],""name"":""allPairsLength"",""outputs"":[{""internalType"":""uint256"",""name"":"""",""type"":""uint256""}],""payable"":false,""stateMutability"":""view"",""type"":""function""},{""constant"":false,""inputs"":[{""internalType"":""address"",""name"":""tokenA"",""type"":""address""},{""internalType"":""address"",""name"":""tokenB"",""type"":""address""}],""name"":""createPair"",""outputs"":[{""internalType"":""address"",""name"":""pair"",""type"":""address""}],""payable"":false,""stateMutability"":""nonpayable"",""type"":""function""},{""constant"":true,""inputs"":[],""name"":""feeTo"",""outputs"":[{""internalType"":""address"",""name"":"""",""type"":""address""}],""payable"":false,""stateMutability"":""view"",""type"":""function""},{""constant"":true,""inputs"":[],""name"":""feeToSetter"",""outputs"":[{""internalType"":""address"",""name"":"""",""type"":""address""}],""payable"":false,""stateMutability"":""view"",""type"":""function""},{""constant"":true,""inputs"":[{""internalType"":""address"",""name"":"""",""type"":""address""},{""internalType"":""address"",""name"":"""",""type"":""address""}],""name"":""getPair"",""outputs"":[{""internalType"":""address"",""name"":"""",""type"":""address""}],""payable"":false,""stateMutability"":""view"",""type"":""function""},{""constant"":false,""inputs"":[{""internalType"":""address"",""name"":""_feeTo"",""type"":""address""}],""name"":""setFeeTo"",""outputs"":[],""payable"":false,""stateMutability"":""nonpayable"",""type"":""function""},{""constant"":false,""inputs"":[{""internalType"":""address"",""name"":""_feeToSetter"",""type"":""address""}],""name"":""setFeeToSetter"",""outputs"":[],""payable"":false,""stateMutability"":""nonpayable"",""type"":""function""}] ";

        private readonly string _uniswapV2RouterAddress = "0x7a250d5630B4cF539739dF2C5dAcb4c659F2488D";
        private readonly string _uniswapV2RouterAbi = @" [{""inputs"":[{""internalType"":""address"",""name"":""_factory"",""type"":""address""},{""internalType"":""address"",""name"":""_WETH"",""type"":""address""}],""stateMutability"":""nonpayable"",""type"":""constructor""},{""inputs"":[],""name"":""WETH"",""outputs"":[{""internalType"":""address"",""name"":"""",""type"":""address""}],""stateMutability"":""view"",""type"":""function""},{""inputs"":[{""internalType"":""address"",""name"":""tokenA"",""type"":""address""},{""internalType"":""address"",""name"":""tokenB"",""type"":""address""},{""internalType"":""uint256"",""name"":""amountADesired"",""type"":""uint256""},{""internalType"":""uint256"",""name"":""amountBDesired"",""type"":""uint256""},{""internalType"":""uint256"",""name"":""amountAMin"",""type"":""uint256""},{""internalType"":""uint256"",""name"":""amountBMin"",""type"":""uint256""},{""internalType"":""address"",""name"":""to"",""type"":""address""},{""internalType"":""uint256"",""name"":""deadline"",""type"":""uint256""}],""name"":""addLiquidity"",""outputs"":[{""internalType"":""uint256"",""name"":""amountA"",""type"":""uint256""},{""internalType"":""uint256"",""name"":""amountB"",""type"":""uint256""},{""internalType"":""uint256"",""name"":""liquidity"",""type"":""uint256""}],""stateMutability"":""nonpayable"",""type"":""function""},{""inputs"":[{""internalType"":""address"",""name"":""token"",""type"":""address""},{""internalType"":""uint256"",""name"":""amountTokenDesired"",""type"":""uint256""},{""internalType"":""uint256"",""name"":""amountTokenMin"",""type"":""uint256""},{""internalType"":""uint256"",""name"":""amountETHMin"",""type"":""uint256""},{""internalType"":""address"",""name"":""to"",""type"":""address""},{""internalType"":""uint256"",""name"":""deadline"",""type"":""uint256""}],""name"":""addLiquidityETH"",""outputs"":[{""internalType"":""uint256"",""name"":""amountToken"",""type"":""uint256""},{""internalType"":""uint256"",""name"":""amountETH"",""type"":""uint256""},{""internalType"":""uint256"",""name"":""liquidity"",""type"":""uint256""}],""stateMutability"":""payable"",""type"":""function""},{""inputs"":[],""name"":""factory"",""outputs"":[{""internalType"":""address"",""name"":"""",""type"":""address""}],""stateMutability"":""view"",""type"":""function""},{""inputs"":[{""internalType"":""uint256"",""name"":""amountOut"",""type"":""uint256""},{""internalType"":""uint256"",""name"":""reserveIn"",""type"":""uint256""},{""internalType"":""uint256"",""name"":""reserveOut"",""type"":""uint256""}],""name"":""getAmountIn"",""outputs"":[{""internalType"":""uint256"",""name"":""amountIn"",""type"":""uint256""}],""stateMutability"":""pure"",""type"":""function""},{""inputs"":[{""internalType"":""uint256"",""name"":""amountIn"",""type"":""uint256""},{""internalType"":""uint256"",""name"":""reserveIn"",""type"":""uint256""},{""internalType"":""uint256"",""name"":""reserveOut"",""type"":""uint256""}],""name"":""getAmountOut"",""outputs"":[{""internalType"":""uint256"",""name"":""amountOut"",""type"":""uint256""}],""stateMutability"":""pure"",""type"":""function""},{""inputs"":[{""internalType"":""uint256"",""name"":""amountOut"",""type"":""uint256""},{""internalType"":""address[]"",""name"":""path"",""type"":""address[]""}],""name"":""getAmountsIn"",""outputs"":[{""internalType"":""uint256[]"",""name"":""amounts"",""type"":""uint256[]""}],""stateMutability"":""view"",""type"":""function""},{""inputs"":[{""internalType"":""uint256"",""name"":""amountIn"",""type"":""uint256""},{""internalType"":""address[]"",""name"":""path"",""type"":""address[]""}],""name"":""getAmountsOut"",""outputs"":[{""internalType"":""uint256[]"",""name"":""amounts"",""type"":""uint256[]""}],""stateMutability"":""view"",""type"":""function""},{""inputs"":[{""internalType"":""uint256"",""name"":""amountA"",""type"":""uint256""},{""internalType"":""uint256"",""name"":""reserveA"",""type"":""uint256""},{""internalType"":""uint256"",""name"":""reserveB"",""type"":""uint256""}],""name"":""quote"",""outputs"":[{""internalType"":""uint256"",""name"":""amountB"",""type"":""uint256""}],""stateMutability"":""pure"",""type"":""function""},{""inputs"":[{""internalType"":""address"",""name"":""tokenA"",""type"":""address""},{""internalType"":""address"",""name"":""tokenB"",""type"":""address""},{""internalType"":""uint256"",""name"":""liquidity"",""type"":""uint256""},{""internalType"":""uint256"",""name"":""amountAMin"",""type"":""uint256""},{""internalType"":""uint256"",""name"":""amountBMin"",""type"":""uint256""},{""internalType"":""address"",""name"":""to"",""type"":""address""},{""internalType"":""uint256"",""name"":""deadline"",""type"":""uint256""}],""name"":""removeLiquidity"",""outputs"":[{""internalType"":""uint256"",""name"":""amountA"",""type"":""uint256""},{""internalType"":""uint256"",""name"":""amountB"",""type"":""uint256""}],""stateMutability"":""nonpayable"",""type"":""function""},{""inputs"":[{""internalType"":""address"",""name"":""token"",""type"":""address""},{""internalType"":""uint256"",""name"":""liquidity"",""type"":""uint256""},{""internalType"":""uint256"",""name"":""amountTokenMin"",""type"":""uint256""},{""internalType"":""uint256"",""name"":""amountETHMin"",""type"":""uint256""},{""internalType"":""address"",""name"":""to"",""type"":""address""},{""internalType"":""uint256"",""name"":""deadline"",""type"":""uint256""}],""name"":""removeLiquidityETH"",""outputs"":[{""internalType"":""uint256"",""name"":""amountToken"",""type"":""uint256""},{""internalType"":""uint256"",""name"":""amountETH"",""type"":""uint256""}],""stateMutability"":""nonpayable"",""type"":""function""},{""inputs"":[{""internalType"":""address"",""name"":""token"",""type"":""address""},{""internalType"":""uint256"",""name"":""liquidity"",""type"":""uint256""},{""internalType"":""uint256"",""name"":""amountTokenMin"",""type"":""uint256""},{""internalType"":""uint256"",""name"":""amountETHMin"",""type"":""uint256""},{""internalType"":""address"",""name"":""to"",""type"":""address""},{""internalType"":""uint256"",""name"":""deadline"",""type"":""uint256""}],""name"":""removeLiquidityETHSupportingFeeOnTransferTokens"",""outputs"":[{""internalType"":""uint256"",""name"":""amountETH"",""type"":""uint256""}],""stateMutability"":""nonpayable"",""type"":""function""},{""inputs"":[{""internalType"":""address"",""name"":""token"",""type"":""address""},{""internalType"":""uint256"",""name"":""liquidity"",""type"":""uint256""},{""internalType"":""uint256"",""name"":""amountTokenMin"",""type"":""uint256""},{""internalType"":""uint256"",""name"":""amountETHMin"",""type"":""uint256""},{""internalType"":""address"",""name"":""to"",""type"":""address""},{""internalType"":""uint256"",""name"":""deadline"",""type"":""uint256""},{""internalType"":""bool"",""name"":""approveMax"",""type"":""bool""},{""internalType"":""uint8"",""name"":""v"",""type"":""uint8""},{""internalType"":""bytes32"",""name"":""r"",""type"":""bytes32""},{""internalType"":""bytes32"",""name"":""s"",""type"":""bytes32""}],""name"":""removeLiquidityETHWithPermit"",""outputs"":[{""internalType"":""uint256"",""name"":""amountToken"",""type"":""uint256""},{""internalType"":""uint256"",""name"":""amountETH"",""type"":""uint256""}],""stateMutability"":""nonpayable"",""type"":""function""},{""inputs"":[{""internalType"":""address"",""name"":""token"",""type"":""address""},{""internalType"":""uint256"",""name"":""liquidity"",""type"":""uint256""},{""internalType"":""uint256"",""name"":""amountTokenMin"",""type"":""uint256""},{""internalType"":""uint256"",""name"":""amountETHMin"",""type"":""uint256""},{""internalType"":""address"",""name"":""to"",""type"":""address""},{""internalType"":""uint256"",""name"":""deadline"",""type"":""uint256""},{""internalType"":""bool"",""name"":""approveMax"",""type"":""bool""},{""internalType"":""uint8"",""name"":""v"",""type"":""uint8""},{""internalType"":""bytes32"",""name"":""r"",""type"":""bytes32""},{""internalType"":""bytes32"",""name"":""s"",""type"":""bytes32""}],""name"":""removeLiquidityETHWithPermitSupportingFeeOnTransferTokens"",""outputs"":[{""internalType"":""uint256"",""name"":""amountETH"",""type"":""uint256""}],""stateMutability"":""nonpayable"",""type"":""function""},{""inputs"":[{""internalType"":""address"",""name"":""tokenA"",""type"":""address""},{""internalType"":""address"",""name"":""tokenB"",""type"":""address""},{""internalType"":""uint256"",""name"":""liquidity"",""type"":""uint256""},{""internalType"":""uint256"",""name"":""amountAMin"",""type"":""uint256""},{""internalType"":""uint256"",""name"":""amountBMin"",""type"":""uint256""},{""internalType"":""address"",""name"":""to"",""type"":""address""},{""internalType"":""uint256"",""name"":""deadline"",""type"":""uint256""},{""internalType"":""bool"",""name"":""approveMax"",""type"":""bool""},{""internalType"":""uint8"",""name"":""v"",""type"":""uint8""},{""internalType"":""bytes32"",""name"":""r"",""type"":""bytes32""},{""internalType"":""bytes32"",""name"":""s"",""type"":""bytes32""}],""name"":""removeLiquidityWithPermit"",""outputs"":[{""internalType"":""uint256"",""name"":""amountA"",""type"":""uint256""},{""internalType"":""uint256"",""name"":""amountB"",""type"":""uint256""}],""stateMutability"":""nonpayable"",""type"":""function""},{""inputs"":[{""internalType"":""uint256"",""name"":""amountOut"",""type"":""uint256""},{""internalType"":""address[]"",""name"":""path"",""type"":""address[]""},{""internalType"":""address"",""name"":""to"",""type"":""address""},{""internalType"":""uint256"",""name"":""deadline"",""type"":""uint256""}],""name"":""swapETHForExactTokens"",""outputs"":[{""internalType"":""uint256[]"",""name"":""amounts"",""type"":""uint256[]""}],""stateMutability"":""payable"",""type"":""function""},{""inputs"":[{""internalType"":""uint256"",""name"":""amountOutMin"",""type"":""uint256""},{""internalType"":""address[]"",""name"":""path"",""type"":""address[]""},{""internalType"":""address"",""name"":""to"",""type"":""address""},{""internalType"":""uint256"",""name"":""deadline"",""type"":""uint256""}],""name"":""swapExactETHForTokens"",""outputs"":[{""internalType"":""uint256[]"",""name"":""amounts"",""type"":""uint256[]""}],""stateMutability"":""payable"",""type"":""function""},{""inputs"":[{""internalType"":""uint256"",""name"":""amountOutMin"",""type"":""uint256""},{""internalType"":""address[]"",""name"":""path"",""type"":""address[]""},{""internalType"":""address"",""name"":""to"",""type"":""address""},{""internalType"":""uint256"",""name"":""deadline"",""type"":""uint256""}],""name"":""swapExactETHForTokensSupportingFeeOnTransferTokens"",""outputs"":[],""stateMutability"":""payable"",""type"":""function""},{""inputs"":[{""internalType"":""uint256"",""name"":""amountIn"",""type"":""uint256""},{""internalType"":""uint256"",""name"":""amountOutMin"",""type"":""uint256""},{""internalType"":""address[]"",""name"":""path"",""type"":""address[]""},{""internalType"":""address"",""name"":""to"",""type"":""address""},{""internalType"":""uint256"",""name"":""deadline"",""type"":""uint256""}],""name"":""swapExactTokensForETH"",""outputs"":[{""internalType"":""uint256[]"",""name"":""amounts"",""type"":""uint256[]""}],""stateMutability"":""nonpayable"",""type"":""function""},{""inputs"":[{""internalType"":""uint256"",""name"":""amountIn"",""type"":""uint256""},{""internalType"":""uint256"",""name"":""amountOutMin"",""type"":""uint256""},{""internalType"":""address[]"",""name"":""path"",""type"":""address[]""},{""internalType"":""address"",""name"":""to"",""type"":""address""},{""internalType"":""uint256"",""name"":""deadline"",""type"":""uint256""}],""name"":""swapExactTokensForETHSupportingFeeOnTransferTokens"",""outputs"":[],""stateMutability"":""nonpayable"",""type"":""function""},{""inputs"":[{""internalType"":""uint256"",""name"":""amountIn"",""type"":""uint256""},{""internalType"":""uint256"",""name"":""amountOutMin"",""type"":""uint256""},{""internalType"":""address[]"",""name"":""path"",""type"":""address[]""},{""internalType"":""address"",""name"":""to"",""type"":""address""},{""internalType"":""uint256"",""name"":""deadline"",""type"":""uint256""}],""name"":""swapExactTokensForTokens"",""outputs"":[{""internalType"":""uint256[]"",""name"":""amounts"",""type"":""uint256[]""}],""stateMutability"":""nonpayable"",""type"":""function""},{""inputs"":[{""internalType"":""uint256"",""name"":""amountIn"",""type"":""uint256""},{""internalType"":""uint256"",""name"":""amountOutMin"",""type"":""uint256""},{""internalType"":""address[]"",""name"":""path"",""type"":""address[]""},{""internalType"":""address"",""name"":""to"",""type"":""address""},{""internalType"":""uint256"",""name"":""deadline"",""type"":""uint256""}],""name"":""swapExactTokensForTokensSupportingFeeOnTransferTokens"",""outputs"":[],""stateMutability"":""nonpayable"",""type"":""function""},{""inputs"":[{""internalType"":""uint256"",""name"":""amountOut"",""type"":""uint256""},{""internalType"":""uint256"",""name"":""amountInMax"",""type"":""uint256""},{""internalType"":""address[]"",""name"":""path"",""type"":""address[]""},{""internalType"":""address"",""name"":""to"",""type"":""address""},{""internalType"":""uint256"",""name"":""deadline"",""type"":""uint256""}],""name"":""swapTokensForExactETH"",""outputs"":[{""internalType"":""uint256[]"",""name"":""amounts"",""type"":""uint256[]""}],""stateMutability"":""nonpayable"",""type"":""function""},{""inputs"":[{""internalType"":""uint256"",""name"":""amountOut"",""type"":""uint256""},{""internalType"":""uint256"",""name"":""amountInMax"",""type"":""uint256""},{""internalType"":""address[]"",""name"":""path"",""type"":""address[]""},{""internalType"":""address"",""name"":""to"",""type"":""address""},{""internalType"":""uint256"",""name"":""deadline"",""type"":""uint256""}],""name"":""swapTokensForExactTokens"",""outputs"":[{""internalType"":""uint256[]"",""name"":""amounts"",""type"":""uint256[]""}],""stateMutability"":""nonpayable"",""type"":""function""},{""stateMutability"":""payable"",""type"":""receive""}] ";

        private readonly string _pairERC20Abi = @" [{""inputs"":[],""payable"":false,""stateMutability"":""nonpayable"",""type"":""constructor""},{""anonymous"":false,""inputs"":[{""indexed"":true,""internalType"":""address"",""name"":""owner"",""type"":""address""},{""indexed"":true,""internalType"":""address"",""name"":""spender"",""type"":""address""},{""indexed"":false,""internalType"":""uint256"",""name"":""value"",""type"":""uint256""}],""name"":""Approval"",""type"":""event""},{""anonymous"":false,""inputs"":[{""indexed"":true,""internalType"":""address"",""name"":""sender"",""type"":""address""},{""indexed"":false,""internalType"":""uint256"",""name"":""amount0"",""type"":""uint256""},{""indexed"":false,""internalType"":""uint256"",""name"":""amount1"",""type"":""uint256""},{""indexed"":true,""internalType"":""address"",""name"":""to"",""type"":""address""}],""name"":""Burn"",""type"":""event""},{""anonymous"":false,""inputs"":[{""indexed"":true,""internalType"":""address"",""name"":""sender"",""type"":""address""},{""indexed"":false,""internalType"":""uint256"",""name"":""amount0"",""type"":""uint256""},{""indexed"":false,""internalType"":""uint256"",""name"":""amount1"",""type"":""uint256""}],""name"":""Mint"",""type"":""event""},{""anonymous"":false,""inputs"":[{""indexed"":true,""internalType"":""address"",""name"":""sender"",""type"":""address""},{""indexed"":false,""internalType"":""uint256"",""name"":""amount0In"",""type"":""uint256""},{""indexed"":false,""internalType"":""uint256"",""name"":""amount1In"",""type"":""uint256""},{""indexed"":false,""internalType"":""uint256"",""name"":""amount0Out"",""type"":""uint256""},{""indexed"":false,""internalType"":""uint256"",""name"":""amount1Out"",""type"":""uint256""},{""indexed"":true,""internalType"":""address"",""name"":""to"",""type"":""address""}],""name"":""Swap"",""type"":""event""},{""anonymous"":false,""inputs"":[{""indexed"":false,""internalType"":""uint112"",""name"":""reserve0"",""type"":""uint112""},{""indexed"":false,""internalType"":""uint112"",""name"":""reserve1"",""type"":""uint112""}],""name"":""Sync"",""type"":""event""},{""anonymous"":false,""inputs"":[{""indexed"":true,""internalType"":""address"",""name"":""from"",""type"":""address""},{""indexed"":true,""internalType"":""address"",""name"":""to"",""type"":""address""},{""indexed"":false,""internalType"":""uint256"",""name"":""value"",""type"":""uint256""}],""name"":""Transfer"",""type"":""event""},{""constant"":true,""inputs"":[],""name"":""DOMAIN_SEPARATOR"",""outputs"":[{""internalType"":""bytes32"",""name"":"""",""type"":""bytes32""}],""payable"":false,""stateMutability"":""view"",""type"":""function""},{""constant"":true,""inputs"":[],""name"":""MINIMUM_LIQUIDITY"",""outputs"":[{""internalType"":""uint256"",""name"":"""",""type"":""uint256""}],""payable"":false,""stateMutability"":""view"",""type"":""function""},{""constant"":true,""inputs"":[],""name"":""PERMIT_TYPEHASH"",""outputs"":[{""internalType"":""bytes32"",""name"":"""",""type"":""bytes32""}],""payable"":false,""stateMutability"":""view"",""type"":""function""},{""constant"":true,""inputs"":[{""internalType"":""address"",""name"":"""",""type"":""address""},{""internalType"":""address"",""name"":"""",""type"":""address""}],""name"":""allowance"",""outputs"":[{""internalType"":""uint256"",""name"":"""",""type"":""uint256""}],""payable"":false,""stateMutability"":""view"",""type"":""function""},{""constant"":false,""inputs"":[{""internalType"":""address"",""name"":""spender"",""type"":""address""},{""internalType"":""uint256"",""name"":""value"",""type"":""uint256""}],""name"":""approve"",""outputs"":[{""internalType"":""bool"",""name"":"""",""type"":""bool""}],""payable"":false,""stateMutability"":""nonpayable"",""type"":""function""},{""constant"":true,""inputs"":[{""internalType"":""address"",""name"":"""",""type"":""address""}],""name"":""balanceOf"",""outputs"":[{""internalType"":""uint256"",""name"":"""",""type"":""uint256""}],""payable"":false,""stateMutability"":""view"",""type"":""function""},{""constant"":false,""inputs"":[{""internalType"":""address"",""name"":""to"",""type"":""address""}],""name"":""burn"",""outputs"":[{""internalType"":""uint256"",""name"":""amount0"",""type"":""uint256""},{""internalType"":""uint256"",""name"":""amount1"",""type"":""uint256""}],""payable"":false,""stateMutability"":""nonpayable"",""type"":""function""},{""constant"":true,""inputs"":[],""name"":""decimals"",""outputs"":[{""internalType"":""uint8"",""name"":"""",""type"":""uint8""}],""payable"":false,""stateMutability"":""view"",""type"":""function""},{""constant"":true,""inputs"":[],""name"":""factory"",""outputs"":[{""internalType"":""address"",""name"":"""",""type"":""address""}],""payable"":false,""stateMutability"":""view"",""type"":""function""},{""constant"":true,""inputs"":[],""name"":""getReserves"",""outputs"":[{""internalType"":""uint112"",""name"":""_reserve0"",""type"":""uint112""},{""internalType"":""uint112"",""name"":""_reserve1"",""type"":""uint112""},{""internalType"":""uint32"",""name"":""_blockTimestampLast"",""type"":""uint32""}],""payable"":false,""stateMutability"":""view"",""type"":""function""},{""constant"":false,""inputs"":[{""internalType"":""address"",""name"":""_token0"",""type"":""address""},{""internalType"":""address"",""name"":""_token1"",""type"":""address""}],""name"":""initialize"",""outputs"":[],""payable"":false,""stateMutability"":""nonpayable"",""type"":""function""},{""constant"":true,""inputs"":[],""name"":""kLast"",""outputs"":[{""internalType"":""uint256"",""name"":"""",""type"":""uint256""}],""payable"":false,""stateMutability"":""view"",""type"":""function""},{""constant"":false,""inputs"":[{""internalType"":""address"",""name"":""to"",""type"":""address""}],""name"":""mint"",""outputs"":[{""internalType"":""uint256"",""name"":""liquidity"",""type"":""uint256""}],""payable"":false,""stateMutability"":""nonpayable"",""type"":""function""},{""constant"":true,""inputs"":[],""name"":""name"",""outputs"":[{""internalType"":""string"",""name"":"""",""type"":""string""}],""payable"":false,""stateMutability"":""view"",""type"":""function""},{""constant"":true,""inputs"":[{""internalType"":""address"",""name"":"""",""type"":""address""}],""name"":""nonces"",""outputs"":[{""internalType"":""uint256"",""name"":"""",""type"":""uint256""}],""payable"":false,""stateMutability"":""view"",""type"":""function""},{""constant"":false,""inputs"":[{""internalType"":""address"",""name"":""owner"",""type"":""address""},{""internalType"":""address"",""name"":""spender"",""type"":""address""},{""internalType"":""uint256"",""name"":""value"",""type"":""uint256""},{""internalType"":""uint256"",""name"":""deadline"",""type"":""uint256""},{""internalType"":""uint8"",""name"":""v"",""type"":""uint8""},{""internalType"":""bytes32"",""name"":""r"",""type"":""bytes32""},{""internalType"":""bytes32"",""name"":""s"",""type"":""bytes32""}],""name"":""permit"",""outputs"":[],""payable"":false,""stateMutability"":""nonpayable"",""type"":""function""},{""constant"":true,""inputs"":[],""name"":""price0CumulativeLast"",""outputs"":[{""internalType"":""uint256"",""name"":"""",""type"":""uint256""}],""payable"":false,""stateMutability"":""view"",""type"":""function""},{""constant"":true,""inputs"":[],""name"":""price1CumulativeLast"",""outputs"":[{""internalType"":""uint256"",""name"":"""",""type"":""uint256""}],""payable"":false,""stateMutability"":""view"",""type"":""function""},{""constant"":false,""inputs"":[{""internalType"":""address"",""name"":""to"",""type"":""address""}],""name"":""skim"",""outputs"":[],""payable"":false,""stateMutability"":""nonpayable"",""type"":""function""},{""constant"":false,""inputs"":[{""internalType"":""uint256"",""name"":""amount0Out"",""type"":""uint256""},{""internalType"":""uint256"",""name"":""amount1Out"",""type"":""uint256""},{""internalType"":""address"",""name"":""to"",""type"":""address""},{""internalType"":""bytes"",""name"":""data"",""type"":""bytes""}],""name"":""swap"",""outputs"":[],""payable"":false,""stateMutability"":""nonpayable"",""type"":""function""},{""constant"":true,""inputs"":[],""name"":""symbol"",""outputs"":[{""internalType"":""string"",""name"":"""",""type"":""string""}],""payable"":false,""stateMutability"":""view"",""type"":""function""},{""constant"":false,""inputs"":[],""name"":""sync"",""outputs"":[],""payable"":false,""stateMutability"":""nonpayable"",""type"":""function""},{""constant"":true,""inputs"":[],""name"":""token0"",""outputs"":[{""internalType"":""address"",""name"":"""",""type"":""address""}],""payable"":false,""stateMutability"":""view"",""type"":""function""},{""constant"":true,""inputs"":[],""name"":""token1"",""outputs"":[{""internalType"":""address"",""name"":"""",""type"":""address""}],""payable"":false,""stateMutability"":""view"",""type"":""function""},{""constant"":true,""inputs"":[],""name"":""totalSupply"",""outputs"":[{""internalType"":""uint256"",""name"":"""",""type"":""uint256""}],""payable"":false,""stateMutability"":""view"",""type"":""function""},{""constant"":false,""inputs"":[{""internalType"":""address"",""name"":""to"",""type"":""address""},{""internalType"":""uint256"",""name"":""value"",""type"":""uint256""}],""name"":""transfer"",""outputs"":[{""internalType"":""bool"",""name"":"""",""type"":""bool""}],""payable"":false,""stateMutability"":""nonpayable"",""type"":""function""},{""constant"":false,""inputs"":[{""internalType"":""address"",""name"":""from"",""type"":""address""},{""internalType"":""address"",""name"":""to"",""type"":""address""},{""internalType"":""uint256"",""name"":""value"",""type"":""uint256""}],""name"":""transferFrom"",""outputs"":[{""internalType"":""bool"",""name"":"""",""type"":""bool""}],""payable"":false,""stateMutability"":""nonpayable"",""type"":""function""}] ";
        public EnumHelper EnumHelper { get; set; }

        private readonly UniswapV3GraphQL _uniswapGraphQL;

        public UniswapV2Controller(IConfiguration configuration)
        {
            EnumHelper = new EnumHelper(configuration);
            _user = configuration.GetSection("User").Get<User>();
        }

        [Produces("application/json")]
        [HttpGet("UniswapV2FactoryAllPairs")]
        public async Task<ActionResult<List<Pair>>> GetUniswapV2allPairsLength(Chain chain)
        {
            try
            {
                Account? account = new Account(_user.PrivateKey, chain);
                Web3? web3 = new Web3(account, EnumHelper.GetStringBasedOnEnum(chain));

                Contract? smartContract = web3.Eth.GetContract(_uniswapv2FactoryAbi, _uniswapV2FactoryAddress);
                Function? allPairsLength = smartContract.GetFunction("allPairsLength");

                long pairsCount = await allPairsLength.CallAsync<long>();
                List<Pair> pairsAddresses = new List<Pair>();

                for (int i = 0; i < Convert.ToDouble(pairsCount); i++)
                {
                    object[] parameters = new object[1] { i };
                    Function? allPairs = smartContract.GetFunction("allPairs");
                    string pairAddress = await allPairs.CallAsync<string>(parameters);
                    Pair pair = new Pair();
                    pair.Id = pairAddress;

                    Contract? smartContractPair = web3.Eth.GetContract(_pairERC20Abi, pair.Id);

                    Function? getToken0 = smartContractPair.GetFunction("token0");
                    pair.Token0.Address = await getToken0.CallAsync<string>();

                    Function? getToken1 = smartContractPair.GetFunction("token1");
                    pair.Token1.Address = await getToken1.CallAsync<string>();

                    pairsAddresses.Add(pair);
                }

                return Ok(pairsAddresses);
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }

        [HttpGet("UniswapV2FactoryGetReserves")]
        public async Task<ActionResult> GetUniswapV2FactoryGetReserves(Chain chain, string addressToken0, string addressToken1)
        {
            try
            {
                Account? account = new Account(_user.PrivateKey, chain);
                Web3? web3 = new Web3(account, EnumHelper.GetStringBasedOnEnum(chain));

                Contract? smartContract = web3.Eth.GetContract(_uniswapv2FactoryAbi, _uniswapV2FactoryAddress);
                Function? getPair = smartContract.GetFunction("getPair");

                object[] parameters = new object[2] { addressToken0, addressToken1 };
                string pairAddress = await getPair.CallAsync<string>(parameters);

                if (pairAddress == "0x0000000000000000000000000000000000000000")
                {
                    return NotFound();
                }

                Contract? smartContractPair = web3.Eth.GetContract(_pairERC20Abi, pairAddress);
                Function? getReserves = smartContractPair.GetFunction("getReserves");
                GetReservesDTO? reserves = await getReserves.CallDeserializingToObjectAsync<GetReservesDTO>();

                Function? getToken0 = smartContractPair.GetFunction("token0");
                string token0 = await getToken0.CallAsync<string>();

                Function? getToken1 = smartContractPair.GetFunction("token1");
                string token1 = await getToken1.CallAsync<string>();

                return Ok($"Pair address {pairAddress} \n\r" +
                        $"BlockTimestamp {reserves.BlockTimeStampLast}\n\r" +
                       $"token0 {token0} and reserve0 {reserves.Reserve0}\n\r" +
                       $"token1 {token1} and reserve1 {reserves.Reserve1}");
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }

        [HttpGet("UniswapV2PairApprove")]
        public async Task<ActionResult> GetUniswapV2PairApprove(Chain chain, string addressToken0, string addressToken1, string spenderAddress, long value)
        {
            try
            {
                Account? account = new Account(_user.PrivateKey, chain);
                Web3? web3 = new Web3(account, EnumHelper.GetStringBasedOnEnum(chain));

                Contract? smartContract = web3.Eth.GetContract(_uniswapv2FactoryAbi, _uniswapV2FactoryAddress);
                Function? getPair = smartContract.GetFunction("getPair");

                object[] parametersForPair = new object[2] { addressToken0, addressToken1 };
                string pairAddress = await getPair.CallAsync<string>(parametersForPair);

                if (pairAddress == "0x0000000000000000000000000000000000000000")
                {
                    return NotFound();
                }

                BigInteger valueToApprove = Web3.Convert.ToWei(value);

                Contract? smartContractPair = web3.Eth.GetContract(_pairERC20Abi, pairAddress);
                Function? approve = smartContractPair.GetFunction("approve");

                object[] parametersForApprove = new object[2] { spenderAddress, valueToApprove };
                HexBigInteger? estimatedGas = await approve.EstimateGasAsync(account.Address, null, null, parametersForApprove);
                TransactionReceipt? transactionReceiptForApprove = await approve.SendTransactionAndWaitForReceiptAsync(account.Address, estimatedGas, null, null, parametersForApprove);

                return Ok($"Transaction Hash for approve {transactionReceiptForApprove?.TransactionHash}");

            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }

        [HttpPost("UniswapV2RouterGetAmountsOut")]
        public async Task<ActionResult> UniswapV2RouterGetAmountsOut(Chain chain, long amountIn, List<string> path)
        {
            try
            {
                Account? account = new Account(_user.PrivateKey, chain);
                Web3? web3 = new Web3(account, EnumHelper.GetStringBasedOnEnum(chain));

                Contract? smartContract = web3.Eth.GetContract(_uniswapV2RouterAbi, _uniswapV2RouterAddress);
                Function? getAmountsOut = smartContract.GetFunction("getAmountsOut");

                object[] parametersForPair = new object[2] { amountIn, path };
                List<long> amount = await getAmountsOut.CallAsync<List<long>>(parametersForPair);

                return Ok($"For {amount[0]} of token {path.FirstOrDefault()} you receive {amount[1]} of {path.LastOrDefault()}");
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }

        [HttpPost("UniswapV2RouterSwapExactETHForTokens")]
        public async Task<ActionResult> SwapExactETHForTokens(Chain chain, double amountToSwap, long amountOutMin, [FromBody] List<string> path, double seconds)
        {
            try
            {
                Account? account = new Account(_user.PrivateKey, chain);
                Web3? web3 = new Web3(account, EnumHelper.GetStringBasedOnEnum(chain));

                string to = account.Address;
                BigInteger deadline = DateTimeOffset.Now.AddSeconds(seconds).ToUnixTimeSeconds();

                Contract? smartContractRouter = web3.Eth.GetContract(_uniswapV2RouterAbi, _uniswapV2RouterAddress);
                Function? swapExactETHForTokens = smartContractRouter.GetFunction("swapExactETHForTokens");

                object[] parametersForSwap = new object[4] { amountOutMin, path, to, deadline };

                BigInteger wei = Web3.Convert.ToWei(amountToSwap);
                HexBigInteger value = new HexBigInteger(wei);

                HexBigInteger? estimatedGas = await swapExactETHForTokens.EstimateGasAsync(account.Address, null, value, parametersForSwap);

                TransactionReceipt? transactionReceiptForSwap = await swapExactETHForTokens.SendTransactionAndWaitForReceiptAsync(account.Address, estimatedGas, value, null, parametersForSwap);

                return Ok($"Transaction Hash for swap {transactionReceiptForSwap.TransactionHash}");
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }

    }
}
