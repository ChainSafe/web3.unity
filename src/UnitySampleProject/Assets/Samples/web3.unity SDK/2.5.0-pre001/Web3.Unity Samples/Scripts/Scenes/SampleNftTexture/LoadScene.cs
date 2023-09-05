using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scenes.SampleNftTexture
{
    public class LoadScene : MonoBehaviour
    {
        public string SceneName = "SampleMain";

        public void Load()
        {
            SceneManager.LoadScene(SceneName);
        }
    }
}