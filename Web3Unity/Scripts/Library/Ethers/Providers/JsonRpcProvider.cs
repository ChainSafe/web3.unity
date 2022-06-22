using System;
using System.Threading.Tasks;
using Nethereum.Hex.HexTypes;
using Nethereum.JsonRpc.Client.RpcMessages;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Web3Unity.Scripts.Library.Ethers.Network;
using Web3Unity.Scripts.Library.Ethers.Signers;

namespace Web3Unity.Scripts.Library.Ethers.Providers
{
    public class JsonRpcProvider : BaseProvider
    {
        private readonly string _connection;

        private readonly JsonSerializerSettings _jsonSerializerSettings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore
        };

        public JsonRpcProvider(string url = "", Network.Network network = null) : base(network)
        {
            if (url == "")
            {
                url = DefaultUrl();
            }

            _connection = url;

            if (network != null) return;

            var task = Task.Run(async () => { base._network = await DetectNetwork(); });
            task.Wait(TimeSpan.FromMilliseconds(0));
        }

        public JsonRpcProvider(string chain, string network) : base(null)
        {
            throw new NotImplementedException();
        }

        public JsonRpcProvider(uint chainId) : base(null)
        {
            throw new NotImplementedException();
        }

        public static string DefaultUrl()
        {
            return "http://localhost:8545";
        }

        public override async Task<Network.Network> DetectNetwork()
        {
            // TODO: cache
            return await _uncachedDetectNetwork();
        }

        private async Task<Network.Network> _uncachedDetectNetwork()
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
                    chainId = new HexBigInteger(await Send<string>("eth_chainId")).ToUlong();
                }
                catch (Exception)
                {
                    // ignored
                }
            }

            if (chainId > 0)
            {
                var chains = await Chains.GetChains();
                var chain = chains[chainId];
                
                if (chain == null)
                {
                    return new Network.Network
                    {
                        Name = "Unknown",
                        ChainId = chainId
                    };
                }
                
                return new Network.Network
                {
                    Name = chain.Name,
                    ChainId = chain.ChainId
                };
            }

            throw new Exception("could not detect network");
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

        public async Task<T> Send<T>(string method, object[] parameters = null)
        {
            var json = JsonConvert.SerializeObject(new RpcRequestMessage(1, method, parameters),
                _jsonSerializerSettings); // TODO: nextId
            var response = await Web.Web.FetchJson<RpcResponseMessage>(_connection, json);
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
    }
}