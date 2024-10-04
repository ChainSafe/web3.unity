using System;
using System.Threading.Tasks;
using ChainSafe.Gaming.RPC.Events;

namespace ChainSafe.Gaming.Evm.Contracts
{
    public interface ICustomContract : IContract, IAsyncDisposable
    {
        public string ABI { get; }

        public string ContractAddress { get; set; }

        public Contract OriginalContract { get; set; }

        public IEventManager EventManager { get; set; }

        public bool Subscribed { get; set; }

        public ValueTask InitAsync();
    }
}