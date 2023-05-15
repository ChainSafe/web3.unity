#nullable enable
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChainSafe.GamingWeb3.Environment;
using Web3Unity.Scripts.Library.Ethers.Network;

namespace Web3Unity.Scripts.Library.Ethers
{
    public class ChainProvider
    {
        private const string FetchUrl = "https://chainid.network/chains.json";

        private readonly Web3Environment _environment;
        private Dictionary<ulong, Chains.Chain>? _chains;

        public ChainProvider(Web3Environment environment)
        {
            _environment = environment;
        }

        public async ValueTask<Chains.Chain?> GetChain(ulong chainId)
        {
            _chains ??= await LoadAllChains();
            return _chains.ContainsKey(chainId) ? _chains[chainId] : null;
        }

        private async ValueTask<Dictionary<ulong, Chains.Chain>> LoadAllChains()
        {
            var httpClient = _environment.HttpClient;
            var response = await httpClient.Get<Chains.Chain[]>(FetchUrl);
            return response.ToDictionary(chain => chain.ChainId, chain => chain);
        }
    }
}