namespace ChainSafe.Gaming.Web3.Core.Debug
{
    public static class ObjectExtensions
    {
#nullable enable
        public static T AssertNotNull<T>(this T? obj, string variableName)
            where T : notnull
        {
            if (obj is null)
            {
                throw new AssertionException($"{variableName} is null.");
            }

            return obj;
        }
#nullable disable
    }
}