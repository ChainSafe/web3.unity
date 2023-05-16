using ChainSafe.GamingSdk.Evm.Unity;
using ChainSafe.GamingWeb3.Environment;
using UnityEngine;

namespace ChainSafe.GamingWeb3.Unity
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
            mainThreadRunner.Enqueue(() => Debug.Log(FormatMessage(message)));
        }

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