using System;
using ChainSafe.Gaming.Web3.Environment;
using ILogger = WalletConnectSharp.Common.Logging.ILogger;

namespace ChainSafe.Gaming.WalletConnect
{
    /// <summary>
    /// Writes Wallet Connect logs to the <see cref="ILogWriter"/>.
    /// </summary>
    public class WCLogWriter : ILogger
    {
        private const string Label = "[WalletConnect SDK]";

        private readonly ILogWriter logWriter;
        private readonly IWalletConnectConfig config;

        /// <summary>
        /// Initializes a new instance of the <see cref="WCLogWriter"/> class.
        /// </summary>
        /// <param name="logWriter">Log Writer used for logging.</param>
        public WCLogWriter(ILogWriter logWriter, IWalletConnectConfig config)
        {
            this.config = config;
            this.logWriter = logWriter;
        }

        /// <summary>
        /// Log Message.
        /// </summary>
        /// <param name="message">Message to be logged.</param>
        public void Log(string message)
        {
            if (config.LogLevel < WalletConnectLogLevel.Enabled)
            {
                return;
            }

            logWriter.Log($"{Label} {message}");
        }

        /// <summary>
        /// Log Error.
        /// </summary>
        /// <param name="message">Error message to be logged.</param>
        public void LogError(string message)
        {
            if (config.LogLevel < WalletConnectLogLevel.ErrorOnly)
            {
                return;
            }

            logWriter.LogError($"{Label} {message}");
        }

        /// <summary>
        /// Log Exception as Error.
        /// </summary>
        /// <param name="e">Exception to be logged.</param>
        public void LogError(Exception e)
        {
            if (config.LogLevel < WalletConnectLogLevel.ErrorOnly)
            {
                return;
            }

            logWriter.LogError($"{Label} {e} {e.Message} {e.StackTrace}");
        }
    }
}