using ChainSafe.Gaming.Web3.Core;

namespace ChainSafe.Gaming.Web3.Core.Debug
{
    public static class AddressExtensions
    {
        public static bool IsPublicAddress(string value)
        {
            // TODO: more accurate test/Regex
            return !string.IsNullOrEmpty(value) && value.Length == 42;
        }

        public static string AssertIsPublicAddress(this string value, string variableName)
        {
            if (!IsPublicAddress(value))
            {
                throw new Web3Exception($"\"{variableName}\" is not public address");
            }

            return value;
        }
    }
}