using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ChainSafe.GamingWeb3;
using ChainSafe.GamingWeb3.Environment;
using Nethereum.Hex.HexTypes;
using Nethereum.JsonRpc.Client.RpcMessages;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Web3Unity.Scripts.Library.Ethers.Providers
{
    public class JsonRpcProvider : BaseProvider
    {
        private readonly JsonRpcProviderConfig config;
        private readonly Web3Environment environment;
        private readonly ChainProvider chainProvider;

        private uint nextMessageId;

        public JsonRpcProvider(
            JsonRpcProviderConfig config,
            Web3Environment environment,
            ChainProvider chainProvider,
            IChainConfig chainConfig)
            : base(config.Network, environment)
        {
            this.chainProvider = chainProvider;
            this.environment = environment;
            this.config = config;

            if (string.IsNullOrEmpty(this.config.RpcNodeUrl))
            {
                this.config.RpcNodeUrl = chainConfig.Rpc;
            }
        }

        // todo can be removed after Evm/Migration removed from project
        public string RpcNodeUrl => config.RpcNodeUrl;

        public override async Task<Network.Network> DetectNetwork()
        {
            // TODO: cache
            var chainIdHexString = await Perform<string>("eth_chainId");
            var chainId = new HexBigInteger(chainIdHexString).ToUlong();

            if (chainId <= 0)
            {
                throw new Web3Exception("Couldn't detect network");
            }

            var chain = await chainProvider.GetChain(chainId);
            return chain != null
                ? new Network.Network { Name = chain.Name, ChainId = chainId }
                : new Network.Network { Name = "Unknown", ChainId = chainId };
        }

        public async Task<string[]> ListAccounts()
        {
            var result = await Perform<string[]>("eth_accounts");
            try
            {
                return result;
            }
            catch (Exception e)
            {
                throw new Exception("bad result from backend", e);
            }
        }

        public override async Task<T> Perform<T>(string method, params object[] parameters)
        {
            try
            {
                var httpClient = environment.HttpClient;
                var request = new RpcRequestMessage(nextMessageId++, method, parameters);
                var response = (await httpClient.Post<RpcRequestMessage, RpcResponseMessage>(config.RpcNodeUrl, request)).EnsureResponse();

                if (response.HasError)
                {
                    var error = response.Error;
                    var errorMessage = $"RPC returned error for \"{method}\": {error.Code} {error.Message} {error.Data}";
                    throw new Web3Exception(errorMessage);
                }

                var serializer = JsonSerializer.Create();
                return serializer.Deserialize<T>(new JTokenReader(response.Result))!;
            }
            catch (Exception ex)
            {
                throw new Web3Exception($"{method}: bad result from RPC endpoint", ex);
            }
        }
    }
}