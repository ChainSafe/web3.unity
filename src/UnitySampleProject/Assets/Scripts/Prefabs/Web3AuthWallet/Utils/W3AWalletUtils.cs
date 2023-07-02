namespace Web3Unity.Scripts.Library.Ethers.Web3AuthWallet
{
    // class contents copied from the dll in plugins so we can modify
    public class W3AWalletUtils
    {
        public static string Account { get; set; }

        public static string PrivateKey { get; set; }

        public static string Amount { get; set; }

        public static string OutgoingContract { get; set; }

        public static string IncomingAction { get; set; }

        public static string IncomingTxData { get; set; }

        public static string IncomingMessageData { get; set; }

        public static string SignedTxResponse { get; set; }

        public static bool IncomingTx { get; set; }

        private static readonly string Host = "https://api.gaming.chainsafe.io/evm";
    }
}