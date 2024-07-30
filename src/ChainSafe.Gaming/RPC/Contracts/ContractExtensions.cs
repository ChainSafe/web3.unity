using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using ChainSafe.Gaming.Evm.Transactions;
using ChainSafe.Gaming.Web3;

namespace ChainSafe.Gaming.Evm.Contracts
{
    public static class ContractExtensions
    {
        private static async void UsageExample() // TODO: move to documentation
        {
            Contract contract = null;

            // read static game data
            {
                var activeChaosOrbs = await contract.Call<int>("activeChaosOrbs");
            }

            // get team mate addresses
            {
                var teamId = 256;
                var teammateAddresses = await contract.Call<List<string>, BigInteger>("getTeammateAddresses", teamId);
                var blockedPlayerAddresses = new[] { "0x20934800uu9283798439873298dfu2f93" };
                var allowedTeammateAddresses = teammateAddresses.Except(blockedPlayerAddresses).ToList();
            }

            // block players
            {
                var blockAddresses = new[] { "0x20934800uu9283798439873298dfu2f93", "0x20934800uu9283798439873298dfu2f93" };
                await Send(contract, "blockPlayers", blockAddresses);
            }

            // greet friends, read their statuses
            {
                var friendIds = new[] { "0x20934800uu9283798439873298dfu2f93", "0x20934800uu9283798439873298dfu2f93" };
                var onlineOnly = false;
                var statuses = (await contract.SendAndGet<List<string>, string[], bool>("greetMany", friendIds, onlineOnly))
                    .ToList();
            }
        }

        /// <summary>
        /// Asynchronously calls a read-only smart contract method that returns single value.
        /// </summary>
        /// <typeparam name="TOut">The expected return type.</typeparam>
        /// <param name="contract">The contract instance on which the method is called.</param>
        /// <param name="methodName">The name of the method to call on the smart contract.</param>
        /// <param name="requestPrototype">Optional. The prototype for the transaction request.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the method return value of type <typeparamref name="TOut"/>.</returns>
        /// <exception cref="Web3Exception">Thrown if the response is empty or cannot be converted to the specified type.</exception>
        [Pure]
        public static async Task<TOut> Call<TOut>(
            this IContract contract,
            string methodName,
            TransactionRequest requestPrototype = null)
        {
            var response = await contract.Call(methodName, null, requestPrototype);
            AssertResponseNotEmpty(response);
            return AssertAndConvertSingle<TOut>(response);
        }

        /// <summary>
        /// Asynchronously calls a read-only smart contract method that returns single value.
        /// </summary>
        /// <typeparam name="TOut">The expected return type.</typeparam>
        /// <typeparam name="TIn1">The first input type.</typeparam>
        /// <param name="contract">The contract instance on which the method is called.</param>
        /// <param name="methodName">The name of the method to call on the smart contract.</param>
        /// <param name="in1">The first input value.</param>
        /// <param name="requestPrototype">Optional. The prototype for the transaction request.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the method return value of type <typeparamref name="TOut"/>.</returns>
        /// <exception cref="Web3Exception">Thrown if the response is empty or cannot be converted to the specified type.</exception>
        [Pure]
        public static async Task<TOut> Call<TOut, TIn1>(
            this IContract contract,
            string methodName,
            TIn1 in1,
            TransactionRequest requestPrototype = null)
        {
            var response = await contract.Call(methodName, new object[] { in1 }, requestPrototype);
            AssertResponseNotEmpty(response);
            return AssertAndConvertSingle<TOut>(response);
        }

        /// <summary>
        /// Asynchronously calls a read-only smart contract method that returns single value.
        /// </summary>
        /// <typeparam name="TOut">The expected return type.</typeparam>
        /// <typeparam name="TIn1">The first input type.</typeparam>
        /// <typeparam name="TIn2">The second input type.</typeparam>
        /// <param name="contract">The contract instance on which the method is called.</param>
        /// <param name="methodName">The name of the method to call on the smart contract.</param>
        /// <param name="in1">The first input value.</param>
        /// <param name="in2">The second input value.</param>
        /// <param name="requestPrototype">Optional. The prototype for the transaction request.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the method return value of type <typeparamref name="TOut"/>.</returns>
        /// <exception cref="Web3Exception">Thrown if the response is empty or cannot be converted to the specified type.</exception>
        [Pure]
        public static async Task<TOut> Call<TOut, TIn1, TIn2>(
            this IContract contract,
            string methodName,
            TIn1 in1,
            TIn2 in2,
            TransactionRequest requestPrototype = null)
        {
            var response = await contract.Call(methodName, new object[] { in1, in2 }, requestPrototype);
            AssertResponseNotEmpty(response);
            return AssertAndConvertSingle<TOut>(response);
        }

        /// <summary>
        /// Asynchronously calls a read-only smart contract method that returns single value.
        /// </summary>
        /// <typeparam name="TOut">The expected return type.</typeparam>
        /// <typeparam name="TIn1">The first input type.</typeparam>
        /// <typeparam name="TIn2">The second input type.</typeparam>
        /// <typeparam name="TIn3">The third input type.</typeparam>
        /// <param name="contract">The contract instance on which the method is called.</param>
        /// <param name="methodName">The name of the method to call on the smart contract.</param>
        /// <param name="in1">The first input value.</param>
        /// <param name="in2">The second input value.</param>
        /// <param name="in3">The third input value.</param>
        /// <param name="requestPrototype">Optional. The prototype for the transaction request.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the method return value of type <typeparamref name="TOut"/>.</returns>
        /// <exception cref="Web3Exception">Thrown if the response is empty or cannot be converted to the specified type.</exception>
        [Pure]
        public static async Task<TOut> Call<TOut, TIn1, TIn2, TIn3>(
            this IContract contract,
            string methodName,
            TIn1 in1,
            TIn2 in2,
            TIn3 in3,
            TransactionRequest requestPrototype = null)
        {
            var response = await contract.Call(methodName, new object[] { in1, in2, in3 }, requestPrototype);
            AssertResponseNotEmpty(response);
            return AssertAndConvertSingle<TOut>(response);
        }

        /// <summary>
        /// Asynchronously sends a transaction to a smart contract method that is not expected to have a return value.
        /// </summary>
        /// <param name="contract">The contract instance on which the transaction is sent.</param>
        /// <param name="methodName">The name of the smart contract method to which the transaction is sent.</param>
        /// <param name="requestPrototype">Optional. A prototype object for the transaction request. This can include transaction parameters such as gas limit, gas price, value, etc.</param>
        /// <returns>A task representing the asynchronous operation of sending the transaction.</returns>
        public static Task Send(
            this IContract contract,
            string methodName,
            TransactionRequest requestPrototype = null)
        {
            return contract.Send(methodName, null, requestPrototype);
        }

        /// <summary>
        /// Asynchronously sends a transaction to a smart contract method that is not expected to have a return value.
        /// </summary>
        /// <typeparam name="TIn1">The type of the first input parameter.</typeparam>
        /// <param name="contract">The contract instance on which the transaction is sent.</param>
        /// <param name="methodName">The name of the smart contract method to which the transaction is sent.</param>
        /// <param name="in1">The first input parameter.</param>
        /// <param name="requestPrototype">Optional. A prototype object for the transaction request. This can include additional parameters for the transaction such as gas limit, gas price, value, etc.</param>
        /// <returns>A task representing the asynchronous operation of sending the transaction.</returns>
        public static Task Send<TIn1>(
            this IContract contract,
            string methodName,
            TIn1 in1,
            TransactionRequest requestPrototype = null)
        {
            return contract.Send(methodName, new object[] { in1 }, requestPrototype);
        }

        /// <summary>
        /// Asynchronously sends a transaction to a smart contract method that is not expected to have a return value.
        /// </summary>
        /// <typeparam name="TIn1">The type of the first input parameter.</typeparam>
        /// <typeparam name="TIn2">The type of the second input parameter.</typeparam>
        /// <param name="contract">The contract instance on which the transaction is sent.</param>
        /// <param name="methodName">The name of the smart contract method to which the transaction is sent.</param>
        /// <param name="in1">The first input parameter.</param>
        /// <param name="in2">The second input parameter.</param>
        /// <param name="requestPrototype">Optional. A prototype object for the transaction request. This can include additional parameters for the transaction such as gas limit, gas price, value, etc.</param>
        /// <returns>A task representing the asynchronous operation of sending the transaction.</returns>
        public static Task Send<TIn1, TIn2>(
            this IContract contract,
            string methodName,
            TIn1 in1,
            TIn2 in2,
            TransactionRequest requestPrototype = null)
        {
            return contract.Send(methodName, new object[] { in1, in2 }, requestPrototype);
        }

        /// <summary>
        /// Asynchronously sends a transaction to a smart contract method that is not expected to have a return value.
        /// </summary>
        /// <typeparam name="TIn1">The type of the first input parameter.</typeparam>
        /// <typeparam name="TIn2">The type of the second input parameter.</typeparam>
        /// <typeparam name="TIn3">The type of the third input parameter.</typeparam>
        /// <param name="contract">The contract instance on which the transaction is sent.</param>
        /// <param name="methodName">The name of the smart contract method to which the transaction is sent.</param>
        /// <param name="in1">The first input parameter.</param>
        /// <param name="in2">The second input parameter.</param>
        /// <param name="in3">The third input parameter.</param>
        /// <param name="requestPrototype">Optional. A prototype object for the transaction request. This can include additional parameters for the transaction such as gas limit, gas price, value, etc.</param>
        /// <returns>A task representing the asynchronous operation of sending the transaction.</returns>
        public static Task Send<TIn1, TIn2, TIn3>(
            this IContract contract,
            string methodName,
            TIn1 in1,
            TIn2 in2,
            TIn3 in3,
            TransactionRequest requestPrototype = null)
        {
            return contract.Send(methodName, new object[] { in1, in2, in3 }, requestPrototype);
        }

        /// <summary>
        /// Asynchronously sends a transaction to a smart contract method that is expected to return a single value of a specified type.
        /// </summary>
        /// <typeparam name="TOut">The expected type of the return value from the smart contract method.</typeparam>
        /// <param name="contract">The contract instance on which the transaction is sent.</param>
        /// <param name="methodName">The name of the smart contract method to which the transaction is sent.</param>
        /// <param name="requestPrototype">Optional. A prototype object for the transaction request. This can include transaction parameters such as gas limit, gas price, value, etc., if necessary.</param>
        /// <returns>A task representing the asynchronous operation, with a result of the specified type <typeparamref name="TOut"/>, containing the return value from the smart contract method.</returns>
        /// <exception cref="Web3Exception">Thrown if the response from the smart contract is empty, or if the response cannot be converted to the specified type <typeparamref name="TOut"/>.</exception>
        public static async Task<TOut> SendAndGet<TOut>(
            this IContract contract,
            string methodName,
            TransactionRequest requestPrototype = null)
        {
            var response = await contract.Send(methodName, null, requestPrototype);
            AssertResponseNotEmpty(response);
            return AssertAndConvertSingle<TOut>(response);
        }

        /// <summary>
        /// Asynchronously sends a transaction to a smart contract method that is expected to return a single value of a specified type.
        /// </summary>
        /// <typeparam name="TOut">The expected type of the return value from the smart contract method.</typeparam>
        /// <typeparam name="TIn1">The type of the first input parameter.</typeparam>
        /// <param name="contract">The contract instance on which the transaction is sent.</param>
        /// <param name="methodName">The name of the smart contract method to which the transaction is sent.</param>
        /// <param name="in1">The first input parameter.</param>
        /// <param name="requestPrototype">Optional. A prototype object for the transaction request. This can include transaction parameters such as gas limit, gas price, value, etc., if necessary.</param>
        /// <returns>A task representing the asynchronous operation, with a result of the specified type <typeparamref name="TOut"/>, containing the return value from the smart contract method.</returns>
        /// <exception cref="Web3Exception">Thrown if the response from the smart contract is empty, or if the response cannot be converted to the specified type <typeparamref name="TOut"/>.</exception>
        public static async Task<TOut> SendAndGet<TOut, TIn1>(
            this IContract contract,
            string methodName,
            TIn1 in1,
            TransactionRequest requestPrototype = null)
        {
            var response = await contract.Send(methodName, new object[] { in1 }, requestPrototype);
            AssertResponseNotEmpty(response);
            return AssertAndConvertSingle<TOut>(response);
        }

        /// <summary>
        /// Asynchronously sends a transaction to a smart contract method that is expected to return a single value of a specified type.
        /// </summary>
        /// <typeparam name="TOut">The expected type of the return value from the smart contract method.</typeparam>
        /// <typeparam name="TIn1">The type of the first input parameter.</typeparam>
        /// <typeparam name="TIn2">The type of the second input parameter.</typeparam>
        /// <param name="contract">The contract instance on which the transaction is sent.</param>
        /// <param name="methodName">The name of the smart contract method to which the transaction is sent.</param>
        /// <param name="in1">The first input parameter.</param>
        /// <param name="in2">The second input parameter.</param>
        /// <param name="requestPrototype">Optional. A prototype object for the transaction request. This can include transaction parameters such as gas limit, gas price, value, etc., if necessary.</param>
        /// <returns>A task representing the asynchronous operation, with a result of the specified type <typeparamref name="TOut"/>, containing the return value from the smart contract method.</returns>
        /// <exception cref="Web3Exception">Thrown if the response from the smart contract is empty, or if the response cannot be converted to the specified type <typeparamref name="TOut"/>.</exception>
        public static async Task<TOut> SendAndGet<TOut, TIn1, TIn2>(
            this IContract contract,
            string methodName,
            TIn1 in1,
            TIn2 in2,
            TransactionRequest requestPrototype = null)
        {
            var response = await contract.Send(methodName, new object[] { in1, in2 }, requestPrototype);
            AssertResponseNotEmpty(response);
            return AssertAndConvertSingle<TOut>(response);
        }

        /// <summary>
        /// Asynchronously sends a transaction to a smart contract method that is expected to return a single value of a specified type.
        /// </summary>
        /// <typeparam name="TOut">The expected type of the return value from the smart contract method.</typeparam>
        /// <typeparam name="TIn1">The type of the first input parameter.</typeparam>
        /// <typeparam name="TIn2">The type of the second input parameter.</typeparam>
        /// <typeparam name="TIn3">The type of the third input parameter.</typeparam>
        /// <param name="contract">The contract instance on which the transaction is sent.</param>
        /// <param name="methodName">The name of the smart contract method to which the transaction is sent.</param>
        /// <param name="in1">The first input parameter.</param>
        /// <param name="in2">The second input parameter.</param>
        /// <param name="in3">The third input parameter.</param>
        /// <param name="requestPrototype">Optional. A prototype object for the transaction request. This can include transaction parameters such as gas limit, gas price, value, etc., if necessary.</param>
        /// <returns>A task representing the asynchronous operation, with a result of the specified type <typeparamref name="TOut"/>, containing the return value from the smart contract method.</returns>
        /// <exception cref="Web3Exception">Thrown if the response from the smart contract is empty, or if the response cannot be converted to the specified type <typeparamref name="TOut"/>.</exception>
        public static async Task<TOut> SendAndGet<TOut, TIn1, TIn2, TIn3>(
            this IContract contract,
            string methodName,
            TIn1 in1,
            TIn2 in2,
            TIn3 in3,
            TransactionRequest requestPrototype = null)
        {
            var response = await contract.Send(methodName, new object[] { in1, in2, in3 }, requestPrototype);
            AssertResponseNotEmpty(response);
            return AssertAndConvertSingle<TOut>(response);
        }

        private static void AssertResponseNotEmpty(object[] response)
        {
            if (response.Length == 0)
            {
                throw new Web3Exception("Response value is empty.");
            }
        }

        private static TOut AssertAndConvertSingle<TOut>(object[] response)
        {
            if (response[0] is not TOut result)
            {
                throw new Web3Exception(
                    $"Can't convert response value to {typeof(TOut).Name}. Actual type {response[0].GetType().Name}");
            }

            return result;
        }
    }
}