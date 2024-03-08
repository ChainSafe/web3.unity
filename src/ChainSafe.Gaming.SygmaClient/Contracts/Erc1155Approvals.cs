using System;
using System.Threading.Tasks;
using ChainSafe.Gaming.Evm.Contracts;
using ChainSafe.Gaming.Evm.Transactions;
using ChainSafe.Gaming.SygmaClient.Types;
using Nethereum.Hex.HexTypes;

namespace ChainSafe.Gaming.SygmaClient.Contracts
{
    public class Erc1155Approvals
    {
        // Resource ID is 0x0000000000000000000000000000000000000000000000000000000000000400
        private const string Erc1155Abi =
            " [{  \"inputs\": [ { \"internalType\": \"string\",  \"name\": \"uri_\",  \"type\": \"string\" } ],  \"stateMutability\": \"nonpayable\",  \"type\": \"constructor\"},{  \"anonymous\": false,  \"inputs\": [ { \"indexed\": true,  \"internalType\": \"address\",  \"name\": \"account\",  \"type\": \"address\"    },    {  \"indexed\": true,  \"internalType\": \"address\",  \"name\": \"operator\",  \"type\": \"address\"    },    {  \"indexed\": false,  \"internalType\": \"bool\",  \"name\": \"approved\",  \"type\": \"bool\" } ],  \"name\": \"ApprovalForAll\",  \"type\": \"event\"},{  \"anonymous\": false,  \"inputs\": [ { \"indexed\": true,  \"internalType\": \"address\",  \"name\": \"operator\",  \"type\": \"address\"    },    {  \"indexed\": true,  \"internalType\": \"address\",  \"name\": \"from\",  \"type\": \"address\"    },    {  \"indexed\": true,  \"internalType\": \"address\",  \"name\": \"to\",  \"type\": \"address\"    },    {  \"indexed\": false,  \"internalType\": \"uint256[]\",  \"name\": \"ids\",  \"type\": \"uint256[]\"    },    {  \"indexed\": false,  \"internalType\": \"uint256[]\",  \"name\": \"values\",  \"type\": \"uint256[]\" } ],  \"name\": \"TransferBatch\",  \"type\": \"event\"},{  \"anonymous\": false,  \"inputs\": [ { \"indexed\": true,  \"internalType\": \"address\",  \"name\": \"operator\",  \"type\": \"address\"    },    {  \"indexed\": true,  \"internalType\": \"address\",  \"name\": \"from\",  \"type\": \"address\"    },    {  \"indexed\": true,  \"internalType\": \"address\",  \"name\": \"to\",  \"type\": \"address\"    },    {  \"indexed\": false,  \"internalType\": \"uint256\",  \"name\": \"id\",  \"type\": \"uint256\"    },    {  \"indexed\": false,  \"internalType\": \"uint256\",  \"name\": \"value\",  \"type\": \"uint256\" } ],  \"name\": \"TransferSingle\",  \"type\": \"event\"},{  \"anonymous\": false,  \"inputs\": [ { \"indexed\": false,  \"internalType\": \"string\",  \"name\": \"value\",  \"type\": \"string\"    },    {  \"indexed\": true,  \"internalType\": \"uint256\",  \"name\": \"id\",  \"type\": \"uint256\" } ],  \"name\": \"URI\",  \"type\": \"event\"},{  \"inputs\": [ { \"internalType\": \"bytes4\",  \"name\": \"interfaceId\",  \"type\": \"bytes4\" } ],  \"name\": \"supportsInterface\",  \"outputs\": [ { \"internalType\": \"bool\",  \"name\": \"\",  \"type\": \"bool\" } ],  \"stateMutability\": \"view\",  \"type\": \"function\"},{  \"inputs\": [ { \"internalType\": \"uint256\",  \"name\": \"\",  \"type\": \"uint256\" } ],  \"name\": \"uri\",  \"outputs\": [ { \"internalType\": \"string\",  \"name\": \"\",  \"type\": \"string\" } ],  \"stateMutability\": \"view\",  \"type\": \"function\"},{  \"inputs\": [ { \"internalType\": \"address\",  \"name\": \"account\",  \"type\": \"address\"    },    {  \"internalType\": \"uint256\",  \"name\": \"id\",  \"type\": \"uint256\" } ],  \"name\": \"balanceOf\",  \"outputs\": [ { \"internalType\": \"uint256\",  \"name\": \"\",  \"type\": \"uint256\" } ],  \"stateMutability\": \"view\",  \"type\": \"function\"},{  \"inputs\": [ { \"internalType\": \"address[]\",  \"name\": \"accounts\",  \"type\": \"address[]\"    },    {  \"internalType\": \"uint256[]\",  \"name\": \"ids\",  \"type\": \"uint256[]\" } ],  \"name\": \"balanceOfBatch\",  \"outputs\": [ { \"internalType\": \"uint256[]\",  \"name\": \"\",  \"type\": \"uint256[]\" } ],  \"stateMutability\": \"view\",  \"type\": \"function\"},{  \"inputs\": [ { \"internalType\": \"address\",  \"name\": \"operator\",  \"type\": \"address\"    },    {  \"internalType\": \"bool\",  \"name\": \"approved\",  \"type\": \"bool\" } ],  \"name\": \"setApprovalForAll\",  \"outputs\": [],  \"stateMutability\": \"nonpayable\",  \"type\": \"function\"},{  \"inputs\": [ { \"internalType\": \"address\",  \"name\": \"account\",  \"type\": \"address\"    },    {  \"internalType\": \"address\",  \"name\": \"operator\",  \"type\": \"address\" } ],  \"name\": \"isApprovedForAll\",  \"outputs\": [ { \"internalType\": \"bool\",  \"name\": \"\",  \"type\": \"bool\" } ],  \"stateMutability\": \"view\",  \"type\": \"function\"},{  \"inputs\": [ { \"internalType\": \"address\",  \"name\": \"from\",  \"type\": \"address\"    },    {  \"internalType\": \"address\",  \"name\": \"to\",  \"type\": \"address\"    },    {  \"internalType\": \"uint256\",  \"name\": \"id\",  \"type\": \"uint256\"    },    {  \"internalType\": \"uint256\",  \"name\": \"amount\",  \"type\": \"uint256\"    },    {  \"internalType\": \"bytes\",  \"name\": \"data\",  \"type\": \"bytes\" } ],  \"name\": \"safeTransferFrom\",  \"outputs\": [],  \"stateMutability\": \"nonpayable\",  \"type\": \"function\"},{  \"inputs\": [ { \"internalType\": \"address\",  \"name\": \"from\",  \"type\": \"address\"    },    {  \"internalType\": \"address\",  \"name\": \"to\",  \"type\": \"address\"    },    {  \"internalType\": \"uint256[]\",  \"name\": \"ids\",  \"type\": \"uint256[]\"    },    {  \"internalType\": \"uint256[]\",  \"name\": \"amounts\",  \"type\": \"uint256[]\"    },    {  \"internalType\": \"bytes\",  \"name\": \"data\",  \"type\": \"bytes\" } ],  \"name\": \"safeBatchTransferFrom\",  \"outputs\": [],  \"stateMutability\": \"nonpayable\",  \"type\": \"function\" } ]";

        private readonly Contract contract;

        public Erc1155Approvals(IContractBuilder contractBuilder, string contractAddress)
        {
            contract = contractBuilder.Build(Erc1155Abi, contractAddress);
        }

        public async Task<TransactionRequest> ApprovalTransactionRequest<T>(Transfer<T> transfer, string handlerAddress)
            where T : TransferType
        {
            var transactionRequest = new TransactionRequest();
            if (await IsApprovedForAll(handlerAddress))
            {
                return await Task.FromResult(transactionRequest);
            }

            return await contract.PrepareTransactionRequest("setApprovalForAll", new object[] { handlerAddress, bool.TrueString });
        }

        private async Task<bool> IsApprovedForAll(string handlerAddress)
        {
            var approvedAddress = await contract.Call("isApprovedForAll", new object[] { handlerAddress });
            return approvedAddress[0].ToString() == handlerAddress;
        }
    }
}