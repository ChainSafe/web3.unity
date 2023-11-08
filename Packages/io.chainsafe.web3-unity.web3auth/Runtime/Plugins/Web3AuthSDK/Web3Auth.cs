using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using UnityEngine;
using System.Net;
using System.Collections;
using Org.BouncyCastle.Math;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

public class Web3Auth : MonoBehaviour
{
    public enum Network
    {
        MAINNET, TESTNET, CYAN, AQUA, SAPPHIRE_DEVNET, SAPPHIRE_MAINNET
    }

    public enum ChainNamespace
    {
        EIP155, SOLANA
    }

    public enum BuildEnv
    {
        PRODUCTION, STAGING, TESTING
    }

    public enum ThemeModes
    {
        light, dark, auto
    }

    public enum Language
    {
        en, de, ja, ko, zh, es, fr, pt, nl
    }

    private Web3AuthOptions web3AuthOptions;
    private Dictionary<string, object> initParams;

    private Web3AuthResponse web3AuthResponse;

    public event Action<Web3AuthResponse> onLogin;
    public event Action onLogout;

    [SerializeField]
    private string clientId;

    [SerializeField]
    private string redirectUri;

    [SerializeField]
    private Web3Auth.Network network;

    private static readonly Queue<Action> _executionQueue = new Queue<Action>();

    public void Awake()
    {
        this.initParams = new Dictionary<string, object>();

        this.initParams["clientId"] = clientId;
        this.initParams["network"] = network.ToString().ToLower();

        if (!string.IsNullOrEmpty(redirectUri))
            this.initParams["redirectUrl"] = redirectUri;

        Application.deepLinkActivated += onDeepLinkActivated;
        if (!string.IsNullOrEmpty(Application.absoluteURL))
            onDeepLinkActivated(Application.absoluteURL);

#if UNITY_EDITOR
        Web3AuthSDK.Editor.Web3AuthDebug.onURLRecieved += (Uri url) =>
        {
            this.setResultUrl(url);
        };

//#elif UNITY_WEBGL
//        var code = Utils.GetAuthCode();
//        Debug.Log("code is " + code);
//        if (Utils.GetAuthCode() != "") 
//        {
//            Debug.Log("I am here");
//            this.setResultUrl(new Uri($"http://localhost#{code}"));
//        } 
#endif
        authorizeSession("");
    }

    public void setOptions(Web3AuthOptions web3AuthOptions)
    {
        JsonSerializerSettings settings = new JsonSerializerSettings
        {
            Converters = new List<JsonConverter> { new StringEnumConverter() },
            Formatting = Formatting.Indented
        };

        this.web3AuthOptions = web3AuthOptions;

        if (this.web3AuthOptions.redirectUrl != null)
            this.initParams["redirectUrl"] = this.web3AuthOptions.redirectUrl;

        if (this.web3AuthOptions.whiteLabel != null)
            this.initParams["whiteLabel"] = JsonConvert.SerializeObject(this.web3AuthOptions.whiteLabel, settings);

        if (this.web3AuthOptions.loginConfig != null)
            this.initParams["loginConfig"] = JsonConvert.SerializeObject(this.web3AuthOptions.loginConfig, settings);

        if (this.web3AuthOptions.clientId != null)
            this.initParams["clientId"] = this.web3AuthOptions.clientId;

        if (this.web3AuthOptions.buildEnv != null)
            this.initParams["buildEnv"] = this.web3AuthOptions.buildEnv.ToString().ToLower();

        this.initParams["network"] = this.web3AuthOptions.network.ToString().ToLower();

        if (this.web3AuthOptions.useCoreKitKey.HasValue)
            this.initParams["useCoreKitKey"] = this.web3AuthOptions.useCoreKitKey.Value;

        if (this.web3AuthOptions.chainNamespace != null)
            this.initParams["chainNamespace"] = this.web3AuthOptions.chainNamespace;
    }

    private void onDeepLinkActivated(string url)
    {
        this.setResultUrl(new Uri(url));
    }

#if UNITY_STANDALONE || UNITY_EDITOR
    private string StartLocalWebserver()
    {
        HttpListener httpListener = new HttpListener();

        var redirectUrl = $"http://localhost:{Utils.GetRandomUnusedPort()}";

        httpListener.Prefixes.Add($"{redirectUrl}/complete/");
        httpListener.Prefixes.Add($"{redirectUrl}/auth/");
        httpListener.Start();
        httpListener.BeginGetContext(new AsyncCallback(IncomingHttpRequest), httpListener);

        return redirectUrl + "/complete/";
    }

    private void IncomingHttpRequest(IAsyncResult result)
    {

        // get back the reference to our http listener
        HttpListener httpListener = (HttpListener)result.AsyncState;

        // fetch the context object
        HttpListenerContext httpContext = httpListener.EndGetContext(result);

        // if we'd like the HTTP listener to accept more incoming requests, we'd just restart the "get context" here:
        // httpListener.BeginGetContext(new AsyncCallback(IncomingHttpRequest),httpListener);
        // however, since we only want/expect the one, single auth redirect, we don't need/want this, now.
        // but this is what you would do if you'd want to implement more (simple) "webserver" functionality
        // in your project.

        // the context object has the request object for us, that holds details about the incoming request
        HttpListenerRequest httpRequest = httpContext.Request;
        HttpListenerResponse httpResponse = httpContext.Response;

        if (httpRequest.Url.LocalPath == "/complete/")
        {

            httpListener.BeginGetContext(new AsyncCallback(IncomingHttpRequest), httpListener);

            var responseString = @"
                <!DOCTYPE html>
                <html>
                <head>
                  <meta charset=""utf-8"">
                  <meta name=""viewport"" content=""width=device-width"">
                  <title>Web3Auth</title>
                  <link href=""https://fonts.googleapis.com/css2?family=DM+Sans:wght@500&display=swap"" rel=""stylesheet"">
                </head>
                <body style=""padding:0;margin:0;font-size:10pt;font-family: 'DM Sans', sans-serif;"">
                  <div style=""display:flex;align-items:center;justify-content:center;height:100vh;display: none;"" id=""success"">
                    <div style=""text-align:center"">
                       <h2 style=""margin-bottom:0""> Authenticated successfully</h2>
                       <p> You can close this tab/window now </p>
                    </div>
                  </div>
                  <div style=""display:flex;align-items:center;justify-content:center;height:100vh;display: none;"" id=""error"">
                    <div style=""text-align:center"">
                       <h2 style=""margin-bottom:0""> Authentication failed</h2>
                       <p> Please try again </p>
                    </div>
                  </div>
                  <script>
                    if (window.location.hash.trim() == """") {
                        document.querySelector(""#error"").style.display=""flex"";
                    } else {
                        fetch(`http://${window.location.host}/auth/?code=${window.location.hash.slice(1,window.location.hash.length)}`).then(function(response) {
                          console.log(response);
                          document.querySelector(""#success"").style.display=""flex"";
                        }).catch(function(error) {
                          console.log(error);
                          document.querySelector(""#error"").style.display=""flex"";
                        });
                    }
                    
                  </script>
                </body>
                </html>
            ";

            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);

            httpResponse.ContentLength64 = buffer.Length;
            System.IO.Stream output = httpResponse.OutputStream;
            output.Write(buffer, 0, buffer.Length);
            output.Close();

        }

        if (httpRequest.Url.LocalPath == "/auth/")
        {
            var responseString = @"ok";

            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);

            httpResponse.ContentLength64 = buffer.Length;
            System.IO.Stream output = httpResponse.OutputStream;
            output.Write(buffer, 0, buffer.Length);
            output.Close();

            string code = httpRequest.QueryString.Get("code");
            if (!string.IsNullOrEmpty(code))
            {
                this.setResultUrl(new Uri($"http://localhost#{code}"));
            }

            httpListener.Close();
        }
    }
#endif

    private async void request(string path, LoginParams loginParams = null, Dictionary<string, object> extraParams = null)
    {
#if UNITY_STANDALONE || UNITY_EDITOR
        this.initParams["redirectUrl"] = StartLocalWebserver();
#elif UNITY_WEBGL
        this.initParams["redirectUrl"] = Utils.GetCurrentURL();
#endif

        loginParams.redirectUrl = loginParams.redirectUrl ?? new Uri(this.initParams["redirectUrl"].ToString());
        Dictionary<string, object> paramMap = new Dictionary<string, object>();
        paramMap["options"] = this.initParams;
        paramMap["params"] = loginParams == null ? (object)new Dictionary<string, object>() : (object)loginParams;
        paramMap["actionType"] = "login";

        if (extraParams != null && extraParams.Count > 0)
            foreach (KeyValuePair<string, object> item in extraParams)
            {
                (paramMap["params"] as Dictionary<string, object>)[item.Key] = item.Value;
            }

        string loginId = await createSession(JsonConvert.SerializeObject(paramMap, Formatting.None,
            new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            }), 600);

        if (!string.IsNullOrEmpty(loginId))
        {
            var loginIdObject = new Dictionary<string, string>
             {
                  { "loginId", loginId }
             };
            string hash = Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(loginIdObject, Formatting.None,
                new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                })));

            UriBuilder uriBuilder = new UriBuilder(this.web3AuthOptions.sdkUrl);
            uriBuilder.Path = path;
            uriBuilder.Fragment = "b64Params=" + hash;

            Utils.LaunchUrl(uriBuilder.ToString(), this.initParams["redirectUrl"].ToString(), gameObject.name);
        }
        else
        {
            throw new Exception("Some went wrong. Please try again later.");
        }
    }

    public void setResultUrl(Uri uri)
    {
        string hash = uri.Fragment;
#if !UNITY_EDITOR && UNITY_WEBGL
        if (hash == null || hash.Length == 0)
            return;
#else
        if (hash == null)
            throw new UserCancelledException();
#endif
        hash = hash.Remove(0, 1);
        Dictionary<string, string> queryParameters = Utils.ParseQuery(uri.Query);

        if (queryParameters.Keys.Contains("error"))
            throw new UnKnownException(queryParameters["error"]);

        string sessionId = hash.Split('&')[0].Split('=')[1];

        //save new sessionId
        this.Enqueue(() => KeyStoreManagerUtils.savePreferenceData(KeyStoreManagerUtils.SESSION_ID, sessionId));

        //call authorize session API
        this.Enqueue(() => authorizeSession(sessionId));

#if !UNITY_EDITOR && UNITY_WEBGL
        if (this.web3AuthResponse != null) 
        {
            Utils.RemoveAuthCodeFromURL();
        } 
#endif
    }

    public void login(LoginParams loginParams)
    {
        if (web3AuthOptions.loginConfig != null)
        {
            var loginConfigItem = web3AuthOptions.loginConfig?.Values.First();
            var share = KeyStoreManagerUtils.getPreferencesData(loginConfigItem?.verifier);

            if (!string.IsNullOrEmpty(share))
            {
                loginParams.dappShare = share;
            }
        }

        request("start", loginParams);
    }

    public void logout(Dictionary<string, object> extraParams)
    {
        sessionTimeOutAPI();
    }

    public void logout(Uri redirectUrl = null, string appState = null)
    {
        Dictionary<string, object> extraParams = new Dictionary<string, object>();
        if (redirectUrl != null)
            extraParams["redirectUrl"] = redirectUrl.ToString();

        if (appState != null)
            extraParams["appState"] = appState;

        logout(extraParams);
    }

    private void authorizeSession(string newSessionId)
    {
        string sessionId = "";
        if (string.IsNullOrEmpty(newSessionId))
        {
            sessionId = KeyStoreManagerUtils.getPreferencesData(KeyStoreManagerUtils.SESSION_ID);
        }
        else
        {
            sessionId = newSessionId;
        }

        if (!string.IsNullOrEmpty(sessionId))
        {
            var pubKey = KeyStoreManagerUtils.getPubKey(sessionId);
            StartCoroutine(Web3AuthApi.getInstance().authorizeSession(pubKey, (response =>
            {
                if (response != null)
                {
                    var shareMetadata = Newtonsoft.Json.JsonConvert.DeserializeObject<ShareMetadata>(response.message);

                    var aes256cbc = new AES256CBC(
                        sessionId,
                        shareMetadata.ephemPublicKey,
                        shareMetadata.iv
                    );

                    var encryptedShareBytes = AES256CBC.toByteArray(new BigInteger(shareMetadata.ciphertext, 16));
                    var share = aes256cbc.decrypt(encryptedShareBytes, shareMetadata.mac);
                    var tempJson = JsonConvert.DeserializeObject<JObject>(System.Text.Encoding.UTF8.GetString(share));

                    this.web3AuthResponse = JsonConvert.DeserializeObject<Web3AuthResponse>(tempJson.ToString());
                    if (this.web3AuthResponse != null)
                    {
                        if (this.web3AuthResponse.error != null)
                        {
                            throw new UnKnownException(this.web3AuthResponse.error ?? "Something went wrong");
                        }

                        if (!string.IsNullOrEmpty(this.web3AuthResponse.sessionId))
                        {
                            KeyStoreManagerUtils.savePreferenceData(KeyStoreManagerUtils.SESSION_ID, this.web3AuthResponse.sessionId);
                        }

                        if (!string.IsNullOrEmpty(web3AuthResponse.userInfo?.dappShare))
                        {
                            KeyStoreManagerUtils.savePreferenceData(
                                        web3AuthResponse.userInfo?.verifier, web3AuthResponse.userInfo?.dappShare
                            );
                        }

                        if (string.IsNullOrEmpty(this.web3AuthResponse.privKey) || string.IsNullOrEmpty(this.web3AuthResponse.privKey.Trim('0')))
                            this.Enqueue(() => this.onLogout?.Invoke());
                        else
                            this.Enqueue(() => this.onLogin?.Invoke(this.web3AuthResponse));
                    }
                }

            })));
        }
    }

    private void sessionTimeOutAPI()
    {
        string sessionId = KeyStoreManagerUtils.getPreferencesData(KeyStoreManagerUtils.SESSION_ID);
        if (!string.IsNullOrEmpty(sessionId))
        {
            var pubKey = KeyStoreManagerUtils.getPubKey(sessionId);
            StartCoroutine(Web3AuthApi.getInstance().authorizeSession(pubKey, (response =>
            {
                if (response != null)
                {
                    var shareMetadata = Newtonsoft.Json.JsonConvert.DeserializeObject<ShareMetadata>(response.message);

                    var aes256cbc = new AES256CBC(
                        sessionId,
                        shareMetadata.ephemPublicKey,
                        shareMetadata.iv
                    );

                    var encryptedData = aes256cbc.encrypt(System.Text.Encoding.UTF8.GetBytes(""));
                    var encryptedMetadata = new ShareMetadata()
                    {
                        iv = shareMetadata.iv,
                        ephemPublicKey = shareMetadata.ephemPublicKey,
                        ciphertext = KeyStoreManagerUtils.convertByteToHexadecimal(encryptedData),
                        mac = shareMetadata.mac
                    };
                    var jsonData = JsonConvert.SerializeObject(encryptedMetadata);

                    StartCoroutine(Web3AuthApi.getInstance().logout(
                        new LogoutApiRequest()
                        {
                            key = KeyStoreManagerUtils.getPubKey(sessionId),
                            data = jsonData,
                            signature = KeyStoreManagerUtils.getECDSASignature(
                                sessionId,
                                jsonData
                            ),
                            timeout = 1
                        }, result =>
                        {
                            if (result != null)
                            {
                                try
                                {
                                    KeyStoreManagerUtils.deletePreferencesData(KeyStoreManagerUtils.SESSION_ID);
                                    KeyStoreManagerUtils.deletePreferencesData(web3AuthOptions.loginConfig?.Values.First()?.verifier);

                                    this.Enqueue(() => this.onLogout?.Invoke());
                                }
                                catch (Exception ex)
                                {
                                    Debug.LogError(ex.Message);
                                }
                            }
                        }
                    ));
                }
            })));
        }
    }

    private async Task<string> createSession(string data, long sessionTime)
    {
        TaskCompletionSource<string> createSessionResponse = new TaskCompletionSource<string>();
        var newSessionKey = KeyStoreManagerUtils.generateRandomSessionKey();
        var ephemKey = KeyStoreManagerUtils.getPubKey(newSessionKey);
        var ivKey = KeyStoreManagerUtils.generateRandomBytes();

        var aes256cbc = new AES256CBC(
            newSessionKey,
            ephemKey,
            KeyStoreManagerUtils.convertByteToHexadecimal(ivKey)
        );
        var encryptedData = aes256cbc.encrypt(System.Text.Encoding.UTF8.GetBytes(data));
        var mac = aes256cbc.getMac(encryptedData);
        var encryptedMetadata = new ShareMetadata()
        {
            iv = KeyStoreManagerUtils.convertByteToHexadecimal(ivKey),
            ephemPublicKey = ephemKey,
            ciphertext = KeyStoreManagerUtils.convertByteToHexadecimal(encryptedData),
            mac = KeyStoreManagerUtils.convertByteToHexadecimal(mac)
        };
        var jsonData = JsonConvert.SerializeObject(encryptedMetadata);
        StartCoroutine(Web3AuthApi.getInstance().createSession(
            new LogoutApiRequest()
            {
                key = KeyStoreManagerUtils.getPubKey(newSessionKey),
                data = jsonData,
                signature = KeyStoreManagerUtils.getECDSASignature(
                    newSessionKey,
                    jsonData
                ),
                timeout = Math.Min(sessionTime, 7 * 86400)
            }, result =>
            {
                if (result != null)
                {
                    try
                    {
                        this.Enqueue(() => KeyStoreManagerUtils.savePreferenceData(KeyStoreManagerUtils.SESSION_ID, newSessionKey));
                        createSessionResponse.SetResult(newSessionKey);
                    }
                    catch (Exception ex)
                    {
                        createSessionResponse.SetException(new Exception("Something went wrong. Please try again later."));
                        Debug.LogError(ex.Message);
                    }
                }
                else
                {
                    createSessionResponse.SetException(new Exception("Something went wrong. Please try again later."));
                }
            }
        ));
        return await createSessionResponse.Task;
    }

    public string getPrivKey()
    {
        if (web3AuthResponse == null)
            return "";

        return web3AuthOptions.useCoreKitKey.Value ? web3AuthResponse.coreKitKey : web3AuthResponse.privKey;
    }

    public string getEd25519PrivKey()
    {
        if (web3AuthResponse == null)
            return "";

        return web3AuthOptions.useCoreKitKey.Value ? web3AuthResponse.coreKitEd25519PrivKey : web3AuthResponse.ed25519PrivKey;
    }

    public UserInfo getUserInfo()
    {
        if (web3AuthResponse == null)
            throw new Exception(Web3AuthError.getError(ErrorCode.NOUSERFOUND));

        return web3AuthResponse.userInfo;
    }

    public void Update()
    {
        lock (_executionQueue)
        {
            while (_executionQueue.Count > 0)
            {
                _executionQueue.Dequeue().Invoke();
            }
        }
    }

    private void Enqueue(Action action)
    {
        lock (_executionQueue)
        {
            _executionQueue.Enqueue(() =>
            {
                StartCoroutine(ActionWrapper(action));
            });
        }
    }

    private IEnumerator ActionWrapper(Action a)
    {
        a();
        yield return null;
    }
}
