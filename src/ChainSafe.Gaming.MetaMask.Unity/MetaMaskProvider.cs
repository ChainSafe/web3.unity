using System;
using System.Threading.Tasks;
using ChainSafe.Gaming.Web3.Core;
using ChainSafe.Gaming.Web3.Environment;
using Nethereum.Hex.HexTypes;
using Nethereum.Unity.Metamask;
using UnityEngine;

namespace ChainSafe.Gaming.MetaMask.Unity
{
    public class MetaMaskProvider : IMetaMaskProvider, ILifecycleParticipant
    {
        private readonly ILogWriter logWriter;

        private readonly MetaMaskController metaMaskController;

        public MetaMaskProvider(ILogWriter logWriter)
        {
            this.logWriter = logWriter;

            metaMaskController = UnityEngine.Object.FindObjectOfType<MetaMaskController>();

            if (metaMaskController == null)
            {
                GameObject controllerObj = new GameObject(nameof(MetaMaskController), typeof(MetaMaskController));

                metaMaskController = controllerObj.GetComponent<MetaMaskController>();
            }

            metaMaskController.Initialize();
        }

        public async ValueTask WillStartAsync()
        {
            string address = await Connect();

            logWriter.Log($"Connected to MetaMask account {address}");
        }

        public async Task<string> Connect()
        {
            return await metaMaskController.Connect();
        }

        public Task<string> Request<T>(T data, long? expiry = null)
        {
            throw new NotImplementedException();
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