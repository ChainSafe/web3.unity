
using System.Threading.Tasks;
using ChainSafe.Gaming.Evm.Unity;
using UnityEngine;
using WalletConnectSharp.Network;
using WalletConnectSharp.Network.Interfaces;

namespace ChainSafe.Gaming.WalletConnect
{
    /// <summary>
    /// This is a custom connection builder for Wallet Connect.
    /// We need this because of Unity's IL2CPP build code stripping and reachability issue.
    /// For version 2022 and above this issue has been fixed by Unity as stated here https://blog.unity.com/engine-platform/il2cpp-full-generic-sharing-in-unity-2022-1-beta so this custom implementation isn't needed.
    /// </summary>
    public class NativeWebSocketConnectionBuilder : MonoBehaviour, IConnectionBuilder
    {
        /// <summary>
        /// Create WebSocket connection for Wallet Connect.
        /// </summary>
        /// <param name="url"></param>
        /// <returns>Created connection.</returns>
        public Task<IJsonRpcConnection> CreateConnection(string url)
        {
            return Task.FromResult<IJsonRpcConnection>(new WebSocketConnection(url));
        }
    }
}
