using System;
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
    public partial class Erc721Contract : ICustomContract
    {
        public string Address => OriginalContract.Address;
       
        public string ABI => "[   {     \"inputs\": [      ],     \"stateMutability\": \"nonpayable\",     \"type\": \"constructor\"   },   {     \"inputs\": [       {         \"internalType\": \"address\",         \"name\": \"sender\",         \"type\": \"address\"       },       {         \"internalType\": \"uint256\",         \"name\": \"tokenId\",         \"type\": \"uint256\"       },       {         \"internalType\": \"address\",         \"name\": \"owner\",         \"type\": \"address\"       }     ],     \"name\": \"ERC721IncorrectOwner\",     \"type\": \"error\"   },   {     \"inputs\": [       {         \"internalType\": \"address\",         \"name\": \"operator\",         \"type\": \"address\"       },       {         \"internalType\": \"uint256\",         \"name\": \"tokenId\",         \"type\": \"uint256\"       }     ],     \"name\": \"ERC721InsufficientApproval\",     \"type\": \"error\"   },   {     \"inputs\": [       {         \"internalType\": \"address\",         \"name\": \"approver\",         \"type\": \"address\"       }     ],     \"name\": \"ERC721InvalidApprover\",     \"type\": \"error\"   },   {     \"inputs\": [       {         \"internalType\": \"address\",         \"name\": \"operator\",         \"type\": \"address\"       }     ],     \"name\": \"ERC721InvalidOperator\",     \"type\": \"error\"   },   {     \"inputs\": [       {         \"internalType\": \"address\",         \"name\": \"owner\",         \"type\": \"address\"       }     ],     \"name\": \"ERC721InvalidOwner\",     \"type\": \"error\"   },   {     \"inputs\": [       {         \"internalType\": \"address\",         \"name\": \"receiver\",         \"type\": \"address\"       }     ],     \"name\": \"ERC721InvalidReceiver\",     \"type\": \"error\"   },   {     \"inputs\": [       {         \"internalType\": \"address\",         \"name\": \"sender\",         \"type\": \"address\"       }     ],     \"name\": \"ERC721InvalidSender\",     \"type\": \"error\"   },   {     \"inputs\": [       {         \"internalType\": \"uint256\",         \"name\": \"tokenId\",         \"type\": \"uint256\"       }     ],     \"name\": \"ERC721NonexistentToken\",     \"type\": \"error\"   },   {     \"anonymous\": false,     \"inputs\": [       {         \"indexed\": true,         \"internalType\": \"address\",         \"name\": \"owner\",         \"type\": \"address\"       },       {         \"indexed\": true,         \"internalType\": \"address\",         \"name\": \"approved\",         \"type\": \"address\"       },       {         \"indexed\": true,         \"internalType\": \"uint256\",         \"name\": \"tokenId\",         \"type\": \"uint256\"       }     ],     \"name\": \"Approval\",     \"type\": \"event\"   },   {     \"anonymous\": false,     \"inputs\": [       {         \"indexed\": true,         \"internalType\": \"address\",         \"name\": \"owner\",         \"type\": \"address\"       },       {         \"indexed\": true,         \"internalType\": \"address\",         \"name\": \"operator\",         \"type\": \"address\"       },       {         \"indexed\": false,         \"internalType\": \"bool\",         \"name\": \"approved\",         \"type\": \"bool\"       }     ],     \"name\": \"ApprovalForAll\",     \"type\": \"event\"   },   {     \"anonymous\": false,     \"inputs\": [       {         \"indexed\": false,         \"internalType\": \"uint256\",         \"name\": \"_fromTokenId\",         \"type\": \"uint256\"       },       {         \"indexed\": false,         \"internalType\": \"uint256\",         \"name\": \"_toTokenId\",         \"type\": \"uint256\"       }     ],     \"name\": \"BatchMetadataUpdate\",     \"type\": \"event\"   },   {     \"anonymous\": false,     \"inputs\": [       {         \"indexed\": false,         \"internalType\": \"uint256\",         \"name\": \"_tokenId\",         \"type\": \"uint256\"       }     ],     \"name\": \"MetadataUpdate\",     \"type\": \"event\"   },   {     \"anonymous\": false,     \"inputs\": [       {         \"indexed\": true,         \"internalType\": \"address\",         \"name\": \"from\",         \"type\": \"address\"       },       {         \"indexed\": true,         \"internalType\": \"address\",         \"name\": \"to\",         \"type\": \"address\"       },       {         \"indexed\": true,         \"internalType\": \"uint256\",         \"name\": \"tokenId\",         \"type\": \"uint256\"       }     ],     \"name\": \"Transfer\",     \"type\": \"event\"   },   {     \"inputs\": [       {         \"internalType\": \"address\",         \"name\": \"to\",         \"type\": \"address\"       },       {         \"internalType\": \"uint256\",         \"name\": \"tokenId\",         \"type\": \"uint256\"       }     ],     \"name\": \"approve\",     \"outputs\": [      ],     \"stateMutability\": \"nonpayable\",     \"type\": \"function\"   },   {     \"inputs\": [       {         \"internalType\": \"address\",         \"name\": \"owner\",         \"type\": \"address\"       }     ],     \"name\": \"balanceOf\",     \"outputs\": [       {         \"internalType\": \"uint256\",         \"name\": \"\",         \"type\": \"uint256\"       }     ],     \"stateMutability\": \"view\",     \"type\": \"function\"   },   {     \"inputs\": [       {         \"internalType\": \"uint256\",         \"name\": \"tokenId\",         \"type\": \"uint256\"       }     ],     \"name\": \"getApproved\",     \"outputs\": [       {         \"internalType\": \"address\",         \"name\": \"\",         \"type\": \"address\"       }     ],     \"stateMutability\": \"view\",     \"type\": \"function\"   },   {     \"inputs\": [       {         \"internalType\": \"address\",         \"name\": \"owner\",         \"type\": \"address\"       },       {         \"internalType\": \"address\",         \"name\": \"operator\",         \"type\": \"address\"       }     ],     \"name\": \"isApprovedForAll\",     \"outputs\": [       {         \"internalType\": \"bool\",         \"name\": \"\",         \"type\": \"bool\"       }     ],     \"stateMutability\": \"view\",     \"type\": \"function\"   },   {     \"inputs\": [      ],     \"name\": \"name\",     \"outputs\": [       {         \"internalType\": \"string\",         \"name\": \"\",         \"type\": \"string\"       }     ],     \"stateMutability\": \"view\",     \"type\": \"function\"   },   {     \"inputs\": [       {         \"internalType\": \"uint256\",         \"name\": \"tokenId\",         \"type\": \"uint256\"       }     ],     \"name\": \"ownerOf\",     \"outputs\": [       {         \"internalType\": \"address\",         \"name\": \"\",         \"type\": \"address\"       }     ],     \"stateMutability\": \"view\",     \"type\": \"function\"   },   {     \"inputs\": [       {         \"internalType\": \"address\",         \"name\": \"_to\",         \"type\": \"address\"       },       {         \"internalType\": \"string\",         \"name\": \"_uri\",         \"type\": \"string\"       }     ],     \"name\": \"safeMint\",     \"outputs\": [      ],     \"stateMutability\": \"nonpayable\",     \"type\": \"function\"   },   {     \"inputs\": [       {         \"internalType\": \"address\",         \"name\": \"from\",         \"type\": \"address\"       },       {         \"internalType\": \"address\",         \"name\": \"to\",         \"type\": \"address\"       },       {         \"internalType\": \"uint256\",         \"name\": \"tokenId\",         \"type\": \"uint256\"       }     ],     \"name\": \"safeTransferFrom\",     \"outputs\": [      ],     \"stateMutability\": \"nonpayable\",     \"type\": \"function\"   },   {     \"inputs\": [       {         \"internalType\": \"address\",         \"name\": \"from\",         \"type\": \"address\"       },       {         \"internalType\": \"address\",         \"name\": \"to\",         \"type\": \"address\"       },       {         \"internalType\": \"uint256\",         \"name\": \"tokenId\",         \"type\": \"uint256\"       },       {         \"internalType\": \"bytes\",         \"name\": \"data\",         \"type\": \"bytes\"       }     ],     \"name\": \"safeTransferFrom\",     \"outputs\": [      ],     \"stateMutability\": \"nonpayable\",     \"type\": \"function\"   },   {     \"inputs\": [       {         \"internalType\": \"address\",         \"name\": \"operator\",         \"type\": \"address\"       },       {         \"internalType\": \"bool\",         \"name\": \"approved\",         \"type\": \"bool\"       }     ],     \"name\": \"setApprovalForAll\",     \"outputs\": [      ],     \"stateMutability\": \"nonpayable\",     \"type\": \"function\"   },   {     \"inputs\": [       {         \"internalType\": \"bytes4\",         \"name\": \"interfaceId\",         \"type\": \"bytes4\"       }     ],     \"name\": \"supportsInterface\",     \"outputs\": [       {         \"internalType\": \"bool\",         \"name\": \"\",         \"type\": \"bool\"       }     ],     \"stateMutability\": \"view\",     \"type\": \"function\"   },   {     \"inputs\": [      ],     \"name\": \"symbol\",     \"outputs\": [       {         \"internalType\": \"string\",         \"name\": \"\",         \"type\": \"string\"       }     ],     \"stateMutability\": \"view\",     \"type\": \"function\"   },   {     \"inputs\": [       {         \"internalType\": \"uint256\",         \"name\": \"tokenId\",         \"type\": \"uint256\"       }     ],     \"name\": \"tokenURI\",     \"outputs\": [       {         \"internalType\": \"string\",         \"name\": \"\",         \"type\": \"string\"       }     ],     \"stateMutability\": \"view\",     \"type\": \"function\"   },   {     \"inputs\": [       {         \"internalType\": \"address\",         \"name\": \"from\",         \"type\": \"address\"       },       {         \"internalType\": \"address\",         \"name\": \"to\",         \"type\": \"address\"       },       {         \"internalType\": \"uint256\",         \"name\": \"tokenId\",         \"type\": \"uint256\"       }     ],     \"name\": \"transferFrom\",     \"outputs\": [      ],     \"stateMutability\": \"nonpayable\",     \"type\": \"function\"   } ]";
        
        public string ContractAddress { get; set; }
        
        public IEventManager EventManager { get; set; }

        public Contract OriginalContract { get; set; }
                
        public bool Subscribed { get; set; }

        
        #region Methods

        public async Task Approve(string to, BigInteger tokenId) 
        {
            var response = await OriginalContract.Send("approve", new object [] {
                to, tokenId
            });
            
            
        }
        public async Task<TransactionReceipt> ApproveWithReceipt(string to, BigInteger tokenId) 
        {
            var response = await OriginalContract.SendWithReceipt("approve", new object [] {
                to, tokenId
            });
            
            return response.receipt;
        }

        public async Task<BigInteger> BalanceOf(string owner) 
        {
            var response = await OriginalContract.Call<BigInteger>("balanceOf", new object [] {
                owner
            });
            
            return response;
        }


        public async Task<string> GetApproved(BigInteger tokenId) 
        {
            var response = await OriginalContract.Call<string>("getApproved", new object [] {
                tokenId
            });
            
            return response;
        }


        public async Task<bool> IsApprovedForAll(string owner, string @operator) 
        {
            var response = await OriginalContract.Call<bool>("isApprovedForAll", new object [] {
                owner, @operator
            });
            
            return response;
        }


        public async Task<string> Name() 
        {
            var response = await OriginalContract.Call<string>("name", new object [] {
                
            });
            
            return response;
        }


        public async Task<string> OwnerOf(BigInteger tokenId) 
        {
            var response = await OriginalContract.Call<string>("ownerOf", new object [] {
                tokenId
            });
            
            return response;
        }


        public async Task SafeMint(string _to, string _uri) 
        {
            var response = await OriginalContract.Send("safeMint", new object [] {
                _to, _uri
            });
            
            
        }
        public async Task<TransactionReceipt> SafeMintWithReceipt(string _to, string _uri) 
        {
            var response = await OriginalContract.SendWithReceipt("safeMint", new object [] {
                _to, _uri
            });
            
            return response.receipt;
        }

        public async Task SafeTransferFrom(string from, string to, BigInteger tokenId) 
        {
            var response = await OriginalContract.Send("safeTransferFrom", new object [] {
                from, to, tokenId
            });
            
            
        }
        public async Task<TransactionReceipt> SafeTransferFromWithReceipt(string from, string to, BigInteger tokenId) 
        {
            var response = await OriginalContract.SendWithReceipt("safeTransferFrom", new object [] {
                from, to, tokenId
            });
            
            return response.receipt;
        }

        public async Task SafeTransferFrom(string from, string to, BigInteger tokenId, byte[] data) 
        {
            var response = await OriginalContract.Send("safeTransferFrom", new object [] {
                from, to, tokenId, data
            });
            
            
        }
        public async Task<TransactionReceipt> SafeTransferFromWithReceipt(string from, string to, BigInteger tokenId, byte[] data) 
        {
            var response = await OriginalContract.SendWithReceipt("safeTransferFrom", new object [] {
                from, to, tokenId, data
            });
            
            return response.receipt;
        }

        public async Task SetApprovalForAll(string @operator, bool approved) 
        {
            var response = await OriginalContract.Send("setApprovalForAll", new object [] {
                @operator, approved
            });
            
            
        }
        public async Task<TransactionReceipt> SetApprovalForAllWithReceipt(string @operator, bool approved) 
        {
            var response = await OriginalContract.SendWithReceipt("setApprovalForAll", new object [] {
                @operator, approved
            });
            
            return response.receipt;
        }

        public async Task<bool> SupportsInterface(byte[] interfaceId) 
        {
            var response = await OriginalContract.Call<bool>("supportsInterface", new object [] {
                interfaceId
            });
            
            return response;
        }


        public async Task<string> Symbol() 
        {
            var response = await OriginalContract.Call<string>("symbol", new object [] {
                
            });
            
            return response;
        }


        public async Task<string> TokenURI(string tokenId) 
        {
            if (IpfsHelper.CanDecodeTokenIdToUri(tokenId))
            {
                return IpfsHelper.DecodeTokenIdToUri(tokenId);
            }
            var response = await OriginalContract.Call<string>("tokenURI", new object [] {
                tokenId
            });
            
            return response;
        }


        public async Task TransferFrom(string from, string to, BigInteger tokenId) 
        {
            var response = await OriginalContract.Send("transferFrom", new object [] {
                from, to, tokenId
            });
            
            
        }
        public async Task<TransactionReceipt> TransferFromWithReceipt(string from, string to, BigInteger tokenId) 
        {
            var response = await OriginalContract.SendWithReceipt("transferFrom", new object [] {
                from, to, tokenId
            });
            
            return response.receipt;
        }


        #endregion
        
        
        #region Event Classes

        public partial class ApprovalEventDTO : ApprovalEventDTOBase { }
        
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
    
        public event Action<ApprovalEventDTO> OnApproval;
        
        private void Approval(ApprovalEventDTO approval)
        {
            OnApproval?.Invoke(approval);
        }

        public partial class ApprovalForAllEventDTO : ApprovalForAllEventDTOBase { }
        
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
    
        public event Action<ApprovalForAllEventDTO> OnApprovalForAll;
        
        private void ApprovalForAll(ApprovalForAllEventDTO approvalForAll)
        {
            OnApprovalForAll?.Invoke(approvalForAll);
        }

        public partial class BatchMetadataUpdateEventDTO : BatchMetadataUpdateEventDTOBase { }
        
        [Event("BatchMetadataUpdate")]
        public class BatchMetadataUpdateEventDTOBase : IEventDTO
        {
                    [Parameter("uint256", "_fromTokenId", 0, false)]
        public virtual BigInteger FromTokenId { get; set; }
        [Parameter("uint256", "_toTokenId", 1, false)]
        public virtual BigInteger ToTokenId { get; set; }

        }
    
        public event Action<BatchMetadataUpdateEventDTO> OnBatchMetadataUpdate;
        
        private void BatchMetadataUpdate(BatchMetadataUpdateEventDTO batchMetadataUpdate)
        {
            OnBatchMetadataUpdate?.Invoke(batchMetadataUpdate);
        }

        public partial class MetadataUpdateEventDTO : MetadataUpdateEventDTOBase { }
        
        [Event("MetadataUpdate")]
        public class MetadataUpdateEventDTOBase : IEventDTO
        {
                    [Parameter("uint256", "_tokenId", 0, false)]
        public virtual BigInteger TokenId { get; set; }

        }
    
        public event Action<MetadataUpdateEventDTO> OnMetadataUpdate;
        
        private void MetadataUpdate(MetadataUpdateEventDTO metadataUpdate)
        {
            OnMetadataUpdate?.Invoke(metadataUpdate);
        }

        public partial class TransferEventDTO : TransferEventDTOBase { }
        
        [Event("Transfer")]
        public class TransferEventDTOBase : IEventDTO
        {
                    [Parameter("address", "from", 0, true)]
        public virtual string From { get; set; }
        [Parameter("address", "to", 1, true)]
        public virtual string To { get; set; }
        [Parameter("uint256", "tokenId", 2, true)]
        public virtual BigInteger TokenId { get; set; }

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
            
            if(!Subscribed)
                return;
                
           
            Subscribed = false;
            try
            {
                if(EventManager == null)
                    return;

			await EventManager.Unsubscribe<ApprovalEventDTO>(Approval, ContractAddress);
			OnApproval = null;
			await EventManager.Unsubscribe<ApprovalForAllEventDTO>(ApprovalForAll, ContractAddress);
			OnApprovalForAll = null;
			await EventManager.Unsubscribe<BatchMetadataUpdateEventDTO>(BatchMetadataUpdate, ContractAddress);
			OnBatchMetadataUpdate = null;
			await EventManager.Unsubscribe<MetadataUpdateEventDTO>(MetadataUpdate, ContractAddress);
			OnMetadataUpdate = null;
			await EventManager.Unsubscribe<TransferEventDTO>(Transfer, ContractAddress);
			OnTransfer = null;

            
            
            }catch(Exception e)
            {
                Debug.LogError("Caught an exception whilst unsubscribing from events\n" + e.Message);
            }
        }
        
        public async ValueTask InitAsync()
        {
            if(Subscribed)
                return;
            Subscribed = true;

            try
            {
                if(EventManager == null)
                    return;

                await EventManager.Subscribe<ApprovalEventDTO>(Approval, ContractAddress);
                await EventManager.Subscribe<ApprovalForAllEventDTO>(ApprovalForAll, ContractAddress);
                await EventManager.Subscribe<BatchMetadataUpdateEventDTO>(BatchMetadataUpdate, ContractAddress);
                await EventManager.Subscribe<MetadataUpdateEventDTO>(MetadataUpdate, ContractAddress);
                await EventManager.Subscribe<TransferEventDTO>(Transfer, ContractAddress);
    
            }catch(Exception e)
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
