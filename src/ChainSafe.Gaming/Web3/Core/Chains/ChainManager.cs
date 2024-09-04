using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChainSafe.Gaming.Web3.Environment;

namespace ChainSafe.Gaming.Web3.Core.Chains
{
    public class ChainManager : IChainManager
    {
        private readonly Dictionary<string, IChainConfig> configs;
        private readonly IList<IChainSwitchHandler> switchHandlers;
        private readonly ILogWriter logWriter;

        public ChainManager(IChainConfigSet configs, IList<IChainSwitchHandler> switchHandlers, ILogWriter logWriter)
        {
            this.logWriter = logWriter;
            this.configs = configs.ToDictionary(config => config.ChainId, config => config);
            this.switchHandlers = switchHandlers;
            Current = configs.First();
        }

        public event Action<IChainConfig> ChainSwitched;

        public IChainConfig Current { get; private set; }

        public async Task SwitchChain(string newChainId)
        {
            if (!configs.TryGetValue(newChainId, out var newChainConfig))
            {
                throw new Web3Exception($"No {nameof(IChainConfig)} was registered with id '{newChainId}'. Make sure to configure settings for the chain before switching to it.");
            }

            Current = newChainConfig;
            var previousChainId = Current.ChainId;
            var succeededHandlers = new Stack<IChainSwitchHandler>();

            try
            {
                foreach (var switchHandler in switchHandlers)
                {
                    await switchHandler.HandleChainSwitch(Current);
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
                        await handlerToRevert.HandleChainSwitch(Current);
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
    }
}