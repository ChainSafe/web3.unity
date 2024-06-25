using System;
using UnityEngine;
using UnityEngine.Scripting;
using WalletConnectSharp.Core.Controllers;
using WalletConnectSharp.Events;
using WalletConnectSharp.Events.Model;
using WalletConnectSharp.Sign.Models.Engine.Methods;

namespace ChainSafe.Gaming.WalletConnect
{
    [Preserve]
    internal class AotConfiguration
    {
#if !UNITY_MONO
        // todo add info code stripping to docs: link.xml should be edited
        // todo hide this method in the package somewhere
        [Preserve]
        void SetupAOT()
        {
            // Reference all required models
            // This is required so AOT code is generated for these generic functions
            var historyFactory = new JsonRpcHistoryFactory(null);
            Debug.Log(historyFactory.JsonRpcHistoryOfType<SessionPropose, SessionProposeResponse>().GetType().FullName);
            Debug.Log(historyFactory.JsonRpcHistoryOfType<SessionSettle, Boolean>().GetType().FullName);
            Debug.Log(historyFactory.JsonRpcHistoryOfType<SessionUpdate, Boolean>().GetType().FullName);
            Debug.Log(historyFactory.JsonRpcHistoryOfType<SessionExtend, Boolean>().GetType().FullName);
            Debug.Log(historyFactory.JsonRpcHistoryOfType<SessionDelete, Boolean>().GetType().FullName);
            Debug.Log(historyFactory.JsonRpcHistoryOfType<SessionPing, Boolean>().GetType().FullName);
            EventManager<string, GenericEvent<string>>.InstanceOf(null).PropagateEvent(null, null);
            throw new InvalidOperationException("This method is only for AOT code generation.");
        }
#endif
    }
}