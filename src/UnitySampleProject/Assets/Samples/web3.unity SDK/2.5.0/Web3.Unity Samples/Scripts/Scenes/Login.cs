using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChainSafe.Gaming.Evm.Contracts;
using ChainSafe.Gaming.Evm.JsonRpc;
using ChainSafe.Gaming.UnityPackage;
using ChainSafe.Gaming.Wallets;
using ChainSafe.Gaming.WalletConnect;
using ChainSafe.Gaming.WalletConnect.Models;
using ChainSafe.Gaming.Web3;
using ChainSafe.Gaming.Web3.Build;
using ChainSafe.Gaming.Web3.Unity;
using ChainSafe.GamingSdk.Gelato;
using ChainSafe.GamingSdk.Web3Auth;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using WalletConnectSharp.Core;
using WalletConnectSharp.Sign.Models;
using WalletConnectSharp.Sign.Models.Engine;

namespace Scenes
{
    [Serializable]
    public class Web3AuthButtonAndProvider
    {
        public Button Button;
        public Provider Provider;
    }

    [Serializable]
    public class Web3AuthSettings
    {
        public string ClientId;
        public string RedirectUri;
        public Web3Auth.Network Network;
    }

    public class Login : MonoBehaviour
    {
        internal const string SavedWalletConnectConfigKey = "SavedWalletConnectConfig";

        [Header("Configuration")]
        public string GelatoApiKey = "";
        public Web3AuthSettings Web3AuthSettings;

        [Header("UI")]
        public Button ExistingWalletButton;
        public Toggle RememberMeToggle;
        public ErrorPopup ErrorPopup;
        public List<Web3AuthButtonAndProvider> Web3AuthButtons;
        
        private bool useWalletConnect;

        private bool redirectToWallet;
        
        private Dictionary<string, WalletConnectWalletModel> supportedWallets;
        
        #region Wallet Connect

        private WalletConnectConfig walletConnectConfig;

        private bool autoLogin;
        
        [field: Header("Wallet Connect")]

        [SerializeField] private TMP_Dropdown supportedWalletsDropdown;
        
        [SerializeField] private Toggle redirectToWalletToggle;
        
        [SerializeField] private WalletConnectModal walletConnectModal;
        
        [field: SerializeField] public string ProjectId { get; private set; }
        
        [field: SerializeField] public string ProjectName { get; private set; }
        
        [field: SerializeField] public string BaseContext { get; private set; }
        
        [field: SerializeField] public Metadata Metadata { get; private set; } = new Metadata
        {
            Name = "Web3.Unity",
            //from package.json
            Description = "web3.unity is an open-source gaming SDK written in C# and developed by ChainSafe Gaming. It connects games built in the Unity game engine to the blockchain. The library currently supports games built for web browsers (WebGL), iOS/Android mobile, and desktop. web3.unity is compatible with most EVM-based chains such as Ethereum, Polygon, Moonbeam, Cronos, Nervos, and Binance Smart Chain, letting developers easily choose and switch between them to create the best in-game experience.",
            Url = "https://chainsafe.io/"
        };
        
        #endregion

        private IEnumerator Start()
        {
            Assert.IsNotNull(Web3AuthButtons);
            Assert.IsTrue(Web3AuthButtons.Count > 0);
            Assert.IsTrue(Web3AuthButtons.All(b => b.Button));
            Assert.IsNotNull(ExistingWalletButton);
            Assert.IsNotNull(RememberMeToggle);

            useWalletConnect = Application.platform != RuntimePlatform.WebGLPlayer;

            // Remember me only works with the WebPageWallet
            RememberMeToggle.gameObject.SetActive(useWalletConnect);
            
            // Wallet Connect
            yield return FetchSupportedWallets();

            // enable this on editor to test UI flow and functions
            if (Application.isMobilePlatform || Application.isEditor)
            {
                InitializeMobileOptions();
            }

#if UNITY_WEBGL
            ProcessWeb3Auth();
#endif
            var autoLoginTask = TryAutoLogin();
            
            yield return new WaitUntil(() => autoLoginTask.IsCompleted);

            ExistingWalletButton.onClick.AddListener(OnLoginWithExistingAccount);

            foreach (var buttonAndProvider in Web3AuthButtons)
            {
                var button = buttonAndProvider.Button;
                var provider = buttonAndProvider.Provider;
                button.onClick.AddListener(() => LoginWithWeb3Auth(provider));
            }
        }

        private void OnDestroy()
        {
            if (walletConnectConfig != null)
            {
                walletConnectConfig.OnConnected -= WalletConnected;

                walletConnectConfig.OnSessionApproved -= SessionApproved;
            }
        }

        private void WalletConnected(ConnectedData data)
        {
            // already redirecting to wallet
            if (redirectToWallet)
            {
                return;
            }

            // might be null in case of auto login
            if (!string.IsNullOrEmpty(data.Uri))
            {
                // display QR and copy to clipboard
                walletConnectModal.WalletConnected(data);
            }
        }
        
        private void SessionApproved(SessionStruct session)
        {
            // save/persist session
            if (walletConnectConfig.KeepSessionAlive)
            {
                walletConnectConfig.SavedSessionTopic = session.Topic;
                
                PlayerPrefs.SetString(SavedWalletConnectConfigKey, JsonConvert.SerializeObject(walletConnectConfig));
            }

            else
            {
                // reset if any saved config
                PlayerPrefs.SetString(SavedWalletConnectConfigKey, null);
            }
            
            Debug.Log($"{session.Topic} Approved");
        }

        // redirect to mobile wallet and select default wallet on IOS
        private void InitializeMobileOptions()
        {
            redirectToWalletToggle.gameObject.SetActive(true);
#if UNITY_IOS
            InitializeWalletDropdown();
#endif
        }
        
        // add all supported wallets
        private void InitializeWalletDropdown()
        {
            redirectToWalletToggle.onValueChanged.AddListener(isOn =>
            {
                supportedWalletsDropdown.gameObject.SetActive(isOn);
            });
            
            // first element is a no select
            List<string> supportedWalletsList = new List<string>
            {
                // default option/unselected
                "Select Wallet",    
            };

            supportedWalletsList.AddRange(supportedWallets.Values.Select(w => w.Name));
            
            supportedWalletsDropdown.AddOptions(supportedWalletsList);
        }
        
        private async Task TryAutoLogin()
        {
            if (!useWalletConnect)
                return;

            string savedConfigJson = PlayerPrefs.GetString(SavedWalletConnectConfigKey, null);

            if (string.IsNullOrEmpty(savedConfigJson))
            {
                return;
            }

            Debug.Log("Attempting to Auto Login...");
            
            try
            {
                autoLogin = true;
            
                walletConnectConfig = JsonConvert.DeserializeObject<WalletConnectConfig>(savedConfigJson);
                
                await LoginWithExistingAccount();
            }
            catch (Exception e)
            {
                Debug.LogError($"Auto Login Failed with Exception {e}");

                autoLogin = false;
            }
        }

        private async void OnLoginWithExistingAccount()
        {
#if UNITY_IOS
            // can't redirect to wallet on IOS if there's no selected wallet
            if (redirectToWalletToggle.isOn && supportedWalletsDropdown.value == 0)
            {
                // feedback
                Debug.LogError("Please select a Wallet first");
                
                return;
            }
#endif

            await LoginWithExistingAccount();
        }
        
        private async Task LoginWithExistingAccount()
        {
            var web3Builder = new Web3Builder(ProjectConfigUtilities.Load())
                .Configure(ConfigureCommonServices)
                .Configure(services =>
                {
                    /* The WebGL wallet only works inside WebGL builds,
                     * and it makes little sense to use the web page wallet
                     * inside WebGL, so the choice can be automated here
                     * by looking at the platform we're running on.
                     */
                    if (useWalletConnect)
                    {
                        services
                            .UseWalletConnect(BuildWalletConnectConfig())
                            .UseWalletConnectSigner()
                            .UseWalletConnectTransactionExecutor();
                    }
                    else
                    {
                        services.UseWebGLWallet();
                    }
                });

            await ProcessLogin(web3Builder);
        }

        private async void LoginWithWeb3Auth(Provider provider)
        {
            var web3Builder = new Web3Builder(ProjectConfigUtilities.Load())
                .Configure(ConfigureCommonServices)
                .Configure(services =>
                {
                    var web3AuthConfig = new Web3AuthWalletConfig
                    {
                        Web3AuthOptions = new()
                        {
                            clientId = Web3AuthSettings.ClientId,
                            redirectUrl = new Uri(Web3AuthSettings.RedirectUri),
                            network = Web3AuthSettings.Network,
                            whiteLabel = new()
                            {
                                dark = true,
                                defaultLanguage = "en",
                                name = "ChainSafe Gaming SDK",
                            }
                        },
                        LoginParams = new() { loginProvider = provider }
                    };
                    services.UseWeb3AuthWallet(web3AuthConfig);
                });
            await ProcessLogin(web3Builder);
        }

        private async void ProcessWeb3Auth()
        {
            var web3Builder = new Web3Builder(ProjectConfigUtilities.Load())
                .Configure(ConfigureCommonServices)
                .Configure(services =>
                {
                    var web3AuthConfig = new Web3AuthWalletConfig
                    {
                        Web3AuthOptions = new()
                        {
                            whiteLabel = new()
                            {
                                dark = true,
                                defaultLanguage = "en",
                                name = "ChainSafe Gaming SDK",
                            },
                            clientId = Web3AuthSettings.ClientId,
                            redirectUrl = new Uri(Web3AuthSettings.RedirectUri),
                            network = Web3AuthSettings.Network,

                        },
                    };
                    services.UseWeb3AuthWallet(web3AuthConfig);
                });
            await ProcessLogin(web3Builder);
        }


        private async Task ProcessLogin(Web3Builder builder)
        {
            Web3 web3;
            try
            {
                web3 = await builder.BuildAsync();
            }
            catch (Web3Exception)
            {
                ErrorPopup.ShowError($"Login failed, please try again\n(see console for more details)");
                throw;
            }
            catch (Exception)
            {
                ErrorPopup.ShowError($"Unknown error occured\n(see console for more details)");
                throw;
            }

            Web3Accessor.Set(web3);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

        private void ConfigureCommonServices(IWeb3ServiceCollection services)
        {
            services
                .UseUnityEnvironment()
                .UseGelato(GelatoApiKey)
                .UseRpcProvider();

            /* As many contracts as needed may be registered here.
             * It is better to register all contracts the application
             * will be interacting with at configuration time if they
             * are known in advance. We're just registering shiba
             * here to show how it's done. You can look at the
             * `Scripts/Prefabs/Wallet/RegisteredContract` script
             * to see how it's used later on.
             */
            services.ConfigureRegisteredContracts(contracts =>
                contracts.RegisterContract(
                    "shiba",
                    "[{\"inputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"constructor\"},{\"anonymous\":false,\"inputs\":[{\"indexed\":true,\"internalType\":\"address\",\"name\":\"owner\",\"type\":\"address\"},{\"indexed\":true,\"internalType\":\"address\",\"name\":\"spender\",\"type\":\"address\"},{\"indexed\":false,\"internalType\":\"uint256\",\"name\":\"value\",\"type\":\"uint256\"}],\"name\":\"Approval\",\"type\":\"event\"},{\"anonymous\":false,\"inputs\":[{\"indexed\":true,\"internalType\":\"address\",\"name\":\"account\",\"type\":\"address\"},{\"indexed\":false,\"internalType\":\"bool\",\"name\":\"isExcluded\",\"type\":\"bool\"}],\"name\":\"ExcludeFromFees\",\"type\":\"event\"},{\"anonymous\":false,\"inputs\":[{\"indexed\":false,\"internalType\":\"address[]\",\"name\":\"accounts\",\"type\":\"address[]\"},{\"indexed\":false,\"internalType\":\"bool\",\"name\":\"isExcluded\",\"type\":\"bool\"}],\"name\":\"ExcludeMultipleAccountsFromFees\",\"type\":\"event\"},{\"anonymous\":false,\"inputs\":[{\"indexed\":true,\"internalType\":\"uint256\",\"name\":\"newValue\",\"type\":\"uint256\"},{\"indexed\":true,\"internalType\":\"uint256\",\"name\":\"oldValue\",\"type\":\"uint256\"}],\"name\":\"GasForProcessingUpdated\",\"type\":\"event\"},{\"anonymous\":false,\"inputs\":[{\"indexed\":true,\"internalType\":\"address\",\"name\":\"newLiquidityWallet\",\"type\":\"address\"},{\"indexed\":true,\"internalType\":\"address\",\"name\":\"oldLiquidityWallet\",\"type\":\"address\"}],\"name\":\"LiquidityWalletUpdated\",\"type\":\"event\"},{\"anonymous\":false,\"inputs\":[{\"indexed\":true,\"internalType\":\"address\",\"name\":\"previousOwner\",\"type\":\"address\"},{\"indexed\":true,\"internalType\":\"address\",\"name\":\"newOwner\",\"type\":\"address\"}],\"name\":\"OwnershipTransferred\",\"type\":\"event\"},{\"anonymous\":false,\"inputs\":[{\"indexed\":false,\"internalType\":\"uint256\",\"name\":\"iterations\",\"type\":\"uint256\"},{\"indexed\":false,\"internalType\":\"uint256\",\"name\":\"claims\",\"type\":\"uint256\"},{\"indexed\":false,\"internalType\":\"uint256\",\"name\":\"lastProcessedIndex\",\"type\":\"uint256\"},{\"indexed\":true,\"internalType\":\"bool\",\"name\":\"automatic\",\"type\":\"bool\"},{\"indexed\":false,\"internalType\":\"uint256\",\"name\":\"gas\",\"type\":\"uint256\"},{\"indexed\":true,\"internalType\":\"address\",\"name\":\"processor\",\"type\":\"address\"}],\"name\":\"ProcessedDividendTracker\",\"type\":\"event\"},{\"anonymous\":false,\"inputs\":[{\"indexed\":false,\"internalType\":\"uint256\",\"name\":\"tokensSwapped\",\"type\":\"uint256\"},{\"indexed\":false,\"internalType\":\"uint256\",\"name\":\"amount\",\"type\":\"uint256\"}],\"name\":\"SendDividends\",\"type\":\"event\"},{\"anonymous\":false,\"inputs\":[{\"indexed\":true,\"internalType\":\"address\",\"name\":\"pair\",\"type\":\"address\"},{\"indexed\":true,\"internalType\":\"bool\",\"name\":\"value\",\"type\":\"bool\"}],\"name\":\"SetAutomatedMarketMakerPair\",\"type\":\"event\"},{\"anonymous\":false,\"inputs\":[{\"indexed\":false,\"internalType\":\"uint256\",\"name\":\"tokensSwapped\",\"type\":\"uint256\"},{\"indexed\":false,\"internalType\":\"uint256\",\"name\":\"ethReceived\",\"type\":\"uint256\"},{\"indexed\":false,\"internalType\":\"uint256\",\"name\":\"tokensIntoLiqudity\",\"type\":\"uint256\"}],\"name\":\"SwapAndLiquify\",\"type\":\"event\"},{\"anonymous\":false,\"inputs\":[{\"indexed\":true,\"internalType\":\"address\",\"name\":\"from\",\"type\":\"address\"},{\"indexed\":true,\"internalType\":\"address\",\"name\":\"to\",\"type\":\"address\"},{\"indexed\":false,\"internalType\":\"uint256\",\"name\":\"value\",\"type\":\"uint256\"}],\"name\":\"Transfer\",\"type\":\"event\"},{\"anonymous\":false,\"inputs\":[{\"indexed\":true,\"internalType\":\"address\",\"name\":\"newAddress\",\"type\":\"address\"},{\"indexed\":true,\"internalType\":\"address\",\"name\":\"oldAddress\",\"type\":\"address\"}],\"name\":\"UpdateDividendTracker\",\"type\":\"event\"},{\"anonymous\":false,\"inputs\":[{\"indexed\":true,\"internalType\":\"address\",\"name\":\"newAddress\",\"type\":\"address\"},{\"indexed\":true,\"internalType\":\"address\",\"name\":\"oldAddress\",\"type\":\"address\"}],\"name\":\"UpdateUniswapV2Router\",\"type\":\"event\"},{\"inputs\":[],\"name\":\"SHIB\",\"outputs\":[{\"internalType\":\"address\",\"name\":\"\",\"type\":\"address\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"UNIRewardsFee\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"_marketingWalletAddress\",\"outputs\":[{\"internalType\":\"address\",\"name\":\"\",\"type\":\"address\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"_maxTxAmount\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"owner\",\"type\":\"address\"},{\"internalType\":\"address\",\"name\":\"spender\",\"type\":\"address\"}],\"name\":\"allowance\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"spender\",\"type\":\"address\"},{\"internalType\":\"uint256\",\"name\":\"amount\",\"type\":\"uint256\"}],\"name\":\"approve\",\"outputs\":[{\"internalType\":\"bool\",\"name\":\"\",\"type\":\"bool\"}],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"\",\"type\":\"address\"}],\"name\":\"automatedMarketMakerPairs\",\"outputs\":[{\"internalType\":\"bool\",\"name\":\"\",\"type\":\"bool\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"account\",\"type\":\"address\"}],\"name\":\"balanceOf\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"\",\"type\":\"address\"}],\"name\":\"blacklist\",\"outputs\":[{\"internalType\":\"bool\",\"name\":\"\",\"type\":\"bool\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"blacklistblock\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"blockcount\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"claim\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"deadWallet\",\"outputs\":[{\"internalType\":\"address\",\"name\":\"\",\"type\":\"address\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"decimals\",\"outputs\":[{\"internalType\":\"uint8\",\"name\":\"\",\"type\":\"uint8\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"spender\",\"type\":\"address\"},{\"internalType\":\"uint256\",\"name\":\"subtractedValue\",\"type\":\"uint256\"}],\"name\":\"decreaseAllowance\",\"outputs\":[{\"internalType\":\"bool\",\"name\":\"\",\"type\":\"bool\"}],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"account\",\"type\":\"address\"}],\"name\":\"dividendTokenBalanceOf\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"dividendTracker\",\"outputs\":[{\"internalType\":\"contract PSDividendTracker\",\"name\":\"\",\"type\":\"address\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"account\",\"type\":\"address\"}],\"name\":\"excludeFromDividends\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"account\",\"type\":\"address\"},{\"internalType\":\"bool\",\"name\":\"excluded\",\"type\":\"bool\"}],\"name\":\"excludeFromFees\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address[]\",\"name\":\"accounts\",\"type\":\"address[]\"},{\"internalType\":\"bool\",\"name\":\"excluded\",\"type\":\"bool\"}],\"name\":\"excludeMultipleAccountsFromFees\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"gasForProcessing\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"account\",\"type\":\"address\"}],\"name\":\"getAccountDividendsInfo\",\"outputs\":[{\"internalType\":\"address\",\"name\":\"\",\"type\":\"address\"},{\"internalType\":\"int256\",\"name\":\"\",\"type\":\"int256\"},{\"internalType\":\"int256\",\"name\":\"\",\"type\":\"int256\"},{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"},{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"},{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"},{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"},{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"uint256\",\"name\":\"index\",\"type\":\"uint256\"}],\"name\":\"getAccountDividendsInfoAtIndex\",\"outputs\":[{\"internalType\":\"address\",\"name\":\"\",\"type\":\"address\"},{\"internalType\":\"int256\",\"name\":\"\",\"type\":\"int256\"},{\"internalType\":\"int256\",\"name\":\"\",\"type\":\"int256\"},{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"},{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"},{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"},{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"},{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"getClaimWait\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"getLastProcessedIndex\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"getNumberOfDividendTokenHolders\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"getTotalDividendsDistributed\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"spender\",\"type\":\"address\"},{\"internalType\":\"uint256\",\"name\":\"addedValue\",\"type\":\"uint256\"}],\"name\":\"increaseAllowance\",\"outputs\":[{\"internalType\":\"bool\",\"name\":\"\",\"type\":\"bool\"}],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"account\",\"type\":\"address\"}],\"name\":\"isExcludedFromFees\",\"outputs\":[{\"internalType\":\"bool\",\"name\":\"\",\"type\":\"bool\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"liquidityFee\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"marketingFee\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"name\",\"outputs\":[{\"internalType\":\"string\",\"name\":\"\",\"type\":\"string\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"owner\",\"outputs\":[{\"internalType\":\"address\",\"name\":\"\",\"type\":\"address\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"uint256\",\"name\":\"gas\",\"type\":\"uint256\"}],\"name\":\"processDividendTracker\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"renounceOwnership\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"addr\",\"type\":\"address\"},{\"internalType\":\"bool\",\"name\":\"isBlack\",\"type\":\"bool\"}],\"name\":\"setAddress\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"pair\",\"type\":\"address\"},{\"internalType\":\"bool\",\"name\":\"value\",\"type\":\"bool\"}],\"name\":\"setAutomatedMarketMakerPair\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"uint256\",\"name\":\"value\",\"type\":\"uint256\"}],\"name\":\"setLiquiditFee\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"uint256\",\"name\":\"value\",\"type\":\"uint256\"}],\"name\":\"setMarketingFee\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address payable\",\"name\":\"wallet\",\"type\":\"address\"}],\"name\":\"setMarketingWallet\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"uint256\",\"name\":\"amount\",\"type\":\"uint256\"}],\"name\":\"setMaxTxAmount\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"uint256\",\"name\":\"value\",\"type\":\"uint256\"}],\"name\":\"setSHIBRewardsFee\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"bool\",\"name\":\"salestatus\",\"type\":\"bool\"}],\"name\":\"setSaleStart\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"swapTokensAtAmount\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"symbol\",\"outputs\":[{\"internalType\":\"string\",\"name\":\"\",\"type\":\"string\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"totalFees\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"totalSupply\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"recipient\",\"type\":\"address\"},{\"internalType\":\"uint256\",\"name\":\"amount\",\"type\":\"uint256\"}],\"name\":\"transfer\",\"outputs\":[{\"internalType\":\"bool\",\"name\":\"\",\"type\":\"bool\"}],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"sender\",\"type\":\"address\"},{\"internalType\":\"address\",\"name\":\"recipient\",\"type\":\"address\"},{\"internalType\":\"uint256\",\"name\":\"amount\",\"type\":\"uint256\"}],\"name\":\"transferFrom\",\"outputs\":[{\"internalType\":\"bool\",\"name\":\"\",\"type\":\"bool\"}],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"newOwner\",\"type\":\"address\"}],\"name\":\"transferOwnership\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"uniswapV2Pair\",\"outputs\":[{\"internalType\":\"address\",\"name\":\"\",\"type\":\"address\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"uniswapV2Router\",\"outputs\":[{\"internalType\":\"contract IUniswapV2Router02\",\"name\":\"\",\"type\":\"address\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"uint256\",\"name\":\"claimWait\",\"type\":\"uint256\"},{\"internalType\":\"address\",\"name\":\"a\",\"type\":\"address\"}],\"name\":\"updateClaimWait\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"newAddress\",\"type\":\"address\"}],\"name\":\"updateDividendTracker\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"uint256\",\"name\":\"newValue\",\"type\":\"uint256\"}],\"name\":\"updateGasForProcessing\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"newAddress\",\"type\":\"address\"}],\"name\":\"updateUniswapV2Router\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"account\",\"type\":\"address\"}],\"name\":\"withdrawableDividendOf\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"stateMutability\":\"payable\",\"type\":\"receive\"}]",
                    "0x1d6f31b71e12a1a584ca20853495161c48ba491f"));

        }

        #region Wallet Connect

        private WalletConnectConfig BuildWalletConnectConfig()
        {
            // build chain
            var projectConfig = ProjectConfigUtilities.Load();

            ChainModel chain = new ChainModel(ChainModel.EvmNamespace, projectConfig.ChainId, projectConfig.Network);

#if UNITY_IOS
            WalletConnectWalletModel defaultWallet = null;
#endif

            // if it's an auto login get these values from saved wallet config
            if (!autoLogin)
            {
                // allow redirection on editor for testing UI flow
                redirectToWallet = (Application.isMobilePlatform || Application.isEditor) && redirectToWalletToggle.isOn;

#if UNITY_IOS
                // make sure there's a selected wallet on IOS
                redirectToWallet = redirectToWallet && supportedWalletsDropdown.value != 0;

                if (redirectToWallet)
                {
                    // offset for the first/default/unselected dropdown option 0
                    int selectedWalletIndex = supportedWalletsDropdown.value - 1;

                    defaultWallet = supportedWallets.Values.ToArray()[selectedWalletIndex];
                }
#endif
            }

            var config = new WalletConnectConfig
            {
                ProjectId = ProjectId,
                ProjectName = ProjectName,
                BaseContext = BaseContext,
                Chain = chain,
                Metadata = Metadata,
                SavedSessionTopic = autoLogin ? walletConnectConfig.SavedSessionTopic : null,
                SupportedWallets = supportedWallets,
                StoragePath = Application.persistentDataPath,
                RedirectToWallet = autoLogin ? walletConnectConfig.RedirectToWallet : redirectToWallet,
                KeepSessionAlive = autoLogin || RememberMeToggle.isOn,
#if UNITY_IOS
                DefaultWallet = autoLogin ? walletConnectConfig.DefaultWallet : defaultWallet,
#endif
            };

            walletConnectConfig = config;
            
            walletConnectConfig.OnConnected += WalletConnected;

            walletConnectConfig.OnSessionApproved += SessionApproved;
            
            return config;
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

                    supportedWallets = JsonConvert.DeserializeObject<Dictionary<string, WalletConnectWalletModel>>(json)
                        .ToDictionary(w => w.Key, w => (WalletConnectWalletModel) w.Value);

                    Debug.Log($"Fetched {supportedWallets.Count} Supported Wallets.");
                }
            }
        }

        #endregion
    }
}