using System.Threading.Tasks;
using ChainSafe.Gaming.Evm.Transactions;
using Nethereum.Hex.HexTypes;

namespace ChainSafe.Gaming.Evm.Contracts.BuiltIn
{
    public abstract class BuiltInContract : IContract
    {
        internal BuiltInContract(Contract contract)
        {
            Original = contract;
        }

        protected Contract Original { get; private set; }

        public string Address => Original.Address;

        public virtual IContract Attach(string address) => Original.Attach(address);

        public virtual Task<object[]> Call(string method, object[] parameters = null, TransactionRequest overwrite = null) =>
            Original.Call(method, parameters, overwrite);

        public object[] Decode(string method, string output) => Original.Decode(method, output);

        public Task<object[]> Send(string method, object[] parameters = null, TransactionRequest overwrite = null) =>
            Original.Send(method, parameters, overwrite);

        public Task<(object[] response, TransactionReceipt receipt)> SendWithReceipt(
            string method,
            object[] parameters = null,
            TransactionRequest overwrite = null) =>
            Original.SendWithReceipt(method, parameters, overwrite);

        public Task<HexBigInteger>
            EstimateGas(string method, object[] parameters, TransactionRequest overwrite = null) =>
            Original.EstimateGas(method, parameters, overwrite);

        public string Calldata(string method, object[] parameters = null) => Original.Calldata(method, parameters);

        public Task<TransactionRequest> PrepareTransactionRequest(
            string method,
            object[] parameters,
            bool isReadCall = false,
            TransactionRequest overwrite = null)
            => Original.PrepareTransactionRequest(method, parameters, isReadCall, overwrite);
    }
}