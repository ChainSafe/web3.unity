using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using CWeb3 = ChainSafe.Gaming.Web3.Web3;

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

        public int Priority => 0;

        public async Task OnWeb3Initialized(CWeb3 web3)
        {
            LoginSceneBuildIndex = SceneManager.GetActiveScene().buildIndex;

            await SceneManager.LoadSceneAsync(sceneToLoad);
        }
    }
}
