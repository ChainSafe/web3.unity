using UnityEngine;
using UnityEngine.SceneManagement;

public class Logout : MonoBehaviour
{
    public void OnLogout()
    {
        // Remove the saved "remember me" data, if any
        PlayerPrefs.DeleteKey(Login.PlayerAccountKey);

        // Clear the web3 instance
        Web3Accessor.Clear();

        // Go back to the first scene to log in again
        SceneManager.LoadScene(0);
    }
}
