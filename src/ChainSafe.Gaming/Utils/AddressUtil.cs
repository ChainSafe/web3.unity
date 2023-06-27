namespace ChainSafe.Gaming.Utils
{
    public static class AddressUtil
    {
        public static bool IsPublicAddress(string value)
        {
            // todo: more accurate test
            // todo: use Nethereum.Util.AddressUtil.Current.IsValidEthereumAddressHexFormat ??
            return value.Length == 42;
        }
    }
}