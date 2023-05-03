using ChainSafe.GamingWeb3.Build;
using ChainSafe.GamingWeb3.Evm;
using ChainSafe.GamingWeb3.Evm.Migration;
using ChainSafe.GamingWeb3.Evm.Providers;
using ChainSafe.GamingWeb3.Evm.Signers;
using ChainSafe.GamingWeb3.Unity;

namespace ChainSafe.GamingSdk.EVM.Unity.Migration
{
  public static class MigrationHelper
  {
    public static JsonRpcProvider NewJsonRpcProvider(string url = "", Network network = null)
    {
      return MigrationHelperBase.NewJsonRpcProvider(url, network, BindEnvironment);
    }

    public static JsonRpcWallet GetSigner(this JsonRpcProvider provider, int index = 0)
    {
      return MigrationHelperBase.GetSigner(provider, index, BindEnvironment);
    }

    private static void BindEnvironment(IWeb3ServiceCollection services)
    {
      services.UseUnityEnvironment();
    }
  }
}