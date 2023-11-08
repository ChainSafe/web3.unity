using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using ChainSafe.Gaming.Evm.Providers;
using ChainSafe.Gaming.Evm.Transactions;
using ChainSafe.Gaming.UnityPackage;
using ChainSafe.Gaming.Web3;
using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.Hex.HexTypes;
using Nethereum.Signer;
using Nethereum.Util;
using Web3Unity.Scripts.Library.IPFS;
using Web3Unity.Scripts.Prefabs;

namespace Scripts.EVM.Token
{
    public class Evm
    {
        private Web3 web3;

        public Evm(Web3 web3)
        {
            this.web3 = web3 ?? throw new Web3Exception(
                "Web3 instance is null. Please ensure that the instance is properly retrieved trough the constructor");
        }
        
        public async Task<object[]> ContractSend(string method, string abi, string contractAddress, object[] args)
        {
            var contract = web3.ContractBuilder.Build(abi, contractAddress);
            return await contract.Send(method, args);
        }
        
        public async Task<object[]> ContractCall(string method, string abi, string contractAddress, object[] args)
        {
            var contract = web3.ContractBuilder.Build(abi, contractAddress);
            return await contract.Call(method, args);
        }

        public async Task<List<List<string>>> GetArray(string contractAddress, string abi, string method)
        {
            var contract = web3.ContractBuilder.Build(abi, contractAddress);
            var rawResponse = await contract.Call(method);
            return rawResponse.Select(raw => raw as List<string>).ToList();
        }

        public async Task<HexBigInteger> GetBlockNumber()
        {
            return await web3.RpcProvider.GetBlockNumber();
        }

        public async Task<HexBigInteger> GetGasLimit(string contractAbi, string contractAddress, string method)
        {
            var contract = web3.ContractBuilder.Build(contractAbi, contractAddress);
            return await contract.EstimateGas(method, new object[] { });
        }

        public async Task<HexBigInteger> GetGasPrice()
        {
            return await web3.RpcProvider.GetGasPrice();
        }

        public async Task<HexBigInteger> GetNonce()
        {
            var transactionRequest = new TransactionRequest
            {
                To = await web3.Signer.GetAddress(),
                Value = new HexBigInteger(100000)
            };
            var transactionResponse = await web3.TransactionExecutor.SendTransaction(transactionRequest);
            return transactionResponse.Nonce;
        }

        public async Task<TransactionReceipt> GetTransactionStatus()
        {
            var transactionRequest = new TransactionRequest
            {
                To = await web3.Signer.GetAddress(),
                Value = new HexBigInteger(10000000)
            };
            var transactionResponse = await web3.TransactionExecutor.SendTransaction(transactionRequest);
            return await web3.RpcProvider.WaitForTransactionReceipt(transactionResponse.Hash);
        }

        // ProviderEvent skipped

        public async Task<BigInteger> UseRegisteredContract(string contractName, string method)
        {
            var account = await web3.Signer.GetAddress();
            var contract = web3.ContractBuilder.Build(contractName);
            var response = await contract.Call(method, new[] { account });
            var balance = BigInteger.Parse(response[0].ToString());
            return balance;
        }

        // todo we shouldn't build contract inside this method, but rather put this logic into the contract or some service
        public async Task<object[]> SendArray(string method, string abi, string contractAddress, string[] stringArray)
        {
            var contract = web3.ContractBuilder.Build(abi, contractAddress);
            return await contract.Send(method, new object[] { stringArray });
        }

        // todo danger - possible money loss 
        // todo rework input
        public async Task<string> SendTransaction(string to)
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
        public string Sha3(string message)
        {
            return new Sha3Keccack().CalculateHash(message);
        }

        public async Task<string> SignMessage(string message)
        {
            return await web3.Signer.SignMessage(message);
        }

        // todo extract in a separate service
        public async Task<bool> SignVerify(string message)
        {
            var playerAccount = await web3.Signer.GetAddress();
            var signatureString = await web3.Signer.SignMessage(message);

            var msg = "\x19" + "Ethereum Signed Message:\n" + message.Length + message;
            var msgHash = new Sha3Keccack().CalculateHash(Encoding.UTF8.GetBytes(msg));
            var signature = MessageSigner.ExtractEcdsaSignature(signatureString);
            var key = EthECKey.RecoverFromSignature(signature, msgHash);

            return key.GetPublicAddress() == playerAccount;
        }

		public static string PrivateKeySignTransaction(string _privateKey, string _transaction, string _chainId)
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

        public static string PrivateKeyGetAddress(string _privateKey)
        {
            EthECKey key = new EthECKey(_privateKey);
            return key.GetPublicAddress();
        }

        public static string PrivateKeySignMessage(string _privateKey, string _message)
        {
            var signer = new EthereumMessageSigner();
            string signature = signer.HashAndSign(_message, _privateKey);
            return signature;
        }
        
        // IPFS upload
        public static async Task<string> Upload(IpfsUploadRequest request)
        {
            var rawData = System.Text.Encoding.UTF8.GetBytes(request.Data);
            var ipfs = new Ipfs(request.ApiKey);
            var cid = await ipfs.Upload(request.BucketId, request.Path, request.Filename, rawData, "application/octet-stream");
            return cid;
        }
    }
}