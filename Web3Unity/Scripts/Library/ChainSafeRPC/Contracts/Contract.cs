using System;
using System.Threading.Tasks;
using GameData;
using Nethereum.Hex.HexTypes;
using Newtonsoft.Json;
using UnityEngine;
using Web3Unity.Scripts.Library.ETHEREUEM.WebGL;
using Web3Unity.Scripts.Library.Ethers.Contracts.Builders;
using Web3Unity.Scripts.Library.Ethers.Providers;
using Web3Unity.Scripts.Library.Ethers.Signers;
using Web3Unity.Scripts.Library.Ethers.Transactions;

namespace Web3Unity.Scripts.Library.Ethers.Contracts
{
    public class Contract
    {
        private readonly string _abi;
        private readonly string _address;
        private readonly IProvider _provider;
        private readonly ISigner _signer;
        private readonly ContractBuilder _contractBuilder;

        public Contract(string abi, string address = "", IProvider provider = null)
        {
            if (string.IsNullOrEmpty(abi))
            {
                throw new ArgumentException("message", nameof(abi));
            }

            _abi = abi;
            _address = address;
            _provider = provider;
            _contractBuilder = new ContractBuilder(abi, address);
        }

        public Contract(string abi, string address, ISigner signer) : this(abi, address, signer.Provider)
        {
            _signer = signer;
        }
        /// <summary>
        /// Returns a new instance of the Contract, but connected to providerOrSigner. By passing in a Provider, this will return a downgraded Contract which only has read-only access (i.e. constant calls). By passing in a Signer. this will return a Contract which will act on behalf of that signer.
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public Contract Connect(IProvider provider)
        {
            if (provider == null)
            {
                throw new Exception("provider is not set");
            }

            return new Contract(_abi, _address, provider);
        }
        /// <summary>
        /// Returns a new instance of the Contract, but connected to providerOrSigner. By passing in a Provider, this will return a downgraded Contract which only has read-only access (i.e. constant calls). By passing in a Signer. this will return a Contract which will act on behalf of that signer.
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public Contract Connect(ISigner signer)
        {
            if (signer == null)
            {
                throw new Exception("signer is not set");
            }

            return new Contract(_abi, _address, signer);
        }
        /// <summary>
        /// Returns a new instance of the Contract attached to a new address. This is useful if there are multiple similar or identical copies of a Contract on the network and you wish to interact with each of them.
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public Contract Attach(string address)
        {
            if (string.IsNullOrEmpty(address))
            {
                throw new Exception("contract address is not set");
            }

            return new Contract(_abi, address, _provider);
        }
        /// <summary>
        ///  Call Contract
        /// </summary>
        /// <param name="method"></param>
        /// <param name="parameters"></param>
        /// <param name="overwrite"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<object[]> Call(string method, object[] parameters = null, TransactionRequest overwrite = null)
        {
            if (string.IsNullOrEmpty(_address))
            {
                throw new Exception("contract address is not set");
            }

            if (_provider == null)
            {
                throw new Exception("provider or signer is not set");
            }

            parameters ??= Array.Empty<object>();

            var function = _contractBuilder.GetFunctionBuilder(method);
            var txReq = overwrite ?? new TransactionRequest();
            txReq.To = _address;
            txReq.Data = function.GetData(parameters);

            var result = await _provider.Call(txReq);
#if UNITY_EDITOR_WIN || UNITY_EDITOR_OSX || UNITY_IOS || UNITY_ANDROID
            var data = new
            {
                Client = "Desktop/Mobile",
                Version = "v2",
                ProjectID = PlayerPrefs.GetString("ProjectID"),
                Method = method,
                To = _address,
                TransactionResult = result,
                Data = JsonConvert.SerializeObject(parameters),
                Player = Web3Wallet.Web3Wallet.Sha3(PlayerPrefs.GetString("Account") + PlayerPrefs.GetString("ProjectID"))
            };
            Logging.SendGameData(data);
#endif

#if UNITY_WEBGL
            var dataWebGL = new
            {
                Client = "WebGL",
                Version = "v2",
                ProjectID = PlayerPrefs.GetString("ProjectID"),
                Method = method,
                To = _address,
                TransactionResult = result,
                Data = JsonConvert.SerializeObject(parameters),
                Player = Web3Wallet.Web3Wallet.Sha3(PlayerPrefs.GetString("Account") + PlayerPrefs.GetString("ProjectID"))
            };
            await GameLogger.Log("", "", dataWebGL);
#endif
            var output = function.DecodeOutput(result);
            var array = new object[output.Count];
            for (var i = 0; i < output.Count; i++)
            {
                array[i] = output[i].Result;
            }

            return array;
        }
        /// <summary>
        /// Sends Transaction
        /// </summary>
        /// <param name="method"></param>
        /// <param name="parameters"></param>
        /// <param name="overwrite"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<object[]> Send(string method, object[] parameters = null, TransactionRequest overwrite = null)
        {
            if (string.IsNullOrEmpty(_address))
            {
                throw new Exception("contract address is not set");
            }

            if (_signer == null)
            {
                throw new Exception("signer is not set");
            }

            parameters ??= Array.Empty<object>();

            var function = _contractBuilder.GetFunctionBuilder(method);
            var txReq = overwrite ?? new TransactionRequest();
            txReq.From = await _signer.GetAddress();
            txReq.To = _address;
            txReq.Data = function.GetData(parameters);

            var tx = await _signer.SendTransaction(txReq);
            var receipt = await tx.Wait();

            var output = function.DecodeOutput(tx.Data);
            var array = new object[output.Count];
            for (var i = 0; i < output.Count; i++)
            {
                array[i] = output[i].Result;
            }

            return array;
        }
        /// <summary>
        /// Estimate Gas
        /// </summary>
        /// <param name="method"></param>
        /// <param name="parameters"></param>
        /// <param name="overwrite"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<HexBigInteger> EstimateGas(string method, object[] parameters,
            TransactionRequest overwrite = null)
        {
            if (string.IsNullOrEmpty(_address))
            {
                throw new Exception("contract address is not set");
            }
            if (_provider == null)
            {
                throw new Exception("provider or signer is not set");
            }

            var function = _contractBuilder.GetFunctionBuilder(method);
            var txReq = overwrite ?? new TransactionRequest();

            if (_signer != null)
            {
                txReq.From = txReq.From = await _signer.GetAddress();
            }

            txReq.To = _address;
            txReq.Data = function.GetData(parameters);

            return await _provider.EstimateGas(txReq);
        }
        /// <summary>
        /// Create Contract Data
        /// </summary>
        /// <param name="method"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public string Calldata(string method, object[] parameters = null)
        {
            parameters ??= Array.Empty<object>();
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
            var function = _contractBuilder.GetFunctionBuilder(method);
            return function.GetData(parameters);
        }
    }
}