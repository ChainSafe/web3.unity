using System;
using ChainSafe.GamingWeb3.Environment;

namespace Web3Unity.Scripts.Library.Ethers.NetCore
{
    public class NetCoreLogWriter : ILogWriter
    {
        public void Log(string message)
        {
            Console.WriteLine(FormatMessage(message, "INFO"));
        }

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