using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Numerics;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using ChainSafe.Gaming.Evm.Signers;
using ChainSafe.Gaming.Evm.Transactions;
using ChainSafe.Gaming.Evm.Utils;

namespace ChainSafe.Gaming.Evm.Contracts.BuiltIn
{
    public class Erc20Service
    {
        private readonly string abi;
        private readonly IContractBuilder contractBuilder;
        private readonly ISigner signer;

        private readonly Dictionary<string, Erc20Contract> contractCache = new();

        private Erc20Service()
        {
            abi = AbiHelper.ReadAbiFromResources(Assembly.GetExecutingAssembly(), "ChainSafe.Gaming.Resources.erc-20-abi.json");
        }

        public Erc20Service(IContractBuilder contractBuilder)
            : this()
        {
            this.contractBuilder = contractBuilder;
        }

        public Erc20Service(IContractBuilder contractBuilder, ISigner signer)
            : this(contractBuilder)
        {
            this.signer = signer;
        }

        /// <summary>
        /// Builds an ERC20 contract instance with the given address.
        /// </summary>
        /// <param name="address">The address of the ERC20 contract.</param>
        /// <returns>An instance of Erc20Contract.</returns>
        public Erc20Contract BuildContract(string address)
        {
            if (contractCache.TryGetValue(address, out var cachedContract))
            {
                return cachedContract;
            }

            var originalContract = contractBuilder.Build(abi, address);
            var contract = new Erc20Contract(originalContract, signer);
            contractCache.Add(address, contract);
            return contract;
        }

        /// <summary>
        /// Retrieves the name of a contract.
        /// </summary>
        /// <param name="contractAddress">The address of the contract to retrieve the name.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the name of the contract.</returns>
        [Pure]
        public Task<string> GetName(string contractAddress) =>
            BuildContract(contractAddress).GetName();

        /// <summary>
        /// Retrieves the symbol of a contract based on its address.
        /// </summary>
        /// <param name="contractAddress">The address of the contract.</param>
        /// <returns>A task representing the asynchronous operation. The symbol of the contract.</returns>
        [Pure]
        public Task<string> GetSymbol(string contractAddress) =>
            BuildContract(contractAddress).GetSymbol();

        /// <summary>
        /// Gets the balance of a contract address.
        /// </summary>
        /// <param name="contractAddress">The contract address to get the balance of.</param>
        /// <returns>The balance of the contract address as a BigInteger.</returns>
        [Pure]
        public Task<BigInteger> GetBalanceOf(string contractAddress) =>
            BuildContract(contractAddress).GetBalanceOf();

        /// <summary>
        /// Retrieves the balance of an account in a specified contract.
        /// </summary>
        /// <param name="contractAddress">The address of the contract.</param>
        /// <param name="accountAddress">The address of the account.</param>
        /// <returns>The balance of the account as a <see cref="BigInteger"/> value.</returns>
        [Pure]
        public Task<BigInteger> GetBalanceOf(string contractAddress, string accountAddress) =>
            BuildContract(contractAddress).GetBalanceOf(accountAddress);

        /// <summary>
        /// Retrieves the number of decimal places for a given contract address.
        /// </summary>
        /// <param name="contractAddress">The address of the contract.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains a <see cref="BigInteger"/>
        /// representing the number of decimal places for the given contract address.
        /// </returns>
        [Pure]
        public Task<BigInteger> GetDecimals(string contractAddress) =>
            BuildContract(contractAddress).GetDecimals();

        /// <summary>
        /// Retrieves the total supply of a specified contract.
        /// </summary>
        /// <param name="contractAddress">The address of the contract to retrieve the total supply from.</param>
        /// <returns>The total supply of the specified contract as a <see cref="BigInteger"/> value.</returns>
        [Pure]
        public Task<BigInteger> GetTotalSupply(string contractAddress) =>
            BuildContract(contractAddress).GetTotalSupply();

        /// <summary>
        /// Mints the specified amount of tokens for the given contract address.
        /// </summary>
        /// <param name="contractAddress">The address of the contract.</param>
        /// <param name="amount">The amount of tokens to mint.</param>
        /// <returns>A task that represents the asynchronous operation. The result is an array of objects.</returns>
        public Task<object[]> Mint(string contractAddress, BigInteger amount) =>
            BuildContract(contractAddress).Mint(amount);

        /// <summary>
        /// Mints tokens by invoking the 'Mint' method of a smart contract.
        /// </summary>
        /// <param name="contractAddress">The address of the smart contract.</param>
        /// <param name="amount">The amount of tokens to mint.</param>
        /// <param name="destinationAddress">The address where the minted tokens should be sent.</param>
        /// <returns>A task representing the asynchronous operation. The task result is an array of objects.</returns>
        public Task<object[]> Mint(string contractAddress, BigInteger amount, string destinationAddress) =>
            BuildContract(contractAddress).Mint(amount, destinationAddress);

        /// <summary>
        /// Mints tokens by invoking the 'Mint' method of a smart contract.
        /// </summary>
        /// <param name="contractAddress">The address of the smart contract.</param>
        /// <param name="amount">The amount of tokens to mint.</param>
        /// <returns>Receipt of the mint.</returns>
        public Task<TransactionReceipt> MintWithReceipt(string contractAddress, BigInteger amount) =>
            BuildContract(contractAddress).MintWithReceipt(amount);

        /// <summary>
        /// Transfers a specified amount of tokens from the current user account to the specified destination address.
        /// </summary>
        /// <param name="contractAddress">The contract address from which the tokens are being transferred.</param>
        /// <param name="destinationAddress">The destination address to which the tokens are being transferred.</param>
        /// <param name="amount">The amount of tokens to be transferred.</param>
        /// <returns>
        /// A <see cref="Task"/> representing the asynchronous operation.
        /// </returns>
        public Task<object[]> Transfer(string contractAddress, string destinationAddress, BigInteger amount) =>
            BuildContract(contractAddress).Transfer(destinationAddress, amount);

        /// <summary>
        /// Transfers a specified amount of tokens from the current user account to the specified destination address.
        /// </summary>
        /// <param name="contractAddress">The contract address from which the tokens are being transferred.</param>
        /// <param name="destinationAddress">The destination address to which the tokens are being transferred.</param>
        /// <param name="amount">The amount of tokens to be transferred.</param>
        /// <returns>Receipt of the transfer.</returns>
        public Task<TransactionReceipt> TransferWithReceipt(string contractAddress, string destinationAddress, BigInteger amount) =>
            BuildContract(contractAddress).TransferWithReceipt(destinationAddress, amount);
    }
}