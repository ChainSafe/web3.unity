using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scenes.SampleNftTexture
{
    public class LoadScene : MonoBehaviour
    {
        public void Load()
        {
            SceneManager.LoadScene(Login.LoginSceneIndex);
        }
    }
}