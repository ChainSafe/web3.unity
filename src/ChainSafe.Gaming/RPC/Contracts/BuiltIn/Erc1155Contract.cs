using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Numerics;
using System.Threading.Tasks;
using ChainSafe.Gaming.Evm.Signers;
using ChainSafe.Gaming.Evm.Transactions;
using ChainSafe.Gaming.Ipfs;
using ChainSafe.Gaming.Web3;

namespace ChainSafe.Gaming.Evm.Contracts.BuiltIn
{
    /// <summary>
    /// Represents a default ERC1155 contract.
    /// </summary>
    public class Erc1155Contract : BuiltInContract
    {
        private readonly ISigner signer;

        internal Erc1155Contract(Contract contract, ISigner signer)
            : base(contract)
        {
            this.signer = signer;
        }

        /// <summary>
        /// Retrieves the balance of a specific token for the current signer.
        /// </summary>
        /// <param name="tokenId">The ID of the token.</param>
        /// <returns>The balance of the specified token as a <see cref="BigInteger"/>.</returns>
        public Task<BigInteger> GetBalanceOf(string tokenId)
        {
            EnsureSigner();
            return GetBalanceOf(tokenId, signer.PublicAddress);
        }

        /// <summary>
        /// Retrieves the balance of a specific token for a given account address.
        /// </summary>
        /// <param name="tokenId">The ID of the token to query.</param>
        /// <param name="accountAddress">The account address to check the balance for.</param>
        /// <returns>The balance of the token as a <see cref="BigInteger"/> value.</returns>
        [Pure]
        public Task<BigInteger> GetBalanceOf(string tokenId, string accountAddress)
        {
            return Original.Call<BigInteger, string, string>(
                EthMethods.BalanceOf,
                accountAddress,
                tokenId);
        }

        /// <summary>
        /// Retrieves the balance of each specified account for multiple tokens.
        /// </summary>
        /// <param name="accountAddresses">An array of account addresses.</param>
        /// <param name="tokenIds">An array of token IDs.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a list of BigInteger objects representing the balances of each specified account for the corresponding token.</returns>
        [Pure]
        public Task<List<BigInteger>> GetBalanceOfBatch(string[] accountAddresses, string[] tokenIds) // TODO refine return value format
        {
            return Original.Call<List<BigInteger>, string[], string[]>(
                EthMethods.BalanceOfBatch,
                accountAddresses,
                tokenIds);
        }

        /// <summary>
        /// Retrieves the URI associated with a given token ID.
        /// </summary>
        /// <param name="tokenId">The token ID.</param>
        /// <returns>The URI associated with the token ID.</returns>
        [Pure]
        public async Task<string> GetUri(string tokenId)
        {
            if (IpfsHelper.CanDecodeTokenIdToUri(tokenId))
            {
                return IpfsHelper.DecodeTokenIdToUri(tokenId);
            }

            return await Original.Call<string, string>(EthMethods.Uri, tokenId);
        }

        /// <summary>
        /// Mints new tokens by calling the Mint method on the contract.
        /// </summary>
        /// <param name="tokenId">The ID of the token to mint.</param>
        /// <param name="amount">The amount of tokens to mint.</param>
        /// <param name="data">(Optional) Additional data to include with the minting.</param>
        /// <returns>A Task that represents the asynchronous operation. The task result is an array of objects representing the minting result.</returns>
        public Task<object[]> Mint(BigInteger tokenId, BigInteger amount, byte[] data = null)
        {
            EnsureSigner();
            data ??= Array.Empty<byte>();
            var parameters = new object[]
            {
                signer.PublicAddress, // destination
                tokenId,
                amount,
                data,
            };
            return Send(EthMethods.Mint, parameters);
        }

        /// <summary>
        /// Mints new tokens by calling the Mint method on the contract.
        /// </summary>
        /// <param name="tokenId">The ID of the token to mint.</param>
        /// <param name="amount">The amount of tokens to mint.</param>
        /// <param name="data">(Optional) Additional data to include with the minting.</param>
        /// <returns>Receipt of the Mint Transaction.</returns>
        public async Task<TransactionReceipt> MintWithReceipt(BigInteger tokenId, BigInteger amount, byte[] data = null)
        {
            EnsureSigner();
            data ??= Array.Empty<byte>();
            var parameters = new object[]
            {
                signer.PublicAddress, // destination
                tokenId,
                amount,
                data,
            };

            var response = await SendWithReceipt(EthMethods.Mint, parameters);

            return response.receipt;
        }

        /// <summary>
        /// Transfers a specified amount of tokens from the user's account to a specified destination address.
        /// </summary>
        /// <param name="tokenId">The identifier of the token to be transferred.</param>
        /// <param name="amount">The amount of tokens to be transferred.</param>
        /// <param name="destinationAddress">The destination address to receive the transferred tokens.</param>
        /// <returns>A task that represents the asynchronous transfer operation. The task result contains an array of objects.</returns>
        public Task<object[]> Transfer(BigInteger tokenId, BigInteger amount, string destinationAddress)
        {
            EnsureSigner();
            var data = Array.Empty<byte>();
            var parameters = new object[]
            {
                signer.PublicAddress, // source
                destinationAddress,
                tokenId,
                amount,
                data,
            };
            return Send(EthMethods.SafeTransferFrom, parameters);
        }

        /// <summary>
        /// Transfers a specified amount of tokens from the user's account to a specified destination address.
        /// </summary>
        /// <param name="tokenId">The identifier of the token to be transferred.</param>
        /// <param name="amount">The amount of tokens to be transferred.</param>
        /// <param name="destinationAddress">The destination address to receive the transferred tokens.</param>
        /// <returns>Receipt of the transfer.</returns>
        public async Task<TransactionReceipt> TransferWithReceipt(BigInteger tokenId, BigInteger amount, string destinationAddress)
        {
            EnsureSigner();
            var data = Array.Empty<byte>();
            var parameters = new object[]
            {
                signer.PublicAddress, // source
                destinationAddress,
                tokenId,
                amount,
                data,
            };

            var response = await SendWithReceipt(EthMethods.SafeTransferFrom, parameters);

            return response.receipt;
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