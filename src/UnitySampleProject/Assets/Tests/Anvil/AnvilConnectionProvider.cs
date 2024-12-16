using System.Threading.Tasks;
using ChainSafe.Gaming.Evm.Signers;
using ChainSafe.Gaming.InProcessSigner;
using ChainSafe.Gaming.UnityPackage;
using ChainSafe.Gaming.UnityPackage.Connection;
using ChainSafe.Gaming.Web3.Build;
using ChainSafe.Gaming.Web3.Core;
using ChainSafe.Gaming.Web3.Core.Evm;
using ChainSafe.Gaming.Web3.Core.Logout;
using ChainSafe.Gaming.Web3.Evm.Wallet;
using Microsoft.Extensions.DependencyInjection;
using UnityEngine;
using UnityEngine.UI;

namespace ChainSafe.Gaming.Unity.Tests
{
    /// <summary>
    /// Anvil connection provider. Stub for unity tests.
    /// </summary>
    public class AnvilConnectionProvider : ConnectionProvider
    {
        // only available when in test mode
        public override bool IsAvailable => Web3Unity.TestMode;

        public override Sprite ButtonIcon { get; protected set; } = null;
        public override string ButtonText { get; protected set; } = null;

        protected override void ConfigureServices(IWeb3ServiceCollection services)
        {
            services.AddSingleton<IWalletProvider, IAccountProvider, AnvilProvider>();

            services.AddSingleton<ISigner, ILifecycleParticipant, ILogoutHandler, AnvilSigner>();

            services.AddSingleton<ITransactionExecutor, InProcessTransactionExecutor.InProcessTransactionExecutor>();
        }

        public override Task<bool> SavedSessionAvailable()
        {
            return Task.FromResult(false);
        }
    }
}