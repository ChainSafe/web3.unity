using ChainSafe.Gaming.Web3.Environment;
using NUnit.Framework;

namespace ChainSafe.Gaming.Tests.Core
{
    public class TestLogWriter : ILogWriter
    {
        public void Log(string message)
        {
            TestContext.Out.WriteLine(message);
        }

        public void LogError(string message)
        {
            TestContext.Error.WriteLine(message);
        }
    }
}