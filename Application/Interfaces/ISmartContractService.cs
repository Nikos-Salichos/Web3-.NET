using Domain.Models;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;

namespace Application.Interfaces
{
    public interface ISmartContractService
    {
        Task<IEnumerable<SmartContract>> GetSmartContractsAsync();

        Task<TransactionReceipt> DeploySmartContractAsync(Account account, SmartContract smartContract, Web3 web3);
    }
}
