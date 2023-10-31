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
#if UNITY_EDITOR
                    // Having to switch to the first scene while working is a pain.
                    // Instead, we refuse to create an instance if the editor is
                    // currently running in any other scene and load the first scene
                    // instead.
                    //TODO replace indexes with Login.LoginSceneIndex
                    if (SceneManager.GetActiveScene().buildIndex > 1)
                    {
                        SceneManager.LoadScene(0);
                        // Throw exception to prevent rest of code from running
                        throw new System.Exception(
                            "Refusing to create Web3 instance since current scene is not first scene");
                    }
#endif
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
