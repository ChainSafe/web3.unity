using System.Threading.Tasks;
using ChainSafe.Gaming.Evm.Transactions;

namespace ChainSafe.Gaming.Web3.Core.Evm
{
    public interface ITransactionExecutor
    {
        public Task<TransactionResponse> SendTransaction(TransactionRequest transaction);
    }
}