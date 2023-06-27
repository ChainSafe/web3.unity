using System;
using System.Threading.Tasks;
using ChainSafe.Gaming.Evm.JsonRpcProvider;
using ChainSafe.Gaming.Unity.Migration;

namespace ChainSafe.Gaming.UnityPackage
{
    public sealed class RPC
    {
        private static RPC instance = null;
        private JsonRpcProvider provider;

        public static RPC GetInstance => instance ?? throw new Exception("RPC instance is not initialized");

        public static async Task InitializeInstance()
        {
            instance = new();
            instance.provider = await ProviderMigration.NewJsonRpcProviderAsync();
        }

        public JsonRpcProvider Provider() => provider;
    }
}
