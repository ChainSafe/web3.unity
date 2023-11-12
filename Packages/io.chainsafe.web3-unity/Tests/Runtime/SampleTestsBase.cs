using System.Collections;
using ChainSafe.Gaming.Evm.Contracts;
using ChainSafe.Gaming.Evm.JsonRpc;
using ChainSafe.Gaming.UnityPackage;
using ChainSafe.Gaming.WalletConnect;
using ChainSafe.Gaming.Web3;
using ChainSafe.Gaming.Web3.Build;
using ChainSafe.Gaming.Web3.Unity;
using ChainSafe.GamingSdk.Gelato;
using Scripts.EVM.Token;
using UnityEngine;
using UnityEngine.TestTools;

public class SampleTestsBase
{
    protected Web3 web3Result;

    protected WalletConnectConfig config;

    [UnitySetUp]
    public virtual IEnumerator Setup()
    {
        // Wait for some time to initialize
        yield return new WaitForSeconds(5f);

        // Set project config, fallback is for github as it doesn't load
        var projectConfigScriptableObject = ProjectConfigUtilities.Load();
        if (projectConfigScriptableObject == null)
        {
            projectConfigScriptableObject = ProjectConfigUtilities.Load("3dc3e125-71c4-4511-a367-e981a6a94371", "11155111",
                    "Ethereum", "Sepolia", "Seth", "https://sepolia.infura.io/v3/287318045c6e455ab34b81d6bcd7a65f");
        }
        
        // Create web3builder & assign services
        var web3Builder = new Web3Builder(projectConfigScriptableObject).Configure(services =>
        {
            services.UseUnityEnvironment();
            services.UseGelato("_UzPz_Yk_WTjWMfcl45fLvQNGQ9ISx5ZE8TnwnVKYrE_");
            services.UseRpcProvider();

            config = new WalletConnectConfig
            {
                // Set wallet to testing
                Testing = true,
                TestWalletAddress = "0xD5c8010ef6dff4c83B19C511221A7F8d1e5cFF44",
            };

            services.UseWalletConnect(config);
            services.UseWalletConnectSigner();
            services.UseWalletConnectTransactionExecutor();

            // Add any contracts we would want to use
            services.ConfigureRegisteredContracts(contracts =>
                contracts.RegisterContract("CsTestErc20", ABI.Erc20, Contracts.Erc20));
        });

        var buildWeb3 = web3Builder.LaunchAsync();

        // Wait until for async task to finish
        yield return new WaitUntil(() => buildWeb3.IsCompleted);

        // Assign result to web3
        web3Result = buildWeb3.Result;
    }

    [UnityTearDown]
    public virtual IEnumerator TearDown()
    {
        config.Testing = false;
        web3Result.TerminateAsync();
        yield return null;
    }
}
