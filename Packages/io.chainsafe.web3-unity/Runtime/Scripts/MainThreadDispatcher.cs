using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;

namespace Web3Unity.Scripts
{
    public class MainThreadDispatcher : MonoBehaviour
    {
        #region Instance

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void Init()
        {
            var obj = new GameObject("Main Thread Dispatcher");
            DontDestroyOnLoad(obj);
            Instance = obj.AddComponent<MainThreadDispatcher>();
        }

        public static MainThreadDispatcher Instance { get; private set; }

        #endregion

        private readonly Queue<Action> _pending = new Queue<Action>();

        public void Invoke(Action action) => _pending.Enqueue(action);

        public static void Enqueue(Action action)
        {
            if (Instance == null) return;

            Instance.Invoke(action);
        }

        private void Update()
        {
            while (_pending.TryDequeue(out var action))
            {
                try
                {
                    action();
                }

                catch (Exception e)
                {
                    Debug.LogError($"{nameof(MainThreadDispatcher)} exception:\n{e}", this);
                }
            }
        }
    }
}