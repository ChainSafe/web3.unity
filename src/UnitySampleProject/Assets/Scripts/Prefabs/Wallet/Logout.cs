using UnityEngine;
using UnityEngine.SceneManagement;

public class Logout : MonoBehaviour
{
    void Start()
    {
        // Remove the saved "remember me" data, if any
        PlayerPrefs.DeleteKey("PlayerAccount");

        // Clear the web3 instance
        Web3Accessor.Instance.Clear();

        // Go back to the first scene to log in again
        SceneManager.LoadScene(0);
    }
}
