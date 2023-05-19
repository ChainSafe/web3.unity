using Web3Unity.Scripts.Library.Ethers.Providers;
using Web3Unity.Scripts.Library.Ethers.Signers;
using Web3Unity.Scripts.Library.Ethers.Transactions;

namespace ChainSafe.GamingSDK.EVM.WebWallet
{
    public class WebSigner : IEvmSigner
    {
        private WebSignerConfiguration configuration;

        public bool Connected { get; }
        public IEvmProvider Provider { get; }

        public WebSigner(WebSignerConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public ValueTask Connect()
        {
            throw new NotImplementedException();
        }

        public Task<string> GetAddress()
        {
            throw new NotImplementedException();
        }

        public Task<string> SignMessage(byte[] message)
        {
            throw new NotImplementedException();
        }

        public Task<string> SignMessage(string message)
        {
            throw new NotImplementedException();
        }

        public Task<string> SignTransaction(TransactionRequest transaction)
        {
            throw new NotImplementedException();
        }

        public Task<TransactionResponse> SendTransaction(TransactionRequest transaction)
        {
            throw new NotImplementedException();
        }
    }
}