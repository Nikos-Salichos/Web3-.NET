using Domain.DTOs;
using Domain.Models;
using Nethereum.RPC.Eth.DTOs;
using System.Linq.Expressions;

namespace Application.Interfaces
{
    public interface ISmartContractService
    {
        Task<IEnumerable<SmartContractDTO>> GetSmartContractsAsync();
        Task<SmartContractDTO> GetSmartContractByIdAsync(long id);
        Task<IEnumerable<SmartContract>> FindSmartContract(Expression<Func<SmartContract, bool>> predicate);
        Task<TransactionReceipt> DeploySmartContractAsync(SmartContractDTO smartContract);
        Task<dynamic> ReadContractFunctionVariableAsync(string variableName, SmartContract smartContractJson);
        Task<dynamic> WriteContractFunctionVariableAsync(string variableName, long sendAsEth, SmartContract smartContractJson);
        Task<dynamic> TrackEventAsync(string eventName, SmartContract smartContractJson);
    }
}
