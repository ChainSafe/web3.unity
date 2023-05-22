using System.Threading.Tasks;
using ChainSafe.GamingSdk.Gelato.Relay;
using ChainSafe.GamingWeb3.Environment;
using Nethereum.Hex.HexTypes;
using Web3Unity.Scripts.Library.Ethers.Providers;
using Web3Unity.Scripts.Library.Ethers.Signers;

namespace ChainSafe.GamingSdk.Gelato
{
    public class GelatoClient
    {
        private IHttpClient httpClient;

        public GelatoClient(IHttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public void Post(RelayCall relayCall, Request request) 
        {
            // export const post = async <Request, Response>(
            // payload: { relayCall: RelayCall; request: Request },
            // config: Config
            // ): Promise<Response> => {
            // try {
            //     const { relayCall, request } = payload;
            //     let path: string;
            //     switch (relayCall) {
            //     case RelayCall.CallWithSyncFee:
            //         path = `${config.url}/relays/v2/call-with-sync-fee`;
            //         break;

            //     case RelayCall.CallWithSyncFeeERC2771:
            //         path = `${config.url}/relays/v2/call-with-sync-fee-erc2771`;
            //         break;

            //     case RelayCall.SponsoredCall:
            //         path = `${config.url}/relays/v2/sponsored-call`;
            //         break;

            //     case RelayCall.SponsoredCallERC2771:
            //         path = `${config.url}/relays/v2/sponsored-call-erc2771`;
            //         break;

            //     default: {
            //         const _exhaustiveCheck: never = relayCall;
            //         return _exhaustiveCheck;
            //     }
            //     }
            //     return (await axios.post(path, request)).data;
            // } catch (error) {
            //     throw new Error(getHttpErrorMessage(error));
            // }
            // };
        }

        public void GetSupportedNetworks()
        {
        }

        public void GetGelatoOracles()
        {
            // try {
            //     return (await axios.get(`${config.url}/oracles/`)).data.oracles;
            // } catch (error) {
            //     throw new Error(
            //     `GelatoRelaySDK/getGelatoOracles: Failed with error: ${getHttpErrorMessage(
            //         error
            //     )}`
            //     );
            // }
        }

        public void GetPaymentTokens(ulong chainId)
        {
            // try {
            //     return (
            //         await axios.get(`${config.url}/oracles/${payload.chainId}/paymentTokens/`)
            //     ).data.paymentTokens;
            // } catch (error) {
            //     throw new Error(
            //         `GelatoRelaySDK/getPaymentTokens: Failed with error: ${getHttpErrorMessage(
            //             error
            //         )}`
            //     );
            // }
        }

        public void GetEstimatedFee(ulong chainId, string paymentToken, HexBigInteger gasLimit, bool isHighPriority, HexBigInteger gasLimitL1 = null)
        {
            if (gasLimitL1 == null)
            {
                gasLimitL1 = new HexBigInteger("0x0");
            }

            // const { chainId, gasLimit, gasLimitL1, isHighPriority, paymentToken } =
            //     payload;
            // const params = {
            //     paymentToken,
            //     gasLimit: gasLimit.toString(),
            //     isHighPriority,
            //     gasLimitL1: gasLimitL1.toString(),
            // };
            // try {
            //     const res = await axios.get(`${config.url}/oracles/${chainId}/estimate`, {
            //     params,
            //     });
            //     return BigNumber.from(res.data.estimatedFee);
            // } catch (error) {
            //     throw new Error(
            //     `GelatoRelaySDK/getEstimatedFee: Failed with error: ${getHttpErrorMessage(
            //         error
            //     )}`
            //     );
            // }
        }

        public void GetTaskStatus(string taskId)
        {
            // try {
            //     return (await axios.get(`${config.url}/tasks/status/${payload.taskId}`))
            //         .data.task;
            // } catch (error) {
            //     throw new Error(
            //         `GelatoRelaySDK/getTaskStatus: Failed with error: ${getHttpErrorMessage(
            //             error
            //         )}`
            //     );
            // }
        }
    }
}