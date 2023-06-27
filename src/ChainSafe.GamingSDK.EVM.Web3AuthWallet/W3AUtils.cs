using System;

namespace ChainSafe.GamingSDK.EVM.Web3AuthWallet
{
    public class W3AUtils
    {
        public string? SavedUserAddress { get; set; } = null;

        public TimeSpan ConnectRequestExpiresAfter { get; set; } = TimeSpan.FromMinutes(1);

        public string? Account { get; set; } = string.Empty;

        public string? Amount { get; set; } = string.Empty;

        public string? OutgoingContract { get; set; } = string.Empty;

        public string? IncomingAction { get; set; } = string.Empty;

        public string? IncomingTxData { get; set; } = string.Empty;

        public string? IncomingMessageData { get; set; } = string.Empty;

        public string? SignedTxResponse { get; set; } = string.Empty;

        public bool IncomingTx { get; set; } = false;

        public string Host { get; } = "https://api.gaming.chainsafe.io/evm";
    }
}