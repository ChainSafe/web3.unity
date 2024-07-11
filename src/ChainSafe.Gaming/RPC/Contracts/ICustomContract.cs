using System;
using System.Threading.Tasks;
using ChainSafe.Gaming.Evm.Providers;
using Nethereum.RPC.Reactive.Eth.Subscriptions;

namespace ChainSafe.Gaming.Evm.Contracts
{
    public interface ICustomContract : IContract, IAsyncDisposable
    {
        public string ABI { get; }

        public string ContractAddress { get; set; }

        public Contract OriginalContract { get;  set; }

        public EthLogsObservableSubscription Subscription { get; set; }

        public ValueTask Init();
    }
}