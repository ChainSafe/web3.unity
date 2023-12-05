using System;
using System.Threading.Tasks;
using ChainSafe.Gaming.Web3.Core;
using ChainSafe.Gaming.Web3.Environment;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ChainSafe.Gaming.MetaMask.Unity
{
    public class MetaMaskProvider : IMetaMaskProvider, ILifecycleParticipant
    {
        private readonly ILogWriter logWriter;

        private readonly MetaMaskController metaMaskController;

        public MetaMaskProvider(ILogWriter logWriter)
        {
            this.logWriter = logWriter;

            if (Application.isEditor || Application.platform != RuntimePlatform.WebGLPlayer)
            {
                this.logWriter.LogError("You need to build to WebGL platform to run Nethereum.Metamask.Unity");

                return;
            }

            metaMaskController = Object.FindObjectOfType<MetaMaskController>();

            if (metaMaskController == null)
            {
                GameObject controllerObj = new GameObject(nameof(MetaMaskController), typeof(MetaMaskController));

                metaMaskController = controllerObj.GetComponent<MetaMaskController>();
            }

            Object.DontDestroyOnLoad(metaMaskController.gameObject);

            metaMaskController.Initialize(logWriter);
        }

        public ValueTask WillStartAsync()
        {
            return new ValueTask(Task.CompletedTask);
        }

        public async Task<string> Connect()
        {
            return await metaMaskController.Connect();
        }

        public async Task<T> Request<T>(string method, params object[] parameters)
        {
            return await metaMaskController.Request<T>(method, parameters);
        }

        public Task Disconnect()
        {
            throw new NotImplementedException();
        }

        public ValueTask WillStopAsync()
        {
            return new ValueTask(Task.CompletedTask);
        }
    }
}