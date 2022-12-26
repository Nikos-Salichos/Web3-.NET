using Domain.DTOs;
using Domain.Models;
using Nethereum.RPC.Eth.DTOs;

namespace Application.Interfaces
{
    public interface ISmartContractService
    {
        Task<IEnumerable<SmartContractDTO>> GetSmartContractsAsync();
        Task<TransactionReceipt> DeploySmartContractAsync(SmartContractDTO smartContract);
        Task<dynamic> ReadContractFunctionVariableAsync(string variableName, SmartContract smartContractJson);
        Task<dynamic> WriteContractFunctionVariableAsync(string variableName, long sendAsEth, SmartContract smartContractJson);
        Task<dynamic> TrackEventAsync(string eventName, SmartContract smartContractJson);
    }
}
