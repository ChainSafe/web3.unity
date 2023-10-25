using ChainSafe.Gaming.UnityPackage;
using Scenes;
using UnityEngine;
using UnityEngine.SceneManagement;

/* This prefab script should be copied & placed on the root of an object.
Change the class name and add any additional changes in the logout function.
The log out function should be called by a method of your choosing */

/// <summary>
/// Logs a user out and resets the web3 instance
/// </summary>
public class Logout : MonoBehaviour
{
    /// <summary>
    /// Starts the task, you can put this in the start function or call it from a button/event
    /// </summary>
    public async void OnLogout()
    {
        // Remove the saved "remember me" data, if any
        PlayerPrefs.DeleteKey(Login.SavedWalletConnectConfigKey);

        // Terminate Web3
        await Web3Accessor.Web3.TerminateAsync();

        // Clear the Web3 instance
        Web3Accessor.Clear();

        // Go back to the first scene to log in again
        SceneManager.LoadScene(0);
    }
}
