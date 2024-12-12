using System.Threading.Tasks;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.RPC.HostWallet;

namespace Reown.Sign.Nethereum
{
    public abstract class ReownSignService
    {
        public virtual bool IsWalletConnected
        {
            get => false;
        }

        public bool IsMethodSupported(string method)
        {
            if (string.IsNullOrEmpty(method))
                throw new System.ArgumentException("Method cannot be null or empty", nameof(method));
            return IsMethodSupportedCore(method);
        }

        public Task<object> SendTransactionAsync(TransactionInput transaction)
        {
            return SendTransactionAsyncCore(transaction);
        }

        public Task<object> PersonalSignAsync(string message)
        {
            if (string.IsNullOrEmpty(message))
                throw new System.ArgumentException("Message cannot be null or empty", nameof(message));
            return PersonalSignAsyncCore(message);
        }

        public Task<object> EthSignTypedDataV4Async(string data)
        {
            if (string.IsNullOrEmpty(data))
                throw new System.ArgumentException("Data cannot be null or empty", nameof(data));
            return EthSignTypedDataV4AsyncCore(data);
        }

        public Task<object> WalletSwitchEthereumChainAsync(SwitchEthereumChainParameter chainId)
        {
            return WalletSwitchEthereumChainAsyncCore(chainId);
        }

        public Task<object> WalletAddEthereumChainAsync(AddEthereumChainParameter chain)
        {
            return WalletAddEthereumChainAsyncCore(chain);
        }

        protected abstract bool IsMethodSupportedCore(string method);
        protected abstract Task<object> SendTransactionAsyncCore(TransactionInput transaction);
        protected abstract Task<object> PersonalSignAsyncCore(string message);
        protected abstract Task<object> EthSignTypedDataV4AsyncCore(string data);
        protected abstract Task<object> WalletSwitchEthereumChainAsyncCore(SwitchEthereumChainParameter chainId);
        protected abstract Task<object> WalletAddEthereumChainAsyncCore(AddEthereumChainParameter chain);
    }
}