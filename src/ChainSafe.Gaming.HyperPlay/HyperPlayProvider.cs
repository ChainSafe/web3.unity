using System;
using System.Threading.Tasks;
using ChainSafe.Gaming.Web3;
using ChainSafe.Gaming.Web3.Core;
using ChainSafe.Gaming.Web3.Environment;
using Newtonsoft.Json;

namespace ChainSafe.Gaming.HyperPlay
{
    /// <summary>
    /// Concrete implementation of <see cref="IHyperPlayProvider"/>
    /// </summary>
    public class HyperPlayProvider : IHyperPlayProvider
    {
        private readonly IHttpClient httpClient;
        private readonly IChainConfig chainConfig;

        /// <summary>
        /// Initializes a new instance of the <see cref="HyperPlayProvider"/> class.
        /// </summary>
        /// <param name="httpClient">HttpClient to make requests.</param>
        /// <param name="chainConfig">ChainConfig to fetch chain data.</param>
        public HyperPlayProvider(IHttpClient httpClient, IChainConfig chainConfig)
        {
            this.httpClient = httpClient;
            this.chainConfig = chainConfig;
        }

        /// <summary>
        /// Connect to wallet via HyperPlay desktop client and return the account address.
        /// </summary>
        /// <returns>Signed-in account public address.</returns>
        public async Task<string> Connect()
        {
            string[] accounts = await Request<string[]>("eth_accounts");

            string account = accounts[0];

            string hash = await Request<string>("personal_sign", "Sign-in with Ethereum.", account);

            return account;
        }

        /// <summary>
        /// Make RPC request to HyperPlay desktop client.
        /// </summary>
        /// <param name="method">RPC request method name.</param>
        /// <param name="parameters">RPC request parameters.</param>
        /// <typeparam name="T">RPC request response type.</typeparam>
        /// <returns>RPC request Response.</returns>
        public async Task<T> Request<T>(string method, params object[] parameters)
        {
            string body = JsonConvert.SerializeObject(new HyperPlayRequest
            {
                Request = new Request
                {
                    Method = method,
                    Params = parameters,
                },
                Chain = new Chain
                {
                    ChainId = chainConfig.ChainId,
                },
            });

            string response = (await httpClient.PostRaw("localhost:9680/rpc", body, "application/json")).Response;

            try
            {
                var error = JsonConvert.DeserializeObject<HyperPlayError>(response);

                throw new Web3Exception($"HyperPlay RPC request failed: {error.Message}");
            }
            catch (Exception)
            {
                if (response is T result)
                {
                    return result;
                }

                return JsonConvert.DeserializeObject<T>(response);
            }
        }
    }
}