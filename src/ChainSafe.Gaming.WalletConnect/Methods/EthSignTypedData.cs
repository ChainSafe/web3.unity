using System;
using System.Collections.Generic;
using ChainSafe.Gaming.Web3.Core.Evm;
using Nethereum.ABI.EIP712;
using Newtonsoft.Json;
using UnityEngine.Scripting;
using WalletConnectSharp.Common.Utils;
using WalletConnectSharp.Network.Models;

namespace ChainSafe.Gaming.WalletConnect.Methods
{
    [RpcMethod("eth_signTypedData")]
    [RpcRequestOptions(Clock.ONE_MINUTE, 99998)]
    public class EthSignTypedData<TStruct> : List<object>
    {
        public EthSignTypedData(string address, SerializableDomain domain, TStruct message)
            : base(new object[]
            {
                address,
                new TypedData<TStruct>(domain, message),
            })
        {
        }

        [Preserve] // Needed for JSON.NET serialization
        public EthSignTypedData()
        {
        }
    }
}