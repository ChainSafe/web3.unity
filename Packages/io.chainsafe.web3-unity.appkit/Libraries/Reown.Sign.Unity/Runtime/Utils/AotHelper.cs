#if !UNITY_MONO && !UNITY_2022_1_OR_NEWER
using System;
using Reown.Core.Controllers;
using Reown.Sign.Models.Engine.Methods;
using UnityEngine;
using UnityEngine.Scripting;

namespace Reown.Sign.Unity.Utils
{
    // NB! Don't attach this to any GameObjects in any scene!
    [AddComponentMenu("")]
    public class AotHelper : MonoBehaviour
    {
        [Preserve]
        private void Awake()
        {
            // Reference all required models
            // This is required so AOT code is generated for these generic functions
            var historyFactory = new JsonRpcHistoryFactory(null);
            Debug.Log(historyFactory.JsonRpcHistoryOfType<SessionPropose, SessionProposeResponse>().GetType().FullName);
            Debug.Log(historyFactory.JsonRpcHistoryOfType<SessionSettle, bool>().GetType().FullName);
            Debug.Log(historyFactory.JsonRpcHistoryOfType<SessionUpdate, bool>().GetType().FullName);
            Debug.Log(historyFactory.JsonRpcHistoryOfType<SessionExtend, bool>().GetType().FullName);
            Debug.Log(historyFactory.JsonRpcHistoryOfType<SessionDelete, bool>().GetType().FullName);
            Debug.Log(historyFactory.JsonRpcHistoryOfType<SessionPing, bool>().GetType().FullName);

            throw new InvalidOperationException("This method is only for AOT code generation.");
        }
    }
}
#endif