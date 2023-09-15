using System;
using System.Threading.Tasks;
using ChainSafe.Gaming.Web3;
using ChainSafe.Gaming.Web3.Core;
using ChainSafe.Gaming.Web3.Environment;
using Nethereum.Hex.HexTypes;
using Nethereum.JsonRpc.Client;
using NJsonRpc = Nethereum.JsonRpc;

namespace ChainSafe.Gaming.Evm.Providers
{
    public class IpcClientProvider : IRpcProvider, ILifecycleParticipant
    {
        private readonly IpcClientConfig config;
        private readonly Web3Environment environment;
        private readonly ChainRegistryProvider chainRegistryProvider;

        private Network.Network network;

        public IpcClientProvider(
            IpcClientConfig config,
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

        public Network.Network LastKnownNetwork
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

        public async Task<Network.Network> DetectNetwork()
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
                ? new Network.Network { Name = chain.Name, ChainId = chainId }
                : new Network.Network { Name = "Unknown", ChainId = chainId };
        }

        public async Task<Network.Network> RefreshNetwork()
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