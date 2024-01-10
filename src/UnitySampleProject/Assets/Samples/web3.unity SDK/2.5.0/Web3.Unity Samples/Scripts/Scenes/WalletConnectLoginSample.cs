using System.Linq;
using System.Threading.Tasks;
using ChainSafe.Gaming.Evm.JsonRpc;
using ChainSafe.Gaming.UnityPackage;
using ChainSafe.Gaming.WalletConnect;
using ChainSafe.Gaming.Web3;
using ChainSafe.Gaming.Web3.Build;
using ChainSafe.Gaming.Web3.Environment;
using ChainSafe.Gaming.Web3.Unity;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WalletConnectLoginSample : MonoBehaviour
{
    [SerializeField] private WalletConnectConfigSO WalletConnectConfig;
    [SerializeField] private Toggle rememberSessionToggle;
    [SerializeField] private string NextSceneName;

    private bool loginInProgress;

    private void Awake()
    {
        // todo add auto login
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

                    var rememberSession = rememberSessionToggle.isOn;
                    services.UseWalletConnectNew(WalletConnectConfig.WithRememberSession(rememberSession))
                        .UseWalletConnectSignerNew()
                        .UseWalletConnectTransactionExecutorNew();
                })
                .LaunchAsync();

            Web3Accessor.Set(web3);
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