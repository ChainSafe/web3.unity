using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using ChainSafe.Gaming.Evm.Providers;
using ChainSafe.Gaming.Evm.Transactions;
using ChainSafe.Gaming.UnityPackage.Model;
using ChainSafe.Gaming.Web3;
using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.Hex.HexTypes;
using Nethereum.Signer;
using Nethereum.Util;
using UnityEngine;
using ChainSafe.Gaming.Marketplace;

namespace Scripts.EVM.Token
{
    public static class Evm
    {
        public static async Task<object[]> ContractSend(Web3 web3, string method, string abi, string contractAddress, object[] args, HexBigInteger value = null)
        {
            var contract = web3.ContractBuilder.Build(abi, contractAddress);
            TransactionRequest overwrite = value != null ? new TransactionRequest { Value = value } : null;
            return await contract.Send(method, args, overwrite);
        }

        public static async Task<object[]> ContractCall(Web3 web3, string method, string abi, string contractAddress, object[] args)
        {
            var contract = web3.ContractBuilder.Build(abi, contractAddress);
            return await contract.Call(method, args);
        }

        public static async Task<List<List<T>>> GetArray<T>(Web3 web3, string contractAddress, string abi, string method, object[] args = null)
        {
            var contract = web3.ContractBuilder.Build(abi, contractAddress);
            var rawResponse = args != null ? await contract.Call(method, args) : await contract.Call(method);
            return rawResponse.Select(raw => raw as List<T>).ToList();
        }

        public static async Task<HexBigInteger> GetBlockNumber(Web3 web3)
        {
            return await web3.RpcProvider.GetBlockNumber();
        }

        public static async Task<HexBigInteger> GetGasLimit(Web3 web3, string contractAbi, string contractAddress, string method, object[] args)
        {
            var contract = web3.ContractBuilder.Build(contractAbi, contractAddress);
            return await contract.EstimateGas(method, args);
        }

        public static async Task<HexBigInteger> GetGasPrice(Web3 web3)
        {
            return await web3.RpcProvider.GetGasPrice();
        }

        public static async Task<HexBigInteger> GetNonce(Web3 web3)
        {
            var transactionRequest = new TransactionRequest
            {
                To = web3.Signer.PublicAddress,
                Value = new HexBigInteger(100000)
            };
            var transactionResponse = await web3.TransactionExecutor.SendTransaction(transactionRequest);
            return transactionResponse.Nonce;
        }

        public static async Task<TransactionReceipt> GetTransactionStatus(Web3 web3)
        {
            var transactionRequest = new TransactionRequest
            {
                To = web3.Signer.PublicAddress,
                Value = new HexBigInteger(10000000)
            };
            var transactionResponse = await web3.TransactionExecutor.SendTransaction(transactionRequest);
            return await web3.RpcProvider.WaitForTransactionReceipt(transactionResponse.Hash);
        }

        // ProviderEvent skipped

        public static async Task<BigInteger> UseRegisteredContract(Web3 web3, string contractName, string method)
        {
            var account = web3.Signer.PublicAddress;
            var contract = web3.ContractBuilder.Build(contractName);
            var response = await contract.Call(method, new[] { account });
            var balance = BigInteger.Parse(response[0].ToString());
            return balance;
        }

        // todo we shouldn't build contract inside this method, but rather put this logic into the contract or some service
        public static async Task<object[]> SendArray<T>(Web3 web3, string method, string abi, string contractAddress, T[] array)
        {
            var contract = web3.ContractBuilder.Build(abi, contractAddress);
            object[] objArray = array.Cast<object>().ToArray();
            return await contract.Send(method, new object[] { objArray });
        }

        // todo danger - possible money loss 
        // todo rework input
        public static async Task<string> SendTransaction(Web3 web3, string to)
        {
            var txRequest = new TransactionRequest
            {
                To = to,
                Value = new HexBigInteger(12300000000000000),
                MaxFeePerGas = new HexBigInteger((await web3.RpcProvider.GetFeeData()).MaxFeePerGas),
            };
            var response = await web3.TransactionExecutor.SendTransaction(txRequest);
            return response.Hash;
        }

        // todo extract in a separate service
        public static string Sha3(string message)
        {
            return new Sha3Keccack().CalculateHash(message);
        }

        public static async Task<string> SignMessage(Web3 web3, string message)
        {
            return await web3.Signer.SignMessage(message);
        }

        // todo extract in a separate service
        public static async Task<bool> SignVerify(Web3 web3, string message)
        {
            var playerAccount = web3.Signer.PublicAddress;
            var signatureString = await web3.Signer.SignMessage(message);
            var msg = "\x19" + "Ethereum Signed Message:\n" + message.Length + message;
            var msgHash = new Sha3Keccack().CalculateHash(Encoding.UTF8.GetBytes(msg));
            var signature = MessageSigner.ExtractEcdsaSignature(signatureString);
            var key = EthECKey.RecoverFromSignature(signature, msgHash);
            return key.GetPublicAddress().ToLower() == playerAccount.ToLower();
        }

        public static string EcdsaSignTransaction(string _privateKey, string _transaction, string _chainId)
        {
            int MATIC_MAIN = 137;
            int MATIC_MUMBAI = 80001;
            int HARMONY_MAINNET = 1666600000;
            int HARMONY_TESTNET = 1666700000;
            int CRONOS_MAINNET = 25;
            int CRONOS_TESTNET = 338;
            int FTM_MAINNET = 250;
            int FTM_TESTNET = 0xfa2;
            int AVA_MAINNET = 43114;
            int AVA_TESTNET = 43113;
            int CHAIN_ID = Convert.ToInt32(_chainId);
            string signature;
            EthECKey key = new EthECKey(_privateKey);
            // convert transaction
            byte[] hashByteArr = HexByteConvertorExtensions.HexToByteArray(_transaction);
            // parse chain id
            BigInteger chainId = BigInteger.Parse(_chainId);
            // sign transaction
            if ((CHAIN_ID == MATIC_MAIN) || (CHAIN_ID == MATIC_MUMBAI) || (CHAIN_ID == HARMONY_MAINNET) ||
                (CHAIN_ID == HARMONY_TESTNET) || (CHAIN_ID == CRONOS_MAINNET) || (CHAIN_ID == CRONOS_TESTNET) || (CHAIN_ID == FTM_MAINNET) || (CHAIN_ID == FTM_TESTNET) || (CHAIN_ID == AVA_MAINNET) || (CHAIN_ID == AVA_TESTNET))
            {
                signature = EthECDSASignature.CreateStringSignature(key.SignAndCalculateYParityV(hashByteArr));
                return signature;

            }
            signature = EthECDSASignature.CreateStringSignature(key.SignAndCalculateV(hashByteArr, chainId));
            return signature;
        }

        public static string EcdsaGetAddress(string _privateKey)
        {
            EthECKey key = new EthECKey(_privateKey);
            return key.GetPublicAddress();
        }

        public static string EcdsaSignMessage(string _privateKey, string _message)
        {
            var signer = new EthereumMessageSigner();
            string signature = signer.HashAndSign(_message, _privateKey);
            return signature;
        }
    }
}