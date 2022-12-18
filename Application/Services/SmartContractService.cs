using Application.Interfaces;
using Domain.Models;
using Infrastructure.Persistence.Interfaces;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;

namespace Application.Services
{
    public class SmartContractService : ISmartContractService
    {
        private readonly ISmartContractRepository _smartContractRepository;

        public SmartContractService(ISmartContractRepository smartContractRepository)
        {
            _smartContractRepository = smartContractRepository;
        }

        public async Task<TransactionReceipt> DeploySmartContractAsync(Account account, SmartContract smartContract, Web3 web3)
        {
            object[]? parameters = null;
            if (smartContract?.Parameters != null)
            {
                parameters = (object[]?)smartContract.Parameters;
                if (string.IsNullOrWhiteSpace(parameters?.FirstOrDefault()?.ToString()))
                {
                    parameters = null;
                }
            }

            HexBigInteger estimatedGas = await web3.Eth.DeployContract.EstimateGasAsync(smartContract?.Abi?.ToString(),
                                                                                      smartContract?.Bytecode,
                                                                                      account.Address,
                                                                                      parameters);

            TransactionReceipt? deployContract = await web3.Eth.DeployContract.SendRequestAndWaitForReceiptAsync(smartContract?.Abi?.ToString(),
                                                                                                                smartContract?.Bytecode,
                                                                                                                account.PrivateKey,
                                                                                                                estimatedGas,
                                                                                                                null, null, null, parameters);

            return deployContract;
        }


        public Task<IEnumerable<SmartContract>> GetSmartContractsAsync()
        {
            return _smartContractRepository.GetSmartContracts();
        }
    }
}
