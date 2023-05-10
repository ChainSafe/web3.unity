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
        private readonly JsonRpcProviderConfiguration _configuration;
        private readonly Web3Environment _environment;
        private readonly ChainProvider _chainProvider;

        private uint _nextMessageId;

        // todo can be removed after Evm/Migration removed from project
        public string RpcNodeUrl => _configuration.RpcNodeUrl;

        private readonly JsonSerializerSettings _jsonSerializerSettings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore
        };


        public JsonRpcProvider(JsonRpcProviderConfiguration configuration, Web3Environment environment,
            ChainProvider chainProvider) : base(configuration.Network)
        {
            _chainProvider = chainProvider;
            _environment = environment;
            _configuration = configuration;

            if (string.IsNullOrEmpty(_configuration.RpcNodeUrl))
            {
                _configuration.RpcNodeUrl = _environment.SettingsProvider.DefaultRpcUrl;
            }
        }

        public override async Task<Network.Network> DetectNetwork()
        {
            // TODO: cache
            var chainIdHexString = await Send<string>("eth_chainId");
            var chainId = new HexBigInteger(chainIdHexString).ToUlong();

            if (chainId <= 0)
            {
                throw new Web3Exception("Couldn't detect network");
            }

            var chain = await _chainProvider.GetChain(chainId);
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
            var httpClient = _environment.HttpClient;
            var request = new RpcRequestMessage(_nextMessageId++, method, parameters);
            var response = await httpClient.Post<RpcRequestMessage, RpcResponseMessage>(_configuration.RpcNodeUrl, request);

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

        protected override void _populateEventProperties(Dictionary<string, object> properties)
        {
            properties["provider"] = "jsonrpc";
            properties["rpc"] = _configuration.RpcNodeUrl;
        }
    }
}