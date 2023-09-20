using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;

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

    private readonly ConcurrentQueue<Action> _pending = new ConcurrentQueue<Action>();

    public void Invoke(Action fn) => _pending.Enqueue(fn);

    public static void Enqueue(Action a)
    {
        if (Instance == null) return;

        Instance.Invoke(a);
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
                Debug.LogError(
                    $"An error has occurred during processing one of the queued actions in the main thread dispatcher:\n{e}",
                    this);
            }
        }
    }
}