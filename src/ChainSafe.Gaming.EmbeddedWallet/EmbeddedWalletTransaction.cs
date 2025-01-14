using System.Threading.Tasks;
using ChainSafe.Gaming.Evm.Transactions;

namespace ChainSafe.Gaming.EmbeddedWallet
{
    public class EmbeddedWalletTransaction
    {
        public EmbeddedWalletTransaction(TransactionRequest request)
        {
            Request = request;

            Response = new TaskCompletionSource<TransactionResponse>();
        }

        public TransactionRequest Request { get; private set; }

        public TaskCompletionSource<TransactionResponse> Response { get; private set; }
    }
}