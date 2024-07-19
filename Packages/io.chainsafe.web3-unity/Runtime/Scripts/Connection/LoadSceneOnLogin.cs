using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ChainSafe.Gaming.UnityPackage.Connection
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

        public Task OnWeb3Initialized()
        {
            LoginSceneBuildIndex = SceneManager.GetActiveScene().buildIndex;

            SceneManager.LoadSceneAsync(sceneToLoad);

            return Task.CompletedTask;
        }
    }
}
