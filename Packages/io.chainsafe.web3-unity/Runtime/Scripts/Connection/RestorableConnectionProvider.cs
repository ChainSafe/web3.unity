using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChainSafe.Gaming.LocalStorage;
using ChainSafe.Gaming.UnityPackage;
using ChainSafe.Gaming.UnityPackage.Connection;
using ChainSafe.Gaming.Web3.Build;

namespace ChainSafe.Gaming
{
    public abstract class RestorableConnectionProvider : ConnectionProvider
    {
        public override Web3Builder ConfigureServices(Web3Builder web3Builder)
        {
            return web3Builder.Configure(services =>
            {
                services.AddSingleton<IStorable, IWeb3InitializedHandler, StoredConnectionProviderData>(
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
    
    public static class RestorableConnectionProviderExtensions
    {
        public static async Task<RestorableConnectionProvider> GetProvider(this IEnumerable<RestorableConnectionProvider> providers)
        {
            var data = new StoredConnectionProviderData();

            await data.LoadOneTime();

            return providers.SingleOrDefault(p => p.GetType() == data.Type);
        }
    }
}
