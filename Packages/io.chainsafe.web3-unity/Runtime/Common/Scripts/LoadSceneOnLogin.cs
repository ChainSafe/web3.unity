using ChainSafe.Gaming.UnityPackage.Common;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ChainSafe.Gaming.UnityPackage.Common
{
    public class LoadSceneOnLogin : MonoBehaviour, IWeb3InitializedHandler
    {
        public static int LoginSceneBuildIndex { get; private set; }
    
        [SerializeField] private string sceneToLoad;
    
        public void OnWeb3Initialized()
        {
            LoginSceneBuildIndex = SceneManager.GetActiveScene().buildIndex;
        
            SceneManager.LoadSceneAsync(sceneToLoad);
        }
    }
}
