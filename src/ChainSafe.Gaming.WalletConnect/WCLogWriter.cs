using System;
using ChainSafe.Gaming.Evm.Unity;
using ChainSafe.Gaming.Web3.Environment;
using UnityEngine;
using ILogger = WalletConnectSharp.Common.Logging.ILogger;

namespace ChainSafe.Gaming.WalletConnect
{
    public class WCLogWriter : ILogger
    {
        private ILogWriter _logWriter;

        public WCLogWriter(ILogWriter logWriter)
        {
            _logWriter = logWriter;
        }

        public void Log(string message)
        {
            Dispatcher.Instance()?.Enqueue(() => _logWriter.Log(message));
        }

        public void LogError(string message)
        {
            Dispatcher.Instance()?.Enqueue(() => _logWriter.LogError(message));
        }

        public void LogError(Exception e)
        {
            Dispatcher.Instance()?.Enqueue(() => _logWriter.LogError($"{e} {e.Message} {e.StackTrace}"));
        }
    }
}