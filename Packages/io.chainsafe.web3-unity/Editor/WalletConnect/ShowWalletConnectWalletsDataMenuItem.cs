using ChainSafe.Gaming.WalletConnect.Wallets;
using UnityEditor;
using UnityEngine;

namespace ChainSafe.Gaming.WalletConnect.Editor
{
    public static class ShowWalletConnectWalletsDataMenuItem
    {
        [MenuItem("Window/ChainSafe SDK/Wallet Connect/Show Wallets Data")]
        public static void ShowWalletsData()
        {
            Application.OpenURL(WalletRegistry.DefaultRegistryUri);
        }
    }
}