using ChainSafe.Gaming.Evm.Contracts;

namespace ChainSafe.Gaming.SygmaClient.Contracts
{
    public class FeeHandlerRouter
    {
        private const string Abi = "";
        private Contract contract;

        public FeeHandlerRouter(IContractBuilder cb, string address)
        {
            contract = cb.Build(Abi, address);
        }
    }
}