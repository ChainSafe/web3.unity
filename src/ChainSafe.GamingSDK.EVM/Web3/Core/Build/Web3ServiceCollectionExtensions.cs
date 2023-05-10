using System.Linq;

namespace ChainSafe.GamingWeb3.Build
{
    public static class Web3ServiceCollectionExtensions
    {
        public static void AssertServiceNotBound<T>(this IWeb3ServiceCollection services)
        {
            var assertType = typeof(T);

            if (services.Any(d => d.ServiceType == assertType))
            {
                throw new Web3BuildException($"Service of type {assertType} was already bound.");
            }
        }

        public static void AssertConfigurationNotBound<T>(this IWeb3ServiceCollection services)
        {
            var assertType = typeof(T);

            if (services.Any(d => d.ServiceType == assertType))
            {
                throw new Web3BuildException($"Configuration object of type {assertType} was already bound.");
            }
        }
    }
}