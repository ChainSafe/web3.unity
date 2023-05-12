using System;
using System.Collections.Generic;
using System.Text;

namespace Web3Unity.Scripts.Library.Ethers.RPC
{
    // TODO: this is a temporary solution meant for demo purposes
    // only. It *could* also end up being the final solution under
    // very specific circumstances though.
    // TODO @Oleksandr: Remove this class when implementing the DI infrastructure.
    public static class RpcEnvironmentStore
    {
        internal static IRpcEnvironment Environment { get; set; }

        public static void Initialize(IRpcEnvironment environment)
        {
            if (Environment != null)
                throw new Exception("RPC environment is already initialized");

            Environment = environment;
        }
    }
}
