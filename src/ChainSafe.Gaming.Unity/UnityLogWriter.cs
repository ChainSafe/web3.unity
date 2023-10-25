using ChainSafe.Gaming.Unity;
using ChainSafe.Gaming.Web3.Environment;
using UnityEngine;

namespace ChainSafe.Gaming.Web3.Unity
{
    /// <summary>
    /// This class is used so that all logs are written to Unity so that user can see the outputs of Console.WriteLine
    /// inside the Unity Editor.
    /// </summary>
    public class UnityLogWriter : ILogWriter
    {
        private readonly IMainThreadRunner mainThreadRunner;

        public UnityLogWriter(IMainThreadRunner mainThreadRunner)
        {
            this.mainThreadRunner = mainThreadRunner;
        }

        /// <summary>
        /// Logs the message to Unity's console as a regular Debug Log message.
        /// </summary>
        /// <param name="message">message to show.</param>
        public void Log(string message)
        {
            mainThreadRunner.Enqueue(() => Debug.Log(FormatMessage(message)));
        }

        /// <summary>
        /// Logs the message to Unity's console as a  Debug LogError message.
        /// </summary>
        /// <param name="message">message to show.</param>
        public void LogError(string message)
        {
            mainThreadRunner.Enqueue(() => Debug.LogError(FormatMessage(message)));
        }

        private static string FormatMessage(string message)
        {
            return $"[Web3] {message}";
        }
    }
}