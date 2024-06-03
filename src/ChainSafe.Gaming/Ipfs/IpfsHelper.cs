namespace ChainSafe.Gaming.Ipfs
{
    public static class IpfsHelper
    {
        private const string BaseUri = "https://ipfs.io/ipfs/";

        public static string RollupIpfsUri(string ipfsUri)
        {
            if (!ipfsUri.StartsWith("ipfs://"))
            {
                return ipfsUri;
            }

            return ipfsUri.Replace("ipfs://", BaseUri);
        }

        public static bool CanDecodeTokenIdToUri(string tokenId)
        {
            return tokenId.StartsWith("0x");
        }

        public static string DecodeTokenIdToUri(string tokenId)
        {
            var decodedId = tokenId.Replace("0x", "f");
            return $"{BaseUri}{decodedId}";
        }
    }
}