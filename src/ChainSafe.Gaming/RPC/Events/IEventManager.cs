using System;
using System.Threading.Tasks;
using Nethereum.ABI.FunctionEncoding.Attributes;

namespace ChainSafe.Gaming.RPC.Events
{
    public interface IEventManager
    {
        Task Subscribe<TEvent>(Action<TEvent> handler)
            where TEvent : IEventDTO, new();

        Task Unsubscribe<TEvent>(Action<TEvent> handler)
            where TEvent : IEventDTO, new();
    }
}