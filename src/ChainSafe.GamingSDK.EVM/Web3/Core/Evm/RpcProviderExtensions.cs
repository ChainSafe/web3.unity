﻿using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;
using ChainSafe.GamingWeb3;
using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.DTOs;
using Web3Unity.Scripts.Library.Ethers.Transactions;

using Block = Web3Unity.Scripts.Library.Ethers.Blocks.Block;
using BlockWithTransactions = Web3Unity.Scripts.Library.Ethers.Blocks.BlockWithTransactions;
using Transaction = Web3Unity.Scripts.Library.Ethers.Transactions.Transaction;
using TransactionReceipt = Web3Unity.Scripts.Library.Ethers.Transactions.TransactionReceipt;

namespace Web3Unity.Scripts.Library.Ethers.Providers
{
    public static class RpcProviderExtensions
    {
        public static async Task<HexBigInteger> GetBalance(this IRpcProvider provider, string address, BlockParameter blockTag = null)
        {
            await provider.GetNetwork();

            blockTag ??= new BlockParameter();

            var parameters = new object[] { address, blockTag };

            return new HexBigInteger(await provider.Perform<string>("eth_getBalance", parameters));
        }

        public static async Task<string> GetCode(this IRpcProvider provider, string address, BlockParameter blockTag = null)
        {
            await provider.GetNetwork();

            blockTag ??= new BlockParameter();

            var parameters = new object[] { address, blockTag };

            return await provider.Perform<string>("eth_getCode", parameters);
        }

        public static async Task<string> GetStorageAt(this IRpcProvider provider, string address, BigInteger position, BlockParameter blockTag = null)
        {
            await provider.GetNetwork();

            blockTag ??= new BlockParameter();

            var parameters = new object[] { address, position.ToHex(BitConverter.IsLittleEndian), blockTag };

            return await provider.Perform<string>("eth_getStorageAt", parameters);
        }

        public static async Task<HexBigInteger> GetTransactionCount(this IRpcProvider provider, string address, BlockParameter blockTag = null)
        {
            await provider.GetNetwork();

            blockTag ??= new BlockParameter();

            var parameters = new object[] { address, blockTag };

            return new HexBigInteger(await provider.Perform<string>("eth_getTransactionCount", parameters));
        }

        public static async Task<Block> GetBlock(this IRpcProvider provider, BlockParameter blockTag = null)
        {
            await provider.GetNetwork();

            blockTag ??= new BlockParameter();

            var parameters = new object[] { blockTag.GetRPCParam(), false };

            return await provider.Perform<Block>("eth_getBlockByNumber", parameters);
        }

        public static async Task<Block> GetBlock(this IRpcProvider provider, string blockHash)
        {
            await provider.GetNetwork();

            if (!blockHash.HasHexPrefix() || blockHash.Length != 66)
            {
                throw new Web3Exception("wrong block hash format");
            }

            var parameters = new object[] { blockHash, false };

            return await provider.Perform<Block>("eth_getBlockByHash", parameters);
        }

        public static async Task<BlockWithTransactions> GetBlockWithTransactions(this IRpcProvider provider, BlockParameter blockTag = null)
        {
            await provider.GetNetwork();

            blockTag ??= new BlockParameter();

            var parameters = new object[] { blockTag.GetRPCParam(), true };

            return await provider.Perform<BlockWithTransactions>("eth_getBlockByNumber", parameters);
        }

        public static async Task<BlockWithTransactions> GetBlockWithTransactions(this IRpcProvider provider, string blockHash)
        {
            await provider.GetNetwork();

            if (!blockHash.HasHexPrefix() || blockHash.Length != 66)
            {
                throw new Web3Exception("wrong block hash format");
            }

            var parameters = new object[] { blockHash, true };

            return await provider.Perform<BlockWithTransactions>("eth_getBlockByHash", parameters);
        }

        public static async Task<HexBigInteger> GetBlockNumber(this IRpcProvider provider)
        {
            await provider.GetNetwork();

            return new HexBigInteger(await provider.Perform<string>("eth_blockNumber", null));
        }

        public static async Task<HexBigInteger> GetGasPrice(this IRpcProvider provider)
        {
            await provider.GetNetwork();

            return new HexBigInteger(await provider.Perform<string>("eth_gasPrice", null));
        }

        public static async Task<FeeData> GetFeeData(this IRpcProvider provider)
        {
            var block = await provider.GetBlock();
            var gasPrice = await provider.GetGasPrice();

            var maxPriorityFeePerGas = BigInteger.Zero;
            var maxFeePerGas = BigInteger.Zero;

            if (block.BaseFeePerGas > BigInteger.Zero)
            {
                maxPriorityFeePerGas = new BigInteger(1500000000);
                maxFeePerGas = (block.BaseFeePerGas * new BigInteger(2)) + maxPriorityFeePerGas;
            }

            return new FeeData
            {
                GasPrice = gasPrice,
                MaxFeePerGas = maxFeePerGas,
                MaxPriorityFeePerGas = maxPriorityFeePerGas,
            };
        }

        public static async Task<string> Call(this IRpcProvider provider, TransactionRequest transaction, BlockParameter blockTag = null)
        {
            await provider.GetNetwork();

            blockTag ??= new BlockParameter();

            var parameters = new object[] { transaction, blockTag.GetRPCParam() };

            return await provider.Perform<string>("eth_call", parameters);
        }

        public static async Task<HexBigInteger> EstimateGas(this IRpcProvider provider, TransactionRequest transaction)
        {
            await provider.GetNetwork();

            var parameters = new object[] { transaction };

            return new HexBigInteger(await provider.Perform<string>("eth_estimateGas", parameters));
        }

        public static async Task<TransactionResponse> GetTransaction(this IRpcProvider provider, string transactionHash)
        {
            await provider.GetNetwork();

            var parameters = new object[] { transactionHash };

            var result = await provider.Perform<TransactionResponse>("eth_getTransactionByHash", parameters);

            if (result == null)
            {
                throw new Web3Exception("transaction not found");
            }

            if (result.BlockNumber == null)
            {
                result.Confirmations = 0;
            }
            else if (result.Confirmations == null)
            {
                var blockNumber = await provider.GetBlockNumber();
                var confirmations = (blockNumber.ToUlong() - result.BlockNumber.ToUlong()) + 1;
                if (confirmations <= 0)
                {
                    confirmations = 1;
                }

                result.Confirmations = confirmations;
            }

            return result;
        }

        public static async Task<TransactionReceipt> GetTransactionReceipt(this IRpcProvider provider, string transactionHash)
        {
            await provider.GetNetwork();

            var parameters = new object[] { transactionHash };

            var result = await provider.Perform<TransactionReceipt>("eth_getTransactionReceipt", parameters);

            if (result == null)
            {
                // throw new Exception("transaction receipt not found");
                return result;
            }

            if (result.BlockNumber == null)
            {
                result.Confirmations = 0;
            }
            else if (result.Confirmations == null)
            {
                var blockNumber = await provider.GetBlockNumber();
                var confirmations = (blockNumber.ToUlong() - result.BlockNumber.ToUlong()) + 1;
                if (confirmations <= 0)
                {
                    confirmations = 1;
                }

                result.Confirmations = confirmations;
            }

            return result;
        }

        public static TransactionResponse WrapTransaction(this IRpcProvider provider, Transaction tx, string hash)
        {
            if (hash != null && hash.Length != 66)
            {
                throw new Web3Exception("invalid response - SendTransaction");
            }

            var result = (TransactionResponse)tx;
            result.Hash = hash;

            return result;
        }

        public static async Task<TransactionReceipt> WaitForTransactionReceipt(
            this IRpcProvider provider,
            string transactionHash,
            uint confirmations = 1,
            uint timeout = 0)
        {
            return await provider.WaitForTransactionInternal(transactionHash, confirmations, timeout);
        }

        private static async Task<TransactionReceipt> WaitForTransactionInternal(
            this IRpcProvider provider,
            string transactionHash,
            uint confirmations = 1,
            uint timeout = 0)
        {
            var receipt = await provider.GetTransactionReceipt(transactionHash);
            if ((receipt != null ? receipt.Confirmations : 0) >= confirmations)
            {
                return receipt;
            }

            var noTimeout = timeout == 0;

            while (true)
            {
                // TODO: implement exponential backoff?
                await Task.Delay(1000);

                receipt = await provider.GetTransactionReceipt(transactionHash);
                if (receipt != null && receipt.Confirmations >= confirmations)
                {
                    return receipt;
                }

                // if timeout disabled
                if (noTimeout)
                {
                    continue;
                }

                timeout--;

                if (timeout == 0)
                {
                    throw new Web3Exception("timeout waiting for transaction");
                }
            }
        }

        public static async Task<FilterLog[]> GetLogs(this IRpcProvider provider, NewFilterInput filter)
        {
            await provider.GetNetwork();

            var parameters = new object[] { filter };

            return await provider.Perform<FilterLog[]>("eth_getLogs", parameters);
        }
    }
}
