using ChainSafe.GamingWeb3;
using ChainSafe.GamingWeb3.Build;

namespace ChainSafe.Gaming.Debugging
{
    public static class DebugExtensions
    {
        public static DebugBuildSubCategory Debug(this IWeb3ServiceCollection services)
        {
            return new DebugBuildSubCategory(services);
        }
    }
}