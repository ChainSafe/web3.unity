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
    public abstract class RestorableConnectionProvider : ConnectionProvider
    {
        [field: SerializeField] public bool RememberSession { get; private set; }
        
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

        protected abstract void ConfigureServices(IWeb3ServiceCollection services);
        
        public abstract Task<bool> SavedSessionAvailable();
    }
}
