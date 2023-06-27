using ChainSafe.Gaming.Utils;

namespace ChainSafe.Gaming.Diagnostics
{
    public static class AddressExtensions
    {
        public static string AssertIsPublicAddress(this string value, string variableName)
        {
            if (!AddressUtil.IsPublicAddress(value))
            {
                throw new Web3Exception($"\"{variableName}\" is not public address");
            }

            return value;
        }
    }
}