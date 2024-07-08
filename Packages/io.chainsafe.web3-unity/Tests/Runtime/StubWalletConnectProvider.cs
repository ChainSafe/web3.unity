using System;
using System.Threading.Tasks;
using ChainSafe.Gaming.Evm;
using ChainSafe.Gaming.Web3;
using ChainSafe.Gaming.Web3.Environment;
using ChainSafe.Gaming.Web3.Evm.Wallet;
using Nethereum.JsonRpc.Client.RpcMessages;
using Newtonsoft.Json;

namespace Tests.Runtime
{
    public class StubWalletConnectProvider : WalletProvider
    {
        private readonly StubWalletConnectProviderConfig config;
        private readonly IChainConfig chainConfig;
        private readonly IHttpClient httpClient;

        public StubWalletConnectProvider(StubWalletConnectProviderConfig config, IHttpClient httpClient, IChainConfig chainConfig, ChainRegistryProvider chainRegistryProvider) : base(chainRegistryProvider: chainRegistryProvider)
        {
            this.config = config;
            this.chainConfig = chainConfig;
            this.httpClient = httpClient;
        }

        public override Task<string> Connect()
        {
            return Task.FromResult(config.WalletAddress);
        }

        public override Task Disconnect()
        {
            // empty
            return Task.CompletedTask;
        }

        public override Task<T> Perform<T>(string method, params object[] parameters)
        {
            switch (method)
            {
                case "personal_sign": case "eth_signTypedData": case "eth_sendTransaction":
                    return Task.FromResult((T)Convert.ChangeType(config.StubResponse, typeof(T)));
                default:
                    return Request<T>(method, parameters);
            }
        }
        
        // Direct RPC request via WalletConnect RPC url.
        private async Task<T> Request<T>(string method, params object[] parameters)
        {
            // Using WalletConnect Blockchain API: https://docs.walletconnect.com/cloud/blockchain-api
            var url = $"https://rpc.walletconnect.com/v1?chainId=eip155:{chainConfig.ChainId}&projectId={config.ProjectId}";

            string body = JsonConvert.SerializeObject(new RpcRequestMessage(Guid.NewGuid().ToString(), method, parameters));
            
            var rawResult = await httpClient.PostRaw(url, body, "application/json");

            RpcResponseMessage response = JsonConvert.DeserializeObject<RpcResponseMessage>(rawResult.Response);
            
            return response.Result.ToObject<T>();
        }
    }
}