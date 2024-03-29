﻿using Nethereum.RPC.Eth.DTOs;
using Nethereum.Signer;
using System.Numerics;

namespace Application.Interfaces
{
    public interface INetworkService
    {
        Task<BlockWithTransactionHashes> GetBlockAsync(BigInteger blockNumber, Chain chain);
        Task<Transaction[]> GetTransactionsOfABlock(BigInteger blockNumber, Chain chain);
        Task<IEnumerable<Transaction?>> GetAllContractCreationTransactionsAsync(BigInteger blockNumber, Chain chai);
    }
}
