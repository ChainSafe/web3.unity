namespace ChainSafe.Gaming.Debugging
{
    using ChainSafe.Gaming.Web3.Build;

    public static class DebugExtensions
    {
        public static DebugBuildSubCategory Debug(this IWeb3ServiceCollection services)
        {
            return new DebugBuildSubCategory(services);
        }
    }
}