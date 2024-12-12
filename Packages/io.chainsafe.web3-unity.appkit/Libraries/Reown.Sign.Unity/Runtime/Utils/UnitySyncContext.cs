using System.Threading;
using UnityEngine;

namespace Reown.Sign.Unity.Utils
{
    public sealed class UnitySyncContext : MonoBehaviour
    {
        public static SynchronizationContext Context { get; private set; }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialize()
        {
            if (Context != null)
                return;
            // var go = new GameObject("[Reown] UnitySyncContext")
            // {
            //     hideFlags = HideFlags.HideAndDontSave
            // };

            var go = new GameObject("[Reown] UnitySyncContext");
            go.AddComponent<UnitySyncContext>();
            DontDestroyOnLoad(go);

            InitializeSyncContext();
        }

        private static void InitializeSyncContext()
        {
            var currentSyncContext = SynchronizationContext.Current;
            if (currentSyncContext == null || currentSyncContext.GetType().FullName != "UnityEngine.UnitySynchronizationContext")
            {
                Debug.LogError($"SynchronizationContext is not of type UnityEngine.UnitySynchronizationContext. Current type is {(currentSyncContext != null ? currentSyncContext.GetType().FullName : "null")}. This might cause issues in multithreaded operations.");
            }

            Context = currentSyncContext;
        }
    }
}