using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChainSafe.Gaming.LocalStorage;
using ChainSafe.Gaming.UnityPackage;
using ChainSafe.Gaming.UnityPackage.Connection;
using ChainSafe.Gaming.Web3.Build;
using ChainSafe.Gaming.Web3.Core.Logout;
using UnityEngine;

namespace ChainSafe.Gaming
{
    /// <summary>
    /// Connection provider that can be restored from a saved session.
    /// </summary>
    public abstract class RestorableConnectionProvider : ConnectionProvider
    {
        [field: SerializeField, Tooltip("Should this connection provider remember a previous session.")]
        public bool RememberSession { get; private set; } = true;
        
        public override Web3Builder ConfigureServices(Web3Builder web3Builder)
        {
            return web3Builder.Configure(services =>
            {
                services.AddSingleton<IStorable, IWeb3InitializedHandler, ILogoutHandler, StoredConnectionProviderData>(
                    _ => new StoredConnectionProviderData
                    {
                        TypeName = GetType().AssemblyQualifiedName
                    });

                ConfigureServices(services);
            });
        }

        /// <summary>
        /// Configure services for the connection provider.
        /// </summary>
        /// <param name="services">Service collection to add services to.</param>
        protected abstract void ConfigureServices(IWeb3ServiceCollection services);
        
        /// <summary>
        /// Check if a saved session is available.
        /// </summary>
        /// <returns>True if a saved session is available.</returns>
        public abstract Task<bool> SavedSessionAvailable();
    }
}
