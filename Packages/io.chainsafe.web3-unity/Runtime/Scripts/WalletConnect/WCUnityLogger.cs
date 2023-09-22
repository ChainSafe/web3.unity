using System;
using UnityEngine;
using ILogger = WalletConnectSharp.Common.Logging.ILogger;

namespace Web3Unity.Scripts.WalletConnect
{
    public class WCUnityLogger : ILogger
    {
        public void Log(string message)
        {
            MainThreadDispatcher.Enqueue(() => Debug.Log(message));
        }

        public void LogError(string message)
        {
            MainThreadDispatcher.Enqueue(() => Debug.LogError(message));
        }

        public void LogError(Exception e)
        {
            MainThreadDispatcher.Enqueue(() => Debug.LogError(e));
        }
    }
}