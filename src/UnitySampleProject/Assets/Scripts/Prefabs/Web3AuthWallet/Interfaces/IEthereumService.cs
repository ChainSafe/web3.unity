using System.Threading.Tasks;
using Nethereum.RPC.Eth.DTOs;

namespace Prefabs.Web3AuthWallet.Interfaces
{
    public interface IEthereumService
    {
        string GetAddressW3A(string privateKey);

        Task<string> CreateAndSignTransactionAsync(TransactionInput txInput);

        Task<string> SendTransactionAsync(string signedTransactionData);
    }
}