using System;
using System.Collections.Generic;
using ChainSafe.Gaming.WalletConnect.Models;
using WalletConnectSharp.Core;
using WalletConnectSharp.Sign.Models;
using WalletConnectSharp.Sign.Models.Engine;

namespace ChainSafe.Gaming.WalletConnect
{
    [Serializable]
    public class WalletConnectConfig
    {
        public delegate void Connected(ConnectedData connectedData);

        public delegate void SessionApproved(SessionStruct session);

        public event Connected OnConnected;

        public event SessionApproved OnSessionApproved;

        public string SavedUserAddress { get; set; } = null;

        public string ProjectId { get; set; }

        public string ProjectName { get; set; }

        public string BaseContext { get; set; }

        public ChainModel Chain { get; set; }

        public Metadata Metadata { get; set; }

        public bool RedirectToWallet { get; set; }

        public WalletConnectWalletModel DefaultWallet { get; set; }

        public Dictionary<string, WalletConnectWalletModel> SupportedWallets { get; set; }

        public bool Testing { get; set; } = false;

        public string TestResponse { get; set; } = string.Empty;

        public void InvokeConnected(ConnectedData connectedData)
        {
            OnConnected?.Invoke(connectedData);
        }

        public void InvokeSessionApproved(SessionStruct session)
        {
            OnSessionApproved?.Invoke(session);
        }
    }
}