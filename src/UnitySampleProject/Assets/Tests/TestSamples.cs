using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ChainSafe.Gaming.UnityPackage;
using ChainSafe.Gaming.Web3;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;
using Debug = UnityEngine.Debug;
using Object = UnityEngine.Object;

namespace ChainSafe.Gaming.Unity.Tests
{
    public class TestSamples
    {
        private const string Mnemonic = "test test test test test test test test test test test junk";

        private Process _anvil;
        
        private readonly List<ISample> _samples = new List<ISample>();

        private bool _initialized;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            // Initialize Anvil
#if UNITY_EDITOR_WIN
            _anvil = new Process()
            {
                StartInfo = new ProcessStartInfo("anvil",
                    $"--fork-url https://rpc.ankr.com/eth_sepolia --mnemonic \"{Mnemonic}\" --silent")
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

            // Give Anvil time to start
            Thread.Sleep(TimeSpan.FromSeconds(10));
#endif
            
            // Instantiate Web3Unity to connect via Anvil
            GameObject web3UnityPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(
                "Assets/Tests/Prefabs/Web3Unity_Tests.prefab");

            Object.Instantiate(web3UnityPrefab);

            GameObject samplesPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(
                "Assets/Tests/Prefabs/SDKCallSamples_Tests.prefab");

            //Instantiate all samples from the prefab
            foreach (ISample sample in samplesPrefab.GetComponentsInChildren<ISample>())
            {
                _samples.Add((ISample)Object.Instantiate((Object)sample));
            }

            Web3Unity.TestMode = true;
        }

        //Initialize Web3Unity and Connect via Anvil
        // Can't be in OneTimeSetUp since OneTimeSetUp doesn't support async properly!
        // More on it here https://discussions.unity.com/t/async-await-in-unittests/
        private IEnumerator Initialize()
        {
            Task initialize = Web3Unity.Instance.Initialize(false);

            yield return AssertTask(initialize);

            Task connect = Web3Unity.Instance.Connect(ScriptableObject.CreateInstance<AnvilConnectionProvider>());

            yield return AssertTask(connect);

            _initialized = true;
        }

        private IEnumerator AssertTask(Task task)
        {
            yield return new WaitUntil(() => task.IsCompleted);

            if (!task.IsCompletedSuccessfully)
            {
                if (task.Exception != null)
                {
                    throw task.Exception;
                }

                throw new Exception($"Task Failed {task.Status}");
            }
        }
        
        [UnityTest]
        public IEnumerator TestErc20Sample()
        {
            yield return TestSample<Erc20Sample>();
        }

        [UnityTest]
        public IEnumerator TestErc721Sample()
        {
            yield return TestSample<Erc721Sample>();
        }

        [UnityTest]
        public IEnumerator TestErc1155Sample()
        {
            yield return TestSample<Erc1155Sample>();
        }

        [UnityTest]
        public IEnumerator TestEvmSample()
        {
            yield return TestSample<EvmSample>();
        }

        // Gave it a high timeout because it's a long test (~60 seconds per method)
        [UnityTest]
        [Timeout(int.MaxValue)]
        public IEnumerator TestGelato()
        {
            // Add a wait time because we can't make too many requests consecutively
            // Standard rate limit for testnet is 1 requests per minute
            yield return TestSample<GelatoSample>(60f);
        }

        private IEnumerator TestSample<T>(float waitTimeInSeconds = 0) where T : class, ISample
        {
            //Initialize if not initialized
            if (!_initialized)
            {
                yield return Initialize();
            }

            T sample = GetSample<T>();

            if (sample.DependentServiceTypes.Any(t => Web3Unity.Web3.ServiceProvider.GetService(t) == null))
            {
                throw new Exception($"Dependent services for {typeof(T).Name} are not registered.");
            }

            // Get methods in samples with signature public|private Task<string> Method()
            var methods = sample.GetType().GetMethods().Where(m =>
                m.ReturnType == typeof(Task<string>)
                && m.GetParameters().Length == 0).ToArray();

            if (!methods.Any())
            {
                throw new Exception(
                    "No testable methods found. Ensure methods have a signature 'public|private Task<string> MethodName()'.");
            }

            float time = Time.realtimeSinceStartup;

            foreach (var method in methods)
            {
                var task = (Task<string>)method.Invoke(sample, null);

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

                // If there's a waitTime wait between method executions
                if (waitTimeInSeconds > 0)
                {
                    time -= Time.realtimeSinceStartup;

                    yield return new WaitForSeconds(Mathf.Clamp(waitTimeInSeconds - time, 0, waitTimeInSeconds));
                }
            }
        }

        private T GetSample<T>() where T : class, ISample
        {
            return _samples.Single(s => s is T) as T;
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            _anvil?.Kill();
            
            Web3Unity.TestMode = false;
        }
    }
}
