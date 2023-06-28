using System.Threading.Tasks;
using Web3Unity.Scripts.Library.Ethers.Transactions;

public interface ITransactionService
{
    Task<string> CreateTransaction(string chain, string network, string account, string to, string value, string data, string gasPrice = "", string gasLimit = "", string rpc = "", string nonce = "");

    Task<string> BroadcastTransaction(string chain, string network, string account, string to, string value, string data, string signature, string gasPrice, string gasLimit, string rpc);

    Task<TransactionResponse?> SendTransaction(TransactionRequest transaction);
}