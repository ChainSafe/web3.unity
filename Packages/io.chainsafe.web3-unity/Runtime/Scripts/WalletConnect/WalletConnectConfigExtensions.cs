﻿using ChainSafe.Gaming.UnityPackage.Connection;
using UnityEngine;

namespace ChainSafe.Gaming.WalletConnect
{
    public static class WalletConnectConfigExtensions
    {
        /// <summary>
        /// Sets <see cref="WalletConnectConnectionProvider.RememberConnection"/> property of this config object.
        /// </summary>
        /// <param name="config">The config object.</param>
        /// <param name="rememberSession">New value for RememberSession property.</param>
        /// <returns>Updated <see cref="WalletConnectConnectionProvider"/> object.</returns>
        public static WalletConnectConnectionProvider WithRememberMe(this WalletConnectConnectionProvider config, bool rememberSession)
        {
            config.RememberConnection = rememberSession;
            return config;
        }

        /// <summary>
        /// Sets <see cref="WalletConnectConnectionProvider.ForceNewSession"/> property of this config object.
        /// </summary>
        /// <param name="provider">The config object.</param>
        /// <param name="forceNewSession">New value for ForceNewSession property.</param>
        /// <returns>Updated <see cref="WalletConnectConnectionProvider"/> object.</returns>
        public static WalletConnectConnectionProvider WithForceNewSession(this WalletConnectConnectionProvider provider, bool forceNewSession)
        {
            provider.ForceNewSession = forceNewSession;
            return provider;
        }
    }
}