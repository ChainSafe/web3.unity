using System.Threading.Tasks;
using ChainSafe.Gaming.UnityPackage;
using ChainSafe.Gaming.Wallets;
using Scenes;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Logout : MonoBehaviour
{
    private bool _quitting;
    
    public async void OnLogout()
    {
        // Remove the saved "remember me" data, if any
        PlayerPrefs.DeleteKey(Login.PlayerAccountKey);

        await TerminateAndClearWeb3();

        // Go back to the first scene to log in again
        SceneManager.LoadScene(0);
    }

    private async Task TerminateAndClearWeb3()
    {
        // Terminate Web3
        await Web3Accessor.Web3.TerminateAsync();

        // Clear the Web3 instance
        Web3Accessor.Clear();
    }
    
    private void OnApplicationQuit()
    {
        Debug.Log("Disconnecting wallet...");

        Task.Run(TerminateAndClearWeb3);
    }
}
