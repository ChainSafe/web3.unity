using UnityEngine;
using UnityEngine.SceneManagement;

namespace ChainSafe.Gaming.UnityPackage
{
    public class Web3Accessor : MonoBehaviour
    {
        static Web3Accessor instance;

        Web3.Web3 web3;

        static Web3Accessor Instance
        {
            get
            {
                if (!instance)
                {
                    var go = new GameObject("Web3Accessor");
                    DontDestroyOnLoad(go);
                    instance = go.AddComponent<Web3Accessor>();
                }

                return instance;
            }
        }

        public static Web3.Web3 Web3 => Instance.web3;

        public static Web3.Web3 TryWeb3 => instance ? instance.web3 : null;

        public static void Set(Web3.Web3 web3)
        {
            if (Instance.web3 != null)
            {
                throw new System.Exception("Web3 instance was already initialized");
            }

            Instance.web3 = web3;
        }

        public static void Clear() => Instance.web3 = null;
    }
}
