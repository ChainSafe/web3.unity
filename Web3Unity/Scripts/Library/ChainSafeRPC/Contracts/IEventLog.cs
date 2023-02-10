using Nethereum.RPC.Eth.DTOs;

namespace Web3Unity.Scripts.Library.Ethers.Contracts
{
    public interface IEventLog
    {
        FilterLog Log { get; }
    }
}