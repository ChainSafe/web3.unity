using System;
using System.Threading;
using Plugins.CountlySDK;
using UnityEngine;

public class CountlyMainThreadHandler : MonoBehaviour
{
    private static CountlyMainThreadHandler _instance;
    private Thread mainThread;
    private Action _queuedAction;
    private readonly object lockObject = new object(); // For thread safety

    public static CountlyMainThreadHandler Instance
    {
        get {
            if (_instance == null) {
                // If instance is null, add this script to the created Countly object
                GameObject gameObject = Countly.Instance.gameObject;
                _instance = gameObject.AddComponent<CountlyMainThreadHandler>();
            }
            return _instance;
        }
        internal set {
            // Allow internal setting of the instance (used during cleanup)
            _instance = value;
        }
    }

    private void Awake()
    {
        // Record the main thread when the script is first initialized
        mainThread = Thread.CurrentThread;
    }

    /// <summary>
    /// Checks if the current thread is the main thread
    /// </summary>
    /// <returns></returns>
    public bool IsMainThread()
    {
        return Thread.CurrentThread.ManagedThreadId == mainThread.ManagedThreadId;
    }

    public void RunOnMainThread(Action action)
    {
        // Check if we are on the main thread
        if (IsMainThread()) {
            // If on the main thread, invoke the action immediately
            action.Invoke();
        } else {
            // If on a different thread, queue the action to be executed on the main thread
            lock (lockObject) {
                _queuedAction = action;
            }
        }
    }

    private void Update()
    {
        // Execute any queued action on the main thread during the Unity Update phase
        if (_queuedAction != null) {
            lock (lockObject) {
                _queuedAction.Invoke();
                _queuedAction = null;
            }
        }
    }
}
