using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChainSafe.Gaming.Web3.Build;
using Microsoft.Extensions.DependencyInjection;

namespace ChainSafe.Gaming.Evm.Contracts
{
    /// <summary>
    /// Extension methods for the <see cref="IWeb3ServiceCollection"/> .
    /// </summary>
    public static class ContractBuilderExtensions
    {
        /// <summary>
        /// Registers and configures contracts.
        /// </summary>
        /// <param name="services">The <c>IWeb3ServiceCollection</c> instance.</param>
        /// <param name="configure">An action used to configure the <c>ContractBuilderConfig</c>.</param>
        /// <returns>The <c>IWeb3ServiceCollection</c> instance that method was invoked on. This return value can be used to chain multiple calls for a fluent configuration syntax.</returns>
        public static IWeb3ServiceCollection ConfigureRegisteredContracts(this IWeb3ServiceCollection services, Action<ContractBuilderConfig> configure)
        {
            ContractBuilderConfig config =
                services.FirstOrDefault(s => s.ServiceType == typeof(ContractBuilderConfig))
                    ?.ImplementationInstance as ContractBuilderConfig;

            if (config == null)
            {
                config = new();
                services.AddSingleton(config);
            }

            configure(config);

            return services;
        }
    }
}
