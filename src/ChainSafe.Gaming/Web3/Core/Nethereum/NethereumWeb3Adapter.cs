using System.Numerics;
using System.Threading.Tasks;
using Nethereum.BlockchainProcessing.Services;
using Nethereum.Contracts.Services;
using Nethereum.JsonRpc.Client;
using Nethereum.RPC;
using Nethereum.RPC.DebugNode;
using Nethereum.RPC.TransactionManagers;
using Nethereum.RPC.TransactionReceipts;
using Nethereum.Web3.Accounts;

namespace ChainSafe.Gaming.Web3.Core.Nethereum
{
    public class NethereumWeb3Adapter : INethereumWeb3Adapter, ILifecycleParticipant
    {
        private readonly ExternalAccount externalAccount;
        private readonly IClient nethClient;

        private global::Nethereum.Web3.Web3 original;

        public NethereumWeb3Adapter(IClient nethClient, IChainConfig chainConfig, NethereumSignerAdapter signerAdapter)
        {
            this.nethClient = nethClient;
            externalAccount = new ExternalAccount(signerAdapter, BigInteger.Parse(chainConfig.ChainId));
        }

        public IClient Client => original.Client;

        public IEthApiContractService Eth => original.Eth;

        public IBlockchainProcessingService Processing => original.Processing;

        public INetApiService Net => original.Net;

        public IPersonalApiService Personal => original.Personal;

        public IShhApiService Shh => original.Shh;

        public IDebugApiService Debug => original.Debug;

        public FeeSuggestionService FeeSuggestion => original.FeeSuggestion;

        public ITransactionManager TransactionManager
        {
            get => original.TransactionManager;
            set => original.TransactionManager = value;
        }

        public ITransactionReceiptService TransactionReceiptPolling
        {
            get => original.TransactionReceiptPolling;
            set => original.TransactionReceiptPolling = value;
        }

        public async ValueTask WillStartAsync()
        {
            await externalAccount.InitialiseAsync();
            externalAccount.InitialiseDefaultTransactionManager(nethClient); // todo: possibly implement a wrapper to use as a custom transaction manager
            original = new global::Nethereum.Web3.Web3(externalAccount, nethClient);
        }

        public ValueTask WillStopAsync() => new(Task.CompletedTask);
    }
}