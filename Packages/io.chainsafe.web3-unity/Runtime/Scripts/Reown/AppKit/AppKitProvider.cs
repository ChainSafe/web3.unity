using System;
using System.Linq;
using System.Threading.Tasks;
using ChainSafe.Gaming.Web3;
using ChainSafe.Gaming.Web3.Core;
using ChainSafe.Gaming.Web3.Core.Chains;
using ChainSafe.Gaming.Web3.Core.Operations;
using ChainSafe.Gaming.Web3.Environment;
using ChainSafe.Gaming.Web3.Evm.Wallet;
using Reown.AppKit.Unity;
using UnityEngine;

namespace ChainSafe.Gaming.Reown.AppKit
{
    using W3AppKit = global::Reown.AppKit.Unity.AppKit;
    public class AppKitProvider : WalletProvider, IAppKitProvider, ILifecycleParticipant, IConnectionHelper
    {
        public W3AppKit AppKit { get; set; }
        public IReownConfig ReownConfig { get; set; }

        private readonly IChainConfigSet _chains;
        private readonly ILogWriter _logWriter;
        private bool initialized;
        
        public AppKitProvider(IChainConfigSet chains, IReownConfig reownConfig, ILogWriter logWriter, Web3Environment web3Environment, IChainConfig chainConfig, IOperationTracker operationTracker) : base(web3Environment, chainConfig, operationTracker)
        {
            _chains = chains;
            _logWriter = logWriter;
            ReownConfig = reownConfig;
            Initalize();
        }

        private async void Initalize()
        {
            try
            {
                var appKitConfig = new AppKitConfig()
                {
                    projectId = ReownConfig.ProjectId,
                    metadata = new Metadata(ReownConfig.Metadata.Name, ReownConfig.Metadata.Description, ReownConfig.Metadata.Url, ReownConfig.Metadata.Icons[0], new RedirectData()
                    {
                        Native = ReownConfig.Metadata.Redirect.Native,
                        Universal = ReownConfig.Metadata.Redirect.Universal,
                    }),
                    supportedChains = GetSupportedChains(),
                };

                AppKit = Resources.Load<AppKitCore>("Reown AppKit");
                _logWriter.Log($"App kit != null {AppKit != null}");
                await W3AppKit.InitializeAsync(appKitConfig);
                
            }
            catch (Exception e)
            {
                throw; // TODO handle exception
            }
        }

        private Chain[] GetSupportedChains()
        {
        
            var allChains = _chains.Configs.Select(x => new Chain(ChainConstants.Namespaces.Evm, x.ChainId, x.Chain, new Currency(x.)))
        }

        public ValueTask WillStartAsync()
        {
            W3AppKit.OpenModal();
            return default;
        }

        public ValueTask WillStopAsync()
        {
            return default;
        }

        public bool StoredSessionAvailable => false;
        
        public override Task<string> Connect()
        {
            
        }

        public override Task Disconnect()
        {
        }

        public override Task<T> Request<T>(string method, params object[] parameters)
        {
        }
    }

}