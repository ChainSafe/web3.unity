using System.Threading.Tasks;
using ChainSafe.Gaming.UnityPackage;
using ChainSafe.Gaming.Wallets;
using Scenes;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Logout : MonoBehaviour
{
    public async void OnLogout()
    {
        // Remove the saved "remember me" data, if any
        PlayerData.Clear();
        
        // Terminate Web3
        await Web3Accessor.Web3.TerminateAsync();

        // Clear the Web3 instance
        Web3Accessor.Clear();

        // Go back to the first scene to log in again
        SceneManager.LoadScene(Login.LoginSceneIndex);
    }
}
