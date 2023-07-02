using System.Threading.Tasks;
using Web3Unity.Scripts.Library.ETHEREUEM.Connect;
using Web3Unity.Scripts.Library.Ethers.Transactions;

namespace Prefabs.Web3AuthWallet.Interfaces
{
    public interface ITransactionService
    {
        Task<EVM.Response<string>> BroadcastTransaction(TransactionRequest txRequest, string account, string signature, string gasPrice, string gasLimit);

        Task<EVM.Response<string>> CreateTransaction(TransactionRequest txRequest, string account, string gasPrice, string gasLimit, string nonce);

        Task<TransactionResponse?> SendTransaction(TransactionRequest txRequest, string signature);
    }
}