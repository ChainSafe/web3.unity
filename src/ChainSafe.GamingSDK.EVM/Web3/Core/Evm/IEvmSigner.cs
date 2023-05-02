using System.Threading.Tasks;
using ChainSafe.GamingWeb3.Evm;

namespace ChainSafe.GamingWeb3.Evm.Signers
{
    public interface IEvmSigner
    {
        public Task<string> GetAddress();
        public Task<string> SignMessage(byte[] message);
        public Task<string> SignMessage(string message);
        public Task<string> SignTransaction(TransactionRequest transaction);
        public Task<TransactionResponse> SendTransaction(TransactionRequest transaction);
        
        // todo remove after migration complete
        public IEvmProvider Provider { get; }
    }
}