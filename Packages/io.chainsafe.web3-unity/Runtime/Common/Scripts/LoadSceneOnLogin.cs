using ChainSafe.Gaming.UnityPackage.Common;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ChainSafe.Gaming.UnityPackage.Common
{
    /// <summary>
    /// Loads scene when Web3 Instance is initialized.
    /// </summary>
    public class LoadSceneOnLogin : MonoBehaviour, IWeb3InitializedHandler
    {
        /// <summary>
        /// Login scene cached/saved for Logout since we can have more than one Login scene.
        /// </summary>
        public static int LoginSceneBuildIndex { get; private set; }

        [SerializeField] private string sceneToLoad;

        public void OnWeb3Initialized()
        {
            LoginSceneBuildIndex = SceneManager.GetActiveScene().buildIndex;

            SceneManager.LoadSceneAsync(sceneToLoad);
        }
    }
}
