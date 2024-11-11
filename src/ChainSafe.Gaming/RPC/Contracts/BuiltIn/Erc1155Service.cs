using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Numerics;
using System.Reflection;
using System.Threading.Tasks;
using ChainSafe.Gaming.Evm.Signers;
using ChainSafe.Gaming.Evm.Transactions;
using ChainSafe.Gaming.Evm.Utils;

namespace ChainSafe.Gaming.Evm.Contracts.BuiltIn
{
    public class Erc1155Service
    {
        private readonly string abi;
        private readonly IContractBuilder contractBuilder;
        private readonly ISigner signer;

        private readonly Dictionary<string, Erc1155Contract> contractCache = new();

        private Erc1155Service()
        {
            abi = AbiHelper.ReadAbiFromResources(Assembly.GetExecutingAssembly(), "ChainSafe.Gaming.Resources.erc-1155-abi.json");
        }

        public Erc1155Service(IContractBuilder contractBuilder)
            : this()
        {
            this.contractBuilder = contractBuilder;
        }

        public Erc1155Service(IContractBuilder contractBuilder, ISigner signer)
            : this(contractBuilder)
        {
            this.signer = signer;
        }

        /// <summary>
        /// Builds an ERC1155 contract instance.
        /// </summary>
        /// <param name="address">The address of the ERC1155 contract.</param>
        /// <returns>An instance of Erc1155Contract.</returns>
        public Erc1155Contract BuildContract(string address)
        {
            if (contractCache.TryGetValue(address, out var cachedContract))
            {
                return cachedContract;
            }

            var originalContract = contractBuilder.Build(abi, address);
            var contract = new Erc1155Contract(originalContract, signer);
            contractCache.Add(address, contract);
            return contract;
        }

        /// <summary>
        /// Retrieves the balance of the current user for a given token in a smart contract.
        /// </summary>
        /// <param name="contractAddress">The address of the token contract.</param>
        /// <param name="tokenId">The ID of the token to retrieve the balance for.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains the balance of the token
        /// as a <see cref="BigInteger"/> object.
        /// </returns>
        [Pure]
        public Task<BigInteger> GetBalanceOf(string contractAddress, string tokenId) =>
            BuildContract(contractAddress).GetBalanceOf(tokenId);

        /// <summary>
        /// Retrieves the balance of a specific account for a given token in a smart contract.
        /// </summary>
        /// <param name="contractAddress">The address of the smart contract.</param>
        /// <param name="tokenId">The ID of the token.</param>
        /// <param name="accountAddress">The address of the account.</param>
        /// <returns>
        /// A task that represents the asynchronous operation.
        /// The task result contains the balance of the specified account for the given token.
        /// </returns>
        [Pure]
        public Task<BigInteger> GetBalanceOf(string contractAddress, string tokenId, string accountAddress) =>
            BuildContract(contractAddress).GetBalanceOf(tokenId, accountAddress);

        /// <summary>
        /// Retrieves the balance of multiple accounts for multiple tokens from a given smart contract.
        /// </summary>
        /// <param name="contractAddress">The address of the smart contract.</param>
        /// <param name="accountAddresses">An array of account addresses to retrieve the balance for.</param>
        /// <param name="tokenIds">An array of token IDs for which to retrieve the balance.</param>
        /// <returns>A list of BigInteger values representing the balance of each account for each token.</returns>
        [Pure]
        public Task<List<BigInteger>> GetBalanceOfBatch(string contractAddress, string[] accountAddresses, string[] tokenIds) =>
            BuildContract(contractAddress).GetBalanceOfBatch(accountAddresses, tokenIds);

        /// <summary>
        /// Retrieves the URI (Uniform Resource Identifier) for the specified token.
        /// </summary>
        /// <param name="contractAddress">The address of the contract.</param>
        /// <param name="tokenId">The unique identifier of the token.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the URI of the token.</returns>
        [Pure]
        public Task<string> GetUri(string contractAddress, string tokenId) =>
            BuildContract(contractAddress).GetUri(tokenId);

        /// <summary>
        /// Mints a specified amount of tokens with the given tokenId of a specified contract address.
        /// </summary>
        /// <param name="contractAddress">The address of the contract.</param>
        /// <param name="tokenId">The ID of the token.</param>
        /// <param name="amount">The amount of tokens to mint.</param>
        /// <param name="data">Additional data to include in the transaction (optional).</param>
        /// <returns>
        /// A task that represents the asynchronous minting operation.
        /// The task result contains an array of objects with information about the minting operation.
        /// </returns>
        public Task<object[]> Mint(string contractAddress, BigInteger tokenId, BigInteger amount, byte[] data = null) =>
            BuildContract(contractAddress).Mint(tokenId, amount, data);

        /// <summary>
        /// Mints a specified amount of tokens with the given tokenId of a specified contract address.
        /// </summary>
        /// <param name="contractAddress">The address of the contract.</param>
        /// <param name="tokenId">The ID of the token.</param>
        /// <param name="amount">The amount of tokens to mint.</param>
        /// <param name="data">Additional data to include in the transaction (optional).</param>
        /// <returns>
        /// Receipt of the mint.
        /// </returns>
        public Task<TransactionReceipt> MintWithReceipt(string contractAddress, BigInteger tokenId, BigInteger amount, byte[] data = null) =>
            BuildContract(contractAddress).MintWithReceipt(tokenId, amount, data);

        /// <summary>
        /// Transfers an amount of tokens from the curren user account to the specified destination address.
        /// </summary>
        /// <param name="contractAddress">The address of the token contract.</param>
        /// <param name="tokenId">The ID of the token.</param>
        /// <param name="amount">The amount of tokens to transfer.</param>
        /// <param name="destinationAddress">The address where the tokens will be transferred to.</param>
        /// <returns>A task that represents the asynchronous transfer operation.</returns>
        public Task<object[]> Transfer(string contractAddress, BigInteger tokenId, BigInteger amount, string destinationAddress) =>
            BuildContract(contractAddress).Transfer(tokenId, amount, destinationAddress);

        /// <summary>
        /// Transfers an amount of tokens from the curren user account to the specified destination address.
        /// </summary>
        /// <param name="contractAddress">The address of the token contract.</param>
        /// <param name="tokenId">The ID of the token.</param>
        /// <param name="amount">The amount of tokens to transfer.</param>
        /// <param name="destinationAddress">The address where the tokens will be transferred to.</param>
        /// <returns>Receipt of the transfer.</returns>
        public Task<TransactionReceipt> TransferWithReceipt(string contractAddress, BigInteger tokenId, BigInteger amount, string destinationAddress) =>
            BuildContract(contractAddress).TransferWithReceipt(tokenId, amount, destinationAddress);
    }
}