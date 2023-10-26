using System;
using ChainSafe.Gaming.Web3.Environment;

namespace ChainSafe.Gaming.NetCore
{
    /// <summary>
    /// Implementation of <see cref="ILogWriter"/> for NetCore environment.
    /// </summary>
    public class NetCoreLogWriter : ILogWriter
    {
        /// <summary>
        /// Log message.
        /// </summary>
        /// <param name="message">Message to be logged.</param>
        public void Log(string message)
        {
            Console.WriteLine(FormatMessage(message, "INFO"));
        }

        /// <summary>
        /// Log error message.
        /// </summary>
        /// <param name="message">Error message to be logged.</param>
        public void LogError(string message)
        {
            Console.WriteLine(FormatMessage(message, "ERROR"));
        }

        private static string FormatMessage(string message, string logType)
        {
            return $"[Web3][{logType}] {message}";
        }
    }
}