using System;
using System.Collections;
using System.Collections.Generic;
using ChainSafe.Gaming.Web3.Build;
using ChainSafe.Gaming.Web3.Evm.Wallet;
using UnityEngine;

namespace ChainSafe.Gaming.HyperPlay
{
    public class HyperPlayConnectionProvider : ConnectionProvider
    {
        public override bool IsAvailable => Application.isEditor || Application.platform != RuntimePlatform.Android || Application.platform != RuntimePlatform.IPhonePlayer;

        public override Web3Builder ConfigureServices(Web3Builder web3Builder)
        {
            return web3Builder.Configure(services =>
            {
                var config = new HyperPlayConfig
                {
                    // RememberSession = rememberMeToggle.isOn || _storedSessionAvailable,
                };
#if UNITY_WEBGL && !UNITY_EDITOR
            services.UseHyperPlay<HyperPlayWebGLProvider>(config);
#else
                services.UseHyperPlay(config);
#endif
                services.UseWalletSigner().UseWalletTransactionExecutor();
            });
        }
    }
}
