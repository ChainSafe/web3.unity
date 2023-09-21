using System.Diagnostics.Contracts;

namespace ChainSafe.GamingSDK.EVM.Web3.Core.Extensions
{
    public static class StringExtensions
    {
        [Pure]
        public static string UnpackIfIpfs(this string uri)
        {
            if (uri.StartsWith("ipfs://"))
            {
                uri = uri.Replace("ipfs://", "https://ipfs.io/ipfs/");
            }

            return uri;
        }
    }
}