using System.Threading.Tasks;
using ChainSafe.GamingSdk.Gelato.Dto;
using Nethereum.Hex.HexTypes;

namespace ChainSafe.GamingSdk.Gelato.Types
{
    /// <summary>
    ///     The IGelato interface serves as the primary interface to interact with the Gelato relay services on the Ethereum
    ///     network.
    ///     Through this class, users can relay transactions, fetch fee estimates, and perform other relay-related operations.
    ///     It is designed to seamlessly integrate with the ChainSafe Gaming SDK and can be optionally initialized with a
    ///     signer to support signed requests.
    /// </summary>
    public interface IGelato
    {
        /// <summary>
        ///     Executes a relay call, taking into consideration a synchronized fee. This method provides an abstraction over
        ///     sending transactions that automatically adjust fees. This relay method does not require a user to sign the request.
        /// </summary>
        /// <param name="request">The details of the call request.</param>
        /// <returns>An instance of <see cref="RelayResponse" /> containing details about the transaction and its result.</returns>
        Task<RelayResponse> CallWithSyncFee(CallWithSyncFeeRequest request);

        /// <summary>
        ///     Executes a relay call conforming to the Erc2771 standard with synchronized fee. This is useful when interacting
        ///     with contracts that support the Erc2771 meta-transaction standard. This method requires a user to sign the request.
        /// </summary>
        /// <param name="request">The details of the call request based on the Erc2771 standard.</param>
        /// <returns>An instance of <see cref="RelayResponse" /> containing details about the transaction and its result.</returns>
        Task<RelayResponse> CallWithSyncFeeErc2771(CallWithSyncFeeErc2771Request request);

        /// <summary>
        ///     Executes a relay call conforming to the Erc2771 standard with synchronized fee, but with additional options for
        ///     specifying relaying criteria. This method requires a user to sign the request.
        /// </summary>
        /// <param name="request">The details of the call request based on the Erc2771 standard.</param>
        /// <param name="options">Additional relay request options for customizing the transaction.</param>
        /// <returns>An instance of <see cref="RelayResponse" /> containing details about the transaction and its result.</returns>
        Task<RelayResponse> CallWithSyncFeeErc2771(CallWithSyncFeeErc2771Request request, RelayRequestOptions options);

        /// <summary>
        ///     Initiates a sponsored relay call. This allows another entity to pay for the transaction fees, making the
        ///     transaction effectively free for the original sender. This method does not require the user to sign the request.
        /// </summary>
        /// <param name="request">The details of the sponsored call request.</param>
        /// <returns>An instance of <see cref="RelayResponse" /> containing details about the transaction and its result.</returns>
        Task<RelayResponse> SponsoredCall(SponsoredCallRequest request);

        /// <summary>
        ///     Executes a sponsored relay call conforming to the Erc2771 standard. This allows meta-transactions where the fee can
        ///     be sponsored by another entity. This method requires the user to sign the request.
        /// </summary>
        /// <param name="request">The details of the sponsored call request based on the Erc2771 standard.</param>
        /// <returns>An instance of <see cref="RelayResponse" /> containing details about the transaction and its result.</returns>
        Task<RelayResponse> SponsoredCallErc2771(SponsoredCallErc2771Request request);

        /// <summary>
        ///     Executes a sponsored relay call conforming to the Erc2771 standard with specific relaying criteria.
        ///     This method requires a user to sign the request.
        /// </summary>
        /// <param name="request">The details of the sponsored call request based on the Erc2771 standard.</param>
        /// <param name="options">Additional relay request options for customizing the transaction.</param>
        /// <returns>An instance of <see cref="RelayResponse" /> containing details about the transaction and its result.</returns>
        Task<RelayResponse> SponsoredCallErc2771(SponsoredCallErc2771Request request, RelayRequestOptions options);

        /// <summary>
        ///     Estimates the transaction fee based on the specified parameters.
        /// </summary>
        /// <param name="paymentToken">The token used for payment.</param>
        /// <param name="gasLimit">The gas limit for the transaction.</param>
        /// <param name="isHighPriority">Indicates if the transaction should be treated as high priority.</param>
        /// <param name="gasLimitL1">The gas limit for the Layer 1 network, if applicable.</param>
        /// <returns>A <see cref="HexBigInteger" /> representation of the estimated fee.</returns>
        Task<HexBigInteger> GetEstimatedFee(string paymentToken, HexBigInteger gasLimit, bool isHighPriority, HexBigInteger gasLimitL1);

        /// <summary>
        ///     Retrieves the current status of a relayed task based on its ID.
        /// </summary>
        /// <param name="taskId">The unique identifier of the relayed task.</param>
        /// <returns>An instance of <see cref="RelayedTask" /> containing details about the task's status.</returns>
        Task<RelayedTask> GetTaskStatus(string taskId);

        /// <summary>
        ///     Fetches the list of tokens that are supported as payment on the current network.
        /// </summary>
        /// <returns>An array of token addresses supported for payment.</returns>
        Task<string[]> GetPaymentTokens();

        /// <summary>
        ///     Determines if the Gelato relay service is currently disabled.
        /// </summary>
        /// <returns>A boolean value indicating if the Gelato relay service is disabled.</returns>
        bool GetGelatoDisabled();
    }
}