using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

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

        public static void AddSingleton<TInterface1, TInterface2, TImplementation>(this IWeb3ServiceCollection serviceCollection)
            where TInterface1 : class
            where TInterface2 : class
            where TImplementation : class, TInterface1, TInterface2
        {
            /* Note: this approach adds the implementation type itself
             * as a service. This is not side effect-free. If this approach
             * turns out to cause trouble, we can replace it with named
             * services which is more difficult to implement but completely
             * side effect-free.
             */

            serviceCollection.AddSingleton<TImplementation>();
            serviceCollection.AddSingleton<TInterface1, TImplementation>(sp => sp.GetRequiredService<TImplementation>());
            serviceCollection.AddSingleton<TInterface2, TImplementation>(sp => sp.GetRequiredService<TImplementation>());
        }

        public static void AddSingleton<TInterface1, TInterface2, TImplementation>(this IWeb3ServiceCollection serviceCollection, Func<IServiceProvider, TImplementation> implementationFactory)
            where TInterface1 : class
            where TInterface2 : class
            where TImplementation : class, TInterface1, TInterface2
        {
            serviceCollection.AddSingleton(implementationFactory);
            serviceCollection.AddSingleton<TInterface1, TImplementation>(sp => sp.GetRequiredService<TImplementation>());
            serviceCollection.AddSingleton<TInterface2, TImplementation>(sp => sp.GetRequiredService<TImplementation>());
        }

        public static void AddSingleton<TInterface1, TInterface2, TInterface3, TImplementation>(this IWeb3ServiceCollection serviceCollection)
            where TInterface1 : class
            where TInterface2 : class
            where TInterface3 : class
            where TImplementation : class, TInterface1, TInterface2, TInterface3
        {
            serviceCollection.AddSingleton<TImplementation>();
            serviceCollection.AddSingleton<TInterface1, TImplementation>(sp => sp.GetRequiredService<TImplementation>());
            serviceCollection.AddSingleton<TInterface2, TImplementation>(sp => sp.GetRequiredService<TImplementation>());
            serviceCollection.AddSingleton<TInterface3, TImplementation>(sp => sp.GetRequiredService<TImplementation>());
        }

        public static void AddSingleton<TInterface1, TInterface2, TInterface3, TImplementation>(this IWeb3ServiceCollection serviceCollection, Func<IServiceProvider, TImplementation> implementationFactory)
            where TInterface1 : class
            where TInterface2 : class
            where TInterface3 : class
            where TImplementation : class, TInterface1, TInterface2, TInterface3
        {
            serviceCollection.AddSingleton(implementationFactory);
            serviceCollection.AddSingleton<TInterface1, TImplementation>(sp => sp.GetRequiredService<TImplementation>());
            serviceCollection.AddSingleton<TInterface2, TImplementation>(sp => sp.GetRequiredService<TImplementation>());
            serviceCollection.AddSingleton<TInterface3, TImplementation>(sp => sp.GetRequiredService<TImplementation>());
        }
    }
}