using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Nethereum.Hex.HexTypes;
using Nethereum.JsonRpc.Client.RpcMessages;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Web3Unity.Scripts.Library.Ethers.Network;
using Web3Unity.Scripts.Library.Ethers.RPC;
using Web3Unity.Scripts.Library.Ethers.Signers;

namespace Web3Unity.Scripts.Library.Ethers.Providers
{
    public class JsonRpcProvider : BaseProvider
    {
        private readonly string connection;
        private readonly JsonSerializerSettings jsonSerializerSettings = new()
        {
            NullValueHandling = NullValueHandling.Ignore,
        };

        private ulong nextId;

        public JsonRpcProvider(string url = "", Network.Network network = null)
            : base(network)
        {
            if (string.IsNullOrEmpty(url))
            {
                url = DefaultUrl();
            }

            connection = url;

            if (network != null)
            {
                return;
            }

            // Note: Task.Run starts the task regardless of whether we wait on it.
            Task.Run(async () =>
            {
                Network = await DetectNetwork();
            });
        }

        public static string DefaultUrl() => RpcEnvironmentStore.Environment.GetDefaultRpcUrl();

        public override async Task<Network.Network> DetectNetwork()
        {
            // TODO: cache
            return await UncachedDetectNetwork();
        }

        private async Task<Network.Network> UncachedDetectNetwork()
        {
            ulong chainId = 0;
            try
            {
                chainId = new HexBigInteger(await Send<string>("eth_chainId")).ToUlong();
            }
            catch (Exception)
            {
                try
                {
                    // TODO: why do this twice?
                    chainId = new HexBigInteger(await Send<string>("eth_chainId")).ToUlong();
                }
                catch (Exception e)
                {
                    RpcEnvironmentStore.Environment.LogError(e.ToString());

                    // TODO: ignored
                }
            }

            if (chainId == 0)
            {
                throw new Exception("could not detect network");
            }

            var chains = await Chains.GetChains();
            var chain = chains[chainId];

            if (chain == null)
            {
                return new Network.Network
                {
                    Name = "Unknown",
                    ChainId = chainId,
                };
            }

            return new Network.Network
            {
                Name = chain.Name,
                ChainId = chain.ChainId,
            };
        }

        public JsonRpcSigner GetSigner(string address)
        {
            return new JsonRpcSigner(this, address);
        }

        public JsonRpcSigner GetSigner(int index = 0)
        {
            return new JsonRpcSigner(this, index);
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

        public virtual async Task<T> Send<T>(string method, object[] parameters = null)
        {
            var json = JsonConvert.SerializeObject(
                new RpcRequestMessage(++nextId, method, parameters),
                jsonSerializerSettings);

            var networkResponse = await RpcEnvironmentStore.Environment.PostAsync(connection, json, "application/json");
            networkResponse.EnsureSuccess();

            var response = JsonConvert.DeserializeObject<RpcResponseMessage>(
                networkResponse.Response,
                jsonSerializerSettings);
            if (response.Error != null)
            {
                throw new Exception($"{method}: {response.Error.Code} {response.Error.Message} {response.Error.Data}");
            }

            var serializer = JsonSerializer.Create();
            return serializer.Deserialize<T>(new JTokenReader(response.Result));
        }

        public override async Task<T> Perform<T>(string method, object[] parameters = null)
        {
            return await Send<T>(method, parameters);
        }

        protected override void PopulateEventProperties(Dictionary<string, object> properties)
        {
            properties["provider"] = "jsonrpc";
            properties["rpc"] = connection;
        }
    }
}