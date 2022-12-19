using Domain.DTOs;
using Domain.Models;
using Nethereum.RPC.Eth.DTOs;

namespace Application.Interfaces
{
    public interface ISmartContractService
    {
        Task<IEnumerable<SmartContractDTO>> GetSmartContractsAsync();
        Task<TransactionReceipt> DeploySmartContractAsync(SmartContract smartContract);

        Task<dynamic> CallContractVariableAsync(string variableName, SmartContract smartContractJson);
    }
}
