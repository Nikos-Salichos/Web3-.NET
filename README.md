# Smart Contracts in Solidity - C#
Connect smart contracts with .NET through web APIs.

Installation
======
1. Download project and restore Nugets
2. Configure appsettings.json with your own details.
3. Launch project.

Technical knowledge:
- Generic Repository.


Entity Framework:

Add-Migration "DB Initialize"

Update-Database

In order to compile a smart contract I am using visual studio code with solidity extension. 
After compilation I use ABI and bytecode from .json file. 

I have create different controllers for different smart contracts:

- SmartContractController
  - DeployAnyContract (deploy contract with or without parameters)
  - CallContractVariable (call any variable in contract)
  - CallReadFunction (call read functions in contract)
  - CallWriteFunction (call write functions in contract)
  - TrackCryptoWhalesForAnyToken (track "Transfer" event in any token of your choice)

- NetworkController
  - GetLatestBlock
  - GetAllTransactionsOfCurrentBlock
  - GetAllTransactionOfABlock
  - GetAllContractCreationTransactions

- Wallet 
  - Check balance
  - Send ether/token to another wallet

- UniswapV2
  - AllPairs
  - GetReserves
  - GetAmountsOut
  - Approve
  - SwapExactETHForTokens

- UniswapV3
  - GetReserves (getPool, tokens and balanceOf)
  - SwapExactTokensForTokens using Router02
  - GetTokenData (using GraphQL)
  - GetMostLiquidPools (using GraphQL)
  - GetPoolData (using GraphQL)
  GetRecentSwapsWithinAPool using GraphQL)  

- Lottery smart contract
  - Deploy
  - GetRandomNumber
  - GetPlayers
  - GetBalance
  - EnterLottery
  - PickWinner

- CryptoToken
  - Deploy ERC20 Token 
  - GetBalance
  - Mint
  - Approve
  - Allowance
  - DestroySmartContract
  - Transfer
  - IncreaseApproval
  - DecreaseApproval
  - Burn
  - TransferFrom
  - BurnFrom

- NFTController
  - Deploy smart contract 
  - Mint NFT

- CryptoCompare

In addition i use the following tools to convert ABI to string:
https://elmah.io/tools/multiline-string-converter/
and from multiline string to single line:
https://tools.techcybo.com/multiline-to-single-line

In order to test UniswapV3 get prices you can use the following addresses:
WETH : 0xA0b86991c6218b36c1d19D4a2e9Eb0cE3606eB48
DAI: 0x6B175474E89094C44Da98b954EedeAC495271d0F

