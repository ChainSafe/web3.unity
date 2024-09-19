using ChainSafe.Gaming.Web3.Build;
using UnityEngine;

namespace ChainSafe.Gaming.UnityPackage.Connection
{
    /// <summary>
    /// A MonoBehaviour implementation of <see cref="IWeb3BuilderServiceAdapter"/>.
    /// If your MonoBehaviour can't be attached to the GameObject containing <see cref="Web3Unity"/> then inherit from this class instead of implementing <see cref="IWeb3ServiceCollection"/>.
    /// </summary>
    public abstract class Web3BuilderServiceAdapter : MonoBehaviour, IWeb3BuilderServiceAdapter
    {
        public abstract Web3Builder ConfigureServices(Web3Builder web3Builder);
    }
}
