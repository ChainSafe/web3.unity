using System.Collections;
using System.Collections.Generic;
using ChainSafe.GamingSDK.EVM.MetaMaskBrowserWallet;
using ChainSafe.GamingSdk.Gelato;
using ChainSafe.GamingWeb3;
using ChainSafe.GamingWeb3.Build;
using ChainSafe.GamingWeb3.Unity;
using UnityEngine;
using UnityEngine.TestTools;
using Web3Unity.Scripts.Library.Ethers.JsonRpc;

public class SampleTestsBase
{
    protected Web3 Web3Result;
    
    [UnitySetUp]
    public virtual IEnumerator Setup()
    {
        //wait for some time to initialize
        yield return new WaitForSeconds(5f);

        //For whatever reason, in github this won't load
        var projectConfigScriptableObject = ProjectConfigUtilities.Load();
        if (projectConfigScriptableObject == null)
        {
            projectConfigScriptableObject = ProjectConfigUtilities.Load("3dc3e125-71c4-4511-a367-e981a6a94371", "5",
                "Ethereum", "Goerli", "Geth", "https://goerli.infura.io/v3/287318045c6e455ab34b81d6bcd7a65f");
        }
        
        var web3Builder = new Web3Builder(projectConfigScriptableObject).Configure(services =>
        {
            services.UseUnityEnvironment();
            services.UseGelato("_UzPz_Yk_WTjWMfcl45fLvQNGQ9ISx5ZE8TnwnVKYrE_");
            services.UseRpcProvider();

            //add any contracts we would want to use
            
            services.UseWebPageWallet(new WebPageWalletConfig { SavedUserAddress = "0x55ffe9E30347266f02b9BdAe20aD3a86493289ea" });
        });

        var buildWeb3 = web3Builder.BuildAsync();

        //wait until for async task to finish
        yield return new WaitUntil(() => buildWeb3.IsCompleted);

        Web3Result = buildWeb3.Result;
        
        WebPageWallet.Testing = true;
    }

    [UnityTearDown]
    public virtual IEnumerator TearDown()
    {
        WebPageWallet.Testing = false;

        yield return null;
    }
}
