using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using ChainSafe.Gaming.Evm;
using ChainSafe.Gaming.HyperPlay.Dto;
using ChainSafe.Gaming.Web3;
using ChainSafe.Gaming.Web3.Core.Debug;
using ChainSafe.Gaming.Web3.Environment;
using ChainSafe.Gaming.Web3.Evm.Wallet;
using Nethereum.Util;
using Newtonsoft.Json;
using UnityEngine;
using Chain = ChainSafe.Gaming.HyperPlay.Dto.Chain;

namespace ChainSafe.Gaming.HyperPlay
{
    /// <summary>
    /// Concrete implementation of <see cref="IWalletProvider"/>.
    /// </summary>
    public class HyperPlayProvider : WalletProvider
    {
        private readonly IHttpClient httpClient;
        private readonly IChainConfig chainConfig;

        /// <summary>
        /// Initializes a new instance of the <see cref="HyperPlayProvider"/> class.
        /// </summary>
        /// <param name="httpClient">HttpClient to make requests.</param>
        /// <param name="chainConfig">ChainConfig to fetch chain data.</param>
        /// <param name="chainRegistryProvider">Injected <see cref="ChainRegistryProvider"/>.</param>
        public HyperPlayProvider(IHttpClient httpClient, IChainConfig chainConfig, ChainRegistryProvider chainRegistryProvider)
            : base(chainRegistryProvider: chainRegistryProvider)
        {
            this.httpClient = httpClient;
            this.chainConfig = chainConfig;
        }

        /// <summary>
        /// Gets the public address of a wallet via signature.
        /// </summary>
        /// <param name="account">The account address to verify.</param>
        /// <returns>Public account address in lower case.</returns>
        private async Task<string> GetAccountViaSignature(string account)
        {
            string message = "Sign-in with Ethereum";

            string hash = await Perform<string>("personal_sign", message, account);

            EthECDSASignature signature = MessageSigner.ExtractEcdsaSignature(hash);

            string messageToHash = "\x19" + "Ethereum Signed Message:\n" + message.Length + message;

            byte[] messageHash = new Sha3Keccack().CalculateHash(Encoding.UTF8.GetBytes(messageToHash));

            var key = EthECKey.RecoverFromSignature(signature, messageHash);

            return key.GetPublicAddress().ToLower().Trim();
        }

        /// <summary>
        /// Get the connected HyperPlay wallet.
        /// </summary>
        /// <param name="chainId">Chain id we're connected to.</param>
        /// <returns>Connected HyperPlay wallet address.</returns>
        public async Task<string> GetConnectedWallet()
        {
            string[] accounts = await Perform<string[]>("eth_accounts");

            string account = accounts[0].AssertIsPublicAddress(nameof(account));

            return account;
        }

        /// <summary>
        /// Connect to wallet via HyperPlay desktop client and return the account address.
        /// </summary>
        /// <returns>Signed in account's public address.</returns>
        public override async Task<string> Connect()
        {
            string account = await GetConnectedWallet();

            string walletPath = Path.Combine(Application.persistentDataPath, HyperPlayConfig.WALLETPATH);

            if (File.Exists(walletPath))
            {
                if (string.Equals(account, File.ReadAllText(walletPath), StringComparison.CurrentCultureIgnoreCase))
                {
                    return account;
                }
            }

            var key = await GetAccountViaSignature(account);

            if (key != account.ToLower().Trim())
            {
                throw new Web3Exception("Fetched address does not match the signing address.");
            }

            return account;
        }

        /// <summary>
        /// Disconnect call, currently HyperPlay doesn't support disconnecting.
        /// </summary>
        /// <returns>Completed task.</returns>
        public override Task Disconnect()
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// Make RPC request to HyperPlay desktop client.
        /// </summary>
        /// <param name="method">RPC request method name.</param>
        /// <param name="parameters">RPC request parameters.</param>
        /// <typeparam name="T">RPC request response type.</typeparam>
        /// <returns>RPC request Response.</returns>
        public override async Task<T> Perform<T>(string method, params object[] parameters)
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

            string response = Application.platform == RuntimePlatform.WebGLPlayer ? (await httpClient.PostRaw("http://localhost:8000/rpc", body, "application/json")).Response : (await httpClient.PostRaw("http://localhost:9680/rpc", body, "application/json")).Response;

            // In case response is just a primitive type like string/number...
            // Deserializing it directly doesn't work.
            if (response is T result)
            {
                return result;
            }

            try
            {
                return JsonConvert.DeserializeObject<T>(response);
            }
            catch (JsonSerializationException)
            {
                var error = JsonConvert.DeserializeObject<HyperPlayError>(response);

                throw new Web3Exception($"HyperPlay RPC request failed: {error.Message}");
            }
        }
    }
}