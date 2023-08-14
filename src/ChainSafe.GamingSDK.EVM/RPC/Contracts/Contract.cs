using System;
using System.Linq;
using System.Threading.Tasks;
using ChainSafe.GamingSDK.EVM.Web3.Core.Evm;
using Nethereum.Hex.HexTypes;
using Web3Unity.Scripts.Library.Ethers.Contracts.Builders;
using Web3Unity.Scripts.Library.Ethers.Providers;
using Web3Unity.Scripts.Library.Ethers.Signers;
using Web3Unity.Scripts.Library.Ethers.Transactions;

namespace Web3Unity.Scripts.Library.Ethers.Contracts
{
    public class Contract
    {
        private readonly string abi;
        private readonly string address;
        private readonly IRpcProvider provider;
        private readonly ISigner signer;
        private readonly Builders.ContractBuilder contractBuilder;
        private readonly ITransactionExecutor transactionExecutor;

        internal Contract(string abi, string address, IRpcProvider provider = null, ISigner signer = null, ITransactionExecutor transactionExecutor = null)
        {
            if (string.IsNullOrEmpty(abi))
            {
                throw new ArgumentException("message", nameof(abi));
            }

            this.abi = abi;
            this.address = address;
            this.provider = provider;
            this.signer = signer;
            this.transactionExecutor = transactionExecutor;
            contractBuilder = new Builders.ContractBuilder(abi, address);
        }

        /// <summary>
        /// Returns a new instance of the Contract attached to a new address. This is useful
        /// if there are multiple similar or identical copies of a Contract on the network
        /// and you wish to interact with each of them.
        /// </summary>
        /// <param name="address">Address of the contract to attach to.</param>
        /// <returns>The new contract.</returns>
        public Contract Attach(string address)
        {
            if (string.IsNullOrEmpty(address))
            {
                throw new Exception("contract address is not set");
            }

            return new Contract(abi, address, provider, signer, transactionExecutor);
        }

        public async Task<object[]> Call(string method, object[] parameters = null, TransactionRequest overwrite = null)
        {
            if (string.IsNullOrEmpty(address))
            {
                throw new Exception("contract address is not set");
            }

            if (provider == null)
            {
                throw new Exception("provider or signer is not set");
            }

            parameters ??= Array.Empty<object>();

            var function = contractBuilder.GetFunctionBuilder(method);
            var txReq = overwrite ?? new TransactionRequest();
            txReq.To ??= address;
            txReq.Data ??= function.GetData(parameters);

            var result = await provider.Call(txReq);

            var output = function.DecodeOutput(result);
            var array = new object[output.Count];
            for (var i = 0; i < output.Count; i++)
            {
                array[i] = output[i].Result;
            }

            return array;
        }

        /// <summary>
        /// Sends the transaction.
        /// </summary>
        /// <param name="method">The method.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="overwrite">An existing TransactionRequest to use instead of making a new one.</param>
        /// <returns>The outputs of the method.</returns>
        public async Task<object[]> Send(string method, object[] parameters = null, TransactionRequest overwrite = null)
        {
            if (string.IsNullOrEmpty(address))
            {
                throw new Exception("contract address is not set");
            }

            if (signer == null)
            {
                throw new Exception("signer is not set");
            }

            parameters ??= Array.Empty<object>();

            var function = contractBuilder.GetFunctionBuilder(method);
            var txReq = overwrite ?? new TransactionRequest();

            txReq.From ??= await signer.GetAddress();
            txReq.To ??= address;
            txReq.Data ??= function.GetData(parameters);

            if (txReq.GasPrice == null && txReq.MaxFeePerGas == null)
            {
                var feeData = await provider.GetFeeData();
                txReq.MaxFeePerGas = new HexBigInteger(feeData.MaxFeePerGas);
            }

            txReq.GasLimit ??= await provider.EstimateGas(txReq);

            var tx = await transactionExecutor.SendTransaction(txReq);
            var receipt = await provider.WaitForTransactionReceipt(tx.Hash);

            var output = function.DecodeOutput(tx.Data);
            return output.Select(x => x.Result).ToArray();
        }

        /// <summary>
        /// Estimate gas.
        /// </summary>
        /// <param name="method">The method.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="overwrite">An existing TransactionRequest to use instead of making a new one.</param>
        /// <returns>The estimated amount of gas.</returns>
        public async Task<HexBigInteger> EstimateGas(
            string method,
            object[] parameters,
            TransactionRequest overwrite = null)
        {
            if (string.IsNullOrEmpty(address))
            {
                throw new Exception("contract address is not set");
            }

            if (provider == null)
            {
                throw new Exception("provider or signer is not set");
            }

            var function = contractBuilder.GetFunctionBuilder(method);
            var txReq = overwrite ?? new TransactionRequest();

            if (signer != null)
            {
                txReq.From ??= await signer.GetAddress();
            }

            txReq.To ??= address;
            txReq.Data ??= function.GetData(parameters);

            if (txReq.GasPrice == null && txReq.MaxFeePerGas == null)
            {
                var feeData = await provider.GetFeeData();
                txReq.MaxFeePerGas = new HexBigInteger(feeData.MaxFeePerGas);
            }

            return await provider.EstimateGas(txReq);
        }

        /// <summary>
        /// Create contract call data.
        /// </summary>
        /// <param name="method">The method.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>The contract call data.</returns>
        public string Calldata(string method, object[] parameters = null)
        {
            parameters ??= Array.Empty<object>();

            // TODO: since this code isn't built in Unity, these blocks will always be left out.
#if UNITY_EDITOR_WIN || UNITY_EDITOR_OSX || UNITY_IOS || UNITY_ANDROID
            var data = new
            {
                Client = "Desktop/Mobile",
                Version = "v2",
                ProjectID = PlayerPrefs.GetString("ProjectID"),
                Player = Web3Wallet.Web3Wallet.Sha3(PlayerPrefs.GetString("Account") + PlayerPrefs.GetString("ProjectID")),
                Method = method,
                Params = parameters
            };
            Logging.SendGameData(data);
#endif
#if UNITY_WEBGL
            var dataWebGL = new
            {
                Client = "WebGL",
                Version = "v2",
                ProjectID = PlayerPrefs.GetString("ProjectID"),
                Player = Web3Wallet.Web3Wallet.Sha3(PlayerPrefs.GetString("Account") + PlayerPrefs.GetString("ProjectID")),
                Method = method,
                Params = parameters
            };
            var dataObject = GameLogger.Log("", "", dataWebGL);
#endif
            var function = contractBuilder.GetFunctionBuilder(method);
            return function.GetData(parameters);
        }
    }
}