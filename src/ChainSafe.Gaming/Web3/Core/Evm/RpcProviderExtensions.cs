using System;
using System.Globalization;
using System.Numerics;
using System.Threading.Tasks;
using ChainSafe.Gaming.Evm.Transactions;
using ChainSafe.Gaming.Web3;
using ChainSafe.Gaming.Web3.Core;
using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.DTOs;
using Block = ChainSafe.Gaming.Evm.Blocks.Block;
using BlockWithTransactions = ChainSafe.Gaming.Evm.Blocks.BlockWithTransactions;
using Transaction = ChainSafe.Gaming.Evm.Transactions.Transaction;
using TransactionReceipt = ChainSafe.Gaming.Evm.Transactions.TransactionReceipt;

namespace ChainSafe.Gaming.Evm.Providers
{
    public static class RpcProviderExtensions
    {
        public static async Task<string> GetChainId(this IRpcProvider provider)
        {
            var rawHexChainId = await provider.Perform<string>("eth_chainId");
            var chainId = new HexBigInteger(rawHexChainId).ToUlong();

            return chainId.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// <b>eth_getBalance</b><br/>Asynchronously retrieves the native balance (ETH for Ethereum) of a specified wallet address.
        /// </summary>
        /// <param name="provider">The RPC provider.</param>
        /// <param name="address">The wallet address to query.</param>
        /// <param name="blockTag">The block parameter (optional).</param>
        /// <returns>
        /// A <see cref="Task"/> representing the asynchronous operation.
        /// The task result contains the native balance as a <see cref="HexBigInteger"/>.
        /// </returns>
        public static async Task<HexBigInteger> GetBalance(this IRpcProvider provider, string address, BlockParameter blockTag = null)
        {
            blockTag ??= new BlockParameter();

            var parameters = new object[] { address, blockTag };

            return new HexBigInteger(await provider.Perform<string>("eth_getBalance", parameters));
        }

        /// <summary>
        /// <b>eth_getCode</b><br/>Asynchronously retrieves the byte code of a smart contract at a specified address.
        /// </summary>
        /// <param name="provider">The RPC provider.</param>
        /// <param name="address">The smart contract address to query.</param>
        /// <param name="blockTag">The block parameter (optional).</param>
        /// <returns>
        /// A <see cref="Task"/> representing the asynchronous operation.
        /// The task result contains the byte code as a hexadecimal string.
        /// </returns>
        public static async Task<string> GetCode(this IRpcProvider provider, string address, BlockParameter blockTag = null)
        {
            blockTag ??= new BlockParameter();

            var parameters = new object[] { address, blockTag };

            return await provider.Perform<string>("eth_getCode", parameters);
        }

        /// <summary>
        /// <b>eth_getStorageAt</b><br/>Asynchronously retrieves the storage data at a specified address and position.
        /// </summary>
        /// <param name="provider">The RPC provider.</param>
        /// <param name="address">The address to query.</param>
        /// <param name="position">The storage position to query.</param>
        /// <param name="blockTag">The block parameter (optional).</param>
        /// <returns>
        /// A <see cref="Task"/> representing the asynchronous operation.
        /// The task result contains the storage data as a string.
        /// </returns>
        public static async Task<string> GetStorageAt(this IRpcProvider provider, string address, BigInteger position, BlockParameter blockTag = null)
        {
            blockTag ??= new BlockParameter();

            var parameters = new object[] { address, position.ToHex(BitConverter.IsLittleEndian), blockTag };

            return await provider.Perform<string>("eth_getStorageAt", parameters);
        }

        /// <summary>
        /// <b>eth_getTransactionCount</b><br/>Asynchronously retrieves the transaction count for a specified address.
        /// </summary>
        /// <param name="provider">The RPC provider.</param>
        /// <param name="address">The address to query.</param>
        /// <param name="blockTag">The block parameter (optional).</param>
        /// <returns>
        /// A <see cref="Task"/> representing the asynchronous operation.
        /// The task result contains the transaction count as a <see cref="HexBigInteger"/>.
        /// </returns>
        public static async Task<HexBigInteger> GetTransactionCount(this IRpcProvider provider, string address, BlockParameter blockTag = null)
        {
            blockTag ??= new BlockParameter();

            var parameters = new object[] { address, blockTag };

            return new HexBigInteger(await provider.Perform<string>("eth_getTransactionCount", parameters));
        }

        /// <summary>
        /// <b>eth_getBlockByNumber</b><br/>Asynchronously retrieves a blockchain block.
        /// </summary>
        /// <param name="provider">The RPC provider.</param>
        /// <param name="blockTag">The block parameter (optional).</param>
        /// <returns>
        /// A <see cref="Task"/> representing the asynchronous operation.
        /// The task result contains the blockchain block as a <see cref="Block"/>.
        /// </returns>
        public static async Task<Block> GetBlock(this IRpcProvider provider, BlockParameter blockTag = null)
        {
            blockTag ??= new BlockParameter();

            var parameters = new object[] { blockTag.GetRPCParam(), false };

            return await provider.Perform<Block>("eth_getBlockByNumber", parameters);
        }

        /// <summary>
        /// <b>eth_getBlockByHash</b><br/>Asynchronously retrieves a blockchain block using its hash.
        /// </summary>
        /// <param name="provider">The RPC provider.</param>
        /// <param name="blockHash">The hash of the block to retrieve.</param>
        /// <returns>
        /// A <see cref="Task"/> representing the asynchronous operation.
        /// The task result contains the blockchain block as a <see cref="Block"/>.
        /// </returns>
        public static async Task<Block> GetBlock(this IRpcProvider provider, string blockHash)
        {
            if (!blockHash.HasHexPrefix() || blockHash.Length != 66)
            {
                throw new Web3Exception("wrong block hash format");
            }

            var parameters = new object[] { blockHash, false };

            return await provider.Perform<Block>("eth_getBlockByHash", parameters);
        }

        /// <summary>
        /// <b>eth_getBlockByNumber</b><br/>Asynchronously retrieves a blockchain block with its transactions.
        /// </summary>
        /// <param name="provider">The RPC provider.</param>
        /// <param name="blockTag">The block parameter (optional).</param>
        /// <returns>
        /// A <see cref="Task"/> representing the asynchronous operation.
        /// The task result contains the blockchain block with transactions as a <see cref="BlockWithTransactions"/>.
        /// </returns>
        public static async Task<BlockWithTransactions> GetBlockWithTransactions(this IRpcProvider provider, BlockParameter blockTag = null)
        {
            blockTag ??= new BlockParameter();

            var parameters = new object[] { blockTag.GetRPCParam(), true };

            return await provider.Perform<BlockWithTransactions>("eth_getBlockByNumber", parameters);
        }

        /// <summary>
        /// <b>eth_getBlockByHash</b><br/>Asynchronously retrieves a blockchain block with its transactions using the block's hash.
        /// </summary>
        /// <param name="provider">The RPC provider.</param>
        /// <param name="blockHash">The hash of the block to retrieve.</param>
        /// <returns>
        /// A <see cref="Task"/> representing the asynchronous operation.
        /// The task result contains the blockchain block with transactions as a <see cref="Blocks.BlockWithTransactions"/>.
        /// </returns>
        public static async Task<BlockWithTransactions> GetBlockWithTransactions(this IRpcProvider provider, string blockHash)
        {
            if (!blockHash.HasHexPrefix() || blockHash.Length != 66)
            {
                throw new Web3Exception("wrong block hash format");
            }

            var parameters = new object[] { blockHash, true };

            return await provider.Perform<BlockWithTransactions>("eth_getBlockByHash", parameters);
        }

        /// <summary>
        /// <b>eth_blockNumber</b><br/>Asynchronously retrieves the latest block number on the blockchain.
        /// </summary>
        /// <param name="provider">The RPC provider.</param>
        /// <returns>
        /// A <see cref="Task"/> representing the asynchronous operation.
        /// The task result contains the latest block number as a <see cref="HexBigInteger"/>.
        /// </returns>
        public static async Task<HexBigInteger> GetBlockNumber(this IRpcProvider provider)
        {
            return new HexBigInteger(await provider.Perform<string>("eth_blockNumber", null));
        }

        /// <summary>
        /// <b>eth_gasPrice</b><br/>Asynchronously retrieves the current gas price on the blockchain.
        /// </summary>
        /// <param name="provider">The RPC provider.</param>
        /// <returns>
        /// A <see cref="Task"/> representing the asynchronous operation.
        /// The task result contains the current gas price as a <see cref="HexBigInteger"/>.
        /// </returns>
        public static async Task<HexBigInteger> GetGasPrice(this IRpcProvider provider)
        {
            return new HexBigInteger(await provider.Perform<string>("eth_gasPrice", null));
        }

        /// <summary>
        /// Asynchronously retrieves fee-related data for transactions.
        /// </summary>
        /// <param name="provider">The RPC provider.</param>
        /// <returns>
        /// A <see cref="Task"/> representing the asynchronous operation.
        /// The task result contains fee data as a <see cref="FeeData"/> object.
        /// </returns>
        public static async Task<FeeData> GetFeeData(this IRpcProvider provider)
        {
            var block = await provider.GetBlock();
            var gasPrice = await provider.GetGasPrice();

            var maxPriorityFeePerGas = BigInteger.Zero;
            var maxFeePerGas = BigInteger.Zero;
            await TryFetchMaxFees();

            return new FeeData
            {
                BaseFeePerGas = block.BaseFeePerGas,
                GasPrice = gasPrice,
                MaxFeePerGas = maxFeePerGas,
                MaxPriorityFeePerGas = maxPriorityFeePerGas,
            };

            async Task TryFetchMaxFees()
            {
                if (block.BaseFeePerGas <= BigInteger.Zero)
                {
                    return;
                }

                HexBigInteger tip;
                try
                {
                    // Post London Fork
                    tip = new HexBigInteger(await provider.Perform<string>("eth_maxPriorityFeePerGas"));
                }
                catch (Web3Exception)
                {
                    // eth_maxPriorityFeePerGas not supported by the RPC node, skipping..
                    maxFeePerGas = block.BaseFeePerGas.Value + 1;
                    return;
                }

                var max = tip.Value + block.BaseFeePerGas.Value - 1;
                maxPriorityFeePerGas = tip;
                maxFeePerGas = max;
            }
        }

        /// <summary>
        /// <b>eth_call</b><br/>Asynchronously invokes a call to a smart contract or blockchain function.
        /// </summary>
        /// <param name="provider">The RPC provider.</param>
        /// <param name="transaction">The transaction request to execute.</param>
        /// <param name="blockTag">The block parameter (optional).</param>
        /// <returns>
        /// A <see cref="Task"/> representing the asynchronous operation.
        /// The task result contains the result of the call as a string.
        /// </returns>
        public static async Task<string> Call(this IRpcProvider provider, TransactionRequest transaction, BlockParameter blockTag = null)
        {
            blockTag ??= new BlockParameter();

            var parameters = new object[] { transaction, blockTag.GetRPCParam() };

            return await provider.Perform<string>("eth_call", parameters);
        }

        /// <summary>
        /// <b>eth_estimateGas</b><br/>Asynchronously estimates the gas required for a transaction.
        /// </summary>
        /// <param name="provider">The RPC provider.</param>
        /// <param name="transaction">The transaction request to estimate gas for.</param>
        /// <returns>
        /// A <see cref="Task"/> representing the asynchronous operation.
        /// The task result contains the estimated gas amount as a <see cref="HexBigInteger"/>.
        /// </returns>
        public static async Task<HexBigInteger> EstimateGas(this IRpcProvider provider, TransactionRequest transaction)
        {
            var parameters = new object[] { transaction };

            return new HexBigInteger(await provider.Perform<string>("eth_estimateGas", parameters));
        }

        /// <summary>
        /// <b>eth_getTransactionByHash</b><br/>Asynchronously retrieves a transaction by its hash.
        /// </summary>
        /// <param name="provider">The RPC provider.</param>
        /// <param name="transactionHash">The hash of the transaction to retrieve.</param>
        /// <param name="timeOut">(Optional) The time after which the method will fail if we can't find the transaction by hash. 15 seconds by default.</param>
        /// <returns>
        /// A <see cref="Task"/> representing the asynchronous operation.
        /// The task result contains the transaction details as a <see cref="TransactionResponse"/>.
        /// </returns>
        public static async Task<TransactionResponse> GetTransaction(this IRpcProvider provider, string transactionHash, TimeSpan? timeOut = null)
        {
            timeOut ??= TimeSpan.FromSeconds(60);

            // Poll transaction till it's available on the given node
            TransactionResponse transaction = null;
            var parameters = new object[] { transactionHash };
            var pollStartTime = DateTime.UtcNow;
            while (DateTime.UtcNow - pollStartTime < timeOut)
            {
                transaction = await provider.Perform<TransactionResponse>("eth_getTransactionByHash", parameters);

                if (transaction != null)
                {
                    break;
                }

                await DelayUtil.SafeDelay(TimeSpan.FromSeconds(1));
            }

            if (transaction == null)
            {
                throw new Web3Exception("Transaction not found.");
            }

            if (transaction.BlockNumber == null)
            {
                transaction.Confirmations = 0;
            }
            else if (transaction.Confirmations == null)
            {
                var blockNumber = await provider.GetBlockNumber();
                var confirmations = (blockNumber.ToUlong() - transaction.BlockNumber.ToUlong()) + 1;
                if (confirmations <= 0)
                {
                    confirmations = 1;
                }

                transaction.Confirmations = confirmations;
            }

            return transaction;
        }

        /// <summary>
        /// <b>eth_getTransactionReceipt</b><br/>Asynchronously retrieves a transaction receipt by its hash.
        /// </summary>
        /// <param name="provider">The RPC provider.</param>
        /// <param name="transactionHash">The hash of the transaction receipt to retrieve.</param>
        /// <returns>
        /// A <see cref="Task"/> representing the asynchronous operation.
        /// The task result contains the transaction receipt as a <see cref="TransactionReceipt"/>.
        /// </returns>
        public static async Task<TransactionReceipt> GetTransactionReceipt(this IRpcProvider provider, string transactionHash)
        {
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

        /// <summary>
        /// Wraps a transaction with its hash to create a <see cref="TransactionResponse"/>.
        /// </summary>
        /// <param name="provider">The RPC provider.</param>
        /// <param name="tx">The original transaction.</param>
        /// <param name="hash">The hash of the transaction.</param>
        /// <returns>
        /// A <see cref="TransactionResponse"/> object with the transaction details.
        /// </returns>
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

        /// <summary>
        /// Asynchronously waits for a transaction receipt with specified confirmations and optional timeout.
        /// </summary>
        /// <param name="provider">The RPC provider.</param>
        /// <param name="transactionHash">The hash of the transaction receipt to wait for.</param>
        /// <param name="confirmations">The number of confirmations to wait for (optional).</param>
        /// <param name="timeout">The maximum number of attempts before timing out (optional).</param>
        /// <returns>
        /// A <see cref="Task"/> representing the asynchronous operation.
        /// The task result contains the transaction receipt as a <see cref="TransactionReceipt"/>.
        /// </returns>
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

        /// <summary>
        /// <b>eth_getLogs</b><br/>Asynchronously retrieves logs based on the provided filter criteria.
        /// </summary>
        /// <param name="provider">The RPC provider.</param>
        /// <param name="filter">The filter input criteria for log retrieval.</param>
        /// <returns>
        /// A <see cref="Task"/> representing the asynchronous operation.
        /// The task result contains an array of <see cref="FilterLog"/> objects representing the logs.
        /// </returns>
        public static async Task<FilterLog[]> GetLogs(this IRpcProvider provider, NewFilterInput filter)
        {
            var parameters = new object[] { filter };

            return await provider.Perform<FilterLog[]>("eth_getLogs", parameters);
        }

        /// <summary>
        /// <b>eth_accounts</b><br/>Asynchronously lists all accounts associated with the RPC node.
        /// </summary>
        /// <param name="provider">The RPC provider.</param>
        /// <returns>
        /// A <see cref="Task"/> representing the asynchronous operation.
        /// The task result contains an array of account addresses as strings.
        /// </returns>
        public static async Task<string[]> ListAccounts(this IRpcProvider provider) =>
            await provider.Perform<string[]>("eth_accounts");
    }
}
