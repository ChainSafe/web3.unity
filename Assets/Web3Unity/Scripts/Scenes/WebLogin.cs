using System;
using System.Runtime.InteropServices;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine;

#if UNITY_WEBGL
public class WebLogin : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void Web3Connect();

    [DllImport("__Internal")]
    private static extern string ConnectAccount();

    [DllImport("__Internal")]
    private static extern void SetConnectAccount(string value);

    private int expirationTime;
    private string account;
    private string path = "Assets/Editor/ServerSettings/webglconfig.txt";

    void Awake()
    {
        // Read values from file
        StreamReader sr = new StreamReader(path);
        for (int i = 0; !sr.EndOfStream; i++) {
            string line = sr.ReadLine ();
            string[] Splitted = line.Split(new char[] {' '}, System.StringSplitOptions.RemoveEmptyEntries);
            PlayerPrefs.SetString("ProjectID", Splitted [0]);
            PlayerPrefs.SetString("RPC", Splitted [1]);
        }
    }

    public void OnLogin()
    {
        Web3Connect();
        OnConnected();
    }

    async private void OnConnected()
    {
        account = ConnectAccount();
        while (account == "")
        {
            await new WaitForSeconds(1f);
            account = ConnectAccount();
        };
        // save account for next scene
        PlayerPrefs.SetString("Account", account);
        // reset login message
        SetConnectAccount("");
        // load next scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void OnSkip()
    {
        // burner account for skipped sign in screen
        PlayerPrefs.SetString("Account", "");
        // move to next scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
#endif