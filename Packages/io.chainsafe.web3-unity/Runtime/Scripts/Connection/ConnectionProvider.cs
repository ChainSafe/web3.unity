using System;
using System.Threading.Tasks;
using ChainSafe.Gaming.LocalStorage;
using ChainSafe.Gaming.Web3.Build;
using ChainSafe.Gaming.Web3.Core.Logout;
using UnityEngine;
using UnityEngine.UI;

namespace ChainSafe.Gaming.UnityPackage.Connection
{
    /// <summary>
    /// Connection Provider Scriptable Object.
    /// </summary>
    public abstract class ConnectionProvider : ScriptableObject, IServiceAdapter
    {
        /// <summary>
        /// Is provider available for connection.
        /// Could be platform specific or other conditions.
        /// </summary>
        public abstract bool IsAvailable { get; }

        /// <summary>
        /// Button to connect to the wallet.
        /// </summary>
        public abstract Button ConnectButtonRow { get; protected set; }

        protected bool RememberSession { get; private set; }

        /// <summary>
        /// Initialize Connection provider.
        /// </summary>
        /// <param name="rememberSession">Remember current session on next login (RememberMe).</param>
        /// <returns>Awaitable Task.</returns>
        public virtual Task Initialize(bool rememberSession)
        {
            RememberSession = rememberSession;

            return Task.CompletedTask;
        }

        /// <summary>
        /// Configure services for the Web3 instance.
        /// This is where you add wallet connection services to <see cref="Web3Builder"/>.
        /// </summary>
        /// <param name="web3Builder"></param>
        /// <returns></returns>
        public Web3Builder ConfigureServices(Web3Builder web3Builder)
        {
            return web3Builder.Configure(services =>
            {
                if (RememberSession)
                {
                    services.AddSingleton<IStorable, IWeb3InitializedHandler, ILogoutHandler, StoredConnectionProviderData>(
                        _ => new StoredConnectionProviderData
                        {
                            TypeName = GetType().AssemblyQualifiedName
                        });
                }

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

        /// <summary>
        /// Handle exception thrown during connection.
        /// Different providers might handler it differently.
        /// It can also serve as a Reset for any failed connection attempts.
        /// </summary>
        /// <param name="exception">Exception thrown during connection.</param>
        /// <exception cref="Exception">Exception thrown during connection.</exception>
        public virtual void HandleException(Exception exception)
        {
            throw exception;
        }
    }
}
