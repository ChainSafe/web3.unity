using System.Threading.Tasks;
using Web3Unity.Scripts.Library.Ethers.Transactions;

namespace ChainSafe.GamingSDK.EVM.Web3.Core.Evm
{
    public interface ITransactionExecutor
    {
        public Task<TransactionResponse> SendTransaction(TransactionRequest transaction);
    }
}