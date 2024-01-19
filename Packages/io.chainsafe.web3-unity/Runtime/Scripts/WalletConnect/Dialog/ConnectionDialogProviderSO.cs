﻿using System.Threading.Tasks;
using ChainSafe.Gaming.WalletConnect.Connection;
using UnityEngine;

namespace ChainSafe.Gaming.WalletConnect.Dialog
{
    public abstract class ConnectionDialogProviderSO : ScriptableObject, IConnectionHandlerProvider
    {
        public abstract Task<IConnectionHandler> ProvideHandler();
    }
}