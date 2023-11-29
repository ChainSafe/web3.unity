using System;

namespace MetaMask.Logging
{

    /// <summary>
    /// The Meta Mask debugging utilities.
    /// </summary>
    /// <remarks>
    /// You can set the <see cref="MetaMaskDebug.Logger"/> property to an implementation of the <see cref="IMetaMaskLogger"/> interface, otherwise the logger will not log anything.
    /// </remarks>
    public static class MetaMaskDebug
    {

        public static IMetaMaskLogger Logger;

        /// <summary>
        /// Logs an information message.
        /// </summary>
        /// <param name="message">The message</param>
        public static void Log(object message)
        {
            Logger?.Log(message);
        }

        /// <summary>
        /// Logs a warning message.
        /// </summary>
        /// <param name="message">The message</param>
        public static void LogWarning(object message)
        {
            Logger?.LogWarning(message);
        }

        /// <summary>
        /// Logs an error message.
        /// </summary>
        /// <param name="message">The message</param>
        public static void LogError(object message)
        {
            Logger?.LogError(message);
        }

        /// <summary>
        /// Logs an exception.
        /// </summary>
        /// <param name="exception">The exception</param>
        public static void LogException(Exception exception)
        {
            Logger?.LogException(exception);
        }

    }
}
