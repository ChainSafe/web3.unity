using System.Collections;
using System.Diagnostics;
using System.Threading.Tasks;
using ChainSafe.Gaming.UnityPackage;
using ChainSafe.Gaming.Web3;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;

public class SampleTestsBase
{
    private const string Mnemonic = "test test test test test test test test test test test junk";

    private Process _anvil;
    
    [UnitySetUp]
    public IEnumerator SetUp()
    {
        #region Anvil

        _anvil = new Process()
        {
            StartInfo = new ProcessStartInfo("anvil", $"--mnemonic \"{Mnemonic}\" --silent")
            {
                CreateNoWindow = true,
                WindowStyle = ProcessWindowStyle.Hidden,
                UseShellExecute = true,
                RedirectStandardOutput = false,
            },
        };

        if (!_anvil.Start())
        {
            throw new Web3Exception("Anvil failed to start.");
        }

        #endregion
        
        GameObject web3UnityPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(
            "Packages/io.chainsafe.web3-unity/Tests/Runtime/Web3Unity_Tests.prefab");

        Object.Instantiate(web3UnityPrefab);

        Web3Unity.TestMode = true;
        
        Task initialize = Web3Unity.Instance.Initialize(false);
        
        yield return new WaitUntil(() => initialize.IsCompleted);
        
        Task connect = Web3Unity.Instance.Connect(ScriptableObject.CreateInstance<AnvilConnectionProvider>());
        
        yield return new WaitUntil(() => connect.IsCompleted);
    }

    [UnityTearDown]
    public virtual IEnumerator TearDown()
    {
        var terminateWeb3Task = Web3Unity.Instance.Disconnect();

        // Wait until for async task to finish
        yield return new WaitUntil(() => terminateWeb3Task.IsCompleted);
        
        _anvil?.Kill();
        
        Web3Unity.TestMode = false;
    }
}
