using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AndroidLogin : MonoBehaviour
{
    [SerializeField]
    private string AndroidHost = "";
    public static AndroidLogin Instance {get; private set;}
    private string deeplinkURL;

    void Start()
    {
        if (Instance == null)
        {
            Instance = this;                
            Application.deepLinkActivated += onDeepLinkActivated;
            if (!String.IsNullOrEmpty(Application.absoluteURL))
            {
                // Cold start and Application.absoluteURL not null so process Deep Link.
                onDeepLinkActivated(Application.absoluteURL);
            }
            // Initialize DeepLink Manager global variable.
            else deeplinkURL = "[none]";
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // link to browser to sign into torus
    public void OnLogin() 
    {
       Application.OpenURL("https://chainsafe.github.io/game-mobile-login?" + AndroidHost);
    }

    private void onDeepLinkActivated(string url)
    {
        // Update DeepLink Manager global variable, so URL can be accessed from anywhere.
        deeplinkURL = url;
        
        // unitydl://mylink?signedMessage
        string signedMessage = url.Split("?"[0])[1];
        VerifySignature(signedMessage);
    }

    private async void VerifySignature(string signedMessage)
    {
        try
        {
            if (signedMessage == "") return;

            string signature = signedMessage.Split('-')[0];
            string account = signedMessage.Split('-')[1];
            string expirationTime = signedMessage.Split('-')[2];

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

            // load next scene
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        catch
        {
            print("invalid code");
        }
    }

    public void OnSkip()
    {
        // move to next scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
