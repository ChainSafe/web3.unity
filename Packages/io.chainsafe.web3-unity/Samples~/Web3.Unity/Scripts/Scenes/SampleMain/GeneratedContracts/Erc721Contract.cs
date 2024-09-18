using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Net.WebSockets;
using System.Threading.Tasks;
using ChainSafe.Gaming.Evm.Transactions;
using ChainSafe.Gaming.Evm.Contracts;
using ChainSafe.Gaming.Evm.Contracts.BuiltIn;
using ChainSafe.Gaming.MultiCall;
using ChainSafe.Gaming.UnityPackage;
using ChainSafe.Gaming.Web3;
using JetBrains.Annotations;
using Nethereum.Hex.HexTypes;
using Nethereum.Contracts;
using Nethereum.RPC.Reactive.Eth.Subscriptions;
using Nethereum.JsonRpc.WebSocketStreamingClient;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts.QueryHandlers.MultiCall;
using UnityEngine;
using WalletConnectSharp.Common.Utils;


namespace ChainSafe.Gaming.Evm.Contracts.Custom
{
    public class Erc721Contract : ICustomContract
    {
        public string Address => OriginalContract.Address;

        public string ABI =>
            "[   {     \"inputs\": [      ],     \"stateMutability\": \"nonpayable\",     \"type\": \"constructor\"   },   {     \"inputs\": [       {         \"internalType\": \"address\",         \"name\": \"sender\",         \"type\": \"address\"       },       {         \"internalType\": \"uint256\",         \"name\": \"tokenId\",         \"type\": \"uint256\"       },       {         \"internalType\": \"address\",         \"name\": \"owner\",         \"type\": \"address\"       }     ],     \"name\": \"ERC721IncorrectOwner\",     \"type\": \"error\"   },   {     \"inputs\": [       {         \"internalType\": \"address\",         \"name\": \"operator\",         \"type\": \"address\"       },       {         \"internalType\": \"uint256\",         \"name\": \"tokenId\",         \"type\": \"uint256\"       }     ],     \"name\": \"ERC721InsufficientApproval\",     \"type\": \"error\"   },   {     \"inputs\": [       {         \"internalType\": \"address\",         \"name\": \"approver\",         \"type\": \"address\"       }     ],     \"name\": \"ERC721InvalidApprover\",     \"type\": \"error\"   },   {     \"inputs\": [       {         \"internalType\": \"address\",         \"name\": \"operator\",         \"type\": \"address\"       }     ],     \"name\": \"ERC721InvalidOperator\",     \"type\": \"error\"   },   {     \"inputs\": [       {         \"internalType\": \"address\",         \"name\": \"owner\",         \"type\": \"address\"       }     ],     \"name\": \"ERC721InvalidOwner\",     \"type\": \"error\"   },   {     \"inputs\": [       {         \"internalType\": \"address\",         \"name\": \"receiver\",         \"type\": \"address\"       }     ],     \"name\": \"ERC721InvalidReceiver\",     \"type\": \"error\"   },   {     \"inputs\": [       {         \"internalType\": \"address\",         \"name\": \"sender\",         \"type\": \"address\"       }     ],     \"name\": \"ERC721InvalidSender\",     \"type\": \"error\"   },   {     \"inputs\": [       {         \"internalType\": \"uint256\",         \"name\": \"tokenId\",         \"type\": \"uint256\"       }     ],     \"name\": \"ERC721NonexistentToken\",     \"type\": \"error\"   },   {     \"anonymous\": false,     \"inputs\": [       {         \"indexed\": true,         \"internalType\": \"address\",         \"name\": \"owner\",         \"type\": \"address\"       },       {         \"indexed\": true,         \"internalType\": \"address\",         \"name\": \"approved\",         \"type\": \"address\"       },       {         \"indexed\": true,         \"internalType\": \"uint256\",         \"name\": \"tokenId\",         \"type\": \"uint256\"       }     ],     \"name\": \"Approval\",     \"type\": \"event\"   },   {     \"anonymous\": false,     \"inputs\": [       {         \"indexed\": true,         \"internalType\": \"address\",         \"name\": \"owner\",         \"type\": \"address\"       },       {         \"indexed\": true,         \"internalType\": \"address\",         \"name\": \"operator\",         \"type\": \"address\"       },       {         \"indexed\": false,         \"internalType\": \"bool\",         \"name\": \"approved\",         \"type\": \"bool\"       }     ],     \"name\": \"ApprovalForAll\",     \"type\": \"event\"   },   {     \"anonymous\": false,     \"inputs\": [       {         \"indexed\": false,         \"internalType\": \"uint256\",         \"name\": \"_fromTokenId\",         \"type\": \"uint256\"       },       {         \"indexed\": false,         \"internalType\": \"uint256\",         \"name\": \"_toTokenId\",         \"type\": \"uint256\"       }     ],     \"name\": \"BatchMetadataUpdate\",     \"type\": \"event\"   },   {     \"anonymous\": false,     \"inputs\": [       {         \"indexed\": false,         \"internalType\": \"uint256\",         \"name\": \"_tokenId\",         \"type\": \"uint256\"       }     ],     \"name\": \"MetadataUpdate\",     \"type\": \"event\"   },   {     \"anonymous\": false,     \"inputs\": [       {         \"indexed\": true,         \"internalType\": \"address\",         \"name\": \"from\",         \"type\": \"address\"       },       {         \"indexed\": true,         \"internalType\": \"address\",         \"name\": \"to\",         \"type\": \"address\"       },       {         \"indexed\": true,         \"internalType\": \"uint256\",         \"name\": \"tokenId\",         \"type\": \"uint256\"       }     ],     \"name\": \"Transfer\",     \"type\": \"event\"   },   {     \"inputs\": [       {         \"internalType\": \"address\",         \"name\": \"to\",         \"type\": \"address\"       },       {         \"internalType\": \"uint256\",         \"name\": \"tokenId\",         \"type\": \"uint256\"       }     ],     \"name\": \"approve\",     \"outputs\": [      ],     \"stateMutability\": \"nonpayable\",     \"type\": \"function\"   },   {     \"inputs\": [       {         \"internalType\": \"address\",         \"name\": \"owner\",         \"type\": \"address\"       }     ],     \"name\": \"balanceOf\",     \"outputs\": [       {         \"internalType\": \"uint256\",         \"name\": \"\",         \"type\": \"uint256\"       }     ],     \"stateMutability\": \"view\",     \"type\": \"function\"   },   {     \"inputs\": [       {         \"internalType\": \"uint256\",         \"name\": \"tokenId\",         \"type\": \"uint256\"       }     ],     \"name\": \"getApproved\",     \"outputs\": [       {         \"internalType\": \"address\",         \"name\": \"\",         \"type\": \"address\"       }     ],     \"stateMutability\": \"view\",     \"type\": \"function\"   },   {     \"inputs\": [       {         \"internalType\": \"address\",         \"name\": \"owner\",         \"type\": \"address\"       },       {         \"internalType\": \"address\",         \"name\": \"operator\",         \"type\": \"address\"       }     ],     \"name\": \"isApprovedForAll\",     \"outputs\": [       {         \"internalType\": \"bool\",         \"name\": \"\",         \"type\": \"bool\"       }     ],     \"stateMutability\": \"view\",     \"type\": \"function\"   },   {     \"inputs\": [      ],     \"name\": \"name\",     \"outputs\": [       {         \"internalType\": \"string\",         \"name\": \"\",         \"type\": \"string\"       }     ],     \"stateMutability\": \"view\",     \"type\": \"function\"   },   {     \"inputs\": [       {         \"internalType\": \"uint256\",         \"name\": \"tokenId\",         \"type\": \"uint256\"       }     ],     \"name\": \"ownerOf\",     \"outputs\": [       {         \"internalType\": \"address\",         \"name\": \"\",         \"type\": \"address\"       }     ],     \"stateMutability\": \"view\",     \"type\": \"function\"   },   {     \"inputs\": [       {         \"internalType\": \"address\",         \"name\": \"_to\",         \"type\": \"address\"       },       {         \"internalType\": \"string\",         \"name\": \"_uri\",         \"type\": \"string\"       }     ],     \"name\": \"safeMint\",     \"outputs\": [      ],     \"stateMutability\": \"nonpayable\",     \"type\": \"function\"   },   {     \"inputs\": [       {         \"internalType\": \"address\",         \"name\": \"from\",         \"type\": \"address\"       },       {         \"internalType\": \"address\",         \"name\": \"to\",         \"type\": \"address\"       },       {         \"internalType\": \"uint256\",         \"name\": \"tokenId\",         \"type\": \"uint256\"       }     ],     \"name\": \"safeTransferFrom\",     \"outputs\": [      ],     \"stateMutability\": \"nonpayable\",     \"type\": \"function\"   },   {     \"inputs\": [       {         \"internalType\": \"address\",         \"name\": \"from\",         \"type\": \"address\"       },       {         \"internalType\": \"address\",         \"name\": \"to\",         \"type\": \"address\"       },       {         \"internalType\": \"uint256\",         \"name\": \"tokenId\",         \"type\": \"uint256\"       },       {         \"internalType\": \"bytes\",         \"name\": \"data\",         \"type\": \"bytes\"       }     ],     \"name\": \"safeTransferFrom\",     \"outputs\": [      ],     \"stateMutability\": \"nonpayable\",     \"type\": \"function\"   },   {     \"inputs\": [       {         \"internalType\": \"address\",         \"name\": \"operator\",         \"type\": \"address\"       },       {         \"internalType\": \"bool\",         \"name\": \"approved\",         \"type\": \"bool\"       }     ],     \"name\": \"setApprovalForAll\",     \"outputs\": [      ],     \"stateMutability\": \"nonpayable\",     \"type\": \"function\"   },   {     \"inputs\": [       {         \"internalType\": \"bytes4\",         \"name\": \"interfaceId\",         \"type\": \"bytes4\"       }     ],     \"name\": \"supportsInterface\",     \"outputs\": [       {         \"internalType\": \"bool\",         \"name\": \"\",         \"type\": \"bool\"       }     ],     \"stateMutability\": \"view\",     \"type\": \"function\"   },   {     \"inputs\": [      ],     \"name\": \"symbol\",     \"outputs\": [       {         \"internalType\": \"string\",         \"name\": \"\",         \"type\": \"string\"       }     ],     \"stateMutability\": \"view\",     \"type\": \"function\"   },   {     \"inputs\": [       {         \"internalType\": \"uint256\",         \"name\": \"tokenId\",         \"type\": \"uint256\"       }     ],     \"name\": \"tokenURI\",     \"outputs\": [       {         \"internalType\": \"string\",         \"name\": \"\",         \"type\": \"string\"       }     ],     \"stateMutability\": \"view\",     \"type\": \"function\"   },   {     \"inputs\": [       {         \"internalType\": \"address\",         \"name\": \"from\",         \"type\": \"address\"       },       {         \"internalType\": \"address\",         \"name\": \"to\",         \"type\": \"address\"       },       {         \"internalType\": \"uint256\",         \"name\": \"tokenId\",         \"type\": \"uint256\"       }     ],     \"name\": \"transferFrom\",     \"outputs\": [      ],     \"stateMutability\": \"nonpayable\",     \"type\": \"function\"   } ]";

        public string ContractAddress { get; set; }

        public IContractBuilder ContractBuilder { get; set; }

        public Contract OriginalContract { get; set; }

        public string WebSocketUrl { get; set; }

        public bool Subscribed { get; set; }

        private StreamingWebSocketClient _webSocketClient;

        #region Methods

        public async Task Approve(string to, BigInteger tokenId)
        {
            var response = await OriginalContract.Send("approve", new object[]
            {
                to, tokenId
            });
        }

        public async Task<TransactionReceipt> ApproveWithReceipt(string to, BigInteger tokenId)
        {
            var response = await OriginalContract.SendWithReceipt("approve", new object[]
            {
                to, tokenId
            });

            return response.receipt;
        }

        public async Task<BigInteger> BalanceOf(string owner)
        {
            var response = await OriginalContract.Call<BigInteger>("balanceOf", new object[]
            {
                owner
            });

            return response;
        }


        public async Task<string> GetApproved(BigInteger tokenId)
        {
            var response = await OriginalContract.Call<string>("getApproved", new object[]
            {
                tokenId
            });

            return response;
        }


        public async Task<bool> IsApprovedForAll(string owner, string @operator)
        {
            var response = await OriginalContract.Call<bool>("isApprovedForAll", new object[]
            {
                owner, @operator
            });

            return response;
        }


        public async Task<string> Name()
        {
            var response = await OriginalContract.Call<string>("name", new object[]
            {
            });

            return response;
        }


        public async Task<string> OwnerOf(BigInteger tokenId)
        {
            var response = await OriginalContract.Call<string>("ownerOf", new object[]
            {
                tokenId
            });

            return response;
        }


        public async Task SafeMint(string _to, string _uri)
        {
            var response = await OriginalContract.Send("safeMint", new object[]
            {
                _to, _uri
            });
        }

        public async Task<TransactionReceipt> SafeMintWithReceipt(string _to, string _uri)
        {
            var response = await OriginalContract.SendWithReceipt("safeMint", new object[]
            {
                _to, _uri
            });

            return response.receipt;
        }

        public async Task SafeTransferFrom(string from, string to, BigInteger tokenId)
        {
            var response = await OriginalContract.Send("safeTransferFrom", new object[]
            {
                from, to, tokenId
            });
        }

        public async Task<TransactionReceipt> SafeTransferFromWithReceipt(string from, string to, BigInteger tokenId)
        {
            var response = await OriginalContract.SendWithReceipt("safeTransferFrom", new object[]
            {
                from, to, tokenId
            });

            return response.receipt;
        }

        public async Task SafeTransferFrom(string from, string to, BigInteger tokenId, byte[] data)
        {
            var response = await OriginalContract.Send("safeTransferFrom", new object[]
            {
                from, to, tokenId, data
            });
        }

        public async Task<TransactionReceipt> SafeTransferFromWithReceipt(string from, string to, BigInteger tokenId,
            byte[] data)
        {
            var response = await OriginalContract.SendWithReceipt("safeTransferFrom", new object[]
            {
                from, to, tokenId, data
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


        public async Task<string> Symbol()
        {
            var response = await OriginalContract.Call<string>("symbol", new object[]
            {
            });

            return response;
        }


        public async Task<string> TokenURI(BigInteger tokenId)
        {
            var response = await OriginalContract.Call<string>("tokenURI", new object[]
            {
                tokenId
            });

            return response;
        }


        public async Task TransferFrom(string from, string to, BigInteger tokenId)
        {
            var response = await OriginalContract.Send("transferFrom", new object[]
            {
                from, to, tokenId
            });
        }

        public async Task<TransactionReceipt> TransferFromWithReceipt(string from, string to, BigInteger tokenId)
        {
            var response = await OriginalContract.SendWithReceipt("transferFrom", new object[]
            {
                from, to, tokenId
            });

            return response.receipt;
        }

        #endregion


        #region Event Classes

        public partial class ApprovalEventDTO : ApprovalEventDTOBase
        {
        }

        [Event("Approval")]
        public class ApprovalEventDTOBase : IEventDTO
        {
            [Parameter("address", "owner", 0, true)]
            public virtual string Owner { get; set; }

            [Parameter("address", "approved", 1, true)]
            public virtual string Approved { get; set; }

            [Parameter("uint256", "tokenId", 2, true)]
            public virtual BigInteger TokenId { get; set; }
        }

        private EthLogsObservableSubscription eventApproval;
        public event Action<ApprovalEventDTO> OnApproval;

        public partial class ApprovalForAllEventDTO : ApprovalForAllEventDTOBase
        {
        }

        [Event("ApprovalForAll")]
        public class ApprovalForAllEventDTOBase : IEventDTO
        {
            [Parameter("address", "owner", 0, true)]
            public virtual string Owner { get; set; }

            [Parameter("address", "operator", 1, true)]
            public virtual string Operator { get; set; }

            [Parameter("bool", "approved", 2, false)]
            public virtual bool Approved { get; set; }
        }

        private EthLogsObservableSubscription eventApprovalForAll;
        public event Action<ApprovalForAllEventDTO> OnApprovalForAll;

        public partial class BatchMetadataUpdateEventDTO : BatchMetadataUpdateEventDTOBase
        {
        }

        [Event("BatchMetadataUpdate")]
        public class BatchMetadataUpdateEventDTOBase : IEventDTO
        {
            [Parameter("uint256", "_fromTokenId", 0, false)]
            public virtual BigInteger FromTokenId { get; set; }

            [Parameter("uint256", "_toTokenId", 1, false)]
            public virtual BigInteger ToTokenId { get; set; }
        }

        private EthLogsObservableSubscription eventBatchMetadataUpdate;
        public event Action<BatchMetadataUpdateEventDTO> OnBatchMetadataUpdate;

        public partial class MetadataUpdateEventDTO : MetadataUpdateEventDTOBase
        {
        }

        [Event("MetadataUpdate")]
        public class MetadataUpdateEventDTOBase : IEventDTO
        {
            [Parameter("uint256", "_tokenId", 0, false)]
            public virtual BigInteger TokenId { get; set; }
        }

        private EthLogsObservableSubscription eventMetadataUpdate;
        public event Action<MetadataUpdateEventDTO> OnMetadataUpdate;

        public partial class TransferEventDTO : TransferEventDTOBase
        {
        }

        [Event("Transfer")]
        public class TransferEventDTOBase : IEventDTO
        {
            [Parameter("address", "from", 0, true)]
            public virtual string From { get; set; }

            [Parameter("address", "to", 1, true)] public virtual string To { get; set; }

            [Parameter("uint256", "tokenId", 2, true)]
            public virtual BigInteger TokenId { get; set; }
        }

        private EthLogsObservableSubscription eventTransfer;
        public event Action<TransferEventDTO> OnTransfer;

        #endregion

        #region Interface Implemented Methods

        public async ValueTask DisposeAsync()
        {
            if (string.IsNullOrEmpty(WebSocketUrl))
                return;
            if (!Subscribed)
                return;

            if (Application.platform == RuntimePlatform.WebGLPlayer)
                return;
            Subscribed = false;
            try
            {
                await eventApproval.UnsubscribeAsync();
                OnApproval = null;
                await eventApprovalForAll.UnsubscribeAsync();
                OnApprovalForAll = null;
                await eventBatchMetadataUpdate.UnsubscribeAsync();
                OnBatchMetadataUpdate = null;
                await eventMetadataUpdate.UnsubscribeAsync();
                OnMetadataUpdate = null;
                await eventTransfer.UnsubscribeAsync();
                OnTransfer = null;

                _webSocketClient?.Dispose();
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

            if (string.IsNullOrEmpty(WebSocketUrl))
            {
                Debug.LogWarning($"WebSocketUrl is not set for this class. Event Subscriptions will not work.");
                return;
            }


            try
            {
                if (Application.platform == RuntimePlatform.WebGLPlayer)
                {
                    Debug.LogWarning("WebGL Platform is currently not supporting event subscription");
                    return;
                }
                

                _webSocketClient ??= new StreamingWebSocketClient(WebSocketUrl);
                
                if (_webSocketClient != null && (_webSocketClient.WebSocketState != WebSocketState.None && _webSocketClient.WebSocketState != WebSocketState.Open &&
                                                 _webSocketClient.WebSocketState != WebSocketState.CloseReceived))
                {
                    Debug.LogWarning(
                        $"Websocket is in an invalid state {_webSocketClient.WebSocketState}. It needs to be in a state Open or CloseReceived");
                    return;
                }
                
                await _webSocketClient.StartAsync();

                var filterApprovalEvent = Event<ApprovalEventDTO>.GetEventABI().CreateFilterInput(ContractAddress);
                eventApproval = new EthLogsObservableSubscription(_webSocketClient);

                eventApproval.GetSubscriptionDataResponsesAsObservable().Subscribe(log =>
                {
                    try
                    {
                        var decoded = Event<ApprovalEventDTO>.DecodeEvent(log);
                        if (decoded != null) OnApproval?.Invoke(decoded.Event);
                    }
                    catch (Exception ex)
                    {
                        Debug.LogError("Log Address: " + log.Address + " is not a standard transfer log:" + ex.Message);
                    }
                });

                await eventApproval.SubscribeAsync(filterApprovalEvent);
                var filterApprovalForAllEvent =
                    Event<ApprovalForAllEventDTO>.GetEventABI().CreateFilterInput(ContractAddress);
                eventApprovalForAll = new EthLogsObservableSubscription(_webSocketClient);

                eventApprovalForAll.GetSubscriptionDataResponsesAsObservable().Subscribe(log =>
                {
                    try
                    {
                        var decoded = Event<ApprovalForAllEventDTO>.DecodeEvent(log);
                        if (decoded != null) OnApprovalForAll?.Invoke(decoded.Event);
                    }
                    catch (Exception ex)
                    {
                        Debug.LogError("Log Address: " + log.Address + " is not a standard transfer log:" + ex.Message);
                    }
                });

                await eventApprovalForAll.SubscribeAsync(filterApprovalForAllEvent);
                var filterBatchMetadataUpdateEvent =
                    Event<BatchMetadataUpdateEventDTO>.GetEventABI().CreateFilterInput(ContractAddress);
                eventBatchMetadataUpdate = new EthLogsObservableSubscription(_webSocketClient);

                eventBatchMetadataUpdate.GetSubscriptionDataResponsesAsObservable().Subscribe(log =>
                {
                    try
                    {
                        var decoded = Event<BatchMetadataUpdateEventDTO>.DecodeEvent(log);
                        if (decoded != null) OnBatchMetadataUpdate?.Invoke(decoded.Event);
                    }
                    catch (Exception ex)
                    {
                        Debug.LogError("Log Address: " + log.Address + " is not a standard transfer log:" + ex.Message);
                    }
                });

                await eventBatchMetadataUpdate.SubscribeAsync(filterBatchMetadataUpdateEvent);
                var filterMetadataUpdateEvent =
                    Event<MetadataUpdateEventDTO>.GetEventABI().CreateFilterInput(ContractAddress);
                eventMetadataUpdate = new EthLogsObservableSubscription(_webSocketClient);

                eventMetadataUpdate.GetSubscriptionDataResponsesAsObservable().Subscribe(log =>
                {
                    try
                    {
                        var decoded = Event<MetadataUpdateEventDTO>.DecodeEvent(log);
                        if (decoded != null) OnMetadataUpdate?.Invoke(decoded.Event);
                    }
                    catch (Exception ex)
                    {
                        Debug.LogError("Log Address: " + log.Address + " is not a standard transfer log:" + ex.Message);
                    }
                });

                await eventMetadataUpdate.SubscribeAsync(filterMetadataUpdateEvent);
                var filterTransferEvent = Event<TransferEventDTO>.GetEventABI().CreateFilterInput(ContractAddress);
                eventTransfer = new EthLogsObservableSubscription(_webSocketClient);

                eventTransfer.GetSubscriptionDataResponsesAsObservable().Subscribe(log =>
                {
                    try
                    {
                        var decoded = Event<TransferEventDTO>.DecodeEvent(log);
                        if (decoded != null) OnTransfer?.Invoke(decoded.Event);
                    }
                    catch (Exception ex)
                    {
                        Debug.LogError("Log Address: " + log.Address + " is not a standard transfer log:" + ex.Message);
                    }
                });

                await eventTransfer.SubscribeAsync(filterTransferEvent);
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

        [Pure]
        public async Task<List<OwnerOfBatchModel>> GetOwnerOfBatch(string[] tokenIds)
        {
            var multiCall = (IMultiCall)Web3Unity.Web3.ServiceProvider.GetService(typeof(IMultiCall));
            if (multiCall == null)
                throw new Web3Exception(
                    $"Can't execute {nameof(GetOwnerOfBatch)}. No MultiCall component was provided during construction.");

            var calls = tokenIds
                .Select(BuildCall)
                .ToList();

            var multiCallResponse = await multiCall.MultiCallAsync(calls.ToArray());

            return multiCallResponse
                .Select(BuildResult)
                .ToList();

            Call3Value BuildCall(string tokenId)
            {
                object param = tokenId.StartsWith("0x") ? tokenId : BigInteger.Parse(tokenId);
                var callData = OriginalContract.Calldata(EthMethods.OwnerOf, new[] { param });
                return new Call3Value
                    { Target = OriginalContract.Address, AllowFailure = true, CallData = callData.HexToByteArray() };
            }

            OwnerOfBatchModel BuildResult(Result result, int index)
            {
                if (result is not { Success: true }) return new OwnerOfBatchModel { Failure = true };

                var owner = OriginalContract.Decode(EthMethods.OwnerOf, result.ReturnData.ToHex());
                return new OwnerOfBatchModel { TokenId = tokenIds[index], Owner = owner[0].ToString() };
            }
        }
    }
}