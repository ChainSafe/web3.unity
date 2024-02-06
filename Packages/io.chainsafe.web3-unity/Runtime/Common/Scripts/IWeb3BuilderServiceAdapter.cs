using ChainSafe.Gaming.Web3.Build;

namespace ChainSafe.Gaming.UnityPackage.Common
{
    /// <summary>
    /// Provides services for building a <see cref="Web3"/> instance.
    /// </summary>
    public interface IWeb3BuilderServiceAdapter
    {
        /// <summary>
        /// Configures services for building a <see cref="Web3"/> instance.
        /// </summary>
        /// <param name="web3Builder">Builder object for a <see cref="Web3"/> instance.</param>
        /// <returns>Builder with services configured to it.</returns>
        public Web3Builder ConfigureServices(Web3Builder web3Builder);
    }
}