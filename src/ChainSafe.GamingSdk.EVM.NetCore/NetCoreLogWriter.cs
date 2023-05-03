using System;
using ChainSafe.GamingWeb3.Logger;

namespace ChainSafe.GamingWeb3.Evm.NetCore
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