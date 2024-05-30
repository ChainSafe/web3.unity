using ChainSafe.Gaming.Common.Scripts;
using UnityEditor;
using UnityEngine;

namespace ChainSafe.Gaming.WalletConnectUnity
{
    public static class WalletConnectMenus
    {
        [MenuItem(itemName: "Assets/Create/" + MenuNames.CreateAsset + "WalletConnect/WalletConnect Wrapper Config", priority = 0)]
        public static void CreateConfigAsset()
        {
            var defaultConfig = WalletConnectUnityExtensions.LoadDefaultConfig();
            var newConfig = Object.Instantiate(defaultConfig);
            ProjectWindowUtil.CreateAsset(newConfig, "WalletConnectUnityConfig.asset");
        }
    }
}