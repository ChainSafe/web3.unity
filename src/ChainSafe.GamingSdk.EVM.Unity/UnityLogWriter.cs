using ChainSafe.GamingSdk.Evm.Unity;
using ChainSafe.GamingWeb3.Environment;
using UnityEngine;

namespace ChainSafe.GamingWeb3.Unity
{
    public class UnityLogWriter : ILogWriter
    {
        private readonly IMainThreadRunner _mainThreadRunner;

        public UnityLogWriter(IMainThreadRunner mainThreadRunner)
        {
            _mainThreadRunner = mainThreadRunner;
        }

        public void Log(string message)
        {
            _mainThreadRunner.Enqueue(() => Debug.Log(FormatMessage(message)));
        }

        public void LogError(string message)
        {
            _mainThreadRunner.Enqueue(() => Debug.LogError(FormatMessage(message)));
        }

        private static string FormatMessage(string message)
        {
            return $"[Web3] {message}";
        }
    }
}