using System;
using ChainSafe.Gaming.Evm.Unity;
using ChainSafe.Gaming.Web3.Environment;
using UnityEngine;
using ILogger = WalletConnectSharp.Common.Logging.ILogger;

namespace ChainSafe.Gaming.WalletConnect
{
    public class WCLogWriter : ILogger
    {
        private readonly ILogWriter logWriter;

        public WCLogWriter(ILogWriter logWriter)
        {
            this.logWriter = logWriter;
        }

        public void Log(string message)
        {
            Dispatcher.Instance()?.Enqueue(() => logWriter.Log(message));
        }

        public void LogError(string message)
        {
            Dispatcher.Instance()?.Enqueue(() => logWriter.LogError(message));
        }

        public void LogError(Exception e)
        {
            Dispatcher.Instance()?.Enqueue(() => logWriter.LogError($"{e} {e.Message} {e.StackTrace}"));
        }
    }
}