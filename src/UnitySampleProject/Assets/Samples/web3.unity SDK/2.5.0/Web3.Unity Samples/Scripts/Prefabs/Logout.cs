using System.Threading.Tasks;
using ChainSafe.Gaming.UnityPackage;
using ChainSafe.Gaming.Wallets;
using ChainSafe.Gaming.UnityPackage.Common;
using Scenes;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Logout : MonoBehaviour
{
    public async void OnLogout()
    {
        // Logout & Terminate Web3
        await Web3Accessor.TerminateAndClear(logout: true);

        // Go back to the first scene to log in again
        await SceneManager.LoadSceneAsync(LoadSceneOnLogin.LoginSceneBuildIndex);
    }
}
