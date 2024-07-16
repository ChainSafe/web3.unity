using System;
using System.Threading.Tasks;
using ChainSafe.Gaming.UnityPackage.Common;
using ChainSafe.Gaming.Web3.Build;
using UnityEngine;
using UnityEngine.UI;

namespace ChainSafe.Gaming
{
    public abstract class ConnectionProvider : MonoBehaviour, IWeb3BuilderServiceAdapter
    {
        public abstract bool IsAvailable { get; }
        
        [field: SerializeField] public Button ConnectButton { get; private set; }
        
        public abstract Task Initialize();
        
        public abstract Web3Builder ConfigureServices(Web3Builder web3Builder);

        public virtual void HandleException(Exception exception)
        {
            throw exception;
        }
    }
}
