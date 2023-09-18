using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using ChainSafe.Gaming.Evm.Transactions;
using ChainSafe.Gaming.Web3;
using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.DTOs;
using Org.BouncyCastle.Utilities.Encoders;
using Block = ChainSafe.Gaming.Evm.Blocks.Block;
using BlockWithTransactions = ChainSafe.Gaming.Evm.Blocks.BlockWithTransactions;
using Transaction = ChainSafe.Gaming.Evm.Transactions.Transaction;
using TransactionReceipt = ChainSafe.Gaming.Evm.Transactions.TransactionReceipt;

namespace ChainSafe.Gaming.Evm.Providers
{
    public static class RpcProviderExtensions
    {
        public static async Task<HexBigInteger> GetBalance(this IRpcProvider provider, string address, BlockParameter blockTag = null)
        {
            blockTag ??= new BlockParameter();

            var parameters = new object[] { address, blockTag };

            return new HexBigInteger(await provider.Perform<string>("eth_getBalance", parameters));
        }

        public static async Task<string> GetCode(this IRpcProvider provider, string address, BlockParameter blockTag = null)
        {
            blockTag ??= new BlockParameter();

            var parameters = new object[] { address, blockTag };

            return await provider.Perform<string>("eth_getCode", parameters);
        }

        public static async Task<string> GetStorageAt(this IRpcProvider provider, string address, BigInteger position, BlockParameter blockTag = null)
        {
            blockTag ??= new BlockParameter();

            var parameters = new object[] { address, position.ToHex(BitConverter.IsLittleEndian), blockTag };

            return await provider.Perform<string>("eth_getStorageAt", parameters);
        }

        public static async Task<HexBigInteger> GetTransactionCount(this IRpcProvider provider, string address, BlockParameter blockTag = null)
        {
            blockTag ??= new BlockParameter();

            var parameters = new object[] { address, blockTag };

            return new HexBigInteger(await provider.Perform<string>("eth_getTransactionCount", parameters));
        }

        public static async Task<Blocks.Block> GetBlock(this IRpcProvider provider, BlockParameter blockTag = null)
        {
            blockTag ??= new BlockParameter();

            var parameters = new object[] { blockTag.GetRPCParam(), false };

            return await provider.Perform<Blocks.Block>("eth_getBlockByNumber", parameters);
        }

        public static async Task<Blocks.Block> GetBlock(this IRpcProvider provider, string blockHash)
        {
            if (!blockHash.HasHexPrefix() || blockHash.Length != 66)
            {
                throw new Web3Exception("wrong block hash format");
            }

            var parameters = new object[] { blockHash, false };

            return await provider.Perform<Blocks.Block>("eth_getBlockByHash", parameters);
        }

        public static async Task<Blocks.BlockWithTransactions> GetBlockWithTransactions(this IRpcProvider provider, BlockParameter blockTag = null)
        {
            blockTag ??= new BlockParameter();

            var parameters = new object[] { blockTag.GetRPCParam(), true };

            return await provider.Perform<Blocks.BlockWithTransactions>("eth_getBlockByNumber", parameters);
        }

        public static async Task<Blocks.BlockWithTransactions> GetBlockWithTransactions(this IRpcProvider provider, string blockHash)
        {
            if (!blockHash.HasHexPrefix() || blockHash.Length != 66)
            {
                throw new Web3Exception("wrong block hash format");
            }

            var parameters = new object[] { blockHash, true };

            return await provider.Perform<Blocks.BlockWithTransactions>("eth_getBlockByHash", parameters);
        }

        public static async Task<HexBigInteger> GetBlockNumber(this IRpcProvider provider)
        {
            return new HexBigInteger(await provider.Perform<string>("eth_blockNumber", null));
        }

        public static async Task<HexBigInteger> GetGasPrice(this IRpcProvider provider)
        {
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
                // Post London Fork
                var tip = new HexBigInteger(await provider.Perform<string>("eth_maxPriorityFeePerGas"));
                var max = tip.Value + block.BaseFeePerGas.Value - 1;
                maxPriorityFeePerGas = tip;
                maxFeePerGas = max;
            }

            return new FeeData
            {
                BaseFeePerGas = block.BaseFeePerGas,
                GasPrice = gasPrice,
                MaxFeePerGas = maxFeePerGas,
                MaxPriorityFeePerGas = maxPriorityFeePerGas,
            };
        }

        public static async Task<TransactionRequest> ApplyGasFeeStrategy(this IRpcProvider provider, TransactionRequest tx)
        {
            if (tx.GasPrice == null && tx.MaxFeePerGas == null)
            {
                var feeData = await provider.GetFeeData();
                if (feeData.MaxFeePerGas == 0)
                {
                    tx.GasPrice = new HexBigInteger(feeData.GasPrice);
                }
                else
                {
                    tx.MaxPriorityFeePerGas = new HexBigInteger(feeData.MaxPriorityFeePerGas);
                    tx.MaxFeePerGas = new HexBigInteger(feeData.MaxFeePerGas);
                }
            }

            return tx;
        }

        public static async Task<string> Call(this IRpcProvider provider, TransactionRequest transaction, BlockParameter blockTag = null)
        {
            blockTag ??= new BlockParameter();

            var parameters = new object[] { transaction, blockTag.GetRPCParam() };

            return await provider.Perform<string>("eth_call", parameters);
        }

        public static async Task<HexBigInteger> EstimateGas(this IRpcProvider provider, TransactionRequest transaction)
        {
            var parameters = new object[] { transaction };

            return new HexBigInteger(await provider.Perform<string>("eth_estimateGas", parameters));
        }

        public static async Task<TransactionResponse> GetTransaction(this IRpcProvider provider, string transactionHash)
        {
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

        public static async Task<Transactions.TransactionReceipt> GetTransactionReceipt(this IRpcProvider provider, string transactionHash)
        {
            var parameters = new object[] { transactionHash };

            var result = await provider.Perform<Transactions.TransactionReceipt>("eth_getTransactionReceipt", parameters);

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

        public static TransactionResponse WrapTransaction(this IRpcProvider provider, Transactions.Transaction tx, string hash)
        {
            if (hash != null && hash.Length != 66)
            {
                throw new Web3Exception("invalid response - SendTransaction");
            }

            var result = (TransactionResponse)tx;
            result.Hash = hash;

            return result;
        }

        public static async Task<Transactions.TransactionReceipt> WaitForTransactionReceipt(
            this IRpcProvider provider,
            string transactionHash,
            uint confirmations = 1,
            uint timeout = 0)
        {
            return await provider.WaitForTransactionInternal(transactionHash, confirmations, timeout);
        }

        private static async Task<Transactions.TransactionReceipt> WaitForTransactionInternal(
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
            var parameters = new object[] { filter };

            return await provider.Perform<FilterLog[]>("eth_getLogs", parameters);
        }

        public static async Task<string[]> ListAccounts(this IRpcProvider provider) =>
            await provider.Perform<string[]>("eth_accounts");
    }
}
