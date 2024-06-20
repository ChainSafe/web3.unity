using System.Threading.Tasks;
using ChainSafe.Gaming.Evm;
using ChainSafe.Gaming.LocalStorage;
using ChainSafe.Gaming.Web3;
using ChainSafe.Gaming.Web3.Environment;
using UnityEngine;

namespace ChainSafe.Gaming.HyperPlay
{
    public class HyperPlayWebGLProvider : HyperPlayProvider
    {
        private readonly IHyperPlayConfig _config;
        private readonly IHyperPlayData _data;
        private readonly DataStorage _dataStorage;
        private readonly HyperPlayController _hyperPlayController;
        
        public HyperPlayWebGLProvider(IHyperPlayConfig config, IHyperPlayData data, DataStorage dataStorage, IHttpClient httpClient, IChainConfig chainConfig, ChainRegistryProvider chainRegistryProvider) : base(config, data, dataStorage, httpClient, chainConfig, chainRegistryProvider)
        {
            _config = config;
            _data = data;
            _dataStorage = dataStorage;
            
            // Initialize Unity controller.
            _hyperPlayController = Object.FindObjectOfType<HyperPlayController>();

            if (_hyperPlayController == null)
            {
                GameObject controllerObj = new GameObject(nameof(HyperPlayController), typeof(HyperPlayController));

                _hyperPlayController = controllerObj.GetComponent<HyperPlayController>();
            }

            Object.DontDestroyOnLoad(_hyperPlayController.gameObject);
        }

        public override async Task<string> Connect()
        {
            string[] accounts = await Perform<string[]>("eth_requestAccounts");

            string account = accounts[0];
            
            // Saved account exists.
            if (_data.RememberSession)
            {
                return account;
            }

            if (_config.RememberSession)
            {
                _data.RememberSession = true;

                _data.SavedAccount = account;

                await _dataStorage.Save(_data);
            }

            return account;
        }

        public override async Task<T> Perform<T>(string method, params object[] parameters)
        {
            var response = await _hyperPlayController.Request(method, parameters);
            
            return response.Result.ToObject<T>();
        }
    }
}
