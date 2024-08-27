using System.Diagnostics.Contracts;
using System.Numerics;
using System.Threading.Tasks;
using ChainSafe.Gaming.Evm.Contracts.GasFees;
using ChainSafe.Gaming.Evm.Signers;
using ChainSafe.Gaming.Web3;

namespace ChainSafe.Gaming.Evm.Contracts.BuiltIn
{
    /// <summary>
    /// Represents an ERC20 contract.
    /// </summary>
    public class Erc20Contract : BuiltInContract
    {
        private ISigner signer;

        internal Erc20Contract(Contract contract, ISigner signer)
            : base(contract)
        {
            this.signer = signer;
        }

        /// <summary>
        /// Gets the name of the contract.
        /// </summary>
        /// <returns>The name of the contract as a string.</returns>
        [Pure]
        public async Task<string> GetName()
        {
            var response = await Call(EthMethods.Name);
            return response[0].ToString();
        }

        /// <summary>
        /// Retrieves the symbol of a contract.
        /// </summary>
        /// <returns>
        /// A string representing the symbol of the contract.
        /// </returns>
        [Pure]
        public async Task<string> GetSymbol()
        {
            var response = await Call(EthMethods.Symbol);
            return response[0].ToString();
        }

        /// <summary>
        /// Retrieves the balance of the current user.
        /// </summary>
        /// <returns>The balance of the current user.</returns>
        [Pure]
        public Task<BigInteger> GetBalanceOf()
        {
            EnsureSigner();
            return GetBalanceOf(signer.PublicAddress);
        }

        /// <summary>
        /// Retrieves the balance of the specified account.
        /// </summary>
        /// <param name="accountAddress">The address of the account for which to retrieve the balance.</param>
        /// <returns>The balance of the account as a <see cref="BigInteger"/>.</returns>
        [Pure]
        public async Task<BigInteger> GetBalanceOf(string accountAddress)
        {
            var response = await Call(
                EthMethods.BalanceOf,
                new object[] { accountAddress });

            return BigInteger.Parse(response[0].ToString());
        }

        /// <summary>
        /// Retrieves the number of decimal places of a token.
        /// </summary>
        /// <returns>The number of decimal places as a BigInteger.</returns>
        [Pure]
        public async Task<BigInteger> GetDecimals()
        {
            var response = await Call(EthMethods.Decimals);
            return BigInteger.Parse(response[0].ToString());
        }

        /// <summary>
        /// Gets the total supply of the contract.
        /// </summary>
        /// <returns>The total supply of the contract as a BigInteger.</returns>
        /// <remarks>
        /// This method sends a call to the ContractMethods.TotalSupply and returns the result as a BigInteger.
        /// </remarks>
        [Pure]
        public async Task<BigInteger> GetTotalSupply()
        {
            var response = await Call(EthMethods.TotalSupply);
            return BigInteger.Parse(response[0].ToString());
        }

        /// <summary>
        /// Mints a specified amount of tokens to the current user address.
        /// </summary>
        /// <param name="amount">The amount of tokens to mint.</param>
        /// <param name="gasFeeModifier">
        /// Optional. If <c>null</c>, the default is an instance of <see cref="NoGasFeeModifier"/>.
        /// Instantiate one of the gas fee modifiers if you want to customize the gas fees for a specific transaction.
        /// </param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an array of objects.</returns>
        public Task<object[]> Mint(BigInteger amount, IGasFeeModifier gasFeeModifier = null)
        {
            EnsureSigner();
            return Mint(amount, signer.PublicAddress, gasFeeModifier: gasFeeModifier);
        }

        /// <summary>
        /// Mint the specified amount of tokens to the specified account address.
        /// </summary>
        /// <param name="amount">The amount of tokens to mint.</param>
        /// <param name="destinationAddress">The account address to mint tokens to.</param>
        /// <returns>An array of objects representing the response after minting.</returns>
        public async Task<object[]> Mint(BigInteger amount, string destinationAddress, IGasFeeModifier gasFeeModifier = null)
        {
            var response = await Send(
                EthMethods.Mint,
                new object[] { destinationAddress, amount },
                gasFeeModifier: gasFeeModifier);

            return response;
        }

        /// <summary>
        /// Transfers a specified amount to the specified account address.
        /// </summary>
        /// <param name="destinationAddress">The address of the account to transfer the amount to.</param>
        /// <param name="amount">The amount to transfer.</param>
        /// <returns>An array of objects representing the response of the transfer operation.</returns>
        public async Task<object[]> Transfer(string destinationAddress, BigInteger amount, IGasFeeModifier gasFeeModifier = null)
        {
            var response = await Send(
                EthMethods.Transfer,
                new object[] { destinationAddress, amount },
                gasFeeModifier: gasFeeModifier);

            return response;
        }

        private void EnsureSigner()
        {
            if (signer is not null)
            {
                return;
            }

            throw new Web3Exception("Can't get player address. No Signer was provided during construction.");
        }
    }
}