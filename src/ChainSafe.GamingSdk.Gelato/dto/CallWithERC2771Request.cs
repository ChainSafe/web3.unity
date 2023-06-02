using System;
using System.Threading.Tasks;
using ChainSafe.GamingSdk.Gelato.Types;
using Nethereum.Hex.HexTypes;
using Newtonsoft.Json;
using Web3Unity.Scripts.Library.Ethers.Providers;
using Web3Unity.Scripts.Library.Ethers.Signers;
using Contract = Web3Unity.Scripts.Library.Ethers.Contracts.Contract;

namespace ChainSafe.GamingSdk.Gelato.Relay
{
    public enum ERC2771Type
    {
        CallWithSyncFee,
        SponsoredCall,
    }

    public class CallWithErc2771Request
    {
        /// <summary>
        ///    QUANTITY - The transaction chain id.
        /// </summary>
        [JsonProperty(PropertyName = "chainId")]
        public HexBigInteger ChainId { get; set; }

        /// <summary>
        ///     DATA, 20 Bytes - The address the transaction is being sent to.
        /// </summary>
        [JsonProperty(PropertyName = "target")]
        public string Target { get; set; }

        /// <summary>
        ///     DATA - the data send along with the transaction.
        /// </summary>
        [JsonProperty(PropertyName = "data")]
        public string Data { get; set; }

        /// <summary>
        ///     DATA, 20 Bytes - the address of the token that is to be used for payment.
        /// </summary>
        [JsonProperty(PropertyName = "feeToken")]
        public string FeeToken { get; set; }

        /// <summary>
        ///     DATA - an optional boolean (default: true ) denoting what data you would prefer appended to the end of the calldata.
        /// </summary>
        [JsonProperty(PropertyName = "isRelayContext")]
        public bool IsRelayContext { get; set; }

        /// <summary>
        ///     DATA, 20 Bytes - the address of the user's EOA.
        /// </summary>
        [JsonProperty(PropertyName = "user")]
        public string User { get; set; }

        /// <summary>
        ///    QUANTITY - optional, this is a nonce similar to Ethereum nonces, stored in a local mapping on the relay contracts. It is used to enforce nonce ordering of relay calls, if the user requires it. Otherwise, this is an optional parameter and if not passed, the relay-SDK will automatically query on-chain for the current value.
        /// </summary>
        [JsonProperty(PropertyName = "userNonce")]
        public HexBigInteger UserNonce { get; set; }

        /// <summary>
        ///    QUANTITY - optional, the amount of time in seconds that a user is willing for the relay call to be active in the relay backend before it is dismissed.
        /// </summary>
        [JsonProperty(PropertyName = "userDeadline")]
        public HexBigInteger UserDeadline { get; set; }

        public CallWithErc2771Request MapRequestToStruct(CallWithERC2771RequestOptionalParameters overrides)
        {
            if (overrides.UserNonce == null && UserNonce == null)
            {
                throw new Exception("UserNonce is not found in the request, nor fetched");
            }

            if (overrides.UserDeadline == null && UserDeadline == null)
            {
                throw new Exception("UserDeadline is not found in the request, nor fetched");
            }

            CallWithErc2771Request newStruct = (CallWithErc2771Request)this.MemberwiseClone();

            newStruct.UserNonce = overrides.UserNonce ?? UserNonce;
            newStruct.UserDeadline = overrides.UserDeadline ?? UserDeadline;
            return newStruct;
        }
    }

    public class CallWithERC2771RequestOptionalParameters
    {
        /// <summary>
        ///    QUANTITY - optional, this is a nonce similar to Ethereum nonces, stored in a local mapping on the relay contracts. It is used to enforce nonce ordering of relay calls, if the user requires it. Otherwise, this is an optional parameter and if not passed, the relay-SDK will automatically query on-chain for the current value.
        /// </summary>
        [JsonProperty(PropertyName = "userNonce")]
        public HexBigInteger UserNonce { get; set; }

        /// <summary>
        ///    QUANTITY - optional, the amount of time in seconds that a user is willing for the relay call to be active in the relay backend before it is dismissed.
        /// </summary>
        [JsonProperty(PropertyName = "userDeadline")]
        public HexBigInteger UserDeadline { get; set; }

        public static async Task<CallWithERC2771RequestOptionalParameters> PopulateOptionalUserParameters(CallWithErc2771Request request, ERC2771Type type, IEvmSigner wallet, Config config)
        {
            return await PopulateOptionalUserParameters(request, type, wallet.Provider, config);
        }

        public static async Task<CallWithERC2771RequestOptionalParameters> PopulateOptionalUserParameters(CallWithErc2771Request request, ERC2771Type type, IEvmProvider provider, Config config)
        {
            var optionalParams = new CallWithERC2771RequestOptionalParameters();
            if (request.UserDeadline == null)
            {
                optionalParams.UserDeadline = CalculateDeadline();
            }

            if (request.UserNonce == null)
            {
                // Must be custom nonce from the relay contract
                optionalParams.UserNonce = await GetUserNonce(request.User, type, provider, config);
            }

            return optionalParams;
        }

        private static HexBigInteger GetGelatoRelayErc2771Address(ERC2771Type type, Config config)
        {
            switch (type)
            {
                case ERC2771Type.CallWithSyncFee:
                    return IsZkSync(config.ChainId)
                        ? config.Contract.RelayERC2771zkSync
                        : config.Contract.RelayERC2771;
                case ERC2771Type.SponsoredCall:
                    return IsZkSync(config.ChainId)
                        ? config.Contract.Relay1BalanceERC2771zkSync
                        : config.Contract.Relay1BalanceERC2771;
                default:
                    throw new Exception("incorrect relay option");
            }
        }

        private static bool IsZkSync(long chainId)
        {
            return chainId == 324 || chainId == 280;
        }

        private static async Task<HexBigInteger> GetUserNonce(string account, ERC2771Type type, IEvmProvider provider, Config config)
        {
            var contract = new Contract(GelatoClient.UserNonceAbi, GetGelatoRelayErc2771Address(type, config).ToString(), provider);
            var result = await contract.Call("userNonce", new object[] { account });
            return (HexBigInteger)result[0];
        }

        private static HexBigInteger CalculateDeadline()
        {
            // BigNumber.from(Math.floor(Date.now() / 1000) + GelatoClient.DEFAULT_DEADLINE_GAP).toString()
            var parsed = ((((DateTimeOffset)DateTime.UtcNow).ToUnixTimeSeconds() / 1000) + GelatoClient.DefaultDeadlineGap).ToString("X");
            return new HexBigInteger($"0x{parsed}");
        }
    }
}