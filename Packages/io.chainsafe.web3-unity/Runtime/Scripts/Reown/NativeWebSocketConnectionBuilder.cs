using System.Threading.Tasks;
using Reown.Core.Network;
using Reown.Core.Network.Interfaces;
using UnityEngine;

namespace ChainSafe.Gaming.Reown
{
    /// <summary>
    /// This is a custom connection builder for Reown.
    /// We need this because of Unity's IL2CPP build code stripping and reachability issue.
    /// For version 2022 and above this issue has been fixed by Unity as stated here https://blog.unity.com/engine-platform/il2cpp-full-generic-sharing-in-unity-2022-1-beta so this custom implementation isn't needed.
    /// </summary>
    public class NativeWebSocketConnectionBuilder : MonoBehaviour, IConnectionBuilder
    {
        /// <summary>
        /// Create WebSocket connection for Reown.
        /// </summary>
        /// <param name="url"></param>
        /// <returns>Created connection.</returns>
        public Task<IJsonRpcConnection> CreateConnection(string url)
        {
            return Task.FromResult<IJsonRpcConnection>(new WebSocketConnection(url));
        }
    }
}
