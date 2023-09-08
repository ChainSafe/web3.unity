using System.Threading.Tasks;
using ChainSafe.GamingSdk.Gelato.Dto;
using Nethereum.Hex.HexTypes;

namespace ChainSafe.GamingSdk.Gelato.Types
{
    public interface IGelato
    {
        Task<RelayResponse> CallWithSyncFee(CallWithSyncFeeRequest request);

        Task<RelayResponse> CallWithSyncFeeErc2771(CallWithSyncFeeErc2771Request request);

        Task<RelayResponse> CallWithSyncFeeErc2771(CallWithSyncFeeErc2771Request request, RelayRequestOptions options);

        Task<RelayResponse> SponsoredCall(SponsoredCallRequest request);

        Task<RelayResponse> SponsoredCallErc2771(
            SponsoredCallErc2771Request request);

        Task<RelayResponse> SponsoredCallErc2771(
            SponsoredCallErc2771Request request,
            RelayRequestOptions options);

        Task<HexBigInteger> GetEstimatedFee(
            string paymentToken,
            HexBigInteger gasLimit,
            bool isHighPriority,
            HexBigInteger gasLimitL1);

        Task<RelayedTask> GetTaskStatus(string taskId);

        Task<string[]> GetPaymentTokens();

        bool GetGelatoDisabled();
    }
}