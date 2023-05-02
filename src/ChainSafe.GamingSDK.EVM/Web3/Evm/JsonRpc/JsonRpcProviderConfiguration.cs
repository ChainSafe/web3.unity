using System;

namespace ChainSafe.GamingWeb3.Evm.Providers
{
  [Serializable]
  public class JsonRpcProviderConfiguration
  {
    /// <summary>
    /// (Optional) Url of RPC Node 
    /// </summary>
    public string RpcNodeUrl;
    
    /// <summary>
    /// (Optional) Network to operate on 
    /// </summary>
    public Network Network;
  }
}