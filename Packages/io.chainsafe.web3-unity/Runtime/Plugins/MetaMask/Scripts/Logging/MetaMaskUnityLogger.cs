using System;

using MetaMask.Unity;

using UnityEngine;

using Object = System.Object;

namespace MetaMask.Logging
{
    public class MetaMaskUnityLogger : IMetaMaskLogger
    {
        protected const string Tag = "MetaMask";

        protected static MetaMaskUnityLogger instance;

        /// <summary>The event that is raised when a log message is written.</summary>
        public Action<Object> onLog;

        /// <summary>Gets the singleton instance of the logger.</summary>
        /// <returns>The singleton instance of the logger.</returns>
        public static MetaMaskUnityLogger Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new MetaMaskUnityLogger();
                }

                return instance;
            }
        }

        protected Logger logger;

        protected MetaMaskUnityLogger()
        {
            logger = new Logger(Debug.unityLogger);
            logger.filterLogType = MetaMaskConfig.DefaultInstance.Log ? LogType.Log : LogType.Assert;
        }

        /// <summary>Initializes the <see cref="MetaMaskDebug"/> class.</summary>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        protected static void Initialize()
        {
            MetaMaskDebug.Logger = Instance;
        }

        /// <summary>Logs a message to the Unity console.</summary>
        /// <param name="message">The message to log.</param>
        public void Log(object message)
        {
            logger.Log(Tag, message);
            this.onLog?.Invoke(message);
        }

        /// <summary>Logs an error message.</summary>
        /// <param name="message">The message to log.</param>
        public void LogError(object message)
        {
            logger.LogError(Tag, message);
        }

        /// <summary>Logs an exception.</summary>
        /// <param name="exception">The exception to log.</param>
        public void LogException(Exception exception)
        {
            logger.LogException(exception);
        }

        /// <summary>Logs a warning message.</summary>
        /// <param name="message">The message to log.</param>
        public void LogWarning(object message)
        {
            logger.LogWarning(Tag, message);
        }
    }
}