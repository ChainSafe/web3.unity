using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;
using WalletConnectSharp.Common.Logging;
using WalletConnectSharp.Core;
using WalletConnectSharp.Core.Models;
using WalletConnectSharp.Sign;
using WalletConnectSharp.Sign.Models;
using WalletConnectSharp.Sign.Models.Engine;
using WalletConnectSharp.Storage;
using WalletConnectSharp.Storage.Interfaces;

public class WalletConnectUnity : MonoBehaviour
{
    public delegate void Connected(ConnectedData connectedData);
    
    public event Connected OnConnected;

    private void InvokeConnected(ConnectedData connectedData)
    {
        OnConnected?.Invoke(connectedData);
    }
    
    public delegate void SessionApproved(SessionStruct session);
    
    public event SessionApproved OnSessionApproved;

    private void InvokeSessionApproved(SessionStruct session)
    {
        OnSessionApproved?.Invoke(session);
    }
    
    [field: SerializeField] public string ProjectId { get; private set; }
    [field: SerializeField] public string ProjectName { get; private set; } = "Web3.Unity";

    [field: SerializeField]
    public Metadata ClientMetadata { get; private set; } = new Metadata
    {
        Name = "Web3.Unity",
        //from package.json
        Description = "web3.unity is an open-source gaming SDK written in C# and developed by ChainSafe Gaming. It connects games built in the Unity game engine to the blockchain. The library currently supports games built for web browsers (WebGL), iOS/Android mobile, and desktop. web3.unity is compatible with most EVM-based chains such as Ethereum, Polygon, Moonbeam, Cronos, Nervos, and Binance Smart Chain, letting developers easily choose and switch between them to create the best in-game experience.",
        Url = "https://chainsafe.io/"
    };

    public WalletConnectCore Core { get; private set; }
    
    public WalletConnectSignClient SignClient { get; private set; }

    private Dictionary<string, Wallet> _supportedWallets = new Dictionary<string, Wallet>();

    private Wallet _defaultWallet;

    public const string BaseContext = "unity-game";
    
    private async void Awake()
    {
        await Initialize();
    }

    internal async Task Initialize()
    {
        WCLogger.Logger = new WCUnityLogger();

        StartCoroutine(FetchSupportedWallets());
        
        Core = new WalletConnectCore(new CoreOptions()
        {
            Name = ProjectName,
            ProjectId = ProjectId,
            Storage = BuildStorage(),
            BaseContext = BaseContext
        });

        await Core.Start();

        await InitializeSignClient();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            ConnectClient();
        }
    }

    public IKeyValueStorage BuildStorage()
    {
        string path = Path.Combine(Application.persistentDataPath, "wallet_connect.json");
        return new FileSystemStorage(path);
    }

    public async Task InitializeSignClient()
    {
        SignClient = await WalletConnectSignClient.Init(new SignClientOptions()
        {
            BaseContext = BaseContext,
            Core = Core,
            Metadata = ClientMetadata,
            Name = ProjectName,
            ProjectId = ProjectId,
            Storage = Core.Storage,
        });
    }

    public async Task<ConnectedData> ConnectClient()
    {
        RequiredNamespaces requiredNamespaces = new RequiredNamespaces();

        var methods = new string[]
        {
            "eth_sendTransaction",
            "eth_signTransaction",
            "eth_sign",
            "personal_sign",
            "eth_signTypedData",
        };

        var events = new string[]
        {
            "chainChanged", "accountsChanged"
        };

        requiredNamespaces.Add(Chain.EvmNamespace, new ProposedNamespace
        {
            Chains = new string[]
            {
                Chain.Goerli.FullChainId,
            },
            Events = events,
            Methods = methods
        });

        //start connecting
        ConnectedData connectData = await SignClient.Connect(new ConnectOptions
        {
            RequiredNamespaces = requiredNamespaces
        });
        
        InvokeConnected(connectData);

        SessionStruct sessionResult = await connectData.Approval;
             
        InvokeSessionApproved(sessionResult);
        
        string nativeUrl = sessionResult.Peer.Metadata.Redirect.Native.Replace("//", string.Empty);
            
        string defaultWalletId = _supportedWallets.FirstOrDefault(t => t.Value.Mobile.NativeProtocol == nativeUrl || t.Value.Desktop.NativeProtocol == nativeUrl).Key;

        _defaultWallet = _supportedWallets[defaultWalletId];
        
        if (Application.isMobilePlatform)
            _defaultWallet.OpenDeeplink(connectData);

        return connectData;
    }
    
    private IEnumerator FetchSupportedWallets()
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get("https://registry.walletconnect.org/data/wallets.json"))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error Getting Supported Wallets: " + webRequest.error);
                
                yield return null;
            }
            
            else
            {
                var json = webRequest.downloadHandler.text;

                _supportedWallets = JsonConvert.DeserializeObject<Dictionary<string, Wallet>>(json);
                
                Debug.Log($"Fetched {_supportedWallets.Count} Supported Wallets.");
            }
        }
    }
}
