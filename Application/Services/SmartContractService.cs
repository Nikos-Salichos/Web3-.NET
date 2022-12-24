using Application.Interfaces;
using Application.Utilities;
using Application.Validators;
using AutoMapper;
using Domain.DTOs;
using Domain.Models;
using Infrastructure.Persistence.Interfaces;
using Microsoft.Extensions.Configuration;
using Nethereum.Contracts;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;

namespace Application.Services
{
    public class SmartContractService : ISmartContractService
    {
        private readonly User _user = new User();
        public EnumHelper EnumHelper { get; set; }

        private readonly IUnitOfWork _unitOfWork;

        private readonly IMapper _mapper;

        public SmartContractService(IConfiguration configuration, IUnitOfWork unitOfWork, IMapper mapper)
        {
            EnumHelper = new EnumHelper(configuration);
            _user = configuration.GetSection("User").Get<User>()!;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<SmartContractDTO>> GetSmartContractsAsync()
        {
            var allSmartContracts = await _unitOfWork.SmartContractRepository.GetSmartContracts();
            return _mapper.Map<List<SmartContract>, List<SmartContractDTO>>(allSmartContracts.ToList());
        }

        public async Task<TransactionReceipt> DeploySmartContractAsync(SmartContract smartContract)
        {
            SmartContractValidator smartContractValidator = new SmartContractValidator();
            var IsValid = await smartContractValidator.ValidateAsync(smartContract);

            if (!IsValid.IsValid)
            {
                throw new ArgumentNullException(IsValid.ToString());
            }

            Account? account = new Account(_user.PrivateKey, smartContract.Chain);
            Web3? web3 = new Web3(account, EnumHelper.GetStringBasedOnEnum(smartContract.Chain));

            object[]? parameters = null;
            if (smartContract?.Parameters?.Count > 0)
            {
                parameters = smartContract.Parameters.ToArray();
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
                                                                                                                account.Address,
                                                                                                                estimatedGas,
                                                                                                                null, null, null, parameters);
            if (smartContract != null && deployContract.Succeeded())
            {
                smartContract.Address = deployContract.ContractAddress;

                await _unitOfWork.SmartContractRepository.Add(smartContract);
                await _unitOfWork.SaveChangesAsync();
            }
            return deployContract;
        }

        public async Task<dynamic> ReadContractFunctionVariableAsync(string variableName, SmartContract smartContractJson)
        {
            Account? account = new Account(_user.PrivateKey, smartContractJson.Chain);
            Web3? web3 = new Web3(account, EnumHelper.GetStringBasedOnEnum(smartContractJson.Chain));

            Contract? smartContractObject = web3.Eth.GetContract(smartContractJson?.Abi?.ToString(), smartContractJson?.Address);
            Function? variable = smartContractObject.GetFunction(variableName);

            dynamic variableValue = await variable.CallAsync<dynamic>();

            if (variableValue is null)
            {
                throw new ArgumentNullException(nameof(variableValue), "VariableName cannot be null");
            }

            return variableValue;
        }

    }
}
