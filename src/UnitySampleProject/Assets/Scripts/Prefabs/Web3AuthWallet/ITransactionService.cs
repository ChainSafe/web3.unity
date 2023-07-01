using System.Threading.Tasks;
using Web3Unity.Scripts.Library.Ethers.Transactions;

public interface ITransactionService
{
    Task<string> CreateTransaction(string account, TransactionRequest txRequest, string gasPrice = "", string gasLimit = "", string rpc = "", string nonce = "");

    Task<string> BroadcastTransaction(TransactionRequest txRequest, string _account, string _signature, string _gasPrice, string _gasLimit, string _rpc);
}