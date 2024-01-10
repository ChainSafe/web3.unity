using System;

namespace ChainSafe.Gaming.WalletConnect
{
    public static class WalletLocationOptionsExtensions
    {
        public static bool LocalEnabled(this WalletLocationOptions options)
        {
            return options switch
            {
                WalletLocationOptions.LocalAndRemote => true,
                WalletLocationOptions.OnlyLocal => true,
                WalletLocationOptions.OnlyRemote => false,
                _ => throw new ArgumentOutOfRangeException(nameof(options))
            };
        }

        public static bool RemoteEnabled(this WalletLocationOptions options)
        {
            return options switch
            {
                WalletLocationOptions.LocalAndRemote => true,
                WalletLocationOptions.OnlyLocal => false,
                WalletLocationOptions.OnlyRemote => true,
                _ => throw new ArgumentOutOfRangeException(nameof(options))
            };
        }
    }
}