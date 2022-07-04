# C# - Solidity
Connect smart contracts with .NET

In order to compile a smart contract I am using visual studio code with solidity extension. 
When compilation finish I use ABI and bytecode from .json file. 

I have create different controllers for different smart contracts:

- Network (current block, current block difficulty etc)
- Wallet (check balance, send ether/token to another wallet)
- UniswapV2
  - AllPairs
  - GetReserves
  - GetAmountsOut
  - Approve
  - SwapExactETHForTokens
- UniswapV3
  - GetReserves (getPool, tokens and balanceOf)
  - SwapExactTokensForTokens in Router02
- Lottery smart contract
  - Deploy
  - GetRandomNumber
  - GetPlayers
  - GetBalance
  - EnterLottery
  - PickWinner

In addition i use the following tools to convert ABI to string:

https://elmah.io/tools/multiline-string-converter/

and from multiline string to single line:

https://tools.techcybo.com/multiline-to-single-line

In order to test UniswapV3 get prices you can use the following addresses:

WETH : 0xA0b86991c6218b36c1d19D4a2e9Eb0cE3606eB48

DAI: 0x6B175474E89094C44Da98b954EedeAC495271d0F
