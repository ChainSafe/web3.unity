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
    public class MarketplaceContract : ICustomContract
    {
        public string Address => OriginalContract.Address;
       
        public string ABI => "[     {         \"type\": \"constructor\",         \"stateMutability\": \"undefined\",         \"payable\": false,         \"inputs\": []     },     {         \"type\": \"error\",         \"name\": \"AlreadySameStatus\",         \"inputs\": []     },     {         \"type\": \"error\",         \"name\": \"CanNotModify\",         \"inputs\": []     },     {         \"type\": \"error\",         \"name\": \"DeadlineInvalid\",         \"inputs\": []     },     {         \"type\": \"error\",         \"name\": \"EtherTransferFailed\",         \"inputs\": []     },     {         \"type\": \"error\",         \"name\": \"FeeReceiverInvalid\",         \"inputs\": []     },     {         \"type\": \"error\",         \"name\": \"IncorrectAmountSupplied\",         \"inputs\": []     },     {         \"type\": \"error\",         \"name\": \"ItemExpired\",         \"inputs\": []     },     {         \"type\": \"error\",         \"name\": \"ItemIdInvalid\",         \"inputs\": []     },     {         \"type\": \"error\",         \"name\": \"MaxFeeInvalid\",         \"inputs\": []     },     {         \"type\": \"error\",         \"name\": \"NftTokenAlreadyAdded\",         \"inputs\": []     },     {         \"type\": \"error\",         \"name\": \"NftTokenInvalid\",         \"inputs\": []     },     {         \"type\": \"error\",         \"name\": \"NotEnoughBalance\",         \"inputs\": []     },     {         \"type\": \"error\",         \"name\": \"NotExpired\",         \"inputs\": []     },     {         \"type\": \"error\",         \"name\": \"OperatorInvalid\",         \"inputs\": []     },     {         \"type\": \"error\",         \"name\": \"TotalFeePercentInvalid\",         \"inputs\": []     },     {         \"type\": \"error\",         \"name\": \"Unauthorized\",         \"inputs\": []     },     {         \"type\": \"error\",         \"name\": \"WhitelistingDisabled\",         \"inputs\": []     },     {         \"type\": \"error\",         \"name\": \"ZeroAddress\",         \"inputs\": []     },     {         \"type\": \"error\",         \"name\": \"ZeroFeePercent\",         \"inputs\": []     },     {         \"type\": \"error\",         \"name\": \"ZeroPrice\",         \"inputs\": []     },     {         \"type\": \"event\",         \"anonymous\": false,         \"name\": \"ChainSafeFeeUpdated\",         \"inputs\": [             {                 \"type\": \"address\",                 \"name\": \"treasury\",                 \"indexed\": false             },             {                 \"type\": \"uint256\",                 \"name\": \"feePercent\",                 \"indexed\": false             }         ]     },     {         \"type\": \"event\",         \"anonymous\": false,         \"name\": \"FeeClaimed\",         \"inputs\": [             {                 \"type\": \"address\",                 \"name\": \"feeCollector\",                 \"indexed\": false             },             {                 \"type\": \"address\",                 \"name\": \"receiver\",                 \"indexed\": false             },             {                 \"type\": \"uint256\",                 \"name\": \"amount\",                 \"indexed\": false             }         ]     },     {         \"type\": \"event\",         \"anonymous\": false,         \"name\": \"FeeReceiverRemoved\",         \"inputs\": [             {                 \"type\": \"address\",                 \"name\": \"feeReceiver\",                 \"indexed\": false             }         ]     },     {         \"type\": \"event\",         \"anonymous\": false,         \"name\": \"FeeReceiverSet\",         \"inputs\": [             {                 \"type\": \"address\",                 \"name\": \"feeReceiver\",                 \"indexed\": false             },             {                 \"type\": \"uint256\",                 \"name\": \"feePercent\",                 \"indexed\": false             }         ]     },     {         \"type\": \"event\",         \"anonymous\": false,         \"name\": \"Initialized\",         \"inputs\": [             {                 \"type\": \"uint8\",                 \"name\": \"version\",                 \"indexed\": false             }         ]     },     {         \"type\": \"event\",         \"anonymous\": false,         \"name\": \"ItemCancelled\",         \"inputs\": [             {                 \"type\": \"uint256\",                 \"name\": \"itemId\",                 \"indexed\": false             },             {                 \"type\": \"address\",                 \"name\": \"owner\",                 \"indexed\": false             }         ]     },     {         \"type\": \"event\",         \"anonymous\": false,         \"name\": \"ItemListed\",         \"inputs\": [             {                 \"type\": \"address\",                 \"name\": \"nftContract\",                 \"indexed\": false             },             {                 \"type\": \"uint256\",                 \"name\": \"tokenId\",                 \"indexed\": false             },             {                 \"type\": \"uint256\",                 \"name\": \"itemId\",                 \"indexed\": false             },             {                 \"type\": \"address\",                 \"name\": \"seller\",                 \"indexed\": false             },             {                 \"type\": \"uint256\",                 \"name\": \"price\",                 \"indexed\": false             },             {                 \"type\": \"uint256\",                 \"name\": \"deadline\",                 \"indexed\": false             }         ]     },     {         \"type\": \"event\",         \"anonymous\": false,         \"name\": \"ItemSold\",         \"inputs\": [             {                 \"type\": \"uint256\",                 \"name\": \"itemId\",                 \"indexed\": false             },             {                 \"type\": \"address\",                 \"name\": \"buyer\",                 \"indexed\": false             }         ]     },     {         \"type\": \"event\",         \"anonymous\": false,         \"name\": \"MaxFeeUpdated\",         \"inputs\": [             {                 \"type\": \"uint256\",                 \"name\": \"feePercent\",                 \"indexed\": false             }         ]     },     {         \"type\": \"event\",         \"anonymous\": false,         \"name\": \"NftTokenAdded\",         \"inputs\": [             {                 \"type\": \"address\",                 \"name\": \"nftToken\",                 \"indexed\": false             }         ]     },     {         \"type\": \"event\",         \"anonymous\": false,         \"name\": \"NftTokenRemoved\",         \"inputs\": [             {                 \"type\": \"address\",                 \"name\": \"nftToken\",                 \"indexed\": false             }         ]     },     {         \"type\": \"event\",         \"anonymous\": false,         \"name\": \"RoleAdminChanged\",         \"inputs\": [             {                 \"type\": \"bytes32\",                 \"name\": \"role\",                 \"indexed\": true             },             {                 \"type\": \"bytes32\",                 \"name\": \"previousAdminRole\",                 \"indexed\": true             },             {                 \"type\": \"bytes32\",                 \"name\": \"newAdminRole\",                 \"indexed\": true             }         ]     },     {         \"type\": \"event\",         \"anonymous\": false,         \"name\": \"RoleGranted\",         \"inputs\": [             {                 \"type\": \"bytes32\",                 \"name\": \"role\",                 \"indexed\": true             },             {                 \"type\": \"address\",                 \"name\": \"account\",                 \"indexed\": true             },             {                 \"type\": \"address\",                 \"name\": \"sender\",                 \"indexed\": true             }         ]     },     {         \"type\": \"event\",         \"anonymous\": false,         \"name\": \"RoleRevoked\",         \"inputs\": [             {                 \"type\": \"bytes32\",                 \"name\": \"role\",                 \"indexed\": true             },             {                 \"type\": \"address\",                 \"name\": \"account\",                 \"indexed\": true             },             {                 \"type\": \"address\",                 \"name\": \"sender\",                 \"indexed\": true             }         ]     },     {         \"type\": \"event\",         \"anonymous\": false,         \"name\": \"WhitelistingStatusUpdated\",         \"inputs\": [             {                 \"type\": \"bool\",                 \"name\": \"isEnabled\",                 \"indexed\": false             }         ]     },     {         \"type\": \"function\",         \"name\": \"CREATOR_ROLE\",         \"constant\": true,         \"stateMutability\": \"view\",         \"payable\": false,         \"inputs\": [],         \"outputs\": [             {                 \"type\": \"bytes32\",                 \"name\": \"\"             }         ]     },     {         \"type\": \"function\",         \"name\": \"DEFAULT_ADMIN_ROLE\",         \"constant\": true,         \"stateMutability\": \"view\",         \"payable\": false,         \"inputs\": [],         \"outputs\": [             {                 \"type\": \"bytes32\",                 \"name\": \"\"             }         ]     },     {         \"type\": \"function\",         \"name\": \"UPDATER_ROLE\",         \"constant\": true,         \"stateMutability\": \"view\",         \"payable\": false,         \"inputs\": [],         \"outputs\": [             {                 \"type\": \"bytes32\",                 \"name\": \"\"             }         ]     },     {         \"type\": \"function\",         \"name\": \"_feeReceiverDetails\",         \"constant\": true,         \"stateMutability\": \"view\",         \"payable\": false,         \"inputs\": [             {                 \"type\": \"address\",                 \"name\": \"\"             }         ],         \"outputs\": [             {                 \"type\": \"uint256\",                 \"name\": \"feePercent\"             },             {                 \"type\": \"uint256\",                 \"name\": \"feeCollected\"             }         ]     },     {         \"type\": \"function\",         \"name\": \"activeItems\",         \"constant\": true,         \"stateMutability\": \"view\",         \"payable\": false,         \"inputs\": [],         \"outputs\": [             {                 \"type\": \"tuple[]\",                 \"name\": \"\",                 \"components\": [                     {                         \"type\": \"address\",                         \"name\": \"nftContract\"                     },                     {                         \"type\": \"uint256\",                         \"name\": \"tokenId\"                     },                     {                         \"type\": \"address\",                         \"name\": \"seller\"                     },                     {                         \"type\": \"uint256\",                         \"name\": \"price\"                     },                     {                         \"type\": \"uint256\",                         \"name\": \"deadline\"                     }                 ]             }         ]     },     {         \"type\": \"function\",         \"name\": \"addNftToken\",         \"constant\": false,         \"payable\": false,         \"inputs\": [             {                 \"type\": \"address\",                 \"name\": \"nftToken\"             }         ],         \"outputs\": []     },     {         \"type\": \"function\",         \"name\": \"cancelExpiredListings\",         \"constant\": false,         \"payable\": false,         \"inputs\": [             {                 \"type\": \"uint256[]\",                 \"name\": \"itemIds\"             }         ],         \"outputs\": []     },     {         \"type\": \"function\",         \"name\": \"cancelListing\",         \"constant\": false,         \"payable\": false,         \"inputs\": [             {                 \"type\": \"uint256\",                 \"name\": \"itemId\"             }         ],         \"outputs\": []     },     {         \"type\": \"function\",         \"name\": \"chainsafeTreasury\",         \"constant\": true,         \"stateMutability\": \"view\",         \"payable\": false,         \"inputs\": [],         \"outputs\": [             {                 \"type\": \"address\",                 \"name\": \"\"             }         ]     },     {         \"type\": \"function\",         \"name\": \"claimFee\",         \"constant\": false,         \"payable\": false,         \"inputs\": [             {                 \"type\": \"address\",                 \"name\": \"receiver\"             }         ],         \"outputs\": []     },     {         \"type\": \"function\",         \"name\": \"enableWhitelisting\",         \"constant\": false,         \"payable\": false,         \"inputs\": [             {                 \"type\": \"bool\",                 \"name\": \"isEnable\"             }         ],         \"outputs\": []     },     {         \"type\": \"function\",         \"name\": \"feeCollectedByReceiver\",         \"constant\": true,         \"stateMutability\": \"view\",         \"payable\": false,         \"inputs\": [             {                 \"type\": \"address\",                 \"name\": \"feeReceiver\"             }         ],         \"outputs\": [             {                 \"type\": \"uint256\",                 \"name\": \"\"             }         ]     },     {         \"type\": \"function\",         \"name\": \"feeReceiver\",         \"constant\": true,         \"stateMutability\": \"view\",         \"payable\": false,         \"inputs\": [             {                 \"type\": \"uint256\",                 \"name\": \"id\"             }         ],         \"outputs\": [             {                 \"type\": \"address\",                 \"name\": \"feeReceiver\"             },             {                 \"type\": \"uint256\",                 \"name\": \"feePercent\"             }         ]     },     {         \"type\": \"function\",         \"name\": \"feeReceiversNumber\",         \"constant\": true,         \"stateMutability\": \"view\",         \"payable\": false,         \"inputs\": [],         \"outputs\": [             {                 \"type\": \"uint256\",                 \"name\": \"\"             }         ]     },     {         \"type\": \"function\",         \"name\": \"getRoleAdmin\",         \"constant\": true,         \"stateMutability\": \"view\",         \"payable\": false,         \"inputs\": [             {                 \"type\": \"bytes32\",                 \"name\": \"role\"             }         ],         \"outputs\": [             {                 \"type\": \"bytes32\",                 \"name\": \"\"             }         ]     },     {         \"type\": \"function\",         \"name\": \"grantRole\",         \"constant\": false,         \"payable\": false,         \"inputs\": [             {                 \"type\": \"bytes32\",                 \"name\": \"role\"             },             {                 \"type\": \"address\",                 \"name\": \"account\"             }         ],         \"outputs\": []     },     {         \"type\": \"function\",         \"name\": \"hasRole\",         \"constant\": true,         \"stateMutability\": \"view\",         \"payable\": false,         \"inputs\": [             {                 \"type\": \"bytes32\",                 \"name\": \"role\"             },             {                 \"type\": \"address\",                 \"name\": \"account\"             }         ],         \"outputs\": [             {                 \"type\": \"bool\",                 \"name\": \"\"             }         ]     },     {         \"type\": \"function\",         \"name\": \"initialize\",         \"constant\": false,         \"payable\": false,         \"inputs\": [             {                 \"type\": \"string\",                 \"name\": \"projectID\"             },             {                 \"type\": \"string\",                 \"name\": \"marketplaceID\"             },             {                 \"type\": \"address\",                 \"name\": \"creator\"             },             {                 \"type\": \"address\",                 \"name\": \"updater\"             },             {                 \"type\": \"address\",                 \"name\": \"treasury\"             },             {                 \"type\": \"bool\",                 \"name\": \"isWhitelistingEnable\"             },             {                 \"type\": \"uint256\",                 \"name\": \"chainsafeFeePercent\"             },             {                 \"type\": \"uint256\",                 \"name\": \"maxPercent\"             }         ],         \"outputs\": []     },     {         \"type\": \"function\",         \"name\": \"isNftToken\",         \"constant\": true,         \"stateMutability\": \"view\",         \"payable\": false,         \"inputs\": [             {                 \"type\": \"address\",                 \"name\": \"token\"             }         ],         \"outputs\": [             {                 \"type\": \"bool\",                 \"name\": \"\"             }         ]     },     {         \"type\": \"function\",         \"name\": \"itemById\",         \"constant\": true,         \"stateMutability\": \"view\",         \"payable\": false,         \"inputs\": [             {                 \"type\": \"uint256\",                 \"name\": \"itemId\"             }         ],         \"outputs\": [             {                 \"type\": \"tuple\",                 \"name\": \"\",                 \"components\": [                     {                         \"type\": \"address\",                         \"name\": \"nftContract\"                     },                     {                         \"type\": \"uint256\",                         \"name\": \"tokenId\"                     },                     {                         \"type\": \"address\",                         \"name\": \"seller\"                     },                     {                         \"type\": \"uint256\",                         \"name\": \"price\"                     },                     {                         \"type\": \"uint256\",                         \"name\": \"deadline\"                     }                 ]             }         ]     },     {         \"type\": \"function\",         \"name\": \"listItem\",         \"constant\": false,         \"payable\": false,         \"inputs\": [             {                 \"type\": \"address\",                 \"name\": \"nftContract\"             },             {                 \"type\": \"uint256\",                 \"name\": \"tokenId\"             },             {                 \"type\": \"uint256\",                 \"name\": \"price\"             },             {                 \"type\": \"uint256\",                 \"name\": \"deadline\"             }         ],         \"outputs\": []     },     {         \"type\": \"function\",         \"name\": \"marketplaceID\",         \"constant\": true,         \"stateMutability\": \"view\",         \"payable\": false,         \"inputs\": [],         \"outputs\": [             {                 \"type\": \"string\",                 \"name\": \"\"             }         ]     },     {         \"type\": \"function\",         \"name\": \"maxFeePercent\",         \"constant\": true,         \"stateMutability\": \"view\",         \"payable\": false,         \"inputs\": [],         \"outputs\": [             {                 \"type\": \"uint256\",                 \"name\": \"\"             }         ]     },     {         \"type\": \"function\",         \"name\": \"nftToken\",         \"constant\": true,         \"stateMutability\": \"view\",         \"payable\": false,         \"inputs\": [             {                 \"type\": \"uint256\",                 \"name\": \"id\"             }         ],         \"outputs\": [             {                 \"type\": \"address\",                 \"name\": \"token\"             }         ]     },     {         \"type\": \"function\",         \"name\": \"onERC1155BatchReceived\",         \"constant\": true,         \"stateMutability\": \"view\",         \"payable\": false,         \"inputs\": [             {                 \"type\": \"address\",                 \"name\": \"operator\"             },             {                 \"type\": \"address\",                 \"name\": \"from\"             },             {                 \"type\": \"uint256[]\",                 \"name\": \"ids\"             },             {                 \"type\": \"uint256[]\",                 \"name\": \"values\"             },             {                 \"type\": \"bytes\",                 \"name\": \"data\"             }         ],         \"outputs\": [             {                 \"type\": \"bytes4\",                 \"name\": \"\"             }         ]     },     {         \"type\": \"function\",         \"name\": \"onERC1155Received\",         \"constant\": true,         \"stateMutability\": \"view\",         \"payable\": false,         \"inputs\": [             {                 \"type\": \"address\",                 \"name\": \"operator\"             },             {                 \"type\": \"address\",                 \"name\": \"from\"             },             {                 \"type\": \"uint256\",                 \"name\": \"id\"             },             {                 \"type\": \"uint256\",                 \"name\": \"value\"             },             {                 \"type\": \"bytes\",                 \"name\": \"data\"             }         ],         \"outputs\": [             {                 \"type\": \"bytes4\",                 \"name\": \"\"             }         ]     },     {         \"type\": \"function\",         \"name\": \"onERC721Received\",         \"constant\": true,         \"stateMutability\": \"view\",         \"payable\": false,         \"inputs\": [             {                 \"type\": \"address\",                 \"name\": \"operator\"             },             {                 \"type\": \"address\",                 \"name\": \"from\"             },             {                 \"type\": \"uint256\",                 \"name\": \"id\"             },             {                 \"type\": \"bytes\",                 \"name\": \"data\"             }         ],         \"outputs\": [             {                 \"type\": \"bytes4\",                 \"name\": \"\"             }         ]     },     {         \"type\": \"function\",         \"name\": \"projectID\",         \"constant\": true,         \"stateMutability\": \"view\",         \"payable\": false,         \"inputs\": [],         \"outputs\": [             {                 \"type\": \"string\",                 \"name\": \"\"             }         ]     },     {         \"type\": \"function\",         \"name\": \"purchaseItem\",         \"constant\": false,         \"stateMutability\": \"payable\",         \"payable\": true,         \"inputs\": [             {                 \"type\": \"uint256\",                 \"name\": \"itemId\"             }         ],         \"outputs\": []     },     {         \"type\": \"function\",         \"name\": \"removeFeeReceiver\",         \"constant\": false,         \"payable\": false,         \"inputs\": [             {                 \"type\": \"address\",                 \"name\": \"feeReceiver\"             }         ],         \"outputs\": []     },     {         \"type\": \"function\",         \"name\": \"removeNftToken\",         \"constant\": false,         \"payable\": false,         \"inputs\": [             {                 \"type\": \"address\",                 \"name\": \"nftToken\"             }         ],         \"outputs\": []     },     {         \"type\": \"function\",         \"name\": \"renounceRole\",         \"constant\": false,         \"payable\": false,         \"inputs\": [             {                 \"type\": \"bytes32\",                 \"name\": \"role\"             },             {                 \"type\": \"address\",                 \"name\": \"account\"             }         ],         \"outputs\": []     },     {         \"type\": \"function\",         \"name\": \"revokeRole\",         \"constant\": false,         \"payable\": false,         \"inputs\": [             {                 \"type\": \"bytes32\",                 \"name\": \"role\"             },             {                 \"type\": \"address\",                 \"name\": \"account\"             }         ],         \"outputs\": []     },     {         \"type\": \"function\",         \"name\": \"setFeeReceiver\",         \"constant\": false,         \"payable\": false,         \"inputs\": [             {                 \"type\": \"address\",                 \"name\": \"feeReceiver\"             },             {                 \"type\": \"uint256\",                 \"name\": \"feePercent\"             }         ],         \"outputs\": []     },     {         \"type\": \"function\",         \"name\": \"setMaxFeePercent\",         \"constant\": false,         \"payable\": false,         \"inputs\": [             {                 \"type\": \"uint256\",                 \"name\": \"feePercent\"             }         ],         \"outputs\": []     },     {         \"type\": \"function\",         \"name\": \"supportsInterface\",         \"constant\": true,         \"stateMutability\": \"view\",         \"payable\": false,         \"inputs\": [             {                 \"type\": \"bytes4\",                 \"name\": \"interfaceId\"             }         ],         \"outputs\": [             {                 \"type\": \"bool\",                 \"name\": \"\"             }         ]     },     {         \"type\": \"function\",         \"name\": \"totalFeePercent\",         \"constant\": true,         \"stateMutability\": \"view\",         \"payable\": false,         \"inputs\": [],         \"outputs\": [             {                 \"type\": \"uint256\",                 \"name\": \"feePercent\"             }         ]     },     {         \"type\": \"function\",         \"name\": \"totalListings\",         \"constant\": true,         \"stateMutability\": \"view\",         \"payable\": false,         \"inputs\": [],         \"outputs\": [             {                 \"type\": \"uint256\",                 \"name\": \"\"             }         ]     },     {         \"type\": \"function\",         \"name\": \"updateChainSafeTreasury\",         \"constant\": false,         \"payable\": false,         \"inputs\": [             {                 \"type\": \"address\",                 \"name\": \"treasury\"             },             {                 \"type\": \"uint256\",                 \"name\": \"feePercent\"             }         ],         \"outputs\": []     },     {         \"type\": \"function\",         \"name\": \"usersListingIds\",         \"constant\": true,         \"stateMutability\": \"view\",         \"payable\": false,         \"inputs\": [             {                 \"type\": \"address\",                 \"name\": \"user\"             }         ],         \"outputs\": [             {                 \"type\": \"uint256[]\",                 \"name\": \"\"             }         ]     },     {         \"type\": \"function\",         \"name\": \"usersListings\",         \"constant\": true,         \"stateMutability\": \"view\",         \"payable\": false,         \"inputs\": [             {                 \"type\": \"address\",                 \"name\": \"user\"             }         ],         \"outputs\": [             {                 \"type\": \"tuple[]\",                 \"name\": \"\",                 \"components\": [                     {                         \"type\": \"address\",                         \"name\": \"nftContract\"                     },                     {                         \"type\": \"uint256\",                         \"name\": \"tokenId\"                     },                     {                         \"type\": \"address\",                         \"name\": \"seller\"                     },                     {                         \"type\": \"uint256\",                         \"name\": \"price\"                     },                     {                         \"type\": \"uint256\",                         \"name\": \"deadline\"                     }                 ]             }         ]     },     {         \"type\": \"function\",         \"name\": \"whitelistingEnable\",         \"constant\": true,         \"stateMutability\": \"view\",         \"payable\": false,         \"inputs\": [],         \"outputs\": [             {                 \"type\": \"bool\",                 \"name\": \"\"             }         ]     },     {         \"type\": \"receive\",         \"stateMutability\": \"payable\"     } ]";
        
        public string ContractAddress { get; set; }
        
        public IContractBuilder ContractBuilder { get; set; }

        public Contract OriginalContract { get; set; }
        
        public string WebSocketUrl { get; set; }
        
        public bool Subscribed { get; set; }

        private StreamingWebSocketClient _webSocketClient;
        
        #region Methods

        public async Task<object> CREATOR_ROLE() 
        {
            var response = await OriginalContract.Call<object>("CREATOR_ROLE", new object [] {
                
            });
            
            return response;
        }


        public async Task<object> DEFAULT_ADMIN_ROLE() 
        {
            var response = await OriginalContract.Call<object>("DEFAULT_ADMIN_ROLE", new object [] {
                
            });
            
            return response;
        }


        public async Task<object> UPDATER_ROLE() 
        {
            var response = await OriginalContract.Call<object>("UPDATER_ROLE", new object [] {
                
            });
            
            return response;
        }


        public async Task<(BigInteger feePercent, BigInteger feeCollected)> FeeReceiverDetails(string address) 
        {
            var response = await OriginalContract.Call("_feeReceiverDetails", new object [] {
                
            });
            
            return ((BigInteger)response[0], (BigInteger)response[1]);
        }


        public async Task<Tuple[]> ActiveItems() 
        {
            var response = await OriginalContract.Call<Tuple[]>("activeItems", new object [] {
                
            });
            
            return response;
        }


        public async Task AddNftToken(string nftToken) 
        {
            var response = await OriginalContract.Send("addNftToken", new object [] {
                nftToken
            });
            
            
        }
        public async Task<TransactionReceipt> AddNftTokenWithReceipt(string nftToken) 
        {
            var response = await OriginalContract.SendWithReceipt("addNftToken", new object [] {
                nftToken
            });
            
            return response.receipt;
        }

        public async Task CancelExpiredListings(BigInteger[] itemIds) 
        {
            var response = await OriginalContract.Send("cancelExpiredListings", new object [] {
                itemIds
            });
            
            
        }
        public async Task<TransactionReceipt> CancelExpiredListingsWithReceipt(BigInteger[] itemIds) 
        {
            var response = await OriginalContract.SendWithReceipt("cancelExpiredListings", new object [] {
                itemIds
            });
            
            return response.receipt;
        }

        public async Task CancelListing(BigInteger itemId) 
        {
            var response = await OriginalContract.Send("cancelListing", new object [] {
                itemId
            });
            
            
        }
        public async Task<TransactionReceipt> CancelListingWithReceipt(BigInteger itemId) 
        {
            var response = await OriginalContract.SendWithReceipt("cancelListing", new object [] {
                itemId
            });
            
            return response.receipt;
        }

        public async Task<string> ChainsafeTreasury() 
        {
            var response = await OriginalContract.Call<string>("chainsafeTreasury", new object [] {
                
            });
            
            return response;
        }


        public async Task ClaimFee(string receiver) 
        {
            var response = await OriginalContract.Send("claimFee", new object [] {
                receiver
            });
            
            
        }
        public async Task<TransactionReceipt> ClaimFeeWithReceipt(string receiver) 
        {
            var response = await OriginalContract.SendWithReceipt("claimFee", new object [] {
                receiver
            });
            
            return response.receipt;
        }

        public async Task EnableWhitelisting(bool isEnable) 
        {
            var response = await OriginalContract.Send("enableWhitelisting", new object [] {
                isEnable
            });
            
            
        }
        public async Task<TransactionReceipt> EnableWhitelistingWithReceipt(bool isEnable) 
        {
            var response = await OriginalContract.SendWithReceipt("enableWhitelisting", new object [] {
                isEnable
            });
            
            return response.receipt;
        }

        public async Task<BigInteger> FeeCollectedByReceiver(string feeReceiver) 
        {
            var response = await OriginalContract.Call<BigInteger>("feeCollectedByReceiver", new object [] {
                feeReceiver
            });
            
            return response;
        }


        public async Task<(string feeReceiver, BigInteger feePercent)> FeeReceiver(BigInteger id) 
        {
            var response = await OriginalContract.Call("feeReceiver", new object [] {
                id
            });
            
            return ((string)response[0], (BigInteger)response[1]);
        }


        public async Task<BigInteger> FeeReceiversNumber() 
        {
            var response = await OriginalContract.Call<BigInteger>("feeReceiversNumber", new object [] {
                
            });
            
            return response;
        }


        public async Task<object> GetRoleAdmin(object role) 
        {
            var response = await OriginalContract.Call<object>("getRoleAdmin", new object [] {
                role
            });
            
            return response;
        }


        public async Task GrantRole(object role, string account) 
        {
            var response = await OriginalContract.Send("grantRole", new object [] {
                role, account
            });
            
            
        }
        public async Task<TransactionReceipt> GrantRoleWithReceipt(object role, string account) 
        {
            var response = await OriginalContract.SendWithReceipt("grantRole", new object [] {
                role, account
            });
            
            return response.receipt;
        }

        public async Task<bool> HasRole(object role, string account) 
        {
            var response = await OriginalContract.Call<bool>("hasRole", new object [] {
                role, account
            });
            
            return response;
        }


        public async Task Initialize(string projectID, string marketplaceID, string creator, string updater, string treasury, bool isWhitelistingEnable, BigInteger chainsafeFeePercent, BigInteger maxPercent) 
        {
            var response = await OriginalContract.Send("initialize", new object [] {
                projectID, marketplaceID, creator, updater, treasury, isWhitelistingEnable, chainsafeFeePercent, maxPercent
            });
            
            
        }
        public async Task<TransactionReceipt> InitializeWithReceipt(string projectID, string marketplaceID, string creator, string updater, string treasury, bool isWhitelistingEnable, BigInteger chainsafeFeePercent, BigInteger maxPercent) 
        {
            var response = await OriginalContract.SendWithReceipt("initialize", new object [] {
                projectID, marketplaceID, creator, updater, treasury, isWhitelistingEnable, chainsafeFeePercent, maxPercent
            });
            
            return response.receipt;
        }

        public async Task<bool> IsNftToken(string token) 
        {
            var response = await OriginalContract.Call<bool>("isNftToken", new object [] {
                token
            });
            
            return response;
        }


        public async Task<Tuple> ItemById(BigInteger itemId) 
        {
            var response = await OriginalContract.Call<Tuple>("itemById", new object [] {
                itemId
            });
            
            return response;
        }


        public async Task ListItem(string nftContract, BigInteger tokenId, BigInteger price, BigInteger deadline) 
        {
            var response = await OriginalContract.Send("listItem", new object [] {
                nftContract, tokenId, price, deadline
            });
            
            
        }
        public async Task<TransactionReceipt> ListItemWithReceipt(string nftContract, BigInteger tokenId, BigInteger price, BigInteger deadline) 
        {
            var response = await OriginalContract.SendWithReceipt("listItem", new object [] {
                nftContract, tokenId, price, deadline
            });
            
            return response.receipt;
        }

        public async Task<string> MarketplaceID() 
        {
            var response = await OriginalContract.Call<string>("marketplaceID", new object [] {
                
            });
            
            return response;
        }


        public async Task<BigInteger> MaxFeePercent() 
        {
            var response = await OriginalContract.Call<BigInteger>("maxFeePercent", new object [] {
                
            });
            
            return response;
        }


        public async Task<string> NftToken(BigInteger id) 
        {
            var response = await OriginalContract.Call<string>("nftToken", new object [] {
                id
            });
            
            return response;
        }


        public async Task<object> OnERC1155BatchReceived(string @operator, string from, BigInteger[] ids, BigInteger[] values, byte[] data) 
        {
            var response = await OriginalContract.Call<object>("onERC1155BatchReceived", new object [] {
                @operator, from, ids, values, data
            });
            
            return response;
        }


        public async Task<object> OnERC1155Received(string @operator, string from, BigInteger id, BigInteger value, byte[] data) 
        {
            var response = await OriginalContract.Call<object>("onERC1155Received", new object [] {
                @operator, from, id, value, data
            });
            
            return response;
        }


        public async Task<object> OnERC721Received(string @operator, string from, BigInteger id, byte[] data) 
        {
            var response = await OriginalContract.Call<object>("onERC721Received", new object [] {
                @operator, from, id, data
            });
            
            return response;
        }


        public async Task<string> ProjectID() 
        {
            var response = await OriginalContract.Call<string>("projectID", new object [] {
                
            });
            
            return response;
        }


        public async Task PurchaseItem(BigInteger itemId) 
        {
            var response = await OriginalContract.Send("purchaseItem", new object [] {
                itemId
            });
            
            
        }
        public async Task<TransactionReceipt> PurchaseItemWithReceipt(BigInteger itemId) 
        {
            var response = await OriginalContract.SendWithReceipt("purchaseItem", new object [] {
                itemId
            });
            
            return response.receipt;
        }

        public async Task RemoveFeeReceiver(string feeReceiver) 
        {
            var response = await OriginalContract.Send("removeFeeReceiver", new object [] {
                feeReceiver
            });
            
            
        }
        public async Task<TransactionReceipt> RemoveFeeReceiverWithReceipt(string feeReceiver) 
        {
            var response = await OriginalContract.SendWithReceipt("removeFeeReceiver", new object [] {
                feeReceiver
            });
            
            return response.receipt;
        }

        public async Task RemoveNftToken(string nftToken) 
        {
            var response = await OriginalContract.Send("removeNftToken", new object [] {
                nftToken
            });
            
            
        }
        public async Task<TransactionReceipt> RemoveNftTokenWithReceipt(string nftToken) 
        {
            var response = await OriginalContract.SendWithReceipt("removeNftToken", new object [] {
                nftToken
            });
            
            return response.receipt;
        }

        public async Task RenounceRole(object role, string account) 
        {
            var response = await OriginalContract.Send("renounceRole", new object [] {
                role, account
            });
            
            
        }
        public async Task<TransactionReceipt> RenounceRoleWithReceipt(object role, string account) 
        {
            var response = await OriginalContract.SendWithReceipt("renounceRole", new object [] {
                role, account
            });
            
            return response.receipt;
        }

        public async Task RevokeRole(object role, string account) 
        {
            var response = await OriginalContract.Send("revokeRole", new object [] {
                role, account
            });
            
            
        }
        public async Task<TransactionReceipt> RevokeRoleWithReceipt(object role, string account) 
        {
            var response = await OriginalContract.SendWithReceipt("revokeRole", new object [] {
                role, account
            });
            
            return response.receipt;
        }

        public async Task SetFeeReceiver(string feeReceiver, BigInteger feePercent) 
        {
            var response = await OriginalContract.Send("setFeeReceiver", new object [] {
                feeReceiver, feePercent
            });
            
            
        }
        public async Task<TransactionReceipt> SetFeeReceiverWithReceipt(string feeReceiver, BigInteger feePercent) 
        {
            var response = await OriginalContract.SendWithReceipt("setFeeReceiver", new object [] {
                feeReceiver, feePercent
            });
            
            return response.receipt;
        }

        public async Task SetMaxFeePercent(BigInteger feePercent) 
        {
            var response = await OriginalContract.Send("setMaxFeePercent", new object [] {
                feePercent
            });
            
            
        }
        public async Task<TransactionReceipt> SetMaxFeePercentWithReceipt(BigInteger feePercent) 
        {
            var response = await OriginalContract.SendWithReceipt("setMaxFeePercent", new object [] {
                feePercent
            });
            
            return response.receipt;
        }

        public async Task<bool> SupportsInterface(object interfaceId) 
        {
            var response = await OriginalContract.Call<bool>("supportsInterface", new object [] {
                interfaceId
            });
            
            return response;
        }


        public async Task<BigInteger> TotalFeePercent() 
        {
            var response = await OriginalContract.Call<BigInteger>("totalFeePercent", new object [] {
                
            });
            
            return response;
        }


        public async Task<BigInteger> TotalListings() 
        {
            var response = await OriginalContract.Call<BigInteger>("totalListings", new object [] {
                
            });
            
            return response;
        }


        public async Task UpdateChainSafeTreasury(string treasury, BigInteger feePercent) 
        {
            var response = await OriginalContract.Send("updateChainSafeTreasury", new object [] {
                treasury, feePercent
            });
            
            
        }
        public async Task<TransactionReceipt> UpdateChainSafeTreasuryWithReceipt(string treasury, BigInteger feePercent) 
        {
            var response = await OriginalContract.SendWithReceipt("updateChainSafeTreasury", new object [] {
                treasury, feePercent
            });
            
            return response.receipt;
        }

        public async Task<BigInteger[]> UsersListingIds(string user) 
        {
            var response = await OriginalContract.Call<BigInteger[]>("usersListingIds", new object [] {
                user
            });
            
            return response;
        }


        public async Task<Tuple[]> UsersListings(string user) 
        {
            var response = await OriginalContract.Call<Tuple[]>("usersListings", new object [] {
                user
            });
            
            return response;
        }


        public async Task<bool> WhitelistingEnable() 
        {
            var response = await OriginalContract.Call<bool>("whitelistingEnable", new object [] {
                
            });
            
            return response;
        }



        #endregion
        
        
        #region Event Classes

    public partial class ChainSafeFeeUpdatedEventDTO : ChainSafeFeeUpdatedEventDTOBase { }
    
    [Event("ChainSafeFeeUpdated")]
    public class ChainSafeFeeUpdatedEventDTOBase : IEventDTO
    {
                [Parameter("address", "treasury", 0, false)]
        public virtual string Treasury { get; set; }
        [Parameter("uint256", "feePercent", 1, false)]
        public virtual BigInteger FeePercent { get; set; }

    }
    
    EthLogsObservableSubscription eventChainSafeFeeUpdated;
    public event Action<ChainSafeFeeUpdatedEventDTO> OnChainSafeFeeUpdated;

    public partial class FeeClaimedEventDTO : FeeClaimedEventDTOBase { }
    
    [Event("FeeClaimed")]
    public class FeeClaimedEventDTOBase : IEventDTO
    {
                [Parameter("address", "feeCollector", 0, false)]
        public virtual string FeeCollector { get; set; }
        [Parameter("address", "receiver", 1, false)]
        public virtual string Receiver { get; set; }
        [Parameter("uint256", "amount", 2, false)]
        public virtual BigInteger Amount { get; set; }

    }
    
    EthLogsObservableSubscription eventFeeClaimed;
    public event Action<FeeClaimedEventDTO> OnFeeClaimed;

    public partial class FeeReceiverRemovedEventDTO : FeeReceiverRemovedEventDTOBase { }
    
    [Event("FeeReceiverRemoved")]
    public class FeeReceiverRemovedEventDTOBase : IEventDTO
    {
                [Parameter("address", "feeReceiver", 0, false)]
        public virtual string FeeReceiver { get; set; }

    }
    
    EthLogsObservableSubscription eventFeeReceiverRemoved;
    public event Action<FeeReceiverRemovedEventDTO> OnFeeReceiverRemoved;

    public partial class FeeReceiverSetEventDTO : FeeReceiverSetEventDTOBase { }
    
    [Event("FeeReceiverSet")]
    public class FeeReceiverSetEventDTOBase : IEventDTO
    {
                [Parameter("address", "feeReceiver", 0, false)]
        public virtual string FeeReceiver { get; set; }
        [Parameter("uint256", "feePercent", 1, false)]
        public virtual BigInteger FeePercent { get; set; }

    }
    
    EthLogsObservableSubscription eventFeeReceiverSet;
    public event Action<FeeReceiverSetEventDTO> OnFeeReceiverSet;

    public partial class InitializedEventDTO : InitializedEventDTOBase { }
    
    [Event("Initialized")]
    public class InitializedEventDTOBase : IEventDTO
    {
                [Parameter("uint8", "version", 0, false)]
        public virtual byte Version { get; set; }

    }
    
    EthLogsObservableSubscription eventInitialized;
    public event Action<InitializedEventDTO> OnInitialized;

    public partial class ItemCancelledEventDTO : ItemCancelledEventDTOBase { }
    
    [Event("ItemCancelled")]
    public class ItemCancelledEventDTOBase : IEventDTO
    {
                [Parameter("uint256", "itemId", 0, false)]
        public virtual BigInteger ItemId { get; set; }
        [Parameter("address", "owner", 1, false)]
        public virtual string Owner { get; set; }

    }
    
    EthLogsObservableSubscription eventItemCancelled;
    public event Action<ItemCancelledEventDTO> OnItemCancelled;

    public partial class ItemListedEventDTO : ItemListedEventDTOBase { }
    
    [Event("ItemListed")]
    public class ItemListedEventDTOBase : IEventDTO
    {
                [Parameter("address", "nftContract", 0, false)]
        public virtual string NftContract { get; set; }
        [Parameter("uint256", "tokenId", 1, false)]
        public virtual BigInteger TokenId { get; set; }
        [Parameter("uint256", "itemId", 2, false)]
        public virtual BigInteger ItemId { get; set; }
        [Parameter("address", "seller", 3, false)]
        public virtual string Seller { get; set; }
        [Parameter("uint256", "price", 4, false)]
        public virtual BigInteger Price { get; set; }
        [Parameter("uint256", "deadline", 5, false)]
        public virtual BigInteger Deadline { get; set; }

    }
    
    EthLogsObservableSubscription eventItemListed;
    public event Action<ItemListedEventDTO> OnItemListed;

    public partial class ItemSoldEventDTO : ItemSoldEventDTOBase { }
    
    [Event("ItemSold")]
    public class ItemSoldEventDTOBase : IEventDTO
    {
                [Parameter("uint256", "itemId", 0, false)]
        public virtual BigInteger ItemId { get; set; }
        [Parameter("address", "buyer", 1, false)]
        public virtual string Buyer { get; set; }

    }
    
    EthLogsObservableSubscription eventItemSold;
    public event Action<ItemSoldEventDTO> OnItemSold;

    public partial class MaxFeeUpdatedEventDTO : MaxFeeUpdatedEventDTOBase { }
    
    [Event("MaxFeeUpdated")]
    public class MaxFeeUpdatedEventDTOBase : IEventDTO
    {
                [Parameter("uint256", "feePercent", 0, false)]
        public virtual BigInteger FeePercent { get; set; }

    }
    
    EthLogsObservableSubscription eventMaxFeeUpdated;
    public event Action<MaxFeeUpdatedEventDTO> OnMaxFeeUpdated;

    public partial class NftTokenAddedEventDTO : NftTokenAddedEventDTOBase { }
    
    [Event("NftTokenAdded")]
    public class NftTokenAddedEventDTOBase : IEventDTO
    {
                [Parameter("address", "nftToken", 0, false)]
        public virtual string NftToken { get; set; }

    }
    
    EthLogsObservableSubscription eventNftTokenAdded;
    public event Action<NftTokenAddedEventDTO> OnNftTokenAdded;

    public partial class NftTokenRemovedEventDTO : NftTokenRemovedEventDTOBase { }
    
    [Event("NftTokenRemoved")]
    public class NftTokenRemovedEventDTOBase : IEventDTO
    {
                [Parameter("address", "nftToken", 0, false)]
        public virtual string NftToken { get; set; }

    }
    
    EthLogsObservableSubscription eventNftTokenRemoved;
    public event Action<NftTokenRemovedEventDTO> OnNftTokenRemoved;

    public partial class RoleAdminChangedEventDTO : RoleAdminChangedEventDTOBase { }
    
    [Event("RoleAdminChanged")]
    public class RoleAdminChangedEventDTOBase : IEventDTO
    {
                [Parameter("bytes32", "role", 0, true)]
        public virtual object Role { get; set; }
        [Parameter("bytes32", "previousAdminRole", 1, true)]
        public virtual object PreviousAdminRole { get; set; }
        [Parameter("bytes32", "newAdminRole", 2, true)]
        public virtual object NewAdminRole { get; set; }

    }
    
    EthLogsObservableSubscription eventRoleAdminChanged;
    public event Action<RoleAdminChangedEventDTO> OnRoleAdminChanged;

    public partial class RoleGrantedEventDTO : RoleGrantedEventDTOBase { }
    
    [Event("RoleGranted")]
    public class RoleGrantedEventDTOBase : IEventDTO
    {
                [Parameter("bytes32", "role", 0, true)]
        public virtual object Role { get; set; }
        [Parameter("address", "account", 1, true)]
        public virtual string Account { get; set; }
        [Parameter("address", "sender", 2, true)]
        public virtual string Sender { get; set; }

    }
    
    EthLogsObservableSubscription eventRoleGranted;
    public event Action<RoleGrantedEventDTO> OnRoleGranted;

    public partial class RoleRevokedEventDTO : RoleRevokedEventDTOBase { }
    
    [Event("RoleRevoked")]
    public class RoleRevokedEventDTOBase : IEventDTO
    {
                [Parameter("bytes32", "role", 0, true)]
        public virtual object Role { get; set; }
        [Parameter("address", "account", 1, true)]
        public virtual string Account { get; set; }
        [Parameter("address", "sender", 2, true)]
        public virtual string Sender { get; set; }

    }
    
    EthLogsObservableSubscription eventRoleRevoked;
    public event Action<RoleRevokedEventDTO> OnRoleRevoked;

    public partial class WhitelistingStatusUpdatedEventDTO : WhitelistingStatusUpdatedEventDTOBase { }
    
    [Event("WhitelistingStatusUpdated")]
    public class WhitelistingStatusUpdatedEventDTOBase : IEventDTO
    {
                [Parameter("bool", "isEnabled", 0, false)]
        public virtual bool IsEnabled { get; set; }

    }
    
    EthLogsObservableSubscription eventWhitelistingStatusUpdated;
    public event Action<WhitelistingStatusUpdatedEventDTO> OnWhitelistingStatusUpdated;


        #endregion
        
        #region Interface Implemented Methods
        
        public async ValueTask DisposeAsync()
        {
            if(string.IsNullOrEmpty(WebSocketUrl))
                return;
            if(!Subscribed)
                return;
            Subscribed = false;

			await eventChainSafeFeeUpdated.UnsubscribeAsync();
			OnChainSafeFeeUpdated = null;
			await eventFeeClaimed.UnsubscribeAsync();
			OnFeeClaimed = null;
			await eventFeeReceiverRemoved.UnsubscribeAsync();
			OnFeeReceiverRemoved = null;
			await eventFeeReceiverSet.UnsubscribeAsync();
			OnFeeReceiverSet = null;
			await eventInitialized.UnsubscribeAsync();
			OnInitialized = null;
			await eventItemCancelled.UnsubscribeAsync();
			OnItemCancelled = null;
			await eventItemListed.UnsubscribeAsync();
			OnItemListed = null;
			await eventItemSold.UnsubscribeAsync();
			OnItemSold = null;
			await eventMaxFeeUpdated.UnsubscribeAsync();
			OnMaxFeeUpdated = null;
			await eventNftTokenAdded.UnsubscribeAsync();
			OnNftTokenAdded = null;
			await eventNftTokenRemoved.UnsubscribeAsync();
			OnNftTokenRemoved = null;
			await eventRoleAdminChanged.UnsubscribeAsync();
			OnRoleAdminChanged = null;
			await eventRoleGranted.UnsubscribeAsync();
			OnRoleGranted = null;
			await eventRoleRevoked.UnsubscribeAsync();
			OnRoleRevoked = null;
			await eventWhitelistingStatusUpdated.UnsubscribeAsync();
			OnWhitelistingStatusUpdated = null;

            _webSocketClient?.Dispose();
        }
        
        public async ValueTask Init()
        {
            if(Subscribed)
                return;
                
            if(string.IsNullOrEmpty(WebSocketUrl))
            {
                Debug.LogWarning($"WebSocketUrl is not set for this class. Event Subscriptions will not work.");
                return;
            }
           
            _webSocketClient ??= new StreamingWebSocketClient(WebSocketUrl);
            await _webSocketClient.StartAsync();
            Subscribed = true;

			var filterChainSafeFeeUpdatedEvent = Event<ChainSafeFeeUpdatedEventDTO>.GetEventABI().CreateFilterInput(ContractAddress);
            eventChainSafeFeeUpdated = new EthLogsObservableSubscription(_webSocketClient);
            
            eventChainSafeFeeUpdated.GetSubscriptionDataResponsesAsObservable().Subscribe(log =>
            {
                try
                {
                    var decoded = Event<ChainSafeFeeUpdatedEventDTO>.DecodeEvent(log);
                    if (decoded != null)
                    {
                       OnChainSafeFeeUpdated?.Invoke(decoded.Event);
                    }  
                }
                catch (Exception ex)
                {
                    Debug.LogError("Log Address: " + log.Address + " is not a standard transfer log:" + ex.Message);
                }
            });
            
            await eventChainSafeFeeUpdated.SubscribeAsync(filterChainSafeFeeUpdatedEvent);
			var filterFeeClaimedEvent = Event<FeeClaimedEventDTO>.GetEventABI().CreateFilterInput(ContractAddress);
            eventFeeClaimed = new EthLogsObservableSubscription(_webSocketClient);
            
            eventFeeClaimed.GetSubscriptionDataResponsesAsObservable().Subscribe(log =>
            {
                try
                {
                    var decoded = Event<FeeClaimedEventDTO>.DecodeEvent(log);
                    if (decoded != null)
                    {
                       OnFeeClaimed?.Invoke(decoded.Event);
                    }  
                }
                catch (Exception ex)
                {
                    Debug.LogError("Log Address: " + log.Address + " is not a standard transfer log:" + ex.Message);
                }
            });
            
            await eventFeeClaimed.SubscribeAsync(filterFeeClaimedEvent);
			var filterFeeReceiverRemovedEvent = Event<FeeReceiverRemovedEventDTO>.GetEventABI().CreateFilterInput(ContractAddress);
            eventFeeReceiverRemoved = new EthLogsObservableSubscription(_webSocketClient);
            
            eventFeeReceiverRemoved.GetSubscriptionDataResponsesAsObservable().Subscribe(log =>
            {
                try
                {
                    var decoded = Event<FeeReceiverRemovedEventDTO>.DecodeEvent(log);
                    if (decoded != null)
                    {
                       OnFeeReceiverRemoved?.Invoke(decoded.Event);
                    }  
                }
                catch (Exception ex)
                {
                    Debug.LogError("Log Address: " + log.Address + " is not a standard transfer log:" + ex.Message);
                }
            });
            
            await eventFeeReceiverRemoved.SubscribeAsync(filterFeeReceiverRemovedEvent);
			var filterFeeReceiverSetEvent = Event<FeeReceiverSetEventDTO>.GetEventABI().CreateFilterInput(ContractAddress);
            eventFeeReceiverSet = new EthLogsObservableSubscription(_webSocketClient);
            
            eventFeeReceiverSet.GetSubscriptionDataResponsesAsObservable().Subscribe(log =>
            {
                try
                {
                    var decoded = Event<FeeReceiverSetEventDTO>.DecodeEvent(log);
                    if (decoded != null)
                    {
                       OnFeeReceiverSet?.Invoke(decoded.Event);
                    }  
                }
                catch (Exception ex)
                {
                    Debug.LogError("Log Address: " + log.Address + " is not a standard transfer log:" + ex.Message);
                }
            });
            
            await eventFeeReceiverSet.SubscribeAsync(filterFeeReceiverSetEvent);
			var filterInitializedEvent = Event<InitializedEventDTO>.GetEventABI().CreateFilterInput(ContractAddress);
            eventInitialized = new EthLogsObservableSubscription(_webSocketClient);
            
            eventInitialized.GetSubscriptionDataResponsesAsObservable().Subscribe(log =>
            {
                try
                {
                    var decoded = Event<InitializedEventDTO>.DecodeEvent(log);
                    if (decoded != null)
                    {
                       OnInitialized?.Invoke(decoded.Event);
                    }  
                }
                catch (Exception ex)
                {
                    Debug.LogError("Log Address: " + log.Address + " is not a standard transfer log:" + ex.Message);
                }
            });
            
            await eventInitialized.SubscribeAsync(filterInitializedEvent);
			var filterItemCancelledEvent = Event<ItemCancelledEventDTO>.GetEventABI().CreateFilterInput(ContractAddress);
            eventItemCancelled = new EthLogsObservableSubscription(_webSocketClient);
            
            eventItemCancelled.GetSubscriptionDataResponsesAsObservable().Subscribe(log =>
            {
                try
                {
                    var decoded = Event<ItemCancelledEventDTO>.DecodeEvent(log);
                    if (decoded != null)
                    {
                       OnItemCancelled?.Invoke(decoded.Event);
                    }  
                }
                catch (Exception ex)
                {
                    Debug.LogError("Log Address: " + log.Address + " is not a standard transfer log:" + ex.Message);
                }
            });
            
            await eventItemCancelled.SubscribeAsync(filterItemCancelledEvent);
			var filterItemListedEvent = Event<ItemListedEventDTO>.GetEventABI().CreateFilterInput(ContractAddress);
            eventItemListed = new EthLogsObservableSubscription(_webSocketClient);
            
            eventItemListed.GetSubscriptionDataResponsesAsObservable().Subscribe(log =>
            {
                try
                {
                    var decoded = Event<ItemListedEventDTO>.DecodeEvent(log);
                    if (decoded != null)
                    {
                       OnItemListed?.Invoke(decoded.Event);
                    }  
                }
                catch (Exception ex)
                {
                    Debug.LogError("Log Address: " + log.Address + " is not a standard transfer log:" + ex.Message);
                }
            });
            
            await eventItemListed.SubscribeAsync(filterItemListedEvent);
			var filterItemSoldEvent = Event<ItemSoldEventDTO>.GetEventABI().CreateFilterInput(ContractAddress);
            eventItemSold = new EthLogsObservableSubscription(_webSocketClient);
            
            eventItemSold.GetSubscriptionDataResponsesAsObservable().Subscribe(log =>
            {
                try
                {
                    var decoded = Event<ItemSoldEventDTO>.DecodeEvent(log);
                    if (decoded != null)
                    {
                       OnItemSold?.Invoke(decoded.Event);
                    }  
                }
                catch (Exception ex)
                {
                    Debug.LogError("Log Address: " + log.Address + " is not a standard transfer log:" + ex.Message);
                }
            });
            
            await eventItemSold.SubscribeAsync(filterItemSoldEvent);
			var filterMaxFeeUpdatedEvent = Event<MaxFeeUpdatedEventDTO>.GetEventABI().CreateFilterInput(ContractAddress);
            eventMaxFeeUpdated = new EthLogsObservableSubscription(_webSocketClient);
            
            eventMaxFeeUpdated.GetSubscriptionDataResponsesAsObservable().Subscribe(log =>
            {
                try
                {
                    var decoded = Event<MaxFeeUpdatedEventDTO>.DecodeEvent(log);
                    if (decoded != null)
                    {
                       OnMaxFeeUpdated?.Invoke(decoded.Event);
                    }  
                }
                catch (Exception ex)
                {
                    Debug.LogError("Log Address: " + log.Address + " is not a standard transfer log:" + ex.Message);
                }
            });
            
            await eventMaxFeeUpdated.SubscribeAsync(filterMaxFeeUpdatedEvent);
			var filterNftTokenAddedEvent = Event<NftTokenAddedEventDTO>.GetEventABI().CreateFilterInput(ContractAddress);
            eventNftTokenAdded = new EthLogsObservableSubscription(_webSocketClient);
            
            eventNftTokenAdded.GetSubscriptionDataResponsesAsObservable().Subscribe(log =>
            {
                try
                {
                    var decoded = Event<NftTokenAddedEventDTO>.DecodeEvent(log);
                    if (decoded != null)
                    {
                       OnNftTokenAdded?.Invoke(decoded.Event);
                    }  
                }
                catch (Exception ex)
                {
                    Debug.LogError("Log Address: " + log.Address + " is not a standard transfer log:" + ex.Message);
                }
            });
            
            await eventNftTokenAdded.SubscribeAsync(filterNftTokenAddedEvent);
			var filterNftTokenRemovedEvent = Event<NftTokenRemovedEventDTO>.GetEventABI().CreateFilterInput(ContractAddress);
            eventNftTokenRemoved = new EthLogsObservableSubscription(_webSocketClient);
            
            eventNftTokenRemoved.GetSubscriptionDataResponsesAsObservable().Subscribe(log =>
            {
                try
                {
                    var decoded = Event<NftTokenRemovedEventDTO>.DecodeEvent(log);
                    if (decoded != null)
                    {
                       OnNftTokenRemoved?.Invoke(decoded.Event);
                    }  
                }
                catch (Exception ex)
                {
                    Debug.LogError("Log Address: " + log.Address + " is not a standard transfer log:" + ex.Message);
                }
            });
            
            await eventNftTokenRemoved.SubscribeAsync(filterNftTokenRemovedEvent);
			var filterRoleAdminChangedEvent = Event<RoleAdminChangedEventDTO>.GetEventABI().CreateFilterInput(ContractAddress);
            eventRoleAdminChanged = new EthLogsObservableSubscription(_webSocketClient);
            
            eventRoleAdminChanged.GetSubscriptionDataResponsesAsObservable().Subscribe(log =>
            {
                try
                {
                    var decoded = Event<RoleAdminChangedEventDTO>.DecodeEvent(log);
                    if (decoded != null)
                    {
                       OnRoleAdminChanged?.Invoke(decoded.Event);
                    }  
                }
                catch (Exception ex)
                {
                    Debug.LogError("Log Address: " + log.Address + " is not a standard transfer log:" + ex.Message);
                }
            });
            
            await eventRoleAdminChanged.SubscribeAsync(filterRoleAdminChangedEvent);
			var filterRoleGrantedEvent = Event<RoleGrantedEventDTO>.GetEventABI().CreateFilterInput(ContractAddress);
            eventRoleGranted = new EthLogsObservableSubscription(_webSocketClient);
            
            eventRoleGranted.GetSubscriptionDataResponsesAsObservable().Subscribe(log =>
            {
                try
                {
                    var decoded = Event<RoleGrantedEventDTO>.DecodeEvent(log);
                    if (decoded != null)
                    {
                       OnRoleGranted?.Invoke(decoded.Event);
                    }  
                }
                catch (Exception ex)
                {
                    Debug.LogError("Log Address: " + log.Address + " is not a standard transfer log:" + ex.Message);
                }
            });
            
            await eventRoleGranted.SubscribeAsync(filterRoleGrantedEvent);
			var filterRoleRevokedEvent = Event<RoleRevokedEventDTO>.GetEventABI().CreateFilterInput(ContractAddress);
            eventRoleRevoked = new EthLogsObservableSubscription(_webSocketClient);
            
            eventRoleRevoked.GetSubscriptionDataResponsesAsObservable().Subscribe(log =>
            {
                try
                {
                    var decoded = Event<RoleRevokedEventDTO>.DecodeEvent(log);
                    if (decoded != null)
                    {
                       OnRoleRevoked?.Invoke(decoded.Event);
                    }  
                }
                catch (Exception ex)
                {
                    Debug.LogError("Log Address: " + log.Address + " is not a standard transfer log:" + ex.Message);
                }
            });
            
            await eventRoleRevoked.SubscribeAsync(filterRoleRevokedEvent);
			var filterWhitelistingStatusUpdatedEvent = Event<WhitelistingStatusUpdatedEventDTO>.GetEventABI().CreateFilterInput(ContractAddress);
            eventWhitelistingStatusUpdated = new EthLogsObservableSubscription(_webSocketClient);
            
            eventWhitelistingStatusUpdated.GetSubscriptionDataResponsesAsObservable().Subscribe(log =>
            {
                try
                {
                    var decoded = Event<WhitelistingStatusUpdatedEventDTO>.DecodeEvent(log);
                    if (decoded != null)
                    {
                       OnWhitelistingStatusUpdated?.Invoke(decoded.Event);
                    }  
                }
                catch (Exception ex)
                {
                    Debug.LogError("Log Address: " + log.Address + " is not a standard transfer log:" + ex.Message);
                }
            });
            
            await eventWhitelistingStatusUpdated.SubscribeAsync(filterWhitelistingStatusUpdatedEvent);

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

public class Tuple
{
            [Parameter("address", "nftContract", 0, false)]
        public virtual string NftContract { get; set; }
        [Parameter("uint256", "tokenId", 1, false)]
        public virtual BigInteger TokenId { get; set; }
        [Parameter("address", "seller", 2, false)]
        public virtual string Seller { get; set; }
        [Parameter("uint256", "price", 3, false)]
        public virtual BigInteger Price { get; set; }
        [Parameter("uint256", "deadline", 4, false)]
        public virtual BigInteger Deadline { get; set; }

}
}
