using Nethereum.RPC.Eth.DTOs;

namespace ChainSafe.Gaming.Evm.Contracts
{
    public interface IEventLog
    {
        FilterLog Log { get; }
    }
}