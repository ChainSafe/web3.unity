using System.Collections.Generic;
using ChainSafe.Gaming.WalletConnect.Models;
using ChainSafe.Gaming.Web3.Core.Evm;
using WalletConnectSharp.Common.Utils;
using WalletConnectSharp.Network.Models;

namespace ChainSafe.Gaming.WalletConnect.Methods
{
    /// <summary>
    /// Sign Typed Data Wallet Connect Json RPC method params.
    /// </summary>
    /// <typeparam name="TStruct">Type of data to be signed.</typeparam>
    [RpcMethod("eth_signTypedData")]
    [RpcRequestOptions(Clock.ONE_MINUTE, 99998)]
    public class EthSignTypedData<TStruct> : List<object>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EthSignTypedData{TStruct}"/> class.
        /// </summary>
        /// <param name="address">Public address of signer.</param>
        /// <param name="domain">Serializable domain for Json RPC.</param>
        /// <param name="message">Typed Data to be signed.</param>
        public EthSignTypedData(string address, SerializableDomain domain, TStruct message)
            : base(new object[]
            {
                address,
                new TypedDataModel<TStruct>(domain, message),
            })
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EthSignTypedData{TStruct}"/> class used by json.net.
        /// Preserved for Unity using ChainSafe.Gaming.Unity/link.xml.
        /// </summary>
        public EthSignTypedData()
        {
        }
    }
}