using System;
using System.Threading.Tasks;
using ChainSafe.Gaming.Evm.Signers;
using ChainSafe.Gaming.Web3.Core;
using ChainSafe.Gaming.Web3.Core.Evm;
using ChainSafe.Gaming.Web3.Environment;
using Nethereum.RPC.AccountSigning;
using Nethereum.Unity.Rpc;
using Newtonsoft.Json;

namespace ChainSafe.Gaming.MetaMask.Unity
{
    public class MetaMaskSigner : ISigner, ILifecycleParticipant
    {
        private readonly IMetaMaskProvider metaMaskProvider;
        private readonly ILogWriter logWriter;

        public MetaMaskSigner(IMetaMaskProvider metaMaskProvider, ILogWriter logWriter)
        {
            this.metaMaskProvider = metaMaskProvider;
            this.logWriter = logWriter;
        }

        public string Address { get; private set; }

        public async ValueTask WillStartAsync()
        {
            Address = await metaMaskProvider.Connect();

            logWriter.Log($"Connected to MetaMask account {Address}");
        }

        public Task<string> GetAddress()
        {
            return Task.FromResult(Address);
        }

        public async Task<string> SignMessage(string message)
        {
            return await metaMaskProvider.Request<string>("personal_sign", message, Address);
        }

        public async Task<string> SignTypedData<TStructType>(SerializableDomain domain, TStructType message)
        {
            SerializableTypedData<TStructType> typedData = new SerializableTypedData<TStructType>(domain, message);

            // MetaMask doesn't work with regular eth_signTypedData method, has to be eth_signTypedData_v4.
            return await metaMaskProvider.Request<string>("eth_signTypedData_v4", typedData, Address);
        }

        public ValueTask WillStopAsync()
        {
            return new ValueTask(Task.CompletedTask);
        }
    }
}