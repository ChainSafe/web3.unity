using System.Threading.Tasks;
using Web3Unity.Scripts.Library.Ethers.Providers;
using Web3Unity.Scripts.Library.Ethers.Transactions;

namespace Web3Unity.Scripts.Library.Ethers.Signers
{
    public interface IEvmSigner
    {
        bool Connected { get; }

        // todo remove after migration complete
        public IEvmProvider Provider { get; }

        ValueTask Connect();

        public Task<string> GetAddress();

        public Task<string> SignMessage(byte[] message);

        public Task<string> SignMessage(string message);

        public Task<string> SignTransaction(TransactionRequest transaction);

        public Task<TransactionResponse> SendTransaction(TransactionRequest transaction);
    }
}