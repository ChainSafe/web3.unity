using System;
using ChainSafe.Gaming.Web3.Environment;
using Reown.Core.Common.Logging;

namespace ChainSafe.Gaming.Reown
{
    /// <summary>
    /// Writes Reown logs to the <see cref="ILogWriter"/>.
    /// </summary>
    public class ReownLogWriter : ILogger
    {
        private const string Label = "[Reown]";

        private readonly ILogWriter logWriter;
        private readonly IReownConfig config;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReownLogWriter"/> class.
        /// </summary>
        /// <param name="logWriter">Log Writer used for logging.</param>
        public ReownLogWriter(ILogWriter logWriter, IReownConfig config)
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
            if (config.LogLevel < ReownLogLevel.Enabled)
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
            if (config.LogLevel < ReownLogLevel.ErrorOnly)
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
            if (config.LogLevel < ReownLogLevel.ErrorOnly)
            {
                return;
            }

            logWriter.LogError($"{Label} {e} {e.Message} {e.StackTrace}");
        }
    }
}