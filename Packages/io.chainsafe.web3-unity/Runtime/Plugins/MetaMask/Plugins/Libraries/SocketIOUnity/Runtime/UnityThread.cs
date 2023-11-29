// https://stackoverflow.com/questions/41330771/use-unity-api-from-another-thread-or-call-a-function-in-the-main-thread

#define ENABLE_UPDATE_FUNCTION_CALLBACK
#define ENABLE_LATEUPDATE_FUNCTION_CALLBACK
#define ENABLE_FIXEDUPDATE_FUNCTION_CALLBACK

using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace MetaMask.SocketIOClient
{
    public class UnityThread : MonoBehaviour
    {
        //our (singleton) instance
        private static UnityThread instance = null;


        ////////////////////////////////////////////////UPDATE IMPL////////////////////////////////////////////////////////
        //Holds actions received from another Thread. Will be coped to actionCopiedQueueUpdateFunc then executed from there
        private static List<System.Action> actionQueuesUpdateFunc = new List<Action>();

        //holds Actions copied from actionQueuesUpdateFunc to be executed
        private List<System.Action> actionCopiedQueueUpdateFunc = new List<System.Action>();

        // Used to know if whe have new Action function to execute. This prevents the use of the lock keyword every frame
        private volatile static bool noActionQueueToExecuteUpdateFunc = true;


        ////////////////////////////////////////////////LATEUPDATE IMPL////////////////////////////////////////////////////////
        //Holds actions received from another Thread. Will be coped to actionCopiedQueueLateUpdateFunc then executed from there
        private static List<System.Action> actionQueuesLateUpdateFunc = new List<Action>();

        //holds Actions copied from actionQueuesLateUpdateFunc to be executed
        private List<System.Action> actionCopiedQueueLateUpdateFunc = new List<System.Action>();

        // Used to know if whe have new Action function to execute. This prevents the use of the lock keyword every frame
        private volatile static bool noActionQueueToExecuteLateUpdateFunc = true;



        ////////////////////////////////////////////////FIXEDUPDATE IMPL////////////////////////////////////////////////////////
        //Holds actions received from another Thread. Will be coped to actionCopiedQueueFixedUpdateFunc then executed from there
        private static List<System.Action> actionQueuesFixedUpdateFunc = new List<Action>();

        //holds Actions copied from actionQueuesFixedUpdateFunc to be executed
        private List<System.Action> actionCopiedQueueFixedUpdateFunc = new List<System.Action>();

        // Used to know if whe have new Action function to execute. This prevents the use of the lock keyword every frame
        private volatile static bool noActionQueueToExecuteFixedUpdateFunc = true;


        //Used to initialize UnityThread. Call once before any function here
        public static void initUnityThread(bool visible = false)
        {
            if (instance != null)
            {
                return;
            }

            if (Application.isPlaying)
            {
                // add an invisible game object to the scene
                GameObject obj = new GameObject("MainThreadExecuter");
                if (!visible)
                {
                    obj.hideFlags = HideFlags.HideAndDontSave;
                }

                DontDestroyOnLoad(obj);
                instance = obj.AddComponent<UnityThread>();
            }
        }

        public void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        //////////////////////////////////////////////COROUTINE IMPL//////////////////////////////////////////////////////
#if (ENABLE_UPDATE_FUNCTION_CALLBACK)
        public static void executeCoroutine(IEnumerator action)
        {
            if (instance != null)
            {
                executeInUpdate(() => instance.StartCoroutine(action));
            }
        }

        ////////////////////////////////////////////UPDATE IMPL////////////////////////////////////////////////////
        public static void executeInUpdate(System.Action action)
        {
            if (action == null)
            {
                throw new ArgumentNullException("action");
            }

            lock (actionQueuesUpdateFunc)
            {
                actionQueuesUpdateFunc.Add(action);
                noActionQueueToExecuteUpdateFunc = false;
            }
        }

        public void Update()
        {
            if (noActionQueueToExecuteUpdateFunc)
            {
                return;
            }

            //Clear the old actions from the actionCopiedQueueUpdateFunc queue
            this.actionCopiedQueueUpdateFunc.Clear();
            lock (actionQueuesUpdateFunc)
            {
                //Copy actionQueuesUpdateFunc to the actionCopiedQueueUpdateFunc variable
                this.actionCopiedQueueUpdateFunc.AddRange(actionQueuesUpdateFunc);
                //Now clear the actionQueuesUpdateFunc since we've done copying it
                actionQueuesUpdateFunc.Clear();
                noActionQueueToExecuteUpdateFunc = true;
            }

            // Loop and execute the functions from the actionCopiedQueueUpdateFunc
            for (int i = 0; i < this.actionCopiedQueueUpdateFunc.Count; i++)
            {
                this.actionCopiedQueueUpdateFunc[i].Invoke();
            }
        }
#endif

        ////////////////////////////////////////////LATEUPDATE IMPL////////////////////////////////////////////////////
#if (ENABLE_LATEUPDATE_FUNCTION_CALLBACK)
        public static void executeInLateUpdate(System.Action action)
        {
            if (action == null)
            {
                throw new ArgumentNullException("action");
            }

            lock (actionQueuesLateUpdateFunc)
            {
                actionQueuesLateUpdateFunc.Add(action);
                noActionQueueToExecuteLateUpdateFunc = false;
            }
        }


        public void LateUpdate()
        {
            if (noActionQueueToExecuteLateUpdateFunc)
            {
                return;
            }

            //Clear the old actions from the actionCopiedQueueLateUpdateFunc queue
            this.actionCopiedQueueLateUpdateFunc.Clear();
            lock (actionQueuesLateUpdateFunc)
            {
                //Copy actionQueuesLateUpdateFunc to the actionCopiedQueueLateUpdateFunc variable
                this.actionCopiedQueueLateUpdateFunc.AddRange(actionQueuesLateUpdateFunc);
                //Now clear the actionQueuesLateUpdateFunc since we've done copying it
                actionQueuesLateUpdateFunc.Clear();
                noActionQueueToExecuteLateUpdateFunc = true;
            }

            // Loop and execute the functions from the actionCopiedQueueLateUpdateFunc
            for (int i = 0; i < this.actionCopiedQueueLateUpdateFunc.Count; i++)
            {
                this.actionCopiedQueueLateUpdateFunc[i].Invoke();
            }
        }
#endif

        ////////////////////////////////////////////FIXEDUPDATE IMPL//////////////////////////////////////////////////
#if (ENABLE_FIXEDUPDATE_FUNCTION_CALLBACK)
        public static void executeInFixedUpdate(System.Action action)
        {
            if (action == null)
            {
                throw new ArgumentNullException("action");
            }

            lock (actionQueuesFixedUpdateFunc)
            {
                actionQueuesFixedUpdateFunc.Add(action);
                noActionQueueToExecuteFixedUpdateFunc = false;
            }
        }

        public void FixedUpdate()
        {
            if (noActionQueueToExecuteFixedUpdateFunc)
            {
                return;
            }

            //Clear the old actions from the actionCopiedQueueFixedUpdateFunc queue
            this.actionCopiedQueueFixedUpdateFunc.Clear();
            lock (actionQueuesFixedUpdateFunc)
            {
                //Copy actionQueuesFixedUpdateFunc to the actionCopiedQueueFixedUpdateFunc variable
                this.actionCopiedQueueFixedUpdateFunc.AddRange(actionQueuesFixedUpdateFunc);
                //Now clear the actionQueuesFixedUpdateFunc since we've done copying it
                actionQueuesFixedUpdateFunc.Clear();
                noActionQueueToExecuteFixedUpdateFunc = true;
            }

            // Loop and execute the functions from the actionCopiedQueueFixedUpdateFunc
            for (int i = 0; i < this.actionCopiedQueueFixedUpdateFunc.Count; i++)
            {
                this.actionCopiedQueueFixedUpdateFunc[i].Invoke();
            }
        }
#endif

        public void OnDisable()
        {
            if (instance == this)
            {
                instance = null;
            }
        }
    }
}