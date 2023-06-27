using System;

namespace ChainSafe.GamingSDK.EVM.Web3AuthWallet
{
    public static class W3AUtils
    {
        public static string? SavedUserAddress { get; set; } = null;

        public static TimeSpan ConnectRequestExpiresAfter { get; set; } = TimeSpan.FromMinutes(1);

        public static string? Account { get; set; } = string.Empty;

        public static string? Amount { get; set; } = string.Empty;

        public static string? OutgoingContract { get; set; } = string.Empty;

        public static string? IncomingAction { get; set; } = string.Empty;

        public static string? IncomingTxData { get; set; } = string.Empty;

        public static string? IncomingMessageData { get; set; } = string.Empty;

        public static string? SignedTxResponse { get; set; } = string.Empty;

        public static string? Transaction { get; set; }

        public static bool IncomingTx { get; set; } = false;
    }
}