using Nethereum.BlockchainProcessing.Services;
using Nethereum.Contracts.Services;
using Nethereum.JsonRpc.Client;
using Nethereum.RPC;
using Nethereum.RPC.DebugNode;
using Nethereum.RPC.TransactionManagers;
using Nethereum.RPC.TransactionReceipts;

namespace ChainSafe.Gaming.Web3.Core.Nethereum
{
    public class NethereumWeb3Adapter : INethereumWeb3Adapter
    {
        private readonly global::Nethereum.Web3.Web3 original;

        // build Read-Only adapter
        public NethereumWeb3Adapter(IClient nethClient)
        {
            original = new global::Nethereum.Web3.Web3(nethClient);
        }

        // build Writing adapter
        public NethereumWeb3Adapter(IClient nethClient, INethereumAccountAdapter accountAdapter)
        {
            original = new global::Nethereum.Web3.Web3(accountAdapter, nethClient);
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
    }
}