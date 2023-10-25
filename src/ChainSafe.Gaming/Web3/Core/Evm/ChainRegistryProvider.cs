#nullable enable
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChainSafe.Gaming.Evm.Network;
using ChainSafe.Gaming.Web3.Environment;

namespace ChainSafe.Gaming.Evm
{
    /// <summary>
    /// Provides access to a registry of chain info by their chain IDs.
    /// </summary>
    public class ChainRegistryProvider
    {
        private const string FetchUrl = "https://chainid.network/chains.json";

        private readonly IHttpClient httpClient;

        private Dictionary<ulong, Chains.Chain>? chains;

        public ChainRegistryProvider(IHttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        /// <summary>
        /// Asynchronously retrieves a blockchain chain by its chain ID.
        /// </summary>
        /// <param name="chainId">The chain ID to retrieve.</param>
        /// <returns>Requested chain info.</returns>
        // TODO: refactor to load all chains on WillStartAsync, make this method sync
        public async ValueTask<Chains.Chain?> GetChain(ulong chainId)
        {
            chains ??= await LoadAllChains();
            return chains.TryGetValue(chainId, out Chains.Chain value) ? value : null;
        }

        private async ValueTask<Dictionary<ulong, Chains.Chain>> LoadAllChains()
        {
            var response = await httpClient.Get<Chains.Chain[]>(FetchUrl);
            return response.AssertSuccess().ToDictionary(chain => chain.ChainId, chain => chain);
        }
    }
}