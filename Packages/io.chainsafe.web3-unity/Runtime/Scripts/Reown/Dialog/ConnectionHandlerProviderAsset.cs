using System.Threading.Tasks;
using ChainSafe.Gaming.Reown.Connection;
using UnityEngine;

namespace ChainSafe.Gaming.Reown.Dialog
{
    public abstract class ConnectionHandlerProviderAsset : ScriptableObject, IConnectionHandlerProvider
    {
        public abstract Task<IConnectionHandler> ProvideHandler();
    }
}