using System;
using System.Collections.Generic;
using Nethereum.ABI.EIP712;
using Newtonsoft.Json;
using UnityEngine.Scripting;
using WalletConnectSharp.Common.Utils;
using WalletConnectSharp.Network.Models;

namespace ChainSafe.Gaming.Wallets.WalletConnect.Methods
{
    [RpcMethod("eth_signTypedData")]
    [RpcRequestOptions(Clock.ONE_MINUTE, 99998)]
    public class EthSignTypedMessage<TTypedData> : List<TypedDataRequest<TTypedData>>
    {
        public EthSignTypedMessage(TTypedData message)
            : base(new TypedDataRequest<TTypedData>[]
            {
                new TypedDataRequest<TTypedData>(JsonConvert.SerializeObject(MemberDescriptionFactory.GetTypesMemberDescription(typeof(TTypedData))), message),
            })
        {
        }

        [Preserve] // Needed for JSON.NET serialization
        public EthSignTypedMessage()
        {
        }
    }
}