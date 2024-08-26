using System.Threading.Tasks;
using ChainSafe.Gaming.Evm.Contracts;
using ChainSafe.Gaming.Evm.Transactions;
using Nethereum.Hex.HexTypes;

namespace ChainSafe.Gaming.Mud.Systems
{
    /// <summary>
    /// Represents a MUD systems client, which is a wrapper class that extends the functionality of a Contract and adds namespace support.
    /// </summary>
    /// <remarks>
    /// Use this to call system functions.
    /// You don't need to specify namespace when calling a function, just use the function name.
    /// </remarks>
    public class MudSystems : IContract
    {
        private readonly string ns;

        private readonly Contract contract;

        /// <summary>
        /// Initializes a new instance of the <see cref="MudSystems"/> class.
        /// </summary>
        /// <param name="namespace">The namespace to be used.</param>
        /// <param name="contract">The contract to be used.</param>
        public MudSystems(string @namespace, Contract contract)
        {
            this.contract = contract;
            ns = @namespace;
        }

        public string Address => contract.Address;

        public IContract Attach(string address)
        {
            return contract.Attach(address);
        }

        public Task<object[]> Call(string method, object[] parameters = null, TransactionRequest overwrite = null)
        {
            return contract.Call(MudUtils.NamespaceFunctionName(ns, method), parameters, overwrite);
        }

        public object[] Decode(string method, string output)
        {
            return contract.Decode(MudUtils.NamespaceFunctionName(ns, method), output);
        }

        public Task<object[]> Send(string method, object[] parameters = null, TransactionRequest overwrite = null)
        {
            return contract.Send(MudUtils.NamespaceFunctionName(ns, method), parameters, overwrite);
        }

        public Task<(object[] response, TransactionReceipt receipt)> SendWithReceipt(string method, object[] parameters = null, TransactionRequest overwrite = null)
        {
            return contract.SendWithReceipt(MudUtils.NamespaceFunctionName(ns, method), parameters, overwrite);
        }

        public Task<HexBigInteger> EstimateGas(string method, object[] parameters, TransactionRequest overwrite = null)
        {
            return contract.EstimateGas(
                MudUtils.NamespaceFunctionName(ns, method),
                parameters,
                overwrite);
        }

        public string Calldata(string method, object[] parameters = null)
        {
            return contract.Calldata(MudUtils.NamespaceFunctionName(ns, method), parameters);
        }

        public Task<TransactionRequest> PrepareTransactionRequest(
            string method,
            object[] parameters,
            bool isReadCall = false,
            TransactionRequest overwrite = null)
        {
            return contract.PrepareTransactionRequest(
                MudUtils.NamespaceFunctionName(ns, method),
                parameters,
                isReadCall,
                overwrite);
        }
    }
}