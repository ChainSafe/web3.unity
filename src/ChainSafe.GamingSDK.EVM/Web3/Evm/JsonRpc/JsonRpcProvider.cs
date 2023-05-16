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
        private readonly JsonRpcProviderConfiguration configuration;
        private readonly Web3Environment environment;
        private readonly ChainProvider chainProvider;

        private uint nextMessageId;

        public JsonRpcProvider(
            JsonRpcProviderConfiguration configuration,
            Web3Environment environment,
            ChainProvider chainProvider)
            : base(configuration.Network, environment)
        {
            this.chainProvider = chainProvider;
            this.environment = environment;
            this.configuration = configuration;

            if (string.IsNullOrEmpty(this.configuration.RpcNodeUrl))
            {
                this.configuration.RpcNodeUrl = this.environment.SettingsProvider.DefaultRpcUrl;
            }
        }

        // todo can be removed after Evm/Migration removed from project
        public string RpcNodeUrl => configuration.RpcNodeUrl;

        public override async Task<Network.Network> DetectNetwork()
        {
            // TODO: cache
            var chainIdHexString = await Send<string>("eth_chainId");
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

        public async Task<TResponse> Send<TResponse>(string method, params object[] parameters)
        {
            var httpClient = environment.HttpClient;
            var request = new RpcRequestMessage(nextMessageId++, method, parameters);
            var response = (await httpClient.Post<RpcRequestMessage, RpcResponseMessage>(configuration.RpcNodeUrl, request)).EnsureResponse();

            if (response.HasError)
            {
                var error = response.Error;
                var errorMessage = $"RPC returned error for \"{method}\": {error.Code} {error.Message} {error.Data}";
                throw new Web3Exception(errorMessage);
            }

            var serializer = JsonSerializer.Create();
            return serializer.Deserialize<TResponse>(new JTokenReader(response.Result))!;
        }

        public override async Task<T> Perform<T>(string method, params object[] parameters)
        {
            return await Send<T>(method, parameters);
        }
    }
}