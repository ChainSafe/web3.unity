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
    public class EthereumService : IEthereumService
    {
        private readonly Web3 _web3;
        private Account _account;

        public EthereumService(string privateKey, string url)
        {
            _web3 = new Web3(new Account(privateKey), url);
        }

        public async Task<string> CreateAndSignTransactionAsync(TransactionInput txInput)
        {
            Debug.Log("Transaction Data: " + txInput.Data);
            var signedTransaction = await _web3.Eth.TransactionManager.SignTransactionAsync(txInput);

            return signedTransaction;
        }

        public async Task<string> SendTransactionAsync(string signedTransactionData)
        {
            return await _web3.Eth.Transactions.SendRawTransaction.SendRequestAsync(signedTransactionData);
        }

        public string GetAddressW3A(string privateKey) => new EthECKey(privateKey).GetPublicAddress();
    }
}