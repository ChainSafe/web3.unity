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

        // Logout user
        await Web3Accessor.Web3.LogoutManager.Logout();

        // Terminate Web3
        await Web3Accessor.TerminateAndClear();

        // Go back to the first scene to log in again
        SceneManager.LoadScene(0);
    }
}
