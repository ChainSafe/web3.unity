using Nethereum.RPC.Eth.DTOs;

namespace ChainSafe.GamingWeb3.Evm.Contracts
{
    public interface IEventLog
    {
        FilterLog Log { get; }
    }
}