using Application.CQRS.Queries;
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
using Newtonsoft.Json;
using System.Numerics;

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

        public async Task<IEnumerable<SmartContractDTO>> Handle(GetSmartContractsListQuery request, CancellationToken cancellationToken)
        {
            var allSmartContracts = await _unitOfWork.SmartContractRepository.GetSmartContracts();
            return _mapper.Map<List<SmartContract>, List<SmartContractDTO>>(allSmartContracts.ToList());
        }

        public async Task<TransactionReceipt> DeploySmartContractAsync(SmartContractDTO smartContractDto)
        {
            SmartContractDtoValidator smartContractValidator = new SmartContractDtoValidator();
            var IsValid = await smartContractValidator.ValidateAsync(smartContractDto);

            if (!IsValid.IsValid)
            {
                throw new ArgumentNullException(IsValid.ToString());
            }

            Account? account = new Account(_user.PrivateKey, smartContractDto.Chain);
            Web3? web3 = new Web3(account, EnumHelper.GetStringBasedOnEnum(smartContractDto.Chain));

            object[]? parameters = null;
            if (smartContractDto?.Parameters?.Count > 0)
            {
                parameters = smartContractDto.Parameters.ToArray();
                if (string.IsNullOrWhiteSpace(parameters?.FirstOrDefault()?.ToString()))
                {
                    parameters = null;
                }
            }

            HexBigInteger estimatedGas = await web3.Eth.DeployContract.EstimateGasAsync(smartContractDto?.Abi?.ToString(),
                                                                                      smartContractDto?.Bytecode,
                                                                                      account.Address,
                                                                                      parameters);

            TransactionReceipt? deployContract = await web3.Eth.DeployContract.SendRequestAndWaitForReceiptAsync(smartContractDto?.Abi?.ToString(),
                                                                                                                smartContractDto?.Bytecode,
                                                                                                                account.Address,
                                                                                                                estimatedGas,
                                                                                                                null, null, null, parameters);
            if (smartContractDto != null && deployContract.Succeeded())
            {
                smartContractDto.Address = deployContract.ContractAddress;
                var smartContract = _mapper.Map<SmartContractDTO, SmartContract>(smartContractDto);

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

        public async Task<dynamic> WriteContractFunctionVariableAsync(string functionName, long sendAsEth, SmartContract smartContractJson)
        {
            Account? account = new Account(_user.PrivateKey, smartContractJson.Chain);
            Web3? web3 = new Web3(account, EnumHelper.GetStringBasedOnEnum(smartContractJson.Chain));

            Contract? smartContract = web3.Eth.GetContract(smartContractJson.Abi.ToString(), smartContractJson.Address);
            Function? writeFunction = smartContract.GetFunction(functionName);
            object[]? parameters = null;

            if (smartContractJson?.Parameters?.Count > 0)
            {
                parameters = smartContractJson.Parameters.ToArray();
                if (string.IsNullOrWhiteSpace(parameters?.FirstOrDefault()?.ToString()))
                {
                    parameters = null;
                }
            }

            HexBigInteger? value = null;
            BigInteger wei = Web3.Convert.ToWei(sendAsEth);
            if (wei != 0)
            {
                value = new HexBigInteger(wei);
            }

            HexBigInteger? estimatedGas = await writeFunction.EstimateGasAsync(account.Address, null, value, parameters);
            TransactionReceipt? functionResult = await writeFunction.SendTransactionAndWaitForReceiptAsync(account.Address, estimatedGas, value, null, parameters);
            return JsonConvert.SerializeObject(functionResult);
        }

    }
}
