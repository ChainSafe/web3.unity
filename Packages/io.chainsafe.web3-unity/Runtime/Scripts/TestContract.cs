using System;
using System.Numerics;
using System.Threading.Tasks;
using ChainSafe.Gaming.Evm.Transactions;
using Nethereum.Hex.HexTypes;
using Nethereum.ABI.FunctionEncoding.Attributes;
using UnityEngine;
using ChainSafe.Gaming.RPC.Events;



namespace ChainSafe.Gaming.Evm.Contracts.Custom
{
    public partial class TestContract : ICustomContract
    {
        public string Address => OriginalContract.Address;

        public string ABI => "[     {         \"constant\": true,         \"inputs\": [],         \"name\": \"name\",         \"outputs\": [             {                 \"name\": \"\",                 \"type\": \"string\"             }         ],         \"payable\": false,         \"stateMutability\": \"view\",         \"type\": \"function\"     },     {         \"constant\": false,         \"inputs\": [             {                 \"name\": \"_spender\",                 \"type\": \"address\"             },             {                 \"name\": \"_value\",                 \"type\": \"uint256\"             }         ],         \"name\": \"approve\",         \"outputs\": [             {                 \"name\": \"\",                 \"type\": \"bool\"             }         ],         \"payable\": false,         \"stateMutability\": \"nonpayable\",         \"type\": \"function\"     },     {         \"constant\": true,         \"inputs\": [],         \"name\": \"totalSupply\",         \"outputs\": [             {                 \"name\": \"\",                 \"type\": \"uint256\"             }         ],         \"payable\": false,         \"stateMutability\": \"view\",         \"type\": \"function\"     },     {         \"constant\": false,         \"inputs\": [             {                 \"name\": \"_from\",                 \"type\": \"address\"             },             {                 \"name\": \"_to\",                 \"type\": \"address\"             },             {                 \"name\": \"_value\",                 \"type\": \"uint256\"             }         ],         \"name\": \"transferFrom\",         \"outputs\": [             {                 \"name\": \"\",                 \"type\": \"bool\"             }         ],         \"payable\": false,         \"stateMutability\": \"nonpayable\",         \"type\": \"function\"     },     {         \"constant\": true,         \"inputs\": [],         \"name\": \"decimals\",         \"outputs\": [             {                 \"name\": \"\",                 \"type\": \"uint8\"             }         ],         \"payable\": false,         \"stateMutability\": \"view\",         \"type\": \"function\"     },     {         \"constant\": true,         \"inputs\": [             {                 \"name\": \"_owner\",                 \"type\": \"address\"             }         ],         \"name\": \"balanceOf\",         \"outputs\": [             {                 \"name\": \"balance\",                 \"type\": \"uint256\"             }         ],         \"payable\": false,         \"stateMutability\": \"view\",         \"type\": \"function\"     },     {         \"constant\": true,         \"inputs\": [],         \"name\": \"symbol\",         \"outputs\": [             {                 \"name\": \"\",                 \"type\": \"string\"             }         ],         \"payable\": false,         \"stateMutability\": \"view\",         \"type\": \"function\"     },     {         \"constant\": false,         \"inputs\": [             {                 \"name\": \"_to\",                 \"type\": \"address\"             },             {                 \"name\": \"_value\",                 \"type\": \"uint256\"             }         ],         \"name\": \"transfer\",         \"outputs\": [             {                 \"name\": \"\",                 \"type\": \"bool\"             }         ],         \"payable\": false,         \"stateMutability\": \"nonpayable\",         \"type\": \"function\"     },     {         \"constant\": true,         \"inputs\": [             {                 \"name\": \"_owner\",                 \"type\": \"address\"             },             {                 \"name\": \"_spender\",                 \"type\": \"address\"             }         ],         \"name\": \"allowance\",         \"outputs\": [             {                 \"name\": \"\",                 \"type\": \"uint256\"             }         ],         \"payable\": false,         \"stateMutability\": \"view\",         \"type\": \"function\"     },     {         \"payable\": true,         \"stateMutability\": \"payable\",         \"type\": \"fallback\"     },     {         \"anonymous\": false,         \"inputs\": [             {                 \"indexed\": true,                 \"name\": \"owner\",                 \"type\": \"address\"             },             {                 \"indexed\": true,                 \"name\": \"spender\",                 \"type\": \"address\"             },             {                 \"indexed\": false,                 \"name\": \"value\",                 \"type\": \"uint256\"             }         ],         \"name\": \"Approval\",         \"type\": \"event\"     },     {         \"anonymous\": false,         \"inputs\": [             {                 \"indexed\": true,                 \"name\": \"from\",                 \"type\": \"address\"             },             {                 \"indexed\": true,                 \"name\": \"to\",                 \"type\": \"address\"             },             {                 \"indexed\": false,                 \"name\": \"value\",                 \"type\": \"uint256\"             }         ],         \"name\": \"Transfer\",         \"type\": \"event\"     } ]";

        public string ContractAddress { get; set; }

        public IEventManager EventManager { get; set; }

        public Contract OriginalContract { get; set; }

        public bool Subscribed { get; set; }


        #region Methods

        public async Task<string> Name()
        {
            var response = await OriginalContract.Call<string>("name", new object[] {

            });

            return response;
        }


        public async Task<bool> Approve(string _spender, BigInteger _value)
        {
            var response = await OriginalContract.Send<bool>("approve", new object[] {
                _spender, _value
            });

            return response;
        }
        public async Task<(bool, TransactionReceipt receipt)> ApproveWithReceipt(string _spender, BigInteger _value)
        {
            var response = await OriginalContract.SendWithReceipt<bool>("approve", new object[] {
                _spender, _value
            });

            return (response.response, response.receipt);
        }

        public async Task<BigInteger> TotalSupply()
        {
            var response = await OriginalContract.Call<BigInteger>("totalSupply", new object[] {

            });

            return response;
        }


        public async Task<bool> TransferFrom(string _from, string _to, BigInteger _value)
        {
            var response = await OriginalContract.Send<bool>("transferFrom", new object[] {
                _from, _to, _value
            });

            return response;
        }
        public async Task<(bool, TransactionReceipt receipt)> TransferFromWithReceipt(string _from, string _to, BigInteger _value)
        {
            var response = await OriginalContract.SendWithReceipt<bool>("transferFrom", new object[] {
                _from, _to, _value
            });

            return (response.response, response.receipt);
        }

        public async Task<BigInteger> Decimals()
        {
            var response = await OriginalContract.Call<BigInteger>("decimals", new object[] {

            });

            return response;
        }


        public async Task<BigInteger> BalanceOf(string _owner)
        {
            var response = await OriginalContract.Call<BigInteger>("balanceOf", new object[] {
                _owner
            });

            return response;
        }


        public async Task<string> Symbol()
        {
            var response = await OriginalContract.Call<string>("symbol", new object[] {

            });

            return response;
        }


        public async Task<bool> Transfer(string _to, BigInteger _value)
        {
            var response = await OriginalContract.Send<bool>("transfer", new object[] {
                _to, _value
            });

            return response;
        }
        public async Task<(bool, TransactionReceipt receipt)> TransferWithReceipt(string _to, BigInteger _value)
        {
            var response = await OriginalContract.SendWithReceipt<bool>("transfer", new object[] {
                _to, _value
            });

            return (response.response, response.receipt);
        }

        public async Task<BigInteger> Allowance(string _owner, string _spender)
        {
            var response = await OriginalContract.Call<BigInteger>("allowance", new object[] {
                _owner, _spender
            });

            return response;
        }



        #endregion


        #region Event Classes

        public partial class ApprovalEventDTO : ApprovalEventDTOBase { }

        [Event("Approval")]
        public class ApprovalEventDTOBase : IEventDTO
        {
            [Parameter("address", "owner", 0, true)]
            public virtual string Owner { get; set; }
            [Parameter("address", "spender", 1, true)]
            public virtual string Spender { get; set; }
            [Parameter("uint256", "value", 2, false)]
            public virtual BigInteger Value { get; set; }

        }

        public event Action<ApprovalEventDTO> OnApproval;

        private void Approval(ApprovalEventDTO approval)
        {
            OnApproval?.Invoke(approval);
        }

        public partial class TransferEventDTO : TransferEventDTOBase { }

        [Event("Transfer")]
        public class TransferEventDTOBase : IEventDTO
        {
            [Parameter("address", "from", 0, true)]
            public virtual string From { get; set; }
            [Parameter("address", "to", 1, true)]
            public virtual string To { get; set; }
            [Parameter("uint256", "value", 2, false)]
            public virtual BigInteger Value { get; set; }

        }

        public event Action<TransferEventDTO> OnTransfer;

        private void Transfer(TransferEventDTO transfer)
        {
            OnTransfer?.Invoke(transfer);
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

                await EventManager.Unsubscribe<ApprovalEventDTO>(Approval, ContractAddress);
                OnApproval = null;
                await EventManager.Unsubscribe<TransferEventDTO>(Transfer, ContractAddress);
                OnTransfer = null;



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

                await EventManager.Subscribe<ApprovalEventDTO>(Approval, ContractAddress);
                await EventManager.Subscribe<TransferEventDTO>(Transfer, ContractAddress);

            }
            catch (Exception e)
            {
                Debug.LogError("Caught an exception whilst subscribing to events. Subscribing to events will not work in this session\n" + e.Message);
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
        public Task<(object[] response, TransactionReceipt receipt)> SendWithReceipt(string method, object[] parameters = null, TransactionRequest overwrite = null)
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
        public Task<TransactionRequest> PrepareTransactionRequest(string method, object[] parameters, bool isReadCall = false, TransactionRequest overwrite = null)
        {
            return OriginalContract.PrepareTransactionRequest(method, parameters, isReadCall, overwrite);
        }
        #endregion
    }


}
