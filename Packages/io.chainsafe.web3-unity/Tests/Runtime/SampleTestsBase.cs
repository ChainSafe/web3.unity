using System.Collections;
using System.Threading.Tasks;
using ChainSafe.Gaming.Evm.Contracts;
using ChainSafe.Gaming.Evm.JsonRpc;
using ChainSafe.Gaming.MultiCall;
using ChainSafe.Gaming.UnityPackage;
using ChainSafe.Gaming.WalletConnect;
using ChainSafe.Gaming.Web3;
using ChainSafe.Gaming.Web3.Build;
using ChainSafe.Gaming.Web3.Evm.Wallet;
using ChainSafe.Gaming.Web3.Unity;
using ChainSafe.GamingSdk.Gelato;
using Microsoft.Extensions.DependencyInjection;
using Scripts.EVM.Token;
using Tests.Runtime;
using UnityEngine;
using UnityEngine.TestTools;

public class SampleTestsBase
{
    protected Web3 web3;

    [UnitySetUp]
    public IEnumerator SetUp()
    {
        // Initiate web3 build
        var buildWeb3 = BuildTestWeb3();

        // Wait until for async task to finish
        yield return new WaitUntil(() => buildWeb3.IsCompleted);

        // Assign result to web3
        web3 = buildWeb3.Result;
    }

    [UnityTearDown]
    public virtual IEnumerator TearDown()
    {
        var terminateWeb3Task = web3.TerminateAsync();

        // Wait until for async task to finish
        yield return new WaitUntil(() => terminateWeb3Task.IsCompleted);
    }

    internal static ValueTask<Web3> BuildTestWeb3(Web3Builder.ConfigureServicesDelegate customConfiguration = null)
    {
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
            services.UseMultiCall();
            services.UseRpcProvider();

            services.AddSingleton(new StubWalletConnectProviderConfig()); // can be replaced
            services.AddSingleton<IWalletProvider, StubWalletConnectProvider>();
            services.UseWalletConnectSigner();
            services.UseWalletConnectTransactionExecutor();

            // Add any contracts we would want to use
            services.ConfigureRegisteredContracts(contracts =>
                contracts.RegisterContract("CsTestErc20", ABI.Erc20, ChainSafeContracts.Erc20));
        });

        if (customConfiguration != null)
        {
            web3Builder.Configure(customConfiguration);
        }

        var buildWeb3 = web3Builder.LaunchAsync();
        return buildWeb3;
    }
}
