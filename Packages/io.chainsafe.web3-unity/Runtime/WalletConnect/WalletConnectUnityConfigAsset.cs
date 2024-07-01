using ChainSafe.Gaming.Common.Scripts;
using UnityEngine;
using WalletConnectUnity.Modal;

namespace ChainSafe.Gaming.WalletConnectUnity
{
    public class WalletConnectUnityConfigAsset : ScriptableObject, IWalletConnectUnityConfig
    {
        // [field:SerializeField] public bool ShouldSpawnModal { get; private set; }
        //
        // [field:SerializeField] public WalletConnectModal ModalPrefab { get; private set; }
        
        [field:SerializeField] public string[] IncludedWalletIds { get; private set; }
        
        [field:SerializeField] public string[] ExcludedWalletIds { get; private set; }
        
        [field:SerializeField] public string SignMessageRpcMethodName { get; private set; }
        
        [field:SerializeField] public string SignTypedMessageRpcMethodName { get; private set; }
    }
}