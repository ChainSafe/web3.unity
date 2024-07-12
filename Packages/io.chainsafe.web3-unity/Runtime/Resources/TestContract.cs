using System.Threading.Tasks;
using System;
using ChainSafe.Gaming.Evm.Transactions;
using Nethereum.Hex.HexTypes;
using ChainSafe.Gaming.Evm.Contracts;
using System.Numerics;
using Nethereum.RPC.Reactive.Eth.Subscriptions;
using Nethereum.JsonRpc.WebSocketStreamingClient;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;
using UnityEngine;



namespace ChainSafe.Gaming.Evm.Contracts.Custom
{
    public class TestContract : ICustomContract
    {
       
        public string ABI => "[     {         \"constant\": true,         \"inputs\": [],         \"name\": \"name\",         \"outputs\": [             {                 \"name\": \"\",                 \"type\": \"string\"             }         ],         \"payable\": false,         \"stateMutability\": \"view\",         \"type\": \"function\"     },     {         \"constant\": false,         \"inputs\": [             {                 \"name\": \"_spender\",                 \"type\": \"address\"             },             {                 \"name\": \"_value\",                 \"type\": \"uint256\"             }         ],         \"name\": \"approve\",         \"outputs\": [             {                 \"name\": \"\",                 \"type\": \"bool\"             }         ],         \"payable\": false,         \"stateMutability\": \"nonpayable\",         \"type\": \"function\"     },     {         \"constant\": true,         \"inputs\": [],         \"name\": \"totalSupply\",         \"outputs\": [             {                 \"name\": \"\",                 \"type\": \"uint256\"             }         ],         \"payable\": false,         \"stateMutability\": \"view\",         \"type\": \"function\"     },     {         \"constant\": false,         \"inputs\": [             {                 \"name\": \"_from\",                 \"type\": \"address\"             },             {                 \"name\": \"_to\",                 \"type\": \"address\"             },             {                 \"name\": \"_value\",                 \"type\": \"uint256\"             }         ],         \"name\": \"transferFrom\",         \"outputs\": [             {                 \"name\": \"\",                 \"type\": \"bool\"             }         ],         \"payable\": false,         \"stateMutability\": \"nonpayable\",         \"type\": \"function\"     },     {         \"constant\": true,         \"inputs\": [],         \"name\": \"decimals\",         \"outputs\": [             {                 \"name\": \"\",                 \"type\": \"uint8\"             }         ],         \"payable\": false,         \"stateMutability\": \"view\",         \"type\": \"function\"     },     {         \"constant\": true,         \"inputs\": [             {                 \"name\": \"_owner\",                 \"type\": \"address\"             }         ],         \"name\": \"balanceOf\",         \"outputs\": [             {                 \"name\": \"balance\",                 \"type\": \"uint256\"             }         ],         \"payable\": false,         \"stateMutability\": \"view\",         \"type\": \"function\"     },     {         \"constant\": true,         \"inputs\": [],         \"name\": \"symbol\",         \"outputs\": [             {                 \"name\": \"\",                 \"type\": \"string\"             }         ],         \"payable\": false,         \"stateMutability\": \"view\",         \"type\": \"function\"     },     {         \"constant\": false,         \"inputs\": [             {                 \"name\": \"_to\",                 \"type\": \"address\"             },             {                 \"name\": \"_value\",                 \"type\": \"uint256\"             }         ],         \"name\": \"transfer\",         \"outputs\": [             {                 \"name\": \"\",                 \"type\": \"bool\"             }         ],         \"payable\": false,         \"stateMutability\": \"nonpayable\",         \"type\": \"function\"     },     {         \"constant\": true,         \"inputs\": [             {                 \"name\": \"_owner\",                 \"type\": \"address\"             },             {                 \"name\": \"_spender\",                 \"type\": \"address\"             }         ],         \"name\": \"allowance\",         \"outputs\": [             {                 \"name\": \"\",                 \"type\": \"uint256\"             }         ],         \"payable\": false,         \"stateMutability\": \"view\",         \"type\": \"function\"     },     {         \"payable\": true,         \"stateMutability\": \"payable\",         \"type\": \"fallback\"     },     {         \"anonymous\": false,         \"inputs\": [             {                 \"indexed\": true,                 \"name\": \"owner\",                 \"type\": \"address\"             },             {                 \"indexed\": true,                 \"name\": \"spender\",                 \"type\": \"address\"             },             {                 \"indexed\": false,                 \"name\": \"value\",                 \"type\": \"uint256\"             }         ],         \"name\": \"Approval\",         \"type\": \"event\"     },     {         \"anonymous\": false,         \"inputs\": [             {                 \"indexed\": true,                 \"name\": \"from\",                 \"type\": \"address\"             },             {                 \"indexed\": true,                 \"name\": \"to\",                 \"type\": \"address\"             },             {                 \"indexed\": false,                 \"name\": \"value\",                 \"type\": \"uint256\"             }         ],         \"name\": \"Transfer\",         \"type\": \"event\"     } ]";
        
        public string ContractAddress { get; set; }
        
        public IContractBuilder ContractBuilder { get; set; }

        public Contract OriginalContract { get; set; }
        
        public string WebSocketUrl { get; set; }

        private StreamingWebSocketClient _webSocketClient;
        
        #region Methods

        public async Task<string> Name() 
        {
            var response = await OriginalContract.Call("name", new object [] {
                
            });
            
            return (string) response[0];
        }


        public async Task<bool> Approve(string _spender, BigInteger _value) 
        {
            var response = await OriginalContract.Send("approve", new object [] {
                _spender, _value
            });
            
            return (bool) response[0];
        }
        public async Task<(bool , TransactionReceipt receipt)> ApproveWithReceipt(string _spender, BigInteger _value) 
        {
            var response = await OriginalContract.SendWithReceipt("approve", new object [] {
                _spender, _value
            });
            
            return ((bool) response.response[0], response.receipt);
        }

        public async Task<BigInteger> TotalSupply() 
        {
            var response = await OriginalContract.Call("totalSupply", new object [] {
                
            });
            
            return (BigInteger) response[0];
        }


        public async Task<bool> TransferFrom(string _from, string _to, BigInteger _value) 
        {
            var response = await OriginalContract.Send("transferFrom", new object [] {
                _from, _to, _value
            });
            
            return (bool) response[0];
        }
        public async Task<(bool , TransactionReceipt receipt)> TransferFromWithReceipt(string _from, string _to, BigInteger _value) 
        {
            var response = await OriginalContract.SendWithReceipt("transferFrom", new object [] {
                _from, _to, _value
            });
            
            return ((bool) response.response[0], response.receipt);
        }

        public async Task<byte> Decimals() 
        {
            var response = await OriginalContract.Call("decimals", new object [] {
                
            });
            
            return (byte) response[0];
        }


        public async Task<BigInteger> BalanceOf(string _owner) 
        {
            var response = await OriginalContract.Call("balanceOf", new object [] {
                _owner
            });
            
            return (BigInteger) response[0];
        }


        public async Task<string> Symbol() 
        {
            var response = await OriginalContract.Call("symbol", new object [] {
                
            });
            
            return (string) response[0];
        }


        public async Task<bool> Transfer(string _to, BigInteger _value) 
        {
            var response = await OriginalContract.Send("transfer", new object [] {
                _to, _value
            });
            
            return (bool) response[0];
        }
        public async Task<(bool , TransactionReceipt receipt)> TransferWithReceipt(string _to, BigInteger _value) 
        {
            var response = await OriginalContract.SendWithReceipt("transfer", new object [] {
                _to, _value
            });
            
            return ((bool) response.response[0], response.receipt);
        }

        public async Task<BigInteger> Allowance(string _owner, string _spender) 
        {
            var response = await OriginalContract.Call("allowance", new object [] {
                _owner, _spender
            });
            
            return (BigInteger) response[0];
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
    
    EthLogsObservableSubscription eventApproval;
    public event Action<ApprovalEventDTO> OnApproval;

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
    
    EthLogsObservableSubscription eventTransfer;
    public event Action<TransferEventDTO> OnTransfer;


        #endregion
        
        #region Interface Implemented Methods
        
        public async ValueTask DisposeAsync()
        {

			await eventApproval.UnsubscribeAsync();
			OnApproval = null;
			await eventTransfer.UnsubscribeAsync();
			OnTransfer = null;

            _webSocketClient?.Dispose();
        }
        
        public async ValueTask Init()
        {
            if(!string.IsNullOrEmpty(WebSocketUrl))
            {
                _webSocketClient = new StreamingWebSocketClient(WebSocketUrl);
                await _webSocketClient.StartAsync();
            }
            else
            {
                Debug.LogWarning($"WebSocketUrl is not set for this class. Event Subscriptions will not work.");
                return;
            }

			var filterApprovalEvent = Event<ApprovalEventDTO>.GetEventABI().CreateFilterInput();
            eventApproval = new EthLogsObservableSubscription(_webSocketClient);
            
            eventApproval.GetSubscriptionDataResponsesAsObservable().Subscribe(log =>
            {
                try
                {
                    var decoded = Event<ApprovalEventDTO>.DecodeEvent(log);
                    if (decoded != null)
                    {
                       OnApproval?.Invoke(decoded.Event);
                    }  
                }
                catch (Exception ex)
                {
                    Debug.LogError("Log Address: " + log.Address + " is not a standard transfer log:" + ex.Message);
                }
            });
            
            await eventApproval.SubscribeAsync(filterApprovalEvent);
			var filterTransferEvent = Event<TransferEventDTO>.GetEventABI().CreateFilterInput();
            eventTransfer = new EthLogsObservableSubscription(_webSocketClient);
            
            eventTransfer.GetSubscriptionDataResponsesAsObservable().Subscribe(log =>
            {
                try
                {
                    var decoded = Event<TransferEventDTO>.DecodeEvent(log);
                    if (decoded != null)
                    {
                       OnTransfer?.Invoke(decoded.Event);
                    }  
                }
                catch (Exception ex)
                {
                    Debug.LogError("Log Address: " + log.Address + " is not a standard transfer log:" + ex.Message);
                }
            });
            
            await eventTransfer.SubscribeAsync(filterTransferEvent);

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
        public Task<TransactionRequest> PrepareTransactionRequest(string method, object[] parameters, TransactionRequest overwrite = null)
        {
            return OriginalContract.PrepareTransactionRequest(method, parameters, overwrite);
        }
        #endregion
    }
}
