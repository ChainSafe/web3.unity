using ChainSafe.GamingWeb3;

namespace ChainSafe.GamingSDK.EVM.Web3.Core.Debug
{
    public static class AddressExtensions
    {
        public static bool IsPublicAddress(string value)
        {
            // todo: more accurate test
            return value.Length == 42;
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