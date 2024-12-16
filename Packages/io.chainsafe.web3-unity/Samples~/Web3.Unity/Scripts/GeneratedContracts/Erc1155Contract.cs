using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;
using ChainSafe.Gaming.Evm.Transactions;
using ChainSafe.Gaming.Ipfs;
using Nethereum.Hex.HexTypes;
using Nethereum.ABI.FunctionEncoding.Attributes;
using UnityEngine;
using ChainSafe.Gaming.RPC.Events;


namespace ChainSafe.Gaming.Evm.Contracts.Custom
{
    public partial class Erc1155Contract : ICustomContract
    {
        public string Address => OriginalContract.Address;

        public string ABI =>
            "[   {     \"inputs\": [      ],     \"stateMutability\": \"nonpayable\",     \"type\": \"constructor\"   },   {     \"inputs\": [       {         \"internalType\": \"address\",         \"name\": \"sender\",         \"type\": \"address\"       },       {         \"internalType\": \"uint256\",         \"name\": \"balance\",         \"type\": \"uint256\"       },       {         \"internalType\": \"uint256\",         \"name\": \"needed\",         \"type\": \"uint256\"       },       {         \"internalType\": \"uint256\",         \"name\": \"tokenId\",         \"type\": \"uint256\"       }     ],     \"name\": \"ERC1155InsufficientBalance\",     \"type\": \"error\"   },   {     \"inputs\": [       {         \"internalType\": \"address\",         \"name\": \"approver\",         \"type\": \"address\"       }     ],     \"name\": \"ERC1155InvalidApprover\",     \"type\": \"error\"   },   {     \"inputs\": [       {         \"internalType\": \"uint256\",         \"name\": \"idsLength\",         \"type\": \"uint256\"       },       {         \"internalType\": \"uint256\",         \"name\": \"valuesLength\",         \"type\": \"uint256\"       }     ],     \"name\": \"ERC1155InvalidArrayLength\",     \"type\": \"error\"   },   {     \"inputs\": [       {         \"internalType\": \"address\",         \"name\": \"operator\",         \"type\": \"address\"       }     ],     \"name\": \"ERC1155InvalidOperator\",     \"type\": \"error\"   },   {     \"inputs\": [       {         \"internalType\": \"address\",         \"name\": \"receiver\",         \"type\": \"address\"       }     ],     \"name\": \"ERC1155InvalidReceiver\",     \"type\": \"error\"   },   {     \"inputs\": [       {         \"internalType\": \"address\",         \"name\": \"sender\",         \"type\": \"address\"       }     ],     \"name\": \"ERC1155InvalidSender\",     \"type\": \"error\"   },   {     \"inputs\": [       {         \"internalType\": \"address\",         \"name\": \"operator\",         \"type\": \"address\"       },       {         \"internalType\": \"address\",         \"name\": \"owner\",         \"type\": \"address\"       }     ],     \"name\": \"ERC1155MissingApprovalForAll\",     \"type\": \"error\"   },   {     \"anonymous\": false,     \"inputs\": [       {         \"indexed\": true,         \"internalType\": \"address\",         \"name\": \"account\",         \"type\": \"address\"       },       {         \"indexed\": true,         \"internalType\": \"address\",         \"name\": \"operator\",         \"type\": \"address\"       },       {         \"indexed\": false,         \"internalType\": \"bool\",         \"name\": \"approved\",         \"type\": \"bool\"       }     ],     \"name\": \"ApprovalForAll\",     \"type\": \"event\"   },   {     \"anonymous\": false,     \"inputs\": [       {         \"indexed\": true,         \"internalType\": \"address\",         \"name\": \"operator\",         \"type\": \"address\"       },       {         \"indexed\": true,         \"internalType\": \"address\",         \"name\": \"from\",         \"type\": \"address\"       },       {         \"indexed\": true,         \"internalType\": \"address\",         \"name\": \"to\",         \"type\": \"address\"       },       {         \"indexed\": false,         \"internalType\": \"uint256[]\",         \"name\": \"ids\",         \"type\": \"uint256[]\"       },       {         \"indexed\": false,         \"internalType\": \"uint256[]\",         \"name\": \"values\",         \"type\": \"uint256[]\"       }     ],     \"name\": \"TransferBatch\",     \"type\": \"event\"   },   {     \"anonymous\": false,     \"inputs\": [       {         \"indexed\": true,         \"internalType\": \"address\",         \"name\": \"operator\",         \"type\": \"address\"       },       {         \"indexed\": true,         \"internalType\": \"address\",         \"name\": \"from\",         \"type\": \"address\"       },       {         \"indexed\": true,         \"internalType\": \"address\",         \"name\": \"to\",         \"type\": \"address\"       },       {         \"indexed\": false,         \"internalType\": \"uint256\",         \"name\": \"id\",         \"type\": \"uint256\"       },       {         \"indexed\": false,         \"internalType\": \"uint256\",         \"name\": \"value\",         \"type\": \"uint256\"       }     ],     \"name\": \"TransferSingle\",     \"type\": \"event\"   },   {     \"anonymous\": false,     \"inputs\": [       {         \"indexed\": false,         \"internalType\": \"string\",         \"name\": \"value\",         \"type\": \"string\"       },       {         \"indexed\": true,         \"internalType\": \"uint256\",         \"name\": \"id\",         \"type\": \"uint256\"       }     ],     \"name\": \"URI\",     \"type\": \"event\"   },   {     \"inputs\": [       {         \"internalType\": \"address\",         \"name\": \"account\",         \"type\": \"address\"       },       {         \"internalType\": \"uint256\",         \"name\": \"id\",         \"type\": \"uint256\"       }     ],     \"name\": \"balanceOf\",     \"outputs\": [       {         \"internalType\": \"uint256\",         \"name\": \"\",         \"type\": \"uint256\"       }     ],     \"stateMutability\": \"view\",     \"type\": \"function\"   },   {     \"inputs\": [       {         \"internalType\": \"address[]\",         \"name\": \"accounts\",         \"type\": \"address[]\"       },       {         \"internalType\": \"uint256[]\",         \"name\": \"ids\",         \"type\": \"uint256[]\"       }     ],     \"name\": \"balanceOfBatch\",     \"outputs\": [       {         \"internalType\": \"uint256[]\",         \"name\": \"\",         \"type\": \"uint256[]\"       }     ],     \"stateMutability\": \"view\",     \"type\": \"function\"   },   {     \"inputs\": [       {         \"internalType\": \"address\",         \"name\": \"account\",         \"type\": \"address\"       },       {         \"internalType\": \"address\",         \"name\": \"operator\",         \"type\": \"address\"       }     ],     \"name\": \"isApprovedForAll\",     \"outputs\": [       {         \"internalType\": \"bool\",         \"name\": \"\",         \"type\": \"bool\"       }     ],     \"stateMutability\": \"view\",     \"type\": \"function\"   },   {     \"inputs\": [       {         \"internalType\": \"address\",         \"name\": \"_to\",         \"type\": \"address\"       },       {         \"internalType\": \"uint256\",         \"name\": \"_id\",         \"type\": \"uint256\"       },       {         \"internalType\": \"uint256\",         \"name\": \"_amount\",         \"type\": \"uint256\"       },       {         \"internalType\": \"bytes\",         \"name\": \"_data\",         \"type\": \"bytes\"       }     ],     \"name\": \"mint\",     \"outputs\": [      ],     \"stateMutability\": \"nonpayable\",     \"type\": \"function\"   },   {     \"inputs\": [       {         \"internalType\": \"address\",         \"name\": \"_to\",         \"type\": \"address\"       },       {         \"internalType\": \"uint256[]\",         \"name\": \"_ids\",         \"type\": \"uint256[]\"       },       {         \"internalType\": \"uint256[]\",         \"name\": \"_amounts\",         \"type\": \"uint256[]\"       },       {         \"internalType\": \"bytes\",         \"name\": \"_data\",         \"type\": \"bytes\"       }     ],     \"name\": \"mintBatch\",     \"outputs\": [      ],     \"stateMutability\": \"nonpayable\",     \"type\": \"function\"   },   {     \"inputs\": [       {         \"internalType\": \"address\",         \"name\": \"from\",         \"type\": \"address\"       },       {         \"internalType\": \"address\",         \"name\": \"to\",         \"type\": \"address\"       },       {         \"internalType\": \"uint256[]\",         \"name\": \"ids\",         \"type\": \"uint256[]\"       },       {         \"internalType\": \"uint256[]\",         \"name\": \"values\",         \"type\": \"uint256[]\"       },       {         \"internalType\": \"bytes\",         \"name\": \"data\",         \"type\": \"bytes\"       }     ],     \"name\": \"safeBatchTransferFrom\",     \"outputs\": [      ],     \"stateMutability\": \"nonpayable\",     \"type\": \"function\"   },   {     \"inputs\": [       {         \"internalType\": \"address\",         \"name\": \"from\",         \"type\": \"address\"       },       {         \"internalType\": \"address\",         \"name\": \"to\",         \"type\": \"address\"       },       {         \"internalType\": \"uint256\",         \"name\": \"id\",         \"type\": \"uint256\"       },       {         \"internalType\": \"uint256\",         \"name\": \"value\",         \"type\": \"uint256\"       },       {         \"internalType\": \"bytes\",         \"name\": \"data\",         \"type\": \"bytes\"       }     ],     \"name\": \"safeTransferFrom\",     \"outputs\": [      ],     \"stateMutability\": \"nonpayable\",     \"type\": \"function\"   },   {     \"inputs\": [       {         \"internalType\": \"address\",         \"name\": \"operator\",         \"type\": \"address\"       },       {         \"internalType\": \"bool\",         \"name\": \"approved\",         \"type\": \"bool\"       }     ],     \"name\": \"setApprovalForAll\",     \"outputs\": [      ],     \"stateMutability\": \"nonpayable\",     \"type\": \"function\"   },   {     \"inputs\": [       {         \"internalType\": \"bytes4\",         \"name\": \"interfaceId\",         \"type\": \"bytes4\"       }     ],     \"name\": \"supportsInterface\",     \"outputs\": [       {         \"internalType\": \"bool\",         \"name\": \"\",         \"type\": \"bool\"       }     ],     \"stateMutability\": \"view\",     \"type\": \"function\"   },   {     \"inputs\": [       {         \"internalType\": \"uint256\",         \"name\": \"\",         \"type\": \"uint256\"       }     ],     \"name\": \"uri\",     \"outputs\": [       {         \"internalType\": \"string\",         \"name\": \"\",         \"type\": \"string\"       }     ],     \"stateMutability\": \"view\",     \"type\": \"function\"   } ]";

        public string ContractAddress { get; set; }

        public IEventManager EventManager { get; set; }

        public Contract OriginalContract { get; set; }

        public bool Subscribed { get; set; }


        #region Methods

        public async Task<BigInteger> BalanceOf(string account, BigInteger id)
        {
            var response = await OriginalContract.Call<BigInteger>("balanceOf", new object[]
            {
                account, id
            });

            return response;
        }


        public async Task<List<BigInteger>> BalanceOfBatch(string[] accounts, BigInteger[] ids)
        {
            var response = await OriginalContract.Call<List<BigInteger>>("balanceOfBatch", new object[]
            {
                accounts, ids
            });

            return response;
        }


        public async Task<bool> IsApprovedForAll(string account, string @operator)
        {
            var response = await OriginalContract.Call<bool>("isApprovedForAll", new object[]
            {
                account, @operator
            });

            return response;
        }


        public async Task Mint(string _to, BigInteger _id, BigInteger _amount, byte[] _data)
        {
            var response = await OriginalContract.Send("mint", new object[]
            {
                _to, _id, _amount, _data
            });
        }

        public async Task<TransactionReceipt> MintWithReceipt(string _to, BigInteger _id, BigInteger _amount,
            byte[] _data)
        {
            var response = await OriginalContract.SendWithReceipt("mint", new object[]
            {
                _to, _id, _amount, _data
            });

            return response.receipt;
        }

        public async Task MintBatch(string _to, BigInteger[] _ids, BigInteger[] _amounts, byte[] _data)
        {
            var response = await OriginalContract.Send("mintBatch", new object[]
            {
                _to, _ids, _amounts, _data
            });
        }

        public async Task<TransactionReceipt> MintBatchWithReceipt(string _to, BigInteger[] _ids, BigInteger[] _amounts,
            byte[] _data)
        {
            var response = await OriginalContract.SendWithReceipt("mintBatch", new object[]
            {
                _to, _ids, _amounts, _data
            });

            return response.receipt;
        }

        public async Task SafeBatchTransferFrom(string from, string to, BigInteger[] ids, BigInteger[] values,
            byte[] data)
        {
            var response = await OriginalContract.Send("safeBatchTransferFrom", new object[]
            {
                from, to, ids, values, data
            });
        }

        public async Task<TransactionReceipt> SafeBatchTransferFromWithReceipt(string from, string to, BigInteger[] ids,
            BigInteger[] values, byte[] data)
        {
            var response = await OriginalContract.SendWithReceipt("safeBatchTransferFrom", new object[]
            {
                from, to, ids, values, data
            });

            return response.receipt;
        }

        public async Task SafeTransferFrom(string from, string to, BigInteger id, BigInteger value, byte[] data)
        {
            var response = await OriginalContract.Send("safeTransferFrom", new object[]
            {
                from, to, id, value, data
            });
        }

        public async Task<TransactionReceipt> SafeTransferFromWithReceipt(string from, string to, BigInteger id,
            BigInteger value, byte[] data)
        {
            var response = await OriginalContract.SendWithReceipt("safeTransferFrom", new object[]
            {
                from, to, id, value, data
            });

            return response.receipt;
        }

        public async Task SetApprovalForAll(string @operator, bool approved)
        {
            var response = await OriginalContract.Send("setApprovalForAll", new object[]
            {
                @operator, approved
            });
        }

        public async Task<TransactionReceipt> SetApprovalForAllWithReceipt(string @operator, bool approved)
        {
            var response = await OriginalContract.SendWithReceipt("setApprovalForAll", new object[]
            {
                @operator, approved
            });

            return response.receipt;
        }

        public async Task<bool> SupportsInterface(byte[] interfaceId)
        {
            var response = await OriginalContract.Call<bool>("supportsInterface", new object[]
            {
                interfaceId
            });

            return response;
        }


        public async Task<string> Uri(string tokenId)
        {
            if (IpfsHelper.CanDecodeTokenIdToUri(tokenId))
            {
                return IpfsHelper.DecodeTokenIdToUri(tokenId);
            }
            
            var response = await OriginalContract.Call<string>("uri", new object[]
            {
                tokenId
            });

            return response;
        }

        #endregion


        #region Event Classes

        public partial class ApprovalForAllEventDTO : ApprovalForAllEventDTOBase
        {
        }

        [Event("ApprovalForAll")]
        public class ApprovalForAllEventDTOBase : IEventDTO
        {
            [Parameter("address", "account", 0, true)]
            public virtual string Account { get; set; }

            [Parameter("address", "operator", 1, true)]
            public virtual string Operator { get; set; }

            [Parameter("bool", "approved", 2, false)]
            public virtual bool Approved { get; set; }
        }

        public event Action<ApprovalForAllEventDTO> OnApprovalForAll;

        private void ApprovalForAll(ApprovalForAllEventDTO approvalForAll)
        {
            OnApprovalForAll?.Invoke(approvalForAll);
        }

        public partial class TransferBatchEventDTO : TransferBatchEventDTOBase
        {
        }

        [Event("TransferBatch")]
        public class TransferBatchEventDTOBase : IEventDTO
        {
            [Parameter("address", "operator", 0, true)]
            public virtual string Operator { get; set; }

            [Parameter("address", "from", 1, true)]
            public virtual string From { get; set; }

            [Parameter("address", "to", 2, true)] public virtual string To { get; set; }

            [Parameter("uint256[]", "ids", 3, false)]
            public virtual BigInteger[] Ids { get; set; }

            [Parameter("uint256[]", "values", 4, false)]
            public virtual BigInteger[] Values { get; set; }
        }

        public event Action<TransferBatchEventDTO> OnTransferBatch;

        private void TransferBatch(TransferBatchEventDTO transferBatch)
        {
            OnTransferBatch?.Invoke(transferBatch);
        }

        public partial class TransferSingleEventDTO : TransferSingleEventDTOBase
        {
        }

        [Event("TransferSingle")]
        public class TransferSingleEventDTOBase : IEventDTO
        {
            [Parameter("address", "operator", 0, true)]
            public virtual string Operator { get; set; }

            [Parameter("address", "from", 1, true)]
            public virtual string From { get; set; }

            [Parameter("address", "to", 2, true)] public virtual string To { get; set; }
            [Parameter("uint256", "id", 3, false)] public virtual BigInteger Id { get; set; }

            [Parameter("uint256", "value", 4, false)]
            public virtual BigInteger Value { get; set; }
        }

        public event Action<TransferSingleEventDTO> OnTransferSingle;

        private void TransferSingle(TransferSingleEventDTO transferSingle)
        {
            OnTransferSingle?.Invoke(transferSingle);
        }

        public partial class URIEventDTO : URIEventDTOBase
        {
        }

        [Event("URI")]
        public class URIEventDTOBase : IEventDTO
        {
            [Parameter("string", "value", 0, false)]
            public virtual string Value { get; set; }

            [Parameter("uint256", "id", 1, true)] public virtual BigInteger Id { get; set; }
        }

        public event Action<URIEventDTO> OnURI;

        private void URI(URIEventDTO uRI)
        {
            OnURI?.Invoke(uRI);
        }

        #endregion

        #region Interface Implemented Methods

        public async ValueTask DisposeAsync()
        {
            if (!Subscribed)
                return;


            Subscribed = false;
            try
            {
                if (EventManager == null)
                    return;

                await EventManager.Unsubscribe<ApprovalForAllEventDTO>(ApprovalForAll, ContractAddress);
                OnApprovalForAll = null;
                await EventManager.Unsubscribe<TransferBatchEventDTO>(TransferBatch, ContractAddress);
                OnTransferBatch = null;
                await EventManager.Unsubscribe<TransferSingleEventDTO>(TransferSingle, ContractAddress);
                OnTransferSingle = null;
                await EventManager.Unsubscribe<URIEventDTO>(URI, ContractAddress);
                OnURI = null;
            }
            catch (Exception e)
            {
                Debug.LogError("Caught an exception whilst unsubscribing from events\n" + e.Message);
            }
        }

        public async ValueTask InitAsync()
        {
            if (Subscribed)
                return;
            Subscribed = true;

            try
            {
                if (EventManager == null)
                    return;

                await EventManager.Subscribe<ApprovalForAllEventDTO>(ApprovalForAll, ContractAddress);
                await EventManager.Subscribe<TransferBatchEventDTO>(TransferBatch, ContractAddress);
                await EventManager.Subscribe<TransferSingleEventDTO>(TransferSingle, ContractAddress);
                await EventManager.Subscribe<URIEventDTO>(URI, ContractAddress);
            }
            catch (Exception e)
            {
                Debug.LogError(
                    "Caught an exception whilst subscribing to events. Subscribing to events will not work in this session\n" +
                    e.Message);
            }
        }

        [Obsolete("It's not advisable to use this method. Use the pre-generated methods instead.")]
        public IContract Attach(string address)
        {
            return OriginalContract.Attach(address);
        }

        [Obsolete("It's not advisable to use this method. Use the pre-generated methods instead.")]
        public Task<object[]> Call(string method, object[] parameters = null, TransactionRequest overwrite = null)
        {
            return OriginalContract.Call(method, parameters, overwrite);
        }

        [Obsolete("It's not advisable to use this method. Use the pre-generated methods instead.")]
        public object[] Decode(string method, string output)
        {
            return OriginalContract.Decode(method, output);
        }

        [Obsolete("It's not advisable to use this method. Use the pre-generated methods instead.")]
        public Task<object[]> Send(string method, object[] parameters = null, TransactionRequest overwrite = null)
        {
            return OriginalContract.Send(method, parameters, overwrite);
        }

        [Obsolete("It's not advisable to use this method. Use the pre-generated methods instead.")]
        public Task<(object[] response, TransactionReceipt receipt)> SendWithReceipt(string method,
            object[] parameters = null, TransactionRequest overwrite = null)
        {
            return OriginalContract.SendWithReceipt(method, parameters, overwrite);
        }

        [Obsolete("It's not advisable to use this method. Use the pre-generated methods instead.")]
        public Task<HexBigInteger> EstimateGas(string method, object[] parameters, TransactionRequest overwrite = null)
        {
            return OriginalContract.EstimateGas(method, parameters, overwrite);
        }

        [Obsolete("It's not advisable to use this method. Use the pre-generated methods instead.")]
        public string Calldata(string method, object[] parameters = null)
        {
            return OriginalContract.Calldata(method, parameters);
        }

        [Obsolete("It's not advisable to use this method. Use the pre-generated methods instead.")]
        public Task<TransactionRequest> PrepareTransactionRequest(string method, object[] parameters,
            bool isReadCall = false, TransactionRequest overwrite = null)
        {
            return OriginalContract.PrepareTransactionRequest(method, parameters, isReadCall, overwrite);
        }

        #endregion
    }
}