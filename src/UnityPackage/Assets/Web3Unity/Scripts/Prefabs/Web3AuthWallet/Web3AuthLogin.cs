using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class Web3AuthLogin : MonoBehaviour
{
    List<LoginVerifier> verifierList = new List<LoginVerifier> {
        new LoginVerifier("Google", Provider.GOOGLE),
        new LoginVerifier("Facebook", Provider.FACEBOOK),
        new LoginVerifier("CUSTOM_VERIFIER", Provider.CUSTOM_VERIFIER),
        new LoginVerifier("Twitch", Provider.TWITCH),
        new LoginVerifier("Discord", Provider.DISCORD),
        new LoginVerifier("Reddit", Provider.REDDIT),
        new LoginVerifier("Apple", Provider.APPLE),
        new LoginVerifier("Github", Provider.GITHUB),
        new LoginVerifier("LinkedIn", Provider.LINKEDIN),
        new LoginVerifier("Twitter", Provider.TWITTER),
        new LoginVerifier("Line", Provider.LINE),
        new LoginVerifier("Hosted Email Passwordless", Provider.EMAIL_PASSWORDLESS),
    };

    Web3Auth web3Auth;

    [SerializeField]
    InputField emailAddressField;

    [SerializeField]
    Dropdown verifierDropdown;

    [SerializeField]
    Button loginButton;

    [SerializeField]
    Text loginResponseText;

    [SerializeField]
    Button logoutButton;
    ProjectConfigScriptableObject projectConfigSO = null;

    void Start()
    {
        PlayerPrefs.SetString("PK", "");
        // loads the data saved from the editor config
        projectConfigSO = (ProjectConfigScriptableObject)Resources.Load("ProjectConfigData", typeof(ScriptableObject));
        PlayerPrefs.SetString("ProjectID", projectConfigSO.ProjectID);
        PlayerPrefs.SetString("ChainID", projectConfigSO.ChainID);
        PlayerPrefs.SetString("Chain", projectConfigSO.Chain);
        PlayerPrefs.SetString("Network", projectConfigSO.Network);
        PlayerPrefs.SetString("RPC", projectConfigSO.RPC);
        var loginConfigItem = new LoginConfigItem()
        {
            verifier = "your_verifierid_from_web3auth_dashboard",
            typeOfLogin = TypeOfLogin.GOOGLE,
            clientId = "your_clientid_from_google_or_etc"
        };

        web3Auth = GetComponent<Web3Auth>();
        web3Auth.setOptions(new Web3AuthOptions()
        {
            whiteLabel = new WhiteLabelData()
            {
                name = "Web3Auth Sample App",
                logoLight = null,
                logoDark = null,
                defaultLanguage = "en",
                dark = true,
                theme = new Dictionary<string, string>
                {
                    { "primary", "#123456" }
                }
            }
            // If using your own custom verifier, uncomment this code. 
            /*
            ,
            loginConfig = new Dictionary<string, LoginConfigItem>
            {
                {"CUSTOM_VERIFIER", loginConfigItem}
            }
            */
        });
        web3Auth.onLogin += onLogin;
        web3Auth.onLogout += onLogout;

        emailAddressField.gameObject.SetActive(false);
        logoutButton.gameObject.SetActive(false);

        loginButton.onClick.AddListener(login);
        logoutButton.onClick.AddListener(logout);

        verifierDropdown.AddOptions(verifierList.Select(x => x.name).ToList());
        verifierDropdown.onValueChanged.AddListener(onVerifierDropDownChange);
    }

    private void onLogin(Web3AuthResponse response)
    {
        loginResponseText.text = JsonConvert.SerializeObject(response, Formatting.Indented);
        var userInfo = JsonConvert.SerializeObject(response.userInfo, Formatting.Indented);
        PlayerPrefs.SetString("PK", response.privKey);

        loginButton.gameObject.SetActive(false);
        verifierDropdown.gameObject.SetActive(false);
        emailAddressField.gameObject.SetActive(false);
        logoutButton.gameObject.SetActive(true);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private void onLogout()
    {
        loginButton.gameObject.SetActive(true);
        verifierDropdown.gameObject.SetActive(true);
        logoutButton.gameObject.SetActive(false);

        loginResponseText.text = "";
    }


    private void onVerifierDropDownChange(int selectedIndex)
    {
        if (verifierList[selectedIndex].loginProvider == Provider.EMAIL_PASSWORDLESS)
            emailAddressField.gameObject.SetActive(true);
        else
            emailAddressField.gameObject.SetActive(false);
    }

    private void login()
    {
        var selectedProvider = verifierList[verifierDropdown.value].loginProvider;

        var options = new LoginParams()
        {
            loginProvider = selectedProvider
        };

        if (selectedProvider == Provider.EMAIL_PASSWORDLESS)
        {
            options.extraLoginOptions = new ExtraLoginOptions()
            {
                login_hint = emailAddressField.text
            };
        }

        web3Auth.login(options);
    }

    private void logout()
    {
        web3Auth.logout();
    }
    /*
    // subscription check to enable/disable Web3Auth login
    
    [System.Serializable]
    public class ValidateOBJ
    {
        public bool response;
    }

    public void CheckSubscription()
    {
        StartCoroutine(Check());
    }
    
    // web request to check subscription
    IEnumerator Check()
    {
        WWWForm form = new WWWForm();
        form.AddField("projectId", PlayerPrefs.GetString("ProjectID"));

        using (UnityWebRequest www =
               UnityWebRequest.Post("https://api.gaming.chainsafe.io/project/checkSubscription", form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
                Debug.Log("Error Checking Project Subscription!");
                Debug.LogError("Subscription Not Valid! Please Renew At Dashboard.Gaming.Chainsafe.io");
            }
            else
            {
                // deserialize object into response and check
                Debug.Log("Checking Subscription!");
                ValidateOBJ validation = new ValidateOBJ();
                ValidateOBJ obj = JsonConvert.DeserializeObject<ValidateOBJ>(www.downloadHandler.text);

                // allows user login if subscription is active
                if (obj.response.ToString() == "True")
                {
                    Debug.Log("Subscription Valid!");
                    login();
                }
                else
                {
                    // stops user login and throws an error if subscription isn't active
                    Debug.LogError("Subscription Not Valid! Please Renew At Dashboard.Gaming.Chainsafe.io");
                }
            }
        }
    }
    */
}
