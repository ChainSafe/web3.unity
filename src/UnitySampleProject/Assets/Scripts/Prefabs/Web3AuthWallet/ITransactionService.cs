using System.Threading.Tasks;
using Web3Unity.Scripts.Library.Ethers.Transactions;

public interface ITransactionService
{
    Task<string> BroadcastTransaction(TransactionRequest txRequest, string account, string signature, string gasPrice, string gasLimit);
}