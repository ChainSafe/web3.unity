using System.Threading.Tasks;
using ChainSafe.Gaming.Evm.Transactions;

namespace Chainsafe.Gaming.Web3.Core.Evm
{
    public interface ITransactionExecutor
    {
        public Task<TransactionResponse> SendTransaction(TransactionRequest transaction);
    }
}