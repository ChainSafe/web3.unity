using System;
using System.Linq;
using System.Threading.Tasks;
using ChainSafe.Gaming.Evm.Contracts.Builders;
using ChainSafe.Gaming.Evm.Providers;
using ChainSafe.Gaming.Evm.Signers;
using ChainSafe.Gaming.Evm.Transactions;
using ChainSafe.Gaming.Web3;
using ChainSafe.Gaming.Web3.Analytics;
using ChainSafe.Gaming.Web3.Core;
using ChainSafe.Gaming.Web3.Core.Evm;
using Nethereum.ABI.Model;
using Nethereum.Hex.HexTypes;

namespace ChainSafe.Gaming.Evm.Contracts
{
    /// <summary>
    /// Representation of a contract.
    /// </summary>
    public class Contract : IContract
    {
        private readonly string abi;
        private readonly IRpcProvider provider;
        private readonly ISigner signer;
        private readonly ContractAbiManager contractAbiManager;
        private readonly ITransactionExecutor transactionExecutor;
        private readonly IAnalyticsClient analyticsClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="Contract"/> class.
        /// </summary>
        /// <param name="abi">The abi.</param>
        /// <param name="address">The contract address.</param>
        /// <param name="provider">The RPC provider.</param>
        /// <param name="signer">The signer.</param>
        /// <param name="transactionExecutor">Transaction executor.</param>
        internal Contract(string abi, string address, IRpcProvider provider, ISigner signer = null, ITransactionExecutor transactionExecutor = null, IAnalyticsClient analyticsClient = null)
        {
            if (string.IsNullOrEmpty(abi))
            {
                throw new ArgumentException("message", nameof(abi));
            }

            this.abi = abi;
            this.Address = address;
            this.provider = provider;
            this.signer = signer;
            this.transactionExecutor = transactionExecutor;
            this.analyticsClient = analyticsClient;
            contractAbiManager = new ContractAbiManager(abi, address);
        }

        public string Address { get; }

        /// <summary>
        /// Returns a new instance of the Contract attached to a new address. This is useful
        /// if there are multiple similar or identical copies of a Contract on the network,
        /// and you wish to interact with each of them.
        /// </summary>
        /// <param name="address">Address of the contract to attach to.</param>
        /// <returns>The new contract.</returns>
        public IContract Attach(string address)
        {
            if (string.IsNullOrEmpty(address))
            {
                throw new Exception("contract address is not set");
            }

            return new Contract(abi, address, provider, signer, transactionExecutor);
        }

        /// <summary>
        /// Call the contract method.
        /// </summary>
        /// <param name="method">The method to call.</param>
        /// <param name="parameters">The parameters for the method.</param>
        /// <param name="overwrite">To overwrite a transaction request.</param>
        /// <returns>The result of calling the method.</returns>
        public async Task<object[]> Call(string method, object[] parameters = null, TransactionRequest overwrite = null)
        {
            AssetServiceIsAccessible(provider);

            parameters ??= Array.Empty<object>();

            var txReq = await PrepareTransactionRequest(method, parameters, true, overwrite);

            var result = await provider.Call(txReq);
            analyticsClient.CaptureEvent(new AnalyticsEvent()
            {
                EventName = method,
                PackageName = "io.chainsafe.web3.unity",
            });

            return Decode(method, result);
        }

        public async Task<T> Call<T>(string method, object[] parameters = null, TransactionRequest overwrite = null)
        {
            AssetServiceIsAccessible(provider);

            parameters ??= Array.Empty<object>();

            var txReq = await PrepareTransactionRequest(method, parameters, true, overwrite);

            var result = await provider.Call(txReq);
            analyticsClient.CaptureEvent(new AnalyticsEvent()
            {
                EventName = method,
                PackageName = "io.chainsafe.web3.unity",
            });

            return contractAbiManager.GetFunctionBuilder(method).DecodeTypeOutput<T>(result);
        }

        /// <summary>
        /// Decodes a result.
        /// </summary>
        /// <param name="method">The method.</param>
        /// <param name="output">The raw output data from an executed function call.</param>
        /// <returns>The decoded outputs of a provided method.</returns>
        public object[] Decode(string method, string output)
        {
            var function = contractAbiManager.GetFunctionBuilder(method);
            var decodedOutput = function.DecodeOutput(output);
            var array = new object[decodedOutput.Count];
            for (var i = 0; i < decodedOutput.Count; i++)
            {
                array[i] = decodedOutput[i].Result;
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
            return (await SendWithReceipt(method, parameters, overwrite)).response;
        }

        /// <summary>
        /// Sends the transaction.
        /// </summary>
        /// <param name="method">The method.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="overwrite">An existing TransactionRequest to use instead of making a new one.</param>
        /// <typeparam name="T">Type of object you want to use.</typeparam>
        /// <returns>The outputs of the method.</returns>
        public async Task<T> Send<T>(string method, object[] parameters = null, TransactionRequest overwrite = null)
        {
            var result = (await SendWithReceipt<T>(method, parameters, overwrite)).response;
            return result;
        }

        /// <summary>
        /// Sends the transaction.
        /// </summary>
        /// <param name="method">The method.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="overwrite">An existing TransactionRequest to use instead of making a new one.</param>
        /// <returns>The outputs of the method and the transaction receipt.</returns>
        public async Task<(object[] response, TransactionReceipt receipt)> SendWithReceipt(
            string method,
            object[] parameters = null,
            TransactionRequest overwrite = null)
        {
            AssetServiceIsAccessible(signer);

            parameters ??= Array.Empty<object>();

            var function = contractAbiManager.GetFunctionBuilder(method);

            var txReq = await PrepareTransactionRequest(method, parameters, false, overwrite);

            var tx = await transactionExecutor.SendTransaction(txReq);
            var receipt = await provider.WaitForTransactionReceipt(tx.Hash);

            var output = function.DecodeOutput(tx.Data);
            var outputValues = output.Select(x => x.Result).ToArray();

            analyticsClient.CaptureEvent(new AnalyticsEvent()
            {
                EventName = method,
                PackageName = "io.chainsafe.web3.unity",
            });

            return (outputValues, receipt);
        }

        /// <summary>
        /// Sends the transaction.
        /// </summary>
        /// <param name="method">The method.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="overwrite">An existing TransactionRequest to use instead of making a new one.</param>
        /// <typeparam name="T">Type of object you want to use.</typeparam>
        /// <returns>The outputs of the method and the transaction receipt.</returns>
        public async Task<(T response, TransactionReceipt receipt)> SendWithReceipt<T>(
            string method,
            object[] parameters = null,
            TransactionRequest overwrite = null)
        {
            AssetServiceIsAccessible(signer);

            parameters ??= Array.Empty<object>();

            var txReq = await PrepareTransactionRequest(method, parameters, false, overwrite);

            var tx = await transactionExecutor.SendTransaction(txReq);
            var receipt = await provider.WaitForTransactionReceipt(tx.Hash);

            analyticsClient.CaptureEvent(new AnalyticsEvent()
            {
                EventName = method,
                PackageName = "io.chainsafe.web3.unity",
            });

            if (tx.Data == null)
            {
                return (default, receipt);
            }

            var outputValues = contractAbiManager.GetFunctionBuilder(method).DecodeTypeOutput<T>(tx.Data);

            return (outputValues, receipt);
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
            AssetServiceIsAccessible(provider);

            return await provider.EstimateGas(await PrepareTransactionRequest(method, parameters, false, overwrite));
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
            var function = contractAbiManager.GetFunctionBuilder(method);

            analyticsClient.CaptureEvent(new AnalyticsEvent()
            {
                EventName = method,
                PackageName = "io.chainsafe.web3.unity",
            });

            return function.GetData(parameters);
        }

        public async Task<TransactionRequest> PrepareTransactionRequest(string method, object[] parameters, bool isReadCall = false, TransactionRequest overwrite = null)
        {
            parameters ??= Array.Empty<object>();

            var function = contractAbiManager.GetFunctionBuilder(method);
            var txReq = overwrite ?? new TransactionRequest();

            txReq.From ??= signer?.PublicAddress;
            txReq.To ??= Address;
            txReq.Data ??= function.GetData(parameters);
            if (isReadCall)
            {
                return txReq;
            }

            try
            {
                var feeData = await provider.GetFeeData();
                txReq.MaxFeePerGas = feeData.MaxFeePerGas.ToHexBigInteger();
                if (!feeData.MaxPriorityFeePerGas.IsZero)
                {
                    txReq.MaxPriorityFeePerGas = feeData.MaxFeePerGas.ToHexBigInteger();
                }

                txReq.GasLimit ??= await provider.EstimateGas(txReq);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return txReq;
        }

        private void AssetServiceIsAccessible<T>(T instance)
        {
            if (string.IsNullOrEmpty(Address))
            {
                throw new Web3Exception("Contract address is not set.");
            }

            if (instance == null)
            {
                throw new ServiceNotBoundWeb3Exception<T>($"{typeof(T).Name} service is not bound.");
            }
        }
    }
}