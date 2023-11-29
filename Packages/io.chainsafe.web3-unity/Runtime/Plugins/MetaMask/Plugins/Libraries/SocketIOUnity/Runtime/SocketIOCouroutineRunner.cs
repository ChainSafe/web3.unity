using System.Collections;

using UnityEngine;

namespace MetaMask.SocketIOClient
{
    public class SocketIOCouroutineRunner : MonoBehaviour
    {
        private static SocketIOCouroutineRunner instance;

        public static SocketIOCouroutineRunner Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GameObject("SocketIOCouroutineRunner").AddComponent<SocketIOCouroutineRunner>();
                    DontDestroyOnLoad(instance.gameObject);
                }

                return instance;
            }
        }

        public void RunCoroutine(IEnumerator coroutine)
        {
            StartCoroutine(coroutine);
        }

        public void EndCoroutine(IEnumerator coroutine)
        {
            StopCoroutine(coroutine);
        }
    }
}