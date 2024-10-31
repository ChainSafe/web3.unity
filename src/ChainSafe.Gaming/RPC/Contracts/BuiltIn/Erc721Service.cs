using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Numerics;
using System.Reflection;
using System.Threading.Tasks;
using ChainSafe.Gaming.Evm.Signers;
using ChainSafe.Gaming.Evm.Utils;
using ChainSafe.Gaming.MultiCall;
using TransactionReceipt = ChainSafe.Gaming.Evm.Transactions.TransactionReceipt;

namespace ChainSafe.Gaming.Evm.Contracts.BuiltIn
{
    public class Erc721Service
    {
        private readonly string abi;
        private readonly IContractBuilder contractBuilder;
        private readonly ISigner signer;
        private readonly IMultiCall multiCall;

        private readonly Dictionary<string, Erc721Contract> contractCache = new();

        private Erc721Service()
        {
            abi = AbiHelper.ReadAbiFromResources(Assembly.GetExecutingAssembly(), "ChainSafe.Gaming.Resources.erc-721-abi.json");
        }

        public Erc721Service(IContractBuilder contractBuilder)
            : this()
        {
            this.contractBuilder = contractBuilder;
        }

        public Erc721Service(IContractBuilder contractBuilder, ISigner signer)
            : this(contractBuilder)
        {
            this.signer = signer;
        }

        public Erc721Service(IContractBuilder contractBuilder, ISigner signer, IMultiCall multiCall)
            : this(contractBuilder, signer)
        {
            this.multiCall = multiCall;
        }

        /// <summary>
        /// Builds an ERC721 contract instance with the given address.
        /// </summary>
        /// <param name="address">The address of the ERC721 contract.</param>
        /// <returns>An instance of Erc721Contract.</returns>
        public Erc721Contract BuildContract(string address)
        {
            if (contractCache.TryGetValue(address, out var cachedContract))
            {
                return cachedContract;
            }

            var originalContract = contractBuilder.Build(abi, address);
            var contract = new Erc721Contract(originalContract, signer, multiCall);
            contractCache.Add(address, contract);
            return contract;
        }

        /// <summary>
        /// Retrieves the balance of a specific account address in a smart contract.
        /// </summary>
        /// <param name="contractAddress">The address of the contract to query.</param>
        /// <param name="accountAddress">The address of the account to get the balance for.</param>
        /// <returns>
        /// A task representing the asynchronous operation that returns the balance of the account.
        /// The balance is represented as an integer value.
        /// </returns>
        [Pure]
        public Task<int> GetBalanceOf(string contractAddress, string accountAddress)
            => BuildContract(contractAddress).GetBalanceOf(accountAddress);

        /// <summary>
        /// Retrieves the owner of a specified token in a given contract.
        /// </summary>
        /// <param name="contractAddress">The address of the contract where the token exists.</param>
        /// <param name="tokenId">The identifier of the token to retrieve the owner for.</param>
        /// <returns>
        /// A task containing the address of the owner of the specified token.
        /// </returns>
        [Pure]
        public Task<string> GetOwnerOf(string contractAddress, string tokenId)
            => BuildContract(contractAddress).GetOwnerOf(tokenId);

        /// <summary>
        /// Retrieves the owner of a token with the given contract address and token ID.
        /// </summary>
        /// <param name="contractAddress">The address of the contract where the token is stored.</param>
        /// <param name="tokenId">The ID of the token to get the owner of.</param>
        /// <returns>
        /// A <see cref="Task{TResult}"/> representing the async operation.
        /// The task result contains the address of the token's owner.
        /// </returns>
        [Pure]
        public Task<string> GetOwnerOf(string contractAddress, BigInteger tokenId)
            => BuildContract(contractAddress).GetOwnerOf(tokenId);

        /// <summary>
        /// Retrieves the owner of multiple tokens in a batch for a given contract address.
        /// </summary>
        /// <param name="contractAddress">The contract address of the tokens.</param>
        /// <param name="tokenIds">An array of token IDs to get the owners for.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains a list
        /// of <see cref="OwnerOfBatchModel"/> objects, where each object represents the owner
        /// of a specific token in the batch.
        /// </returns>
        [Pure]
        public Task<List<OwnerOfBatchModel>> GetOwnerOfBatch(string contractAddress, string[] tokenIds)
            => BuildContract(contractAddress).GetOwnerOfBatch(tokenIds);

        /// <summary>
        /// Retrieves the URI of a particular token specified by its contract address and token ID.
        /// </summary>
        /// <param name="contractAddress">The contract address of the token.</param>
        /// <param name="tokenId">The ID of the token to retrieve the URI for.</param>
        /// <returns>
        /// A task that represents the asynchronous operation.
        /// The task result contains the URI of the specified token.
        /// </returns>
        [Pure]
        public Task<string> GetUri(string contractAddress, string tokenId)
            => BuildContract(contractAddress).GetUri(tokenId);

        /// <summary>
        /// Mints a new token by calling the Mint function on a contract.
        /// </summary>
        /// <param name="contractAddress">The address of the contract to interact with.</param>
        /// <param name="uri">The URI of the token to be minted.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an array of objects representing the minted token.</returns>
        public Task<object[]> Mint(string contractAddress, string uri)
            => BuildContract(contractAddress).Mint(uri);

        /// <summary>
        /// Mints a new token by invoking the Mint method in a smart contract.
        /// </summary>
        /// <param name="contractAddress">The address of the smart contract.</param>
        /// <param name="uri">The URI of the token.</param>
        /// <param name="destinationAddress">The destination address where the minted token will be sent.</param>
        /// <returns>A Task object representing the asynchronous operation that returns an array of objects.</returns>
        public Task<object[]> Mint(string contractAddress, string uri, string destinationAddress)
            => BuildContract(contractAddress).Mint(uri, destinationAddress);

        /// <summary>
        /// Mints a new token by invoking the Mint method in a smart contract.
        /// </summary>
        /// <param name="contractAddress">The address of the smart contract.</param>
        /// <param name="uri">The URI of the token.</param>
        /// <returns>Receipt of the mint.</returns>
        public Task<TransactionReceipt> MintWithReceipt(string contractAddress, string uri)
            => BuildContract(contractAddress).MintWithReceipt(uri);

        /// <summary>
        /// Transfers a token to a specified account.
        /// </summary>
        /// <param name="contractAddress">The address of the contract.</param>
        /// <param name="toAccount">The address of the account to transfer the token to.</param>
        /// <param name="tokenId">The ID of the token to transfer.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation. The task result will be an array of objects.</returns>
        public Task<object[]> Transfer(string contractAddress, string toAccount, BigInteger tokenId)
            => BuildContract(contractAddress).Transfer(toAccount, tokenId);

        /// <summary>
        /// Transfers a token to the specified account.
        /// </summary>
        /// <param name="contractAddress">The address of the token contract.</param>
        /// <param name="toAccount">The account to transfer the token to.</param>
        /// <param name="tokenId">The ID of the token to transfer.</param>
        /// <returns>A task that represents the asynchronous transfer operation. The task result is an array of objects representing the transfer result.</returns>
        public Task<object[]> Transfer(string contractAddress, string toAccount, string tokenId)
            => BuildContract(contractAddress).Transfer(toAccount, tokenId);

        /// <summary>
        /// Transfers a token to the specified account.
        /// </summary>
        /// <param name="contractAddress">The address of the token contract.</param>
        /// <param name="toAccount">The account to transfer the token to.</param>
        /// <param name="tokenId">The ID of the token to transfer.</param>
        /// <returns>Receipt of the transaction.</returns>
        public Task<TransactionReceipt> TransferWithReceipt(string contractAddress, string toAccount, BigInteger tokenId)
            => BuildContract(contractAddress).TransferWithReceipt(toAccount, tokenId);
    }
}