using System.Diagnostics;
using System.Threading;
using ChainSafe.GamingWeb3;

namespace ChainSafe.GamingSDK.EVM.Tests.Node
{
    public static class Emulator
    {
        private const string Mnemonic = "test test test test test test test test test test test junk";

        public static Process CreateInstance(uint port = 8545)
        {
            var anvilProc =
                new ProcessStartInfo("anvil", $"-m \"{Mnemonic}\" -p {port}")
                {
                    CreateNoWindow = true,
                    WindowStyle = ProcessWindowStyle.Hidden,
                    UseShellExecute = true,
                    RedirectStandardOutput = false,
                };

            var anvil = new Process()
            {
                StartInfo = anvilProc,
            };

            if (!anvil.Start())
            {
                throw new Web3Exception("Anvil is not starting");
            }

            // Wait 2 seconds since it's hard to figure out when anvil finish starting
            Thread.Sleep(2000);

            return anvil;
        }
    }
}