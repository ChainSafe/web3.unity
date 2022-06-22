using System.Threading.Tasks;
using Nethereum.RPC.Eth.DTOs;
using Web3Unity.Scripts.Library.Ethers.Providers;

namespace Web3Unity.Scripts.Library.Ethers.Signers
{
    public interface ISigner
    {
        public Task<string> GetAddress();
        public Task<string> SignMessage(byte[] message);
        public Task<string> SignMessage(string message);
        public Task<string> SignTransaction(Transaction transaction);
        public ISigner Connect(IProvider provider);
    }
}