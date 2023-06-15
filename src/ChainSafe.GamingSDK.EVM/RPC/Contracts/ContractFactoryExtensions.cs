using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChainSafe.GamingWeb3.Build;
using Microsoft.Extensions.DependencyInjection;

namespace Web3Unity.Scripts.Library.Ethers.Contracts
{
    public static class ContractFactoryExtensions
    {
        public static IWeb3ServiceCollection ConfigureRegisteredContracts(this IWeb3ServiceCollection services, Action<ContractFactoryConfig> configure)
        {
            ContractFactoryConfig config =
                services.FirstOrDefault(s => s.ServiceType == typeof(ContractFactoryConfig))
                    ?.ImplementationInstance as ContractFactoryConfig;

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
