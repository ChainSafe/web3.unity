using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;
using Nethereum.Contracts.QueryHandlers.MultiCall;

namespace ChainSafe.GamingSDK.EVM.Web3.Core.Evm.MultiCall3.Dto
{
    public class MultiCallRequest<TFunctionMessage, TFunctionOutput> : MulticallInputOutput<TFunctionMessage, TFunctionOutput>
        where TFunctionMessage : FunctionMessage, new()
        where TFunctionOutput : IFunctionOutputDTO, new()
    {
        public MultiCallRequest(TFunctionMessage functionMessage, string contractAddressTarget)
            : base(functionMessage, contractAddressTarget)
        {
        }
    }
}