
using Nethereum.ABI.EIP712;
using Nethereum.Signer;
using System.Threading.Tasks;
using ChainSafe.Gaming.Evm.Providers;
using ChainSafe.Gaming.Evm.Signers;
using ChainSafe.Gaming.Evm.Transactions;
using ChainSafe.Gaming.InProcessSigner;
using ChainSafe.Gaming.Web3;
using ChainSafe.Gaming.Web3.Core;
using ChainSafe.Gaming.Web3.Core.Evm;
using ChainSafe.Gaming.Web3.Evm.JsonRpc;
using UnityEngine;

using Object = UnityEngine.Object;
using TWeb3Auth = Web3Auth;

namespace ChainSafe.GamingSdk.Web3Auth
{
    public class Web3AuthWallet : ISigner, ILifecycleParticipant
    {
        private InProcessSigner signer;
        private JsonRpcTransactionExecutor transactionExecutor;
        private TWeb3Auth coreInstance;

        private readonly Web3AuthWalletConfig config;
        private readonly IChainConfig chainConfig;
        private readonly IRpcProvider rpcProvider;

        public Web3AuthWallet(Web3AuthWalletConfig config, IChainConfig chainConfig, IRpcProvider rpcProvider)
        {
            this.config = config;
            this.chainConfig = chainConfig;
            this.rpcProvider = rpcProvider;
        }

        public async ValueTask WillStartAsync()
        {
            coreInstance = CreateCoreInstance();
            TaskCompletionSource<string> loginTcs = new();
            coreInstance.onLogin += Web3Auth_OnLogin;

            if (config.LoginParams != null)
            {
                coreInstance.login(config.LoginParams);
            }

            var privateKeyString = await loginTcs.Task;

            var privateKey = new EthECKey(privateKeyString);
            var signerConfig = new InProcessSignerConfig { PrivateKey = privateKey };
            signer = new(signerConfig);

            transactionExecutor = new(signer, rpcProvider);

            void Web3Auth_OnLogin(Web3AuthResponse response) => loginTcs.SetResult(response.privKey);
        }

        public async ValueTask WillStopAsync()
        {
            TaskCompletionSource<object> logoutTcs = new();
            coreInstance.onLogout += Web3Auth_OnLogout;
            coreInstance.logout();

            await logoutTcs.Task;

            coreInstance.onLogout -= Web3Auth_OnLogout;
            Object.Destroy(coreInstance);

            void Web3Auth_OnLogout() => logoutTcs.SetResult(null);
        }

        public Task<string> GetAddress() => signer.GetAddress();

        public Task<string> SignMessage(string message) => signer.SignMessage(message);
        public Task<string> SignTransaction(TransactionRequest transaction) => signer.SignTransaction(transaction);

        public Task<string> SignTypedData<TStructData>(SerializableDomain domain, TStructData message) => signer.SignTypedData(domain, message);

        public Task<TransactionResponse> SendTransaction(TransactionRequest transaction) => transactionExecutor.SendTransaction(transaction);

        private TWeb3Auth CreateCoreInstance()
        {
            var gameObject = new GameObject("Web3Auth", typeof(TWeb3Auth));
            gameObject.hideFlags = HideFlags.HideInHierarchy;
            Object.DontDestroyOnLoad(gameObject);

            var instance = gameObject.GetComponent<TWeb3Auth>();
            instance.Initialize();
            instance.setOptions(config.Web3AuthOptions);

            return instance;
        }
    }
}
