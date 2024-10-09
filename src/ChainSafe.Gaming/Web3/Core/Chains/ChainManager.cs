using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ChainSafe.Gaming.NetCore;
using ChainSafe.Gaming.Web3.Environment;

namespace ChainSafe.Gaming.Web3.Core.Chains
{
    public class ChainManager : IChainManager
    {
        private readonly Dictionary<string, IChainConfig> configs;
        private readonly ILogWriter logWriter;
        private readonly SwitchChainHandlersProvider switchHandlersProvider;

        public ChainManager(IChainConfigSet configSet, SwitchChainHandlersProvider switchHandlersProvider, ILogWriter logWriter)
        {
            // Referencing IEnumerable<IChainSwitchHandler> in ChainManager causes a deadlock, so we use SwitchChainHandlersProvider
            this.switchHandlersProvider = switchHandlersProvider;
            this.logWriter = logWriter;
            Current = configSet.Configs.First();

            // build configs map
            try
            {
                configs = configSet.Configs.ToDictionary(config => config.ChainId, config => config.Clone()); // cloning all the configs
            }
            catch (ArgumentException)
            {
                logWriter.LogError("There are 2 or more Chain Configs with the same Chain ID. " +
                                   "Please make sure to remove the duplicates that you don't need. " +
                                   "Using first occurrences for now..");

                var usedIds = configSet.Configs.Select(config => config.ChainId).Distinct().ToArray();
                var filteredConfigs =
                    usedIds.Select(chainId => configSet.Configs.First(config => config.ChainId == chainId));

                configs = filteredConfigs.ToDictionary(config => config.ChainId, config => config.Clone()); // cloning all the configs
            }
        }

        public event Action<IChainConfig> ChainSwitched;

        public IChainConfig Current { get; private set; }

        public bool IsSwitching { get; private set; }

        public async Task SwitchChain(string newChainId) // todo add cancellation token as an argument and use it
        {
            if (IsSwitching)
            {
                throw new InvalidOperationException(
                    "Can't switch chain. The last chain switching procedure has not yet finished.");
            }

            if (newChainId == Current.ChainId)
            {
                logWriter.Log("Tried to switch to the chain id that's currently active. Ignoring...");
                return;
            }

            IsSwitching = true;

            try
            {
                var previousChainId = Current.ChainId;

                if (!configs.TryGetValue(newChainId, out var newChainConfig))
                {
                    throw new ArgumentException($"No {nameof(IChainConfig)} was registered with id '{newChainId}'. " +
                                                "Make sure to configure settings for the provided chain before switching to it.");
                }

                Current = newChainConfig;
                var succeededHandlers = new Stack<IChainSwitchHandler>();

                try
                {
                    foreach (var switchHandler in switchHandlersProvider.Handlers)
                    {
                        await switchHandler.HandleChainSwitching();
                        succeededHandlers.Push(switchHandler);
                    }
                }
                catch (Exception switchException)
                {
                    // revert everything
                    Current = configs[previousChainId];

                    while (succeededHandlers.Count != 0)
                    {
                        var handlerToRevert = succeededHandlers.Pop();

                        try
                        {
                            await handlerToRevert.HandleChainSwitching();
                        }
                        catch (Exception revertException)
                        {
                            logWriter.LogError(
                                $"Error occured while reverting handler {handlerToRevert.GetType().Name}. " +
                                $"Proceeding with revert.\n{revertException}");
                        }
                    }

                    throw new Web3Exception(
                        $"One of the handlers thrown an exception. Reverted {nameof(ChainManager)} to the previous chain configuration.",
                        switchException);
                }

                logWriter.Log($"Successfully switched to the chain with id '{newChainId}'.");

                try
                {
                    ChainSwitched?.Invoke(Current);
                }
                catch (Exception e)
                {
                    logWriter.LogError(e.ToString());
                }
            }
            finally
            {
                IsSwitching = false;
            }
        }

        public void AddChainConfig(IChainConfig newConfig)
        {
            if (configs.ContainsKey(newConfig.ChainId))
            {
                throw new Web3Exception(
                    "Couldn't add Chain Config. A Chain Config with the same Chain Id is already present in the map.");
            }

            configs[newConfig.ChainId] = newConfig;
        }
    }
}