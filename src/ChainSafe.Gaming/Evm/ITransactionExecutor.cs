using System.Threading.Tasks;

namespace ChainSafe.Gaming.Evm
{
    public interface ITransactionExecutor
    {
        public Task<TransactionResponse> SendTransaction(TransactionRequest transaction);
    }
}