﻿using Application.CQRS.Commands;
using Application.CQRS.Queries;
using Application.Helpers;
using Application.Interfaces;
using Application.Validators;
using AutoMapper;
using Domain.DTOs;
using Domain.Models;
using MediatR;
using Nethereum.Contracts;
using Nethereum.Contracts.Standards.ERC20.ContractDefinition;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using Newtonsoft.Json;
using System.Linq.Expressions;
using System.Numerics;

namespace Application.Services
{
    public class SmartContractService : ISmartContractService
    {
        public EnumHelper EnumHelper { get; set; }

        private readonly IMapper _mapper;

        private readonly IMediator _mediator;

        private readonly ISingletonOptionsService _singletonOptionsService;

        public SmartContractService(IMapper mapper, IMediator mediator, ISingletonOptionsService singletonOptionsService)
        {
            _singletonOptionsService = singletonOptionsService;
            EnumHelper = new EnumHelper(_singletonOptionsService);
            _mapper = mapper;
            _mediator = mediator;
            _singletonOptionsService = singletonOptionsService;
        }

        public async Task<IEnumerable<SmartContractDTO>> GetSmartContractsAsync(int pageSize, int pageNumber)
        {
            var allSmartContracts = await _mediator.Send(new GetSmartContractsListQuery(pageSize, pageNumber), default);
            return _mapper.Map<List<SmartContract>, List<SmartContractDTO>>(allSmartContracts.ToList());
        }

        public async Task<SmartContractDTO> GetSmartContractAsync(long id)
        {
            var smartContract = await _mediator.Send(new GetSmartContractQuery(id), default);
            return _mapper.Map<SmartContract, SmartContractDTO>(smartContract);
        }

        public async Task<IEnumerable<SmartContractDTO>> GetSmartContractsAsync(Expression<Func<SmartContract, bool>> predicate)
        {
            var allSmartContracts = await _mediator.Send(new FindSmartContractQuery(predicate), default);
            return _mapper.Map<List<SmartContract>, List<SmartContractDTO>>(allSmartContracts.ToList());
        }

        public async Task<TransactionReceipt> DeploySmartContractAsync(SmartContractDTO smartContractDto)
        {
            SmartContractDtoValidator smartContractValidator = new SmartContractDtoValidator();
            var validationResult = await smartContractValidator.ValidateAsync(smartContractDto);

            if (!validationResult.IsValid)
            {
                throw new ArgumentException(validationResult.ToString());
            }

            Account? account = new(_singletonOptionsService.GetUserSettings().PrivateKey, smartContractDto.Chain);
            Web3? web3 = new(account, EnumHelper.GetStringBasedOnEnum(smartContractDto.Chain));

            object[]? parameters = null;
            if (smartContractDto.Parameters?.Count > 0
                && !string.IsNullOrWhiteSpace(smartContractDto.Parameters?.FirstOrDefault()?.ToString()))
            {
                parameters = smartContractDto.Parameters.ToArray();
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
                await _mediator.Send(new CreateSmartContractCommand(smartContract), default);
            }
            return deployContract;
        }

        public async Task<dynamic> ReadContractFunctionVariableAsync(string variableName, SmartContract smartContractJson)
        {
            Account? account = new Account(_singletonOptionsService.GetUserSettings().PrivateKey, smartContractJson.Chain);
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
            Account? account = new Account(_singletonOptionsService.GetUserSettings().PrivateKey, smartContractJson.Chain);
            Web3? web3 = new Web3(account, EnumHelper.GetStringBasedOnEnum(smartContractJson.Chain));

            Contract? smartContract = web3.Eth.GetContract(smartContractJson?.Abi?.ToString(), smartContractJson?.Address);
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

        public async Task<dynamic> TrackEventAsync(string eventName, SmartContract smartContractJson)
        {
            Account? account = new Account(_singletonOptionsService.GetUserSettings().PrivateKey, smartContractJson.Chain);
            Web3? web3 = new Web3(account, EnumHelper.GetStringBasedOnEnum(smartContractJson.Chain));

            Contract? smartContract = web3.Eth.GetContract(smartContractJson?.Abi?.ToString(), smartContractJson?.Address);
            Event transferEvent = smartContract.GetEvent(eventName);
            BlockParameter? _lastBlock = BlockParameter.CreateLatest();
            NewFilterInput? filterInput = transferEvent.CreateFilterInput(_lastBlock, _lastBlock);
            List<EventLog<TransferEventDTO>>? eventLogs = await transferEvent.GetAllChangesAsync<TransferEventDTO>(filterInput);
            return JsonConvert.SerializeObject(eventLogs);
        }
    }
}
