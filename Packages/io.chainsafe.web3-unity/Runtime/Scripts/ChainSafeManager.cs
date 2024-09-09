using System;
using System.Linq;
using System.Threading.Tasks;
using ChainSafe.Gaming.UnityPackage.Connection;
using ChainSafe.Gaming.UnityPackage.UI;
using ChainSafe.Gaming.Web3;
using UnityEngine;
using CWeb3 = ChainSafe.Gaming.Web3.Web3;

namespace ChainSafe.Gaming.UnityPackage
{
    [RequireComponent(typeof(ConnectionHandler))]
    public class ChainSafeManager : MonoBehaviour
    {
        private static ChainSafeManager _instance;
        
        public static ChainSafeManager Instance
        {
            get
            {
                if (!_instance)
                {
                    _instance = FindObjectOfType<ChainSafeManager>();
                }
                
                return _instance;
            }

            private set => _instance = value;
        }

        public static CWeb3 Web3 => Instance._web3;
        
        public bool Connected { get; private set; }

        [SerializeField] private bool connectOnInitialize = true;
        
        [DefaultAssetValue("Packages/io.chainsafe.web3-unity/Runtime/Prefabs/Connect.prefab")]
        [SerializeField] private ConnectModal connectModalPrefab;

        private CWeb3 _web3;
        
        private ConnectionHandler _connectionHandler;
        
        private ConnectModal _connectModal;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        public async Task Initialize()
        {
            _connectionHandler = FindObjectOfType<ConnectionHandler>();
            
            await _connectionHandler.Initialize();

            try
            {
                if (connectOnInitialize)
                {
                    _web3 = await _connectionHandler.Restore();
                }
            }
            finally
            {
                if (!Connected)
                {
                    _connectModal = Instantiate(connectModalPrefab);
                    
                    _connectModal.Initialize(_connectionHandler.Providers);
                }
            }
        }

        public async Task Connect(ConnectionProvider provider)
        {
            _web3 = await (_connectionHandler as IConnectionHandler).Connect(provider);

            if (Web3 != null)
            {
                Connected = true;
            }
        }
        
        public async Task Connect<T>() where T : ConnectionProvider
        {
            if (!_connectionHandler.GetProvider(out T provider))
            {
                throw new Web3Exception($"{typeof(T).Name} unavailable or disabled.");
            }
            
            await Connect(provider);
        }

        public void ShowModal()
        {
            _connectModal.Show();
        }
        
        public async Task Disconnect(bool logout = false)
        {
            if (Web3 != null)
            {
                await Web3.TerminateAsync(logout);

                Connected = false;
                
                _web3 = null;
            }
        }

        private async void OnDestroy()
        {
            // Terminate Web3 instance
            await Disconnect();
        }
    }
}
