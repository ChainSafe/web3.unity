using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using ChainSafe.GamingWeb3;
using Nethereum.Hex.HexTypes;
using Nethereum.Signer;
using Nethereum.Util;
using Web3Unity.Scripts.Library.Ethers.Providers;
using Web3Unity.Scripts.Library.Ethers.Transactions;

namespace Web3Unity.Scripts.Prefabs
{
    public class UnsortedSample
    {
        private Web3 web3;

        public UnsortedSample(Web3 web3)
        {
            this.web3 = web3;
        }

        public async Task<object[]> ContractSend(string method, string abi, string contractAddress)
        {
            var contract = web3.ContractBuilder.Build(abi, contractAddress);
            return await contract.Send(method, new object[] { 1 });
        }

        public async Task<List<List<string>>> GetArray()
        {
            var contractAddress = "0x5244d0453A727EDa96299384370359f4A2B5b20a";
            var abi = "[{\"inputs\":[{\"internalType\":\"address[]\",\"name\":\"_addresses\",\"type\":\"address[]\"}],\"name\":\"setStore\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"}],\"name\":\"bought\",\"outputs\":[{\"internalType\":\"address\",\"name\":\"\",\"type\":\"address\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"getStore\",\"outputs\":[{\"internalType\":\"address[]\",\"name\":\"\",\"type\":\"address[]\"}],\"stateMutability\":\"view\",\"type\":\"function\"}]";
            var method = "getStore";

            var contract = web3.ContractBuilder.Build(abi, contractAddress);
            var rawResponse = await contract.Call(method);
            return rawResponse.Select(raw => raw as List<string>).ToList();
        }

        public async Task<HexBigInteger> GetBlockNumber()
        {
            return await web3.RpcProvider.GetBlockNumber();
        }

        public async Task<HexBigInteger> GetGasLimit(string contractAbi, string contractAddress)
        {
            var contract = web3.ContractBuilder.Build(contractAbi, contractAddress);
            return await contract.EstimateGas("addTotal", new object[] { });
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

        // todo weird one, needs explanation on how to interpret the response
        public async Task<object[]> Mint721(string abi, string contractAddress, string uri)
        {
            const string method = "safeMint";
            var destination = await web3.Signer.GetAddress();
            var contract = web3.ContractBuilder.Build(abi, contractAddress);
            return await contract.Send(method, new object[] { destination, uri });
        }

        // ProviderEvent skipped

        public async Task<BigInteger> UseRegisteredContract()
        {
            var account = await web3.Signer.GetAddress();
            var contract = web3.ContractBuilder.Build("shiba");
            var response = await contract.Call(EthMethod.BalanceOf, new[] { account });
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

        // todo danger - possible money loss
        // todo not sure what is output
        public async Task<object[]> TransferErc20(string contractAddress, string toAccount, string amount)
        {
            var abi = ABI.ERC_20;
            var method = EthMethod.Transfer;
            var contract = web3.ContractBuilder.Build(abi, contractAddress);

            var response = await contract.Send(method, new object[]
            {
                toAccount,
                amount
            });

            return response;
        }

        // todo not sure what is output
        public async Task<object[]> TransferErc721(string contractAddress, string toAccount, int tokenId)
        {
            var abi = ABI.ERC_721;
            var method = EthMethod.SafeTransferFrom;
            var account = await web3.Signer.GetAddress();
            var contract = web3.ContractBuilder.Build(abi, contractAddress);

            var response = await contract.Send(method, new object[]
            {
                account,
                toAccount,
                tokenId.ToString()
            });

            return response;
        }

        // todo not sure what is output
        public async Task<object[]> TransferErc1155(string contractAddress, int tokenId, int amount, string toAccount)
        {
            var account = await web3.Signer.GetAddress();
            var abi = ABI.ERC_1155;
            var method = EthMethod.SafeTransferFrom;
            byte[] dataObject = { };

            var contract = web3.ContractBuilder.Build(abi, contractAddress);
            var response = await contract.Send(method, new object[]
            {
                account,
                toAccount,
                tokenId,
                amount,
                dataObject
            });

            return response;
        }
    }
}