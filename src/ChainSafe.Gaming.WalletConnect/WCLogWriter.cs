using System;
using ChainSafe.Gaming.Evm.Unity;
using ChainSafe.Gaming.Web3.Environment;
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

        public void Log(string message) => logWriter.Log(message);

        public void LogError(string message) => logWriter.LogError(message);

        public void LogError(Exception e) => logWriter.LogError($"{e} {e.Message} {e.StackTrace}");
    }
}