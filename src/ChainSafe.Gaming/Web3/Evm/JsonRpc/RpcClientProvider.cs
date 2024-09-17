using System;
using System.Threading.Tasks;
using ChainSafe.Gaming.Web3;
using ChainSafe.Gaming.Web3.Analytics;
using ChainSafe.Gaming.Web3.Environment;
using Nethereum.JsonRpc.Client;
using Nethereum.JsonRpc.Client.RpcMessages;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ChainSafe.Gaming.Evm.Providers
{
    public class RpcClientProvider : ClientBase, IRpcProvider
    {
        private readonly Web3Environment environment;
        private readonly IChainConfig chainConfig;

        public RpcClientProvider(
            Web3Environment environment,
            IChainConfig chainConfig)
        {
            this.environment = environment;
            this.chainConfig = chainConfig;
        }

        public async Task<T> Perform<T>(string method, params object[] parameters)
        {
            // parameters should be skipped or be an empty array if there are none
            parameters ??= Array.Empty<object>();
            RpcResponseMessage response = null;
            try
            {
                var request = new RpcRequestMessage(Guid.NewGuid().ToString(), method, parameters);
                response = await SendAsync(request);

                // Check if response.Result is null
                if (response.Result == null)
                {
                    throw new Web3Exception($"RPC method \"{method}\" returned a null result.");
                }
            }
            catch (Exception ex)
            {
                throw new Web3Exception($"RPC method \"{method}\" threw an exception:" + response?.Error?.Message, ex);
            }
            finally
            {
                environment.AnalyticsClient.CaptureEvent(new AnalyticsEvent()
                {
                    EventName = $"{method}",
                    GameData = new AnalyticsGameData()
                    {
                        Params = parameters,
                    },
                    PackageName = "io.chainsafe.web3-unity",
                });
            }

            var serializer = JsonSerializer.Create();
            return serializer.Deserialize<T>(new JTokenReader(response.Result))!;
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
            var result = await environment.HttpClient.PostRaw(chainConfig.Rpc, body, "application/json");

            return JsonConvert.DeserializeObject<T>(result.Response);
        }
    }
}