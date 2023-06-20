using System.Threading.Tasks;
using ChainSafe.GamingSdk.Gelato.Dto;
using Nethereum.Hex.HexTypes;
using Web3Unity.Scripts.Library.Ethers.Signers;

namespace ChainSafe.GamingSdk.Gelato.Types
{
    public interface IGelatoModule
    {
        Task<RelayResponse> CallWithSyncFee(CallWithSyncFeeRequest request);

        void CallWithSyncFeeErc2771(CallWithSyncFeeErc2771Request request, ISigner wallet, RelayRequestOptions options);

        Task<RelayResponse> SponsoredCall(SponsoredCallRequest request, string sponsorApiKey);

        void SponsoredCallErc2771(
            SponsoredCallErc2771Request request,
            ISigner wallet,
            string sponsorApiKey,
            RelayRequestOptions options);

        Task<HexBigInteger> GetEstimatedFee(
            ulong chainId,
            string paymentToken,
            HexBigInteger gasLimit,
            bool isHighPriority,
            HexBigInteger gasLimitL1);
    }
}