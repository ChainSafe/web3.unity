using ChainSafe.Gaming.Environment;
using ChainSafe.Gaming.Unity.Threading;

namespace ChainSafe.Gaming.Unity.Environment
{
    public class UnityLogWriter : ILogWriter
    {
        private readonly IMainThreadRunner mainThreadRunner;

        public UnityLogWriter(IMainThreadRunner mainThreadRunner)
        {
            this.mainThreadRunner = mainThreadRunner;
        }

        public void Log(string message)
        {
            mainThreadRunner.Enqueue(() => UnityEngine.Debug.Log(FormatMessage(message)));
        }

        public void LogError(string message)
        {
            mainThreadRunner.Enqueue(() => UnityEngine.Debug.LogError(FormatMessage(message)));
        }

        private static string FormatMessage(string message)
        {
            return $"[Web3] {message}";
        }
    }
}