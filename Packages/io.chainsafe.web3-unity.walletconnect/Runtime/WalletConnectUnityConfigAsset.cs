using ChainSafe.Gaming.Common.Scripts;
using UnityEngine;

namespace ChainSafe.Gaming.WalletConnectUnity
{
    [CreateAssetMenu(menuName = MenuNames.CreateAsset + "WalletConnectUnityConfig", fileName = "WalletConnectUnityConfig", order = 0)]
    public class WalletConnectUnityConfigAsset : ScriptableObject, IWalletConnectUnityConfig
    {
        [field:SerializeField] public string[] IncludedWalletIds { get; private set; }
        
        [field:SerializeField] public string[] ExcludedWalletIds { get; private set; }
    }
}