#nullable enable
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChainSafe.GamingWeb3.Environment;
using ChainSafe.GamingWeb3.Evm;

namespace ChainSafe.GamingWeb3.Evm
{
  public class ChainProvider
  {
    private const string FetchUrl = "https://chainid.network/chains.json";

    private readonly IWeb3Environment _environment;
    private Dictionary<ulong, Chain>? _chains;

    public ChainProvider(IWeb3Environment environment)
    {
      _environment = environment;
    }
    
    public async ValueTask<Chain?> GetChain(ulong chainId)
    {
      _chains ??= await LoadAllChains();
      return _chains.ContainsKey(chainId) ? _chains[chainId] : null;
    }

    private async ValueTask<Dictionary<ulong, Chain>> LoadAllChains()
    {
      var httpClient = _environment.HttpClient;
      var response = await httpClient.Get<Chain[]>(FetchUrl);
      return response.ToDictionary(chain => chain.ChainId, chain => chain);
    }
  }
}