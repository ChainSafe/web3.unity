using System;
using ChainSafe.Gaming.Evm.Unity;
using UnityEngine;
using ILogger = WalletConnectSharp.Common.Logging.ILogger;

namespace Web3Unity.Scripts.WalletConnect
{
    public class WCUnityLogger : ILogger
    {
        public void Log(string message)
        {
            Dispatcher.Instance()?.Enqueue(() => Debug.Log(message));
        }

        public void LogError(string message)
        {
            Dispatcher.Instance()?.Enqueue(() => Debug.LogError(message));
        }

        public void LogError(Exception e)
        {
            Dispatcher.Instance()?.Enqueue(() => Debug.LogError(e));
        }
    }
}