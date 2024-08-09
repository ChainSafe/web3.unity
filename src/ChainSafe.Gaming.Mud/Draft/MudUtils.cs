using Nethereum.Mud.EncodingDecoding;

namespace ChainSafe.Gaming.Mud.Draft
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
    }
}