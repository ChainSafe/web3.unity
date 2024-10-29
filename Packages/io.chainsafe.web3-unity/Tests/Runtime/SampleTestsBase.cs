using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ChainSafe.Gaming;
using ChainSafe.Gaming.UnityPackage;
using ChainSafe.Gaming.Web3;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;
using Debug = UnityEngine.Debug;
using Object = UnityEngine.Object;

public class SampleTestsBase
{
    private const string Mnemonic = "test test test test test test test test test test test junk";

    private Process _anvil;

    private readonly List<ISample> _samples = new List<ISample>();
    
    [UnitySetUp]
    public IEnumerator SetUp()
    {
        #region Anvil

        _anvil = new Process()
        {
            StartInfo = new ProcessStartInfo("anvil", $"--fork-url https://rpc.ankr.com/eth_sepolia --mnemonic \"{Mnemonic}\" --silent")
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
            "Packages/io.chainsafe.web3-unity/Tests/Runtime/Prefabs/Web3Unity_Tests.prefab");

        Object.Instantiate(web3UnityPrefab);

        GameObject samplesPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(
            "Packages/io.chainsafe.web3-unity/Tests/Runtime/Prefabs/SDKCallSamples_Tests.prefab");

        foreach (var sample in samplesPrefab.GetComponentsInChildren<ISample>())
        {
            _samples.Add((ISample) Object.Instantiate((Object) sample));
        }
        
        Web3Unity.TestMode = true;
        
        Task initialize = Web3Unity.Instance.Initialize(false);
        
        yield return new WaitUntil(() => initialize.IsCompleted);
        
        Task connect = Web3Unity.Instance.Connect(ScriptableObject.CreateInstance<AnvilConnectionProvider>());
        
        yield return new WaitUntil(() => connect.IsCompleted);
    }

    [UnityTest]
    public IEnumerator TestSamples()
    {
        foreach (var sample in _samples)
        {
            if (sample.DependentServiceTypes.Any(t => Web3Unity.Web3.ServiceProvider.GetService(t) == null))
            {
                continue;
            }
            
            if (sample.GetType().FullName.ToLower().Contains("ipfs")
                || sample.GetType().FullName.ToLower().Contains("switchchain"))
            {
                continue;
            }
            
            var methods = sample.GetType().GetMethods().Where(m =>
                m.ReturnType == typeof(Task<string>)
                && m.GetParameters().Length == 0).ToArray();

            if (!methods.Any())
            {
                continue;
            }
            
            foreach (var method in methods)
            {
                var task = (Task<string>) method.Invoke(sample, null);

                yield return new WaitUntil(() => task.IsCompleted);

                if (!task.IsCompletedSuccessfully)
                {
                    if (task.Exception != null)
                    {
                        throw task.Exception;
                    }
                    
                    throw new Exception($"{sample.GetType().Name}.{method.Name} failed. {task.Status}");
                }
                
                Debug.Log($"{task.Result} \n {sample.Title} - {sample.GetType().Name}.{method.Name}");
            }
        }
    }
    
    [UnityTearDown]
    public virtual IEnumerator TearDown()
    {
        _anvil?.Kill();
        
        Web3Unity.TestMode = false;

        yield return null;
    }
}
