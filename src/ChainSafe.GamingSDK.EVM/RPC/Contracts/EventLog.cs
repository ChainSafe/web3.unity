using Nethereum.RPC.Eth.DTOs;

namespace Web3Unity.Scripts.Library.Ethers.Contracts
{
    public class EventLog<T> : IEventLog
    {
        public EventLog(T eventObject, FilterLog log)
        {
            Event = eventObject;
            Log = log;
        }

        public T Event { get; }

        public FilterLog Log { get; }
    }
}