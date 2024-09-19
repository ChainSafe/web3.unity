using ChainSafe.Gaming.Web3.Build;
using UnityEngine;

namespace ChainSafe.Gaming.UnityPackage.Connection
{
    public abstract class Web3BuilderServiceAdapter : MonoBehaviour, IWeb3BuilderServiceAdapter
    {
        public abstract Web3Builder ConfigureServices(Web3Builder web3Builder);
    }
}
