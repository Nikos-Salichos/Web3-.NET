# .NET - Solidity
Connect smart contracts with .NET

In order to compile a smart contract i am using visual studio code with solidity extension. 
When compilation finish i use ABI and bin (bytecode) in C#. 
In addition i use the following tools to convert ABI to string:

https://elmah.io/tools/multiline-string-converter/

and from multiline string to single line:

https://tools.techcybo.com/multiline-to-single-line

I will create different controllers for different cases:
- Network (current block, current block difficulty etc)
- Wallet
