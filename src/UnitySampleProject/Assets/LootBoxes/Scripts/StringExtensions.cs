public static class StringExtensions
{
    public static string UnpackUriIfIpfs(this string originalUri)
    {
        if (!originalUri.StartsWith("ipfs://"))
            return originalUri;

        return originalUri.Replace("ipfs://", "https://ipfs.io/ipfs/");
    }
}