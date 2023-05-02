using System;
using System.Threading.Tasks;
using Nethereum.JsonRpc.Client.RpcMessages;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ChainSafe.GamingWeb3.Evm.Providers
{
    // public class Web3Provider : JsonRpcProvider
    // {
    //     private readonly IExternalProvider _provider;
    //
    //     public Web3Provider(IExternalProvider provider,
    //         Network network = null)
    //     {
    //         if (provider == null)
    //         {
    //             throw new Exception($"missing provider {nameof(provider)}");
    //         }
    //
    //         // TODO: why make an unused variable?
    //         var path = provider.GetPath();
    //
    //         _provider = provider;
    //     }
    //
    //     public override async Task<T> Send<T>(string method, object[] parameters = null)
    //     {
    //         return await _provider.Send<T>(method, parameters);
    //     }
    // }
}