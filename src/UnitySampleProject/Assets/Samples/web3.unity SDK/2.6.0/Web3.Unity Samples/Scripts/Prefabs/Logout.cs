using ChainSafe.Gaming.UnityPackage;
using ChainSafe.Gaming.UnityPackage.Connection;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Logout : MonoBehaviour
{
    public async void OnLogout()
    {
        // Logout & Terminate Web3
        await Web3Unity.Instance.Disconnect();

        // Go back to the first scene to log in again
        await SceneManager.LoadSceneAsync(LoadSceneOnLogin.LoginSceneBuildIndex);
    }
}
