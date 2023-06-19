using System;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_WEBGL
// todo: can be removed
public class WebLogin : MonoBehaviour
{
    // [DllImport("__Internal")]
    // private static extern void Web3Connect();
    //
    // [DllImport("__Internal")]
    // private static extern string ConnectAccount();
    //
    // [DllImport("__Internal")]
    // private static extern void SetConnectAccount(string value);
    //
    // private int expirationTime;
    // private string account;
    // ProjectConfigScriptableObject projectConfigSO = null;
    
    void Start()
    {
        // // loads the data saved from the editor config
        // projectConfigSO = ProjectConfigUtilities.Load();
        // PlayerPrefs.SetString("ProjectID", projectConfigSO.ProjectId);
        // PlayerPrefs.SetString("ChainID", projectConfigSO.ChainId);
        // PlayerPrefs.SetString("Chain", projectConfigSO.Chain);
        // PlayerPrefs.SetString("Network", projectConfigSO.Network);
        // PlayerPrefs.SetString("RPC", projectConfigSO.Rpc);
    }

    public void OnLogin()
    {
        // Web3Connect();
        // OnConnected();
    }

    async private void OnConnected()
    {
        // account = ConnectAccount();
        // while (account == "")
        // {
        //     await new WaitForSeconds(1f);
        //     account = ConnectAccount();
        // };
        // // save account for next scene
        // PlayerPrefs.SetString("Account", account);
        // // reset login message
        // SetConnectAccount("");
        // // load next scene
        // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void OnSkip()
    {
        // // burner account for skipped sign in screen
        // PlayerPrefs.SetString("Account", "");
        // // move to next scene
        // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
#endif
