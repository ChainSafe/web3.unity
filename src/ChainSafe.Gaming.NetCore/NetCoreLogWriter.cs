using System;
using ChainSafe.Gaming.Web3.Environment;

namespace ChainSafe.Gaming.NetCore
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