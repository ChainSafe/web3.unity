#nullable enable
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChainSafe.Gaming.Evm.Network;
using ChainSafe.Gaming.Web3.Environment;

namespace ChainSafe.Gaming.Evm
{
    public class ChainRegistryProvider
    {
        private const string FetchUrl = "https://chainid.network/chains.json";

        private readonly Web3Environment environment;
        private Dictionary<ulong, Chains.Chain>? chains;

        public ChainRegistryProvider(Web3Environment environment)
        {
            this.environment = environment;
        }

        public async ValueTask<Chains.Chain?> GetChain(ulong chainId)
        {
            chains ??= await LoadAllChains();
            return chains.TryGetValue(chainId, out Chains.Chain value) ? value : null;
        }

        private async ValueTask<Dictionary<ulong, Chains.Chain>> LoadAllChains()
        {
            var httpClient = environment.HttpClient;
            var response = await httpClient.Get<Chains.Chain[]>(FetchUrl);
            return response.EnsureResponse().ToDictionary(chain => chain.ChainId, chain => chain);
        }
    }
}