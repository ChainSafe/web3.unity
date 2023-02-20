using System;
using System.Threading.Tasks;
using Nethereum.JsonRpc.Client.RpcMessages;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Web3Unity.Scripts.Library.Ethers.Runtime;

namespace Web3Unity.Scripts.Library.Ethers.Providers
{
    public class Web3Provider : JsonRpcProvider
    {
        private readonly IExternalProvider _provider;

        public Web3Provider(IExternalProvider provider,
            Network.Network network = null)
        {
            if (provider == null)
            {
                throw new Exception($"missing provider {nameof(provider)}");
            }

            var path = provider.GetPath();

            _provider = provider;
        }

        public override async Task<T> Send<T>(string method, object[] parameters = null)
        {
            return await _provider.Send<T>(method, parameters);
        }
    }
}