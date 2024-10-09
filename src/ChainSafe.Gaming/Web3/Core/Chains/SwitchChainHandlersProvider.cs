using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace ChainSafe.Gaming.Web3.Core.Chains
{
    // We're using this class to lazily obtain an enumeration of IChainSwitchHandlers
    public class SwitchChainHandlersProvider
    {
        private readonly IServiceProvider serviceProvider;

        public SwitchChainHandlersProvider(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public IEnumerable<IChainSwitchHandler> Handlers => serviceProvider.GetServices<IChainSwitchHandler>();
    }
}