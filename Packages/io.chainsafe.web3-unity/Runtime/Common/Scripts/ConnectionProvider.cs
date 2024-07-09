using System;
using System.Collections;
using System.Collections.Generic;
using ChainSafe.Gaming.UnityPackage.Common;
using ChainSafe.Gaming.Web3.Build;
using Scenes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ChainSafe.Gaming
{
    public abstract class ConnectionProvider : MonoBehaviour, IWeb3BuilderServiceAdapter
    {
        public abstract bool IsAvailable { get; }
        
        [field: SerializeField] public Button ConnectButton { get; private set; }
        
        public abstract Web3Builder ConfigureServices(Web3Builder web3Builder);
    }
}
