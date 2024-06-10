using System;
using System.Threading.Tasks;
using ChainSafe.Gaming.Web3;
using ChainSafe.Gaming.Web3.Analytics;
using ChainSafe.Gaming.Web3.Core;
using ChainSafe.Gaming.Web3.Environment;
using Nethereum.Hex.HexTypes;
using Nethereum.JsonRpc.Client;
using Nethereum.JsonRpc.Client.RpcMessages;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ChainSafe.Gaming.Evm.Providers
{
    public class RpcClientProvider : ClientBase, IRpcProvider, ILifecycleParticipant
    {
        private readonly RpcClientConfig config;
        private readonly Web3Environment environment;
        private readonly ChainRegistryProvider chainRegistryProvider;
        private readonly IChainConfig chainConfig;

        private Network.Network network;

        public RpcClientProvider(
            RpcClientConfig config,
            Web3Environment environment,
            ChainRegistryProvider chainRegistryProvider,
            IChainConfig chainConfig)
        {
            this.chainRegistryProvider = chainRegistryProvider;
            this.environment = environment;
            this.config = config;
            this.chainConfig = chainConfig;

            if (string.IsNullOrEmpty(this.config.RpcNodeUrl))
            {
                this.config.RpcNodeUrl = chainConfig.Rpc;
            }
        }

        public Network.Network LastKnownNetwork
        {
            get => network;
            protected set => network = value;
        }

        public async ValueTask WillStartAsync()
        {
            if (network is null || network.ChainId == 0)
            {
                if (ulong.TryParse(chainConfig.ChainId, out var chainId))
                {
                    var chain = await chainRegistryProvider.GetChain(chainId);
                    network = new Network.Network()
                    {
                        ChainId = chainId,
                        Name = chain?.Name,
                    };
                }

                network = await RefreshNetwork();
            }
        }

        public ValueTask WillStopAsync() => new(Task.CompletedTask);

        public async Task<Network.Network> DetectNetwork()
        {
            // TODO: cache
            var chainIdHexString = await Perform<string>("eth_chainId");
            var chainId = new HexBigInteger(chainIdHexString).ToUlong();

            if (chainId <= 0)
            {
                throw new Web3Exception("Couldn't detect network");
            }

            var chain = await chainRegistryProvider.GetChain(chainId);
            return chain != null
                ? new Network.Network { Name = chain.Name, ChainId = chainId }
                : new Network.Network { Name = "Unknown", ChainId = chainId };
        }

        public async Task<Network.Network> RefreshNetwork()
        {
            var currentNetwork = await DetectNetwork();

            if (network != null && network.ChainId == currentNetwork.ChainId)
            {
                return network;
            }

            network = currentNetwork;
            return network;
        }

        public async Task<T> Perform<T>(string method, params object[] parameters)
        {
            // parameters should be skipped or be an empty array if there are none
            parameters ??= Array.Empty<object>();

            try
            {
                var request = new RpcRequestMessage(Guid.NewGuid().ToString(), method, parameters);

                var response = await SendAsync(request);

                var serializer = JsonSerializer.Create();
                return serializer.Deserialize<T>(new JTokenReader(response.Result))!;
            }
            catch (Exception ex)
            {
                throw new Web3Exception($"{method}: bad result from RPC endpoint", ex);
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
            var result = await environment.HttpClient.PostRaw(config.RpcNodeUrl, body, "application/json");

            return JsonConvert.DeserializeObject<T>(result.Response);
        }
    }
}