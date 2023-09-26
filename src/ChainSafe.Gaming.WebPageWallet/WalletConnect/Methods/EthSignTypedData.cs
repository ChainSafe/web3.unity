using System;
using System.Collections.Generic;
using ChainSafe.Gaming.Web3.Core.Evm;
using Nethereum.ABI.EIP712;
using Newtonsoft.Json;
using UnityEngine.Scripting;
using WalletConnectSharp.Common.Utils;
using WalletConnectSharp.Network.Models;

namespace ChainSafe.Gaming.Wallets.WalletConnect.Methods
{
    [RpcMethod("eth_signTypedData")]
    [RpcRequestOptions(Clock.ONE_MINUTE, 99998)]
    public class EthSignTypedData<TTypedData> : List<string>
    {
        public EthSignTypedData(string address, SerializableDomain domain, TTypedData message)
            : base(new string[]
            {
                address,
                JsonConvert.SerializeObject(new TypedDataRequest<TTypedData>(MemberDescriptionFactory.GetTypesMemberDescription(typeof(TTypedData)), domain, message)),
            })
        {
        }

        [Preserve] // Needed for JSON.NET serialization
        public EthSignTypedData()
        {
        }
    }
}