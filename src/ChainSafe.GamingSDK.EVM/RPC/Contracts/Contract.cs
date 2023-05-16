using System;
using System.Linq;
using System.Threading.Tasks;
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
        private readonly IEvmProvider provider;
        private readonly IEvmSigner signer;
        private readonly ContractBuilder contractBuilder;

        public Contract(string abi, string address = "", IEvmProvider provider = null)
        {
            if (string.IsNullOrEmpty(abi))
            {
                throw new ArgumentException("message", nameof(abi));
            }

            this.abi = abi;
            this.address = address;
            this.provider = provider;
            contractBuilder = new ContractBuilder(abi, address);
        }

        public Contract(string abi, string address, IEvmSigner signer)
            : this(abi, address, signer.Provider)
        {
            this.signer = signer;
        }

        /// <summary>
        /// Returns a new instance of the Contract, but connected to provider. This
        /// will return a downgraded Contract which only has read-only access
        /// (i.e. constant calls).
        /// </summary>
        /// <param name="provider">The provider.</param>
        /// <returns>The new contract.</returns>
        public Contract Connect(IEvmProvider provider)
        {
            if (provider == null)
            {
                throw new Exception("provider is not set");
            }

            return new Contract(abi, address, provider);
        }

        /// <summary>
        /// Returns a new instance of the Contract, but connected to signer.
        /// This will return a Contract which will act on behalf of the signer.
        /// </summary>
        /// <param name="signer">The signer.</param>
        /// <returns>The new contract.</returns>
        public Contract Connect(IEvmSigner signer)
        {
            if (signer == null)
            {
                throw new Exception("signer is not set");
            }

            return new Contract(abi, address, signer);
        }

        /// <summary>
        /// Returns a new instance of the Contract attached to a new address. This is useful if there are multiple similar or identical copies of a Contract on the network and you wish to interact with each of them.
        /// </summary>
        /// <param name="address">Address of the contract to attach to.</param>
        /// <returns>The new contract.</returns>
        public Contract Attach(string address)
        {
            if (string.IsNullOrEmpty(address))
            {
                throw new Exception("contract address is not set");
            }

            return new Contract(abi, address, provider);
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
            txReq.To = address;
            txReq.Data = function.GetData(parameters);

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
            txReq.From = await signer.GetAddress();
            txReq.To = address;
            txReq.Data = function.GetData(parameters);

            var tx = await signer.SendTransaction(txReq);
            var receipt = await tx.Wait();

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
                txReq.From = txReq.From = await signer.GetAddress();
            }

            txReq.To = address;
            txReq.Data = function.GetData(parameters);

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