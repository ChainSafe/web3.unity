using System.Net;
using System.Threading.Tasks;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Signer;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using Prefabs.Web3AuthWallet.Interfaces;
using UnityEngine;

namespace Prefabs.Web3AuthWallet.Utils
{
    /// <summary>
    /// Provides Ethereum-related functionality.
    /// </summary>
    public class EthereumService : IEthereumService
    {
        private readonly Web3 _web3;
        private Account _account;

        /// <summary>
        /// Initializes a new instance of the <see cref="EthereumService"/> class.
        /// </summary>
        /// <param name="privateKey">The private key associated with the Ethereum account.</param>
        /// <param name="url">The URL of the Ethereum node.</param>
        public EthereumService(string privateKey, string url)
        {
            _web3 = new Web3(new Account(privateKey), url);
        }

        /// <summary>
        /// Creates and signs a transaction asynchronously.
        /// </summary>
        /// <param name="txInput">The transaction input parameters.</param>
        /// <returns>The signed transaction data as a string.</returns>
        public async Task<string> CreateAndSignTransactionAsync(TransactionInput txInput)
        {
            var signedTransaction = await _web3.Eth.TransactionManager.SignTransactionAsync(txInput);

            return signedTransaction;
        }

        /// <summary>
        /// Sends a signed transaction asynchronously.
        /// </summary>
        /// <param name="signedTransactionData">The signed transaction data.</param>
        /// <returns>The transaction hash as a string.</returns>
        public async Task<string> SendTransactionAsync(string signedTransactionData)
        {
            return await _web3.Eth.Transactions.SendRawTransaction.SendRequestAsync(signedTransactionData);
        }

        /// <summary>
        /// Gets the Ethereum address associated with the provided private key.
        /// </summary>
        /// <param name="privateKey">The private key.</param>
        /// <returns>The Ethereum address as a string.</returns>
        public string GetAddressW3A(string privateKey) => new EthECKey(privateKey).GetPublicAddress();
    }
}
