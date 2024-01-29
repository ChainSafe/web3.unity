using ChainSafe.Gaming.Evm.Contracts;
using ChainSafe.Gaming.Evm.JsonRpc;
using ChainSafe.Gaming.MultiCall;
using ChainSafe.Gaming.UnityPackage;
using ChainSafe.Gaming.WalletConnect;
using ChainSafe.Gaming.Web3;
using ChainSafe.Gaming.Web3.Build;
using ChainSafe.Gaming.Web3.Unity;
using ChainSafe.GamingSdk.Gelato;
using Scripts.EVM.Token;
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
        var connectionHelper = await new Web3Builder(ProjectConfigUtilities.Load()) // build lightweight web3 
            .BuildConnectionHelper(walletConnectConfig);
        
        storedSessionAvailable = connectionHelper.StoredSessionAvailable;
        
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
                    services.UseMultiCall();

                    /* As many contracts as needed may be registered here.
                     * It is better to register all contracts the application
                     * will be interacting with at configuration time if they
                     * are known in advance. We're just registering CsTestErc20
                     * here to show how it's done.
                     */
                    services.ConfigureRegisteredContracts(contracts =>
                        contracts.RegisterContract("CsTestErc20", ABI.Erc20, Contracts.Erc20));
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