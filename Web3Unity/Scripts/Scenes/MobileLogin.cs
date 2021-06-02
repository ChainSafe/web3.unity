using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MobileLogin : MonoBehaviour
{
    [SerializeField]
    Button loginButton;

    [SerializeField]
    InputField loginInputField;

    public void Update()
    {
        // enable login button if code is valid
        loginButton.interactable = (loginInputField.text.Length == 186);
    }

    public void OnGenerateCode()
    {
        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            Application
                .ExternalEval("window.open(\"https://underscoredlabs.github.io/metamask-auth/\",\"_blank\")");
            return;
        }
        Application.OpenURL("https://underscoredlabs.github.io/metamask-auth/");
    }

    public async void OnLogin()
    {
        try
        {
            // parse signature and expiration time from code
            string signature = loginInputField.text.Split('-')[0];
            string account = loginInputField.text.Split('-')[1];
            string expirationTime = loginInputField.text.Split('-')[2];

            // get current time
            DateTime epochStart =
                new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            int currentTime = (int)(DateTime.UtcNow - epochStart).TotalSeconds;

            // disable login if expired
            if (currentTime > Int32.Parse(expirationTime))
            {
                loginInputField.text = "Code Expired";
                return;
            }

            // get owner of signature
            string message = account + '-' + expirationTime;
            string owner = await Ethereum.Verify(message, signature);

            // disable login if wrong owner
            if (owner != account)
            {
                loginInputField.text = "Invalid Code";
                return;
            }

            // save account for next scene
            _Config.Account = account;

            // load next scene
            SceneManager
                .LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        catch
        {
            // handle error
            loginInputField.text = "Invalid Code";
        }
    }
}
