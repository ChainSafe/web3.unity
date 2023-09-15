using System;
using System.Diagnostics;
using System.Threading;
using ChainSafe.Gaming.Web3;

namespace ChainSafe.Gaming.Tests.Node
{
    public static class Emulator
    {
        private const string Mnemonic = "test test test test test test test test test test test junk";

        public static Process CreateInstance()
        {
            var anvilProc =
                new ProcessStartInfo("anvil", $"--mnemonic \"{Mnemonic}\" --silent")
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
            Thread.Sleep(TimeSpan.FromSeconds(2));

            return anvil;
        }
    }
}