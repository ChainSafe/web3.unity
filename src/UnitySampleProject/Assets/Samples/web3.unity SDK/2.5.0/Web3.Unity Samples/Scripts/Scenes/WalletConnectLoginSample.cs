using ChainSafe.Gaming.Evm.JsonRpc;
using ChainSafe.Gaming.UnityPackage;
using ChainSafe.Gaming.WalletConnect;
using ChainSafe.Gaming.Web3;
using ChainSafe.Gaming.Web3.Build;
using ChainSafe.Gaming.Web3.Unity;
using ChainSafe.GamingSdk.Gelato;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WalletConnectLoginSample : MonoBehaviour
{
    [SerializeField] private WalletConnectConfigSO walletConnectConfig;
    [SerializeField] private string GelatoApiKey;
    [SerializeField] private Toggle rememberSessionToggle;
    [SerializeField] private string NextSceneName;
    [SerializeField] private bool AutoLoginPreviousSession = true;

    private bool loginInProgress;
    private bool storedSessionAvailable;

    private async void Awake()
    {
        var loginHelper = await new Web3Builder(ProjectConfigUtilities.Load())
            .BuildLoginHelper(walletConnectConfig);
        
        storedSessionAvailable = loginHelper.StoredSessionAvailable;
        
        if (AutoLoginPreviousSession && storedSessionAvailable) // auto-login
        {
            Debug.Log("Proceeding with auto-login.");
            Login();
        }
    }

    public async void Login()
    {
        if (loginInProgress)
        {
            return;
        }
        
        try
        {
            loginInProgress = true;
            var web3 = await new Web3Builder(ProjectConfigUtilities.Load())
                .Configure(services =>
                {
                    services.UseUnityEnvironment();
                    services.UseRpcProvider();

                    var rememberSession = rememberSessionToggle.isOn || storedSessionAvailable;

                    services.UseWalletConnect(walletConnectConfig.WithRememberSession(rememberSession))
                        .UseWalletConnectSigner()
                        .UseWalletConnectTransactionExecutor();

                    services.UseGelato(GelatoApiKey);
                })
                .LaunchAsync();

            Web3Accessor.Set(web3); // store web3 in singleton
        }
        catch (Web3Exception exception)
        {
            Debug.LogError($"Login failed. {exception} {exception.StackTrace}");
            return;
        }
        finally
        {
            loginInProgress = false;
        }

        SceneManager.LoadScene(NextSceneName);
    }
}