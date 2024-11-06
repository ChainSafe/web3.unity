using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using ChainSafe.Gaming.Evm.Signers;
using ChainSafe.Gaming.Evm.Transactions;
using ChainSafe.Gaming.Ipfs;
using ChainSafe.Gaming.MultiCall;
using ChainSafe.Gaming.Web3;
using Nethereum.Contracts.QueryHandlers.MultiCall;
using Nethereum.Hex.HexConvertors.Extensions;

namespace ChainSafe.Gaming.Evm.Contracts.BuiltIn
{
    /// <summary>
    /// Represents a default ERC721 contract.
    /// </summary>
    public class Erc721Contract : BuiltInContract
    {
        private readonly IMultiCall multiCall;
        private readonly ISigner signer;

        internal Erc721Contract(Contract contract, ISigner signer, IMultiCall multiCall)
            : base(contract)
        {
            this.signer = signer;
            this.multiCall = multiCall;
        }

        /// <summary>
        /// Gets the balance of the specified account address.
        /// </summary>
        /// <param name="accountAddress">The address of the account.</param>
        /// <returns>The balance of the account as an integer.</returns>
        [Pure]
        public async Task<int> GetBalanceOf(string accountAddress)
        {
            var response = await Call(EthMethods.BalanceOf, new object[] { accountAddress });
            return int.Parse(response[0].ToString());
        }

        /// <summary>
        /// Retrieves the owner of a token identified by the given token ID.
        /// </summary>
        /// <param name="tokenId">The ID of the token.</param>
        /// <returns>A Task object that represents the asynchronous operation. The result of the Task contains the owner of the token as a string.</returns>
        [Pure]
        public Task<string> GetOwnerOf(string tokenId) => GetOwnerOfInternal(tokenId);

        /// <summary>
        /// Retrieves the owner of a token identified by the given token ID.
        /// </summary>
        /// <param name="tokenId">The ID of the token.</param>
        /// <returns>A task that represents the asynchronous operation. The result of the task contains the owner's address as a string.</returns>
        [Pure]
        public Task<string> GetOwnerOf(BigInteger tokenId) => GetOwnerOfInternal(tokenId);

        [Pure]
        private async Task<string> GetOwnerOfInternal(object id)
        {
            var response = await Call(EthMethods.OwnerOf, new[] { id });
            return response[0].ToString();
        }

        /// <summary>
        /// Retrieves the owner of multiple tokens in batch.
        /// </summary>
        /// <param name="tokenIds">An array of token IDs.</param>
        /// <returns>A list of OwnerOfBatchModel objects representing the owners of the tokens.</returns>
        /// <exception cref="Web3Exception">Thrown when the multiCall component was not provided during construction.</exception>
        /// <remarks>
        /// This method internally uses the multiCall component to execute multiple calls in parallel for better performance.
        /// Each token ID is used to build a call, which is then sent to the multiCall component.
        /// The response from multiCall is processed to extract the owner information for each token ID.
        /// If a call fails, the respective OwnerOfBatchModel object will have the Failure property set to true.
        /// </remarks>
        [Pure]
        public async Task<List<OwnerOfBatchModel>> GetOwnerOfBatch(string[] tokenIds)
        {
            if (multiCall == null)
            {
                throw new Web3Exception(
                    $"Can't execute {nameof(GetOwnerOfBatch)}. No MultiCall component was provided during construction.");
            }

            var calls = tokenIds
                .Select(BuildCall)
                .ToList();

            var multiCallResponse = await multiCall.MultiCallAsync(calls.ToArray());

            return multiCallResponse
                .Select(BuildResult)
                .ToList();

            Call3Value BuildCall(string tokenId)
            {
                object param = tokenId.StartsWith("0x") ? tokenId : BigInteger.Parse(tokenId);
                var callData = Calldata(EthMethods.OwnerOf, new[] { param });
                return new Call3Value { Target = Original.Address, AllowFailure = true, CallData = callData.HexToByteArray(), };
            }

            OwnerOfBatchModel BuildResult(Result result, int index)
            {
                if (result is not { Success: true })
                {
                    return new OwnerOfBatchModel { Failure = true };
                }

                var owner = Decode(EthMethods.OwnerOf, result.ReturnData.ToHex());
                return new OwnerOfBatchModel { TokenId = tokenIds[index], Owner = owner[0].ToString() };
            }
        }

        /// <summary>
        /// Retrieves the URI associated with a given token ID.
        /// </summary>
        /// <param name="tokenId">The token ID for which to retrieve the URI.</param>
        /// <returns>
        /// A <see cref="Task"/> representing the asynchronous operation. The task result contains the URI associated with the token ID.
        /// </returns>
        [Pure]
        public async Task<string> GetUri(string tokenId)
        {
            if (IpfsHelper.CanDecodeTokenIdToUri(tokenId))
            {
                return IpfsHelper.DecodeTokenIdToUri(tokenId);
            }

            var response = await Original.Call(EthMethods.TokenUri, new object[] { tokenId });
            var uri = response[0].ToString();
            return uri;
        }

        /// <summary>
        /// Mint a new token with the given URI.
        /// </summary>
        /// <param name="uri">The URI of the new token.</param>
        /// <returns>An array of objects representing the response from the mint operation.</returns>
        /// <exception cref="Web3Exception">Thrown if no signer was provided during construction.</exception>
        public async Task<object[]> Mint(string uri) // todo review if still relevant
        {
            if (signer == null)
            {
                throw new Web3Exception("Can't mint to the player address. No signer was provided during construction.");
            }

            var parameters = new object[] { signer.PublicAddress, uri };
            var response = await Send(EthMethods.SafeMint, parameters);
            return response;
        }

        /// <summary>
        /// Mint method to safely mint an object.
        /// </summary>
        /// <param name="uri">The URI of the object to be minted.</param>
        /// <param name="destinationAddress">The address where the minted object will be transferred.</param>
        /// <returns>An array of objects representing the response from the mint operation.</returns>
        public async Task<object[]> Mint(string uri, string destinationAddress) // todo review if still relevant
        {
            var parameters = new object[] { destinationAddress, uri };
            var response = await Send(EthMethods.SafeMint, parameters);
            return response;
        }

        /// <summary>
        /// Mint method to safely mint an object.
        /// </summary>
        /// <param name="uri">The URI of the object to be minted.</param>
        /// <returns>Receipt of the mint.</returns>
        public async Task<TransactionReceipt> MintWithReceipt(string uri)
        {
            if (signer == null)
            {
                throw new Web3Exception("Can't mint to the player address. No signer was provided during construction.");
            }

            var parameters = new object[] { signer.PublicAddress, uri };
            var response = await SendWithReceipt(EthMethods.SafeMint, parameters);
            return response.receipt;
        }

        /// <summary>
        /// Transfers the specified token to the given account.
        /// </summary>
        /// <param name="toAccount">The account to transfer the token to.</param>
        /// <param name="tokenId">The ID of the token to transfer.</param>
        /// <returns>A Task object representing the asynchronous operation. The result of the task is an array of objects.</returns>
        public Task<object[]> Transfer(string toAccount, BigInteger tokenId)
        {
            return Transfer(toAccount, tokenId.ToString());
        }

        /// <summary>
        /// Transfers a token to the specified account.
        /// </summary>
        /// <param name="toAccount">The address of the account to which the token will be transferred.</param>
        /// <param name="tokenId">The unique identifier of the token.</param>
        /// <returns>An array of objects representing the response from the transfer operation.</returns>
        public async Task<object[]> Transfer(string toAccount, string tokenId)
        {
            if (signer == null)
            {
                throw new Web3Exception("Can't transfer to the player address. No signer was provided during construction.");
            }

            var parameters = new object[] { signer.PublicAddress, toAccount, tokenId };
            var response = await Send(EthMethods.SafeTransferFrom, parameters);
            return response;
        }

        /// <summary>
        /// Transfers a token to the specified account.
        /// </summary>
        /// <param name="to">The address of the account to which the token will be transferred.</param>
        /// <param name="tokenId">The unique identifier of the token.</param>
        /// <returns>Receipt of the transfer.</returns>
        public async Task<TransactionReceipt> TransferWithReceipt(string to, BigInteger tokenId)
        {
            if (signer == null)
            {
                throw new Web3Exception("Can't transfer to the player address. No signer was provided during construction.");
            }

            var response = await SendWithReceipt(EthMethods.SafeTransferFrom, new object[] { signer.PublicAddress, to, tokenId });

            return response.receipt;
        }
    }
}