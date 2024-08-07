using System.Threading.Tasks;
using ChainSafe.Gaming.Evm.Transactions;
using Nethereum.Hex.HexTypes;

namespace ChainSafe.Gaming.Evm.Contracts
{
    public interface IContract
    {
        /// <summary>
        /// The address of the contract.
        /// </summary>
        string Address { get; }

        /// <summary>
        /// Returns a new instance of the Contract attached to a new address. This is useful
        /// if there are multiple similar or identical copies of a Contract on the network
        /// and you wish to interact with each of them.
        /// </summary>
        /// <param name="address">Address of the contract to attach to.</param>
        /// <returns>The new contract.</returns>
        IContract Attach(string address);

        /// <summary>
        /// Call the contract method.
        /// </summary>
        /// <param name="method">The method to call.</param>
        /// <param name="parameters">The parameters for the method.</param>
        /// <param name="overwrite">To overwrite a transaction request.</param>
        /// <returns>The result of calling the method.</returns>
        Task<object[]> Call(string method, object[] parameters = null, TransactionRequest overwrite = null);

        /// <summary>
        /// Decodes a result.
        /// </summary>
        /// <param name="method">The method.</param>
        /// <param name="output">The raw output data from an executed function call.</param>
        /// <returns>The decoded outputs of a provided method.</returns>
        object[] Decode(string method, string output);

        /// <summary>
        /// Sends the transaction.
        /// </summary>
        /// <param name="method">The method.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="overwrite">An existing TransactionRequest to use instead of making a new one.</param>
        /// <returns>The outputs of the method.</returns>
        Task<object[]> Send(string method, object[] parameters = null, TransactionRequest overwrite = null);

        /// <summary>
        /// Sends the transaction.
        /// </summary>
        /// <param name="method">The method.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="overwrite">An existing TransactionRequest to use instead of making a new one.</param>
        /// <returns>The outputs of the method and the transaction receipt.</returns>
        Task<(object[] response, TransactionReceipt receipt)> SendWithReceipt(
            string method,
            object[] parameters = null,
            TransactionRequest overwrite = null);

        /// <summary>
        /// Estimate gas.
        /// </summary>
        /// <param name="method">The method.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="overwrite">An existing TransactionRequest to use instead of making a new one.</param>
        /// <returns>The estimated amount of gas.</returns>
        Task<HexBigInteger> EstimateGas(
            string method,
            object[] parameters,
            TransactionRequest overwrite = null);

        /// <summary>
        /// Create contract call data.
        /// </summary>
        /// <param name="method">The method.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>The contract call data.</returns>
        string Calldata(string method, object[] parameters = null);

        Task<TransactionRequest> PrepareTransactionRequest(string method, object[] parameters, bool isReadCall = false, TransactionRequest overwrite = null);
    }
}