using System.Collections.Generic;
using System.Threading.Tasks;
using ChainSafe.Gaming.Web3.Core;
using Nethereum.Contracts.QueryHandlers.MultiCall;

namespace ChainSafe.Gaming.MultiCall
{
    public interface IMultiCall : ILifecycleParticipant
    {
        public Task<List<object[]>> MultiCallAsync(Call3Value[] multiCalls, int pageSize = 3000);
    }
}