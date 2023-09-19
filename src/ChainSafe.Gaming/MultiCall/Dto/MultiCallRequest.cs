using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;
using Nethereum.Contracts.QueryHandlers.MultiCall;

namespace ChainSafe.Gaming.MultiCall.Dto
{
    public class MultiCallRequest<TFunctionMessage, TFunctionOutput> : MulticallInputOutput<TFunctionMessage, TFunctionOutput>, IMultiCallRequest
        where TFunctionMessage : FunctionMessage, new()
        where TFunctionOutput : IFunctionOutputDTO, new()
    {
        public MultiCallRequest(TFunctionMessage functionMessage, string contractAddressTarget)
            : base(functionMessage, contractAddressTarget)
        {
        }
    }
}