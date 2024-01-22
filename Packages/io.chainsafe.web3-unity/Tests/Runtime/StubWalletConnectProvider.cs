﻿using System.Threading.Tasks;
using ChainSafe.Gaming.WalletConnect;

namespace Tests.Runtime
{
    public class StubWalletConnectProvider : IWalletConnectProvider
    {
        private readonly StubWalletConnectProviderConfig config;

        public StubWalletConnectProvider(StubWalletConnectProviderConfig config)
        {
            this.config = config;
        }

        public Task<string> Connect()
        {
            return Task.FromResult(config.WalletAddress);
        }

        public Task Disconnect()
        {
            throw new System.NotImplementedException();
        }

        public Task<string> Request<T>(T data, long? expiry = null)
        {
            return Task.FromResult(config.StubResponse);
        }
    }
}