using Nethereum.Mud.EncodingDecoding;

namespace ChainSafe.Gaming.Mud
{
    public static class MudUtils
    {
        public static byte[] TableResourceId(string @namespace, string tableName, bool isOffChain = false)
        {
            var trimmedName = ResourceEncoder.TrimNameAsValidSize(tableName);

            return isOffChain
                ? ResourceEncoder.EncodeOffchainTable(@namespace, trimmedName)
                : ResourceEncoder.EncodeTable(@namespace, trimmedName);
        }

        public static string NamespaceFunctionName(string @namespace, string function)
        {
            return !function.StartsWith($"{@namespace}__")
                ? $"{@namespace}__{function}"
                : function; // already contains namespace
        }
    }
}