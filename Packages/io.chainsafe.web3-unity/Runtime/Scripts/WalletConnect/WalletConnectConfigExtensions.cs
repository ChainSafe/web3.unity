using UnityEngine;

namespace ChainSafe.Gaming.WalletConnect
{
    public static class WalletConnectConfigExtensions
    {
        public static WalletConnectConfigSO WithRememberSession(this WalletConnectConfigSO config, bool rememberSession)
        {
            config.RememberSession = rememberSession;
            return config;
        }
    }
}