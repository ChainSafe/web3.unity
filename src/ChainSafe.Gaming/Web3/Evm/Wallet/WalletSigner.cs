using System.Threading.Tasks;
using ChainSafe.Gaming.Evm.Signers;
using ChainSafe.Gaming.Web3.Core;
using ChainSafe.Gaming.Web3.Core.Debug;
using ChainSafe.Gaming.Web3.Core.Evm;
using ChainSafe.Gaming.Web3.Core.Logout;

namespace ChainSafe.Gaming.Web3.Evm.Wallet
{
    /// <summary>
    /// Concrete implementation of <see cref="ISigner"/> for signing messages using a wallet.
    /// This can be used with any wallet provider that implements <see cref="IWalletProvider"/>.
    /// </summary>
    public class WalletSigner : ISigner, ILifecycleParticipant, ILogoutHandler
    {
        private readonly IWalletProvider walletProvider;
        private readonly IWalletProviderConfig walletConfig;

        public WalletSigner(IWalletProvider walletProvider, IWalletProviderConfig walletConfig)
        {
            this.walletProvider = walletProvider;
            this.walletConfig = walletConfig;
        }

        public string PublicAddress { get; private set; }

        public virtual async ValueTask WillStartAsync()
        {
            string address = await walletProvider.Connect();

            PublicAddress = address.AssertIsPublicAddress();
        }

        public virtual async Task<string> SignMessage(string message)
        {
            string hash = await walletProvider.Request<string>(walletConfig.SignMessageRpcMethodName, message, PublicAddress);

            return hash.AssertSignatureValid(message, PublicAddress);
        }

        public virtual async Task<string> SignTypedData<TStructType>(SerializableDomain domain, TStructType message)
        {
            SerializableTypedData<TStructType> typedData = new SerializableTypedData<TStructType>(domain, message);

            string hash = await walletProvider.Request<string>(walletConfig.SignTypedMessageRpcMethodName, PublicAddress, typedData);

            return hash.AssertTypedDataSignatureValid(typedData, PublicAddress);
        }

        public virtual ValueTask WillStopAsync()
        {
            return new ValueTask(Task.CompletedTask);
        }

        public async Task OnLogout()
        {
            await walletProvider.Disconnect();
        }
    }
}