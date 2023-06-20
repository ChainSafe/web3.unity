namespace ChainSafe.GamingSDK.EVM.Web3.Core.Debug
{
    public static class ObjectExtensions
    {
        public static T AssertNotNull<T>(this T obj, string variableName)
            where T : class
        {
            if (obj is null)
            {
                throw new AssertionException($"{variableName} is null.");
            }

            return obj;
        }
    }
}