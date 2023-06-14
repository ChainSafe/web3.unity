using System.Threading.Tasks;
using Web3Unity.Scripts.Library.Ethers.Transactions;

namespace Web3Unity.Scripts.Library.Ethers.Signers
{
    public interface ISigner
    {
        public Task<string> GetAddress();

        public Task<string> SignMessage(string message);

        public Task<string> SignTransaction(TransactionRequest transaction);
    }
}