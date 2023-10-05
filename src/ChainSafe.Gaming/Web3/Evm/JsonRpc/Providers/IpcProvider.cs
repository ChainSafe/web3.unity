using System;
using System.Threading.Tasks;
using ChainSafe.Gaming.Evm;
using ChainSafe.Gaming.Evm.Providers;
using ChainSafe.Gaming.Web3.Core;
using ChainSafe.Gaming.Web3.Environment;
using Nethereum.Hex.HexTypes;
using Nethereum.JsonRpc.Client;
using NJsonRpc = Nethereum.JsonRpc;

namespace ChainSafe.Gaming.Web3.Evm.JsonRpc.Providers
{
    public class IpcProvider : IRpcProvider, ILifecycleParticipant
    {
        private readonly IpcConfig config;
        private readonly Web3Environment environment;
        private readonly ChainRegistryProvider chainRegistryProvider;

        private Gaming.Evm.Network.Network network;

        public IpcProvider(
            IpcConfig config,
            Web3Environment environment,
            ChainRegistryProvider chainRegistryProvider,
            IChainConfig chainConfig)
        {
            this.chainRegistryProvider = chainRegistryProvider;
            this.environment = environment;
            this.config = config;

            if (string.IsNullOrEmpty(this.config.IpcPath))
            {
                this.config.IpcPath = chainConfig.Ipc;
            }
        }

        public Gaming.Evm.Network.Network LastKnownNetwork
        {
            get => network;
            protected set => network = value;
        }

        public async ValueTask WillStartAsync()
        {
            if (network is null || network.ChainId == 0)
            {
                network = await RefreshNetwork();
            }
        }

        public ValueTask WillStopAsync() => new(Task.CompletedTask);

        public async Task<Gaming.Evm.Network.Network> DetectNetwork()
        {
            // TODO: cache
            var chainIdHexString = await Perform<string>("eth_chainId");
            var chainId = new HexBigInteger(chainIdHexString).ToUlong();

            if (chainId <= 0)
            {
                throw new Web3Exception("Couldn't detect network");
            }

            var chain = await chainRegistryProvider.GetChain(chainId);
            return chain != null
                ? new Gaming.Evm.Network.Network { Name = chain.Name, ChainId = chainId }
                : new Gaming.Evm.Network.Network { Name = "Unknown", ChainId = chainId };
        }

        public async Task<Gaming.Evm.Network.Network> RefreshNetwork()
        {
            var currentNetwork = await DetectNetwork();

            if (network != null && network.ChainId == currentNetwork.ChainId)
            {
                return network;
            }

            network = currentNetwork;
            return network;
        }

        public async Task<T> Perform<T>(string method, params object[] parameters)
        {
            try
            {
                var ipcClient = new NJsonRpc.IpcClient.IpcClient(config.IpcPath);
                var request = new RpcRequest(Guid.NewGuid().ToString(), method, parameters);
                return await ipcClient.SendRequestAsync<T>(request);
            }
            catch (Exception ex)
            {
                throw new Web3Exception($"{method}: bad result from RPC communication", ex);
            }
        }
    }
}