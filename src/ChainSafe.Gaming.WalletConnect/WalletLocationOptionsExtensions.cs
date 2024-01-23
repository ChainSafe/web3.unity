using System;

namespace ChainSafe.Gaming.WalletConnect
{
    public static class WalletLocationOptionsExtensions
    {
        /// <summary>
        /// Returns true if local wallets option enabled.
        /// </summary>
        /// <returns>True if local option enabled.</returns>
        public static bool LocalEnabled(this WalletLocationOption option)
        {
            return option switch
            {
                WalletLocationOption.LocalAndRemote => true,
                WalletLocationOption.OnlyLocal => true,
                WalletLocationOption.OnlyRemote => false,
                _ => throw new ArgumentOutOfRangeException(nameof(option))
            };
        }

        /// <summary>
        /// Returns true if remote wallets option enabled.
        /// </summary>
        /// <returns>True if remote option enabled.</returns>
        public static bool RemoteEnabled(this WalletLocationOption option)
        {
            return option switch
            {
                WalletLocationOption.LocalAndRemote => true,
                WalletLocationOption.OnlyLocal => false,
                WalletLocationOption.OnlyRemote => true,
                _ => throw new ArgumentOutOfRangeException(nameof(option))
            };
        }
    }
}