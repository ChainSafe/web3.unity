using System.Threading.Tasks;
using ChainSafe.Gaming.Evm.Signers;
using ChainSafe.Gaming.Web3.Core;
using ChainSafe.Gaming.Web3.Core.Evm;

namespace ChainSafe.Gaming.Web3.Evm.Wallet
{
    public class WalletSigner : ISigner, ILifecycleParticipant
    {
        private readonly IWalletProvider walletProvider;
        private readonly IWalletConfig walletConfig;

        public WalletSigner(IWalletProvider walletProvider, IWalletConfig walletConfig)
        {
            this.walletProvider = walletProvider;
            this.walletConfig = walletConfig;
        }

        public string PublicAddress { get; private set; }

        public virtual async ValueTask WillStartAsync()
        {
            PublicAddress = await walletProvider.Connect();
        }

        public virtual Task<string> SignMessage(string message)
        {
            return walletProvider.Perform<string>(walletConfig.SignMessageRpcMethodName, message, PublicAddress);
        }

        public virtual Task<string> SignTypedData<TStructType>(SerializableDomain domain, TStructType message)
        {
            SerializableTypedData<TStructType> typedData = new SerializableTypedData<TStructType>(domain, message);

            // MetaMask doesn't work with regular eth_signTypedData method, has to be eth_signTypedData_v4.
            return walletProvider.Perform<string>(walletConfig.SignTypedMessageRpcMethodName, typedData, PublicAddress);
        }

        public virtual async ValueTask WillStopAsync()
        {
            await walletProvider.Disconnect();
        }
    }
}