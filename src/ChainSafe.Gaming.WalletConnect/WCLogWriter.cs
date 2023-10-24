using System;
using ChainSafe.Gaming.Evm.Unity;
using ChainSafe.Gaming.Web3.Environment;
using ILogger = WalletConnectSharp.Common.Logging.ILogger;

namespace ChainSafe.Gaming.WalletConnect
{
    /// <summary>
    /// Writes Wallet Connect logs to platform.
    /// </summary>
    public class WCLogWriter : ILogger
    {
        private readonly ILogWriter logWriter;

        /// <summary>
        /// Initializes a new instance of the <see cref="WCLogWriter"/> class.
        /// </summary>
        /// <param name="logWriter">Log Writer used for logging.</param>
        public WCLogWriter(ILogWriter logWriter)
        {
            this.logWriter = logWriter;
        }

        /// <summary>
        /// Log Message.
        /// </summary>
        /// <param name="message">Message to be logged.</param>
        public void Log(string message) => logWriter.Log(message);

        /// <summary>
        /// Log Error.
        /// </summary>
        /// <param name="message">Error message to be logged.</param>
        public void LogError(string message) => logWriter.LogError(message);

        /// <summary>
        /// Log Exception as Error.
        /// </summary>
        /// <param name="e">Exception to be logged.</param>
        public void LogError(Exception e) => logWriter.LogError($"{e} {e.Message} {e.StackTrace}");
    }
}