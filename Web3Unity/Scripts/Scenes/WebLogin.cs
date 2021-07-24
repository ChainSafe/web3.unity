using System;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_WEBGL
public class WebLogin : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void Web3Login();

    [DllImport("__Internal")]
    private static extern string LoginMessage();

    [DllImport("__Internal")]
    private static extern void ResetLoginMessage();

    private float elapsed = 0f;

    void Update()
    {
        // check every second
        elapsed += Time.deltaTime;
        if (elapsed >= 1f)
        {
            elapsed = 0;
            VerifySignature();
        }
    }

    private async void VerifySignature()
    {
        try
        {
            // get loginMessage from jslib
            string loginMessage = LoginMessage();

            if (loginMessage == "") return;

            string signature = loginMessage.Split('-')[0];
            string account = loginMessage.Split('-')[1];
            string expirationTime = loginMessage.Split('-')[2];

            // get current time
            DateTime epochStart = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            int currentTime = (int)(DateTime.UtcNow - epochStart).TotalSeconds;

            // return if date expired
            if (currentTime > Int32.Parse(expirationTime)) return;

            // get owner of signature
            string message = account + '-' + expirationTime;
            string owner = await EVM.Verify(message, signature);

            // return if not owner
            if (owner != account) return;

            // save account for next scene
            _Config.Account = account;

            // reset login message
            ResetLoginMessage();

            // load next scene
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        catch
        {
            print("invalid code");
        }
    }

    public void OnLogin()
    {
        Web3Login();
    }

    public void OnSkip()
    {
        // move to next scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
#endif
