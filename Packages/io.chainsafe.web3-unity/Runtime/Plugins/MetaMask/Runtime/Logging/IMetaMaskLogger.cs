using System;

namespace MetaMask.Logging
{

    /// <summary>
    /// Provides an interface for loggers.
    /// </summary>
    public interface IMetaMaskLogger
    {

        /// <summary>
        /// Logs an information message.
        /// </summary>
        /// <param name="message">The message</param>
        void Log(object message);

        /// <summary>
        /// Logs a warning message.
        /// </summary>
        /// <param name="message">The message</param>
        void LogWarning(object message);

        /// <summary>
        /// Logs an error message.
        /// </summary>
        /// <param name="message">The message</param>
        void LogError(object message);

        /// <summary>
        /// Logs an exception.
        /// </summary>
        /// <param name="exception">The exception</param>
        void LogException(Exception exception);

    }
}
