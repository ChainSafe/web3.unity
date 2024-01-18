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
        
        public static WalletConnectConfigSO WithForceNewSession(this WalletConnectConfigSO config, bool forceNewSession)
        {
            config.ForceNewSession = forceNewSession;
            return config;
        }
    }
}