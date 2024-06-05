using System.Threading.Tasks;
using ChainSafe.Gaming.Web3;
using ChainSafe.Gaming.Web3.Environment;
using Nethereum.JsonRpc.Client;
using Nethereum.JsonRpc.Client.RpcMessages;
using Newtonsoft.Json;

namespace ChainSafe.Gaming.InProcessTransactionExecutor
{
    public class RpcClient : ClientBase
    {
        private readonly IHttpClient httpClient;
        private readonly IChainConfig chainConfig;

        public RpcClient(IHttpClient httpClient, IChainConfig chainConfig)
        {
            this.httpClient = httpClient;
            this.chainConfig = chainConfig;
        }

        protected override async Task<RpcResponseMessage> SendAsync(RpcRequestMessage request, string route = null)
        {
            string body = JsonConvert.SerializeObject(request);

            return await SendAsyncInternally<RpcResponseMessage>(body);
        }

        protected override async Task<RpcResponseMessage[]> SendAsync(RpcRequestMessage[] requests)
        {
            string body = JsonConvert.SerializeObject(requests);

            return await SendAsyncInternally<RpcResponseMessage[]>(body);
        }

        private async Task<T> SendAsyncInternally<T>(string body)
        {
            var result = await httpClient.PostRaw(chainConfig.Rpc, body, "application/json");

            return JsonConvert.DeserializeObject<T>(result.Response);
        }
    }
}