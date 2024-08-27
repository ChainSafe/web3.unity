using System;
using System.Threading.Tasks;
using ChainSafe.Gaming.Web3.Build;
using UnityEngine;
using UnityEngine.UI;

namespace ChainSafe.Gaming.UnityPackage.Connection
{
    /// <summary>
    /// Connection Provider Scriptable Object.
    /// </summary>
    public abstract class ConnectionProvider : ScriptableObject, IWeb3BuilderServiceAdapter
    {
        /// <summary>
        /// Is provider available for connection.
        /// Could be platform specific or other conditions.
        /// </summary>
        public abstract bool IsAvailable { get; }
        
        /// <summary>
        /// Button to connect to the wallet.
        /// </summary>
        [field: SerializeField] public Button ConnectButtonRow { get; private set; }
        
        /// <summary>
        /// Initialize Connection provider.
        /// </summary>
        /// <returns>Awaitable Task.</returns>
        public abstract Task Initialize();
        
        /// <summary>
        /// Configure services for the Web3 instance.
        /// This is where you add wallet connection services to <see cref="Web3Builder"/>.
        /// </summary>
        /// <param name="web3Builder"></param>
        /// <returns></returns>
        public abstract Web3Builder ConfigureServices(Web3Builder web3Builder);

        /// <summary>
        /// Handle exception thrown during connection.
        /// Different providers might handler it differently.
        /// </summary>
        /// <param name="exception">Exception thrown during connection.</param>
        /// <exception cref="Exception">Exception thrown during connection.</exception>
        public virtual void HandleException(Exception exception)
        {
            throw exception;
        }
    }
}
