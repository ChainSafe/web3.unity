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
        public string Address => OriginalContract.Address;

        public string ABI =>
            "[   {     \"inputs\": [],     \"stateMutability\": \"nonpayable\",     \"type\": \"constructor\"   },   {     \"inputs\": [],     \"name\": \"AlreadySameStatus\",     \"type\": \"error\"   },   {     \"inputs\": [],     \"name\": \"AmountInvalid\",     \"type\": \"error\"   },   {     \"inputs\": [],     \"name\": \"CanNotModify\",     \"type\": \"error\"   },   {     \"inputs\": [],     \"name\": \"DeadlineInvalid\",     \"type\": \"error\"   },   {     \"inputs\": [],     \"name\": \"EtherTransferFailed\",     \"type\": \"error\"   },   {     \"inputs\": [],     \"name\": \"FeeReceiverInvalid\",     \"type\": \"error\"   },   {     \"inputs\": [],     \"name\": \"IncorrectAmountSupplied\",     \"type\": \"error\"   },   {     \"inputs\": [],     \"name\": \"IncorrectLength\",     \"type\": \"error\"   },   {     \"inputs\": [],     \"name\": \"ItemExpired\",     \"type\": \"error\"   },   {     \"inputs\": [],     \"name\": \"ItemIdInvalid\",     \"type\": \"error\"   },   {     \"inputs\": [],     \"name\": \"MaxFeeInvalid\",     \"type\": \"error\"   },   {     \"inputs\": [],     \"name\": \"NFTAlreadyWhitelisted\",     \"type\": \"error\"   },   {     \"inputs\": [],     \"name\": \"NftTokenInvalid\",     \"type\": \"error\"   },   {     \"inputs\": [],     \"name\": \"NotEnoughBalance\",     \"type\": \"error\"   },   {     \"inputs\": [],     \"name\": \"NotExpired\",     \"type\": \"error\"   },   {     \"inputs\": [],     \"name\": \"OperatorInvalid\",     \"type\": \"error\"   },   {     \"inputs\": [],     \"name\": \"TotalFeePercentInvalid\",     \"type\": \"error\"   },   {     \"inputs\": [],     \"name\": \"Unauthorized\",     \"type\": \"error\"   },   {     \"inputs\": [],     \"name\": \"WhitelistingDisabled\",     \"type\": \"error\"   },   {     \"inputs\": [],     \"name\": \"ZeroAddress\",     \"type\": \"error\"   },   {     \"inputs\": [],     \"name\": \"ZeroFeePercent\",     \"type\": \"error\"   },   {     \"inputs\": [],     \"name\": \"ZeroPrice\",     \"type\": \"error\"   },   {     \"anonymous\": false,     \"inputs\": [       {         \"indexed\": false,         \"internalType\": \"address\",         \"name\": \"treasury\",         \"type\": \"address\"       },       {         \"indexed\": false,         \"internalType\": \"uint256\",         \"name\": \"feePercent\",         \"type\": \"uint256\"       }     ],     \"name\": \"ChainSafeFeeUpdated\",     \"type\": \"event\"   },   {     \"anonymous\": false,     \"inputs\": [       {         \"indexed\": false,         \"internalType\": \"address\",         \"name\": \"feeCollector\",         \"type\": \"address\"       },       {         \"indexed\": false,         \"internalType\": \"address\",         \"name\": \"receiver\",         \"type\": \"address\"       },       {         \"indexed\": false,         \"internalType\": \"uint256\",         \"name\": \"amount\",         \"type\": \"uint256\"       }     ],     \"name\": \"FeeClaimed\",     \"type\": \"event\"   },   {     \"anonymous\": false,     \"inputs\": [       {         \"indexed\": false,         \"internalType\": \"address\",         \"name\": \"feeReceiver\",         \"type\": \"address\"       }     ],     \"name\": \"FeeReceiverRemoved\",     \"type\": \"event\"   },   {     \"anonymous\": false,     \"inputs\": [       {         \"indexed\": false,         \"internalType\": \"address\",         \"name\": \"feeReceiver\",         \"type\": \"address\"       },       {         \"indexed\": false,         \"internalType\": \"uint256\",         \"name\": \"feePercent\",         \"type\": \"uint256\"       }     ],     \"name\": \"FeeReceiverSet\",     \"type\": \"event\"   },   {     \"anonymous\": false,     \"inputs\": [       {         \"indexed\": false,         \"internalType\": \"uint8\",         \"name\": \"version\",         \"type\": \"uint8\"       }     ],     \"name\": \"Initialized\",     \"type\": \"event\"   },   {     \"anonymous\": false,     \"inputs\": [       {         \"indexed\": false,         \"internalType\": \"uint256\",         \"name\": \"itemId\",         \"type\": \"uint256\"       },       {         \"indexed\": false,         \"internalType\": \"address\",         \"name\": \"owner\",         \"type\": \"address\"       }     ],     \"name\": \"ItemCancelled\",     \"type\": \"event\"   },   {     \"anonymous\": false,     \"inputs\": [       {         \"indexed\": false,         \"internalType\": \"address\",         \"name\": \"nftContract\",         \"type\": \"address\"       },       {         \"indexed\": false,         \"internalType\": \"uint256\",         \"name\": \"tokenId\",         \"type\": \"uint256\"       },       {         \"indexed\": false,         \"internalType\": \"uint256\",         \"name\": \"itemId\",         \"type\": \"uint256\"       },       {         \"indexed\": false,         \"internalType\": \"address\",         \"name\": \"seller\",         \"type\": \"address\"       },       {         \"indexed\": false,         \"internalType\": \"uint256\",         \"name\": \"price\",         \"type\": \"uint256\"       },       {         \"indexed\": false,         \"internalType\": \"uint256\",         \"name\": \"deadline\",         \"type\": \"uint256\"       }     ],     \"name\": \"ItemListed\",     \"type\": \"event\"   },   {     \"anonymous\": false,     \"inputs\": [       {         \"indexed\": false,         \"internalType\": \"uint256\",         \"name\": \"itemId\",         \"type\": \"uint256\"       },       {         \"indexed\": false,         \"internalType\": \"address\",         \"name\": \"buyer\",         \"type\": \"address\"       }     ],     \"name\": \"ItemSold\",     \"type\": \"event\"   },   {     \"anonymous\": false,     \"inputs\": [       {         \"indexed\": false,         \"internalType\": \"uint256\",         \"name\": \"feePercent\",         \"type\": \"uint256\"       }     ],     \"name\": \"MaxFeeUpdated\",     \"type\": \"event\"   },   {     \"anonymous\": false,     \"inputs\": [       {         \"indexed\": false,         \"internalType\": \"address[]\",         \"name\": \"nftAddresses\",         \"type\": \"address[]\"       }     ],     \"name\": \"NFTBlacklisted\",     \"type\": \"event\"   },   {     \"anonymous\": false,     \"inputs\": [       {         \"indexed\": false,         \"internalType\": \"address[]\",         \"name\": \"nftAddresses\",         \"type\": \"address[]\"       }     ],     \"name\": \"NFTWhitelisted\",     \"type\": \"event\"   },   {     \"anonymous\": false,     \"inputs\": [       {         \"indexed\": true,         \"internalType\": \"bytes32\",         \"name\": \"role\",         \"type\": \"bytes32\"       },       {         \"indexed\": true,         \"internalType\": \"bytes32\",         \"name\": \"previousAdminRole\",         \"type\": \"bytes32\"       },       {         \"indexed\": true,         \"internalType\": \"bytes32\",         \"name\": \"newAdminRole\",         \"type\": \"bytes32\"       }     ],     \"name\": \"RoleAdminChanged\",     \"type\": \"event\"   },   {     \"anonymous\": false,     \"inputs\": [       {         \"indexed\": true,         \"internalType\": \"bytes32\",         \"name\": \"role\",         \"type\": \"bytes32\"       },       {         \"indexed\": true,         \"internalType\": \"address\",         \"name\": \"account\",         \"type\": \"address\"       },       {         \"indexed\": true,         \"internalType\": \"address\",         \"name\": \"sender\",         \"type\": \"address\"       }     ],     \"name\": \"RoleGranted\",     \"type\": \"event\"   },   {     \"anonymous\": false,     \"inputs\": [       {         \"indexed\": true,         \"internalType\": \"bytes32\",         \"name\": \"role\",         \"type\": \"bytes32\"       },       {         \"indexed\": true,         \"internalType\": \"address\",         \"name\": \"account\",         \"type\": \"address\"       },       {         \"indexed\": true,         \"internalType\": \"address\",         \"name\": \"sender\",         \"type\": \"address\"       }     ],     \"name\": \"RoleRevoked\",     \"type\": \"event\"   },   {     \"anonymous\": false,     \"inputs\": [       {         \"indexed\": false,         \"internalType\": \"bool\",         \"name\": \"isEnabled\",         \"type\": \"bool\"       }     ],     \"name\": \"WhitelistingStatusUpdated\",     \"type\": \"event\"   },   {     \"inputs\": [],     \"name\": \"CREATOR_ROLE\",     \"outputs\": [       {         \"internalType\": \"bytes32\",         \"name\": \"\",         \"type\": \"bytes32\"       }     ],     \"stateMutability\": \"view\",     \"type\": \"function\"   },   {     \"inputs\": [],     \"name\": \"DEFAULT_ADMIN_ROLE\",     \"outputs\": [       {         \"internalType\": \"bytes32\",         \"name\": \"\",         \"type\": \"bytes32\"       }     ],     \"stateMutability\": \"view\",     \"type\": \"function\"   },   {     \"inputs\": [],     \"name\": \"UPDATER_ROLE\",     \"outputs\": [       {         \"internalType\": \"bytes32\",         \"name\": \"\",         \"type\": \"bytes32\"       }     ],     \"stateMutability\": \"view\",     \"type\": \"function\"   },   {     \"inputs\": [       {         \"internalType\": \"address\",         \"name\": \"\",         \"type\": \"address\"       }     ],     \"name\": \"_feeReceiverDetails\",     \"outputs\": [       {         \"internalType\": \"uint256\",         \"name\": \"feePercent\",         \"type\": \"uint256\"       },       {         \"internalType\": \"uint256\",         \"name\": \"feeCollected\",         \"type\": \"uint256\"       }     ],     \"stateMutability\": \"view\",     \"type\": \"function\"   },   {     \"inputs\": [],     \"name\": \"activeItems\",     \"outputs\": [       {         \"components\": [           {             \"internalType\": \"address\",             \"name\": \"nftContract\",             \"type\": \"address\"           },           {             \"internalType\": \"uint256\",             \"name\": \"tokenId\",             \"type\": \"uint256\"           },           {             \"internalType\": \"address\",             \"name\": \"seller\",             \"type\": \"address\"           },           {             \"internalType\": \"uint256\",             \"name\": \"price\",             \"type\": \"uint256\"           },           {             \"internalType\": \"uint256\",             \"name\": \"deadline\",             \"type\": \"uint256\"           }         ],         \"internalType\": \"struct Marketplace.MarketItem[]\",         \"name\": \"\",         \"type\": \"tuple[]\"       }     ],     \"stateMutability\": \"view\",     \"type\": \"function\"   },   {     \"inputs\": [       {         \"internalType\": \"address[]\",         \"name\": \"nftAddresses\",         \"type\": \"address[]\"       }     ],     \"name\": \"blacklistNFTContracts\",     \"outputs\": [],     \"stateMutability\": \"nonpayable\",     \"type\": \"function\"   },   {     \"inputs\": [       {         \"internalType\": \"uint256[]\",         \"name\": \"itemIds\",         \"type\": \"uint256[]\"       }     ],     \"name\": \"cancelExpiredListings\",     \"outputs\": [],     \"stateMutability\": \"nonpayable\",     \"type\": \"function\"   },   {     \"inputs\": [       {         \"internalType\": \"uint256\",         \"name\": \"itemId\",         \"type\": \"uint256\"       }     ],     \"name\": \"cancelListing\",     \"outputs\": [],     \"stateMutability\": \"nonpayable\",     \"type\": \"function\"   },   {     \"inputs\": [],     \"name\": \"chainsafeTreasury\",     \"outputs\": [       {         \"internalType\": \"address\",         \"name\": \"\",         \"type\": \"address\"       }     ],     \"stateMutability\": \"view\",     \"type\": \"function\"   },   {     \"inputs\": [       {         \"internalType\": \"address\",         \"name\": \"receiver\",         \"type\": \"address\"       }     ],     \"name\": \"claimFee\",     \"outputs\": [],     \"stateMutability\": \"nonpayable\",     \"type\": \"function\"   },   {     \"inputs\": [       {         \"internalType\": \"bool\",         \"name\": \"isEnable\",         \"type\": \"bool\"       }     ],     \"name\": \"enableWhitelisting\",     \"outputs\": [],     \"stateMutability\": \"nonpayable\",     \"type\": \"function\"   },   {     \"inputs\": [       {         \"internalType\": \"uint256\",         \"name\": \"from\",         \"type\": \"uint256\"       },       {         \"internalType\": \"uint256\",         \"name\": \"to\",         \"type\": \"uint256\"       }     ],     \"name\": \"expiredListingIds\",     \"outputs\": [       {         \"internalType\": \"uint256[]\",         \"name\": \"\",         \"type\": \"uint256[]\"       }     ],     \"stateMutability\": \"view\",     \"type\": \"function\"   },   {     \"inputs\": [       {         \"internalType\": \"address\",         \"name\": \"feeReceiver\",         \"type\": \"address\"       }     ],     \"name\": \"feeCollectedByReceiver\",     \"outputs\": [       {         \"internalType\": \"uint256\",         \"name\": \"\",         \"type\": \"uint256\"       }     ],     \"stateMutability\": \"view\",     \"type\": \"function\"   },   {     \"inputs\": [       {         \"internalType\": \"uint256\",         \"name\": \"id\",         \"type\": \"uint256\"       }     ],     \"name\": \"feeReceiver\",     \"outputs\": [       {         \"internalType\": \"address\",         \"name\": \"feeReceiver\",         \"type\": \"address\"       },       {         \"internalType\": \"uint256\",         \"name\": \"feePercent\",         \"type\": \"uint256\"       }     ],     \"stateMutability\": \"view\",     \"type\": \"function\"   },   {     \"inputs\": [],     \"name\": \"feeReceiversNumber\",     \"outputs\": [       {         \"internalType\": \"uint256\",         \"name\": \"\",         \"type\": \"uint256\"       }     ],     \"stateMutability\": \"view\",     \"type\": \"function\"   },   {     \"inputs\": [       {         \"internalType\": \"bytes32\",         \"name\": \"role\",         \"type\": \"bytes32\"       }     ],     \"name\": \"getRoleAdmin\",     \"outputs\": [       {         \"internalType\": \"bytes32\",         \"name\": \"\",         \"type\": \"bytes32\"       }     ],     \"stateMutability\": \"view\",     \"type\": \"function\"   },   {     \"inputs\": [       {         \"internalType\": \"bytes32\",         \"name\": \"role\",         \"type\": \"bytes32\"       },       {         \"internalType\": \"address\",         \"name\": \"account\",         \"type\": \"address\"       }     ],     \"name\": \"grantRole\",     \"outputs\": [],     \"stateMutability\": \"nonpayable\",     \"type\": \"function\"   },   {     \"inputs\": [       {         \"internalType\": \"bytes32\",         \"name\": \"role\",         \"type\": \"bytes32\"       },       {         \"internalType\": \"address\",         \"name\": \"account\",         \"type\": \"address\"       }     ],     \"name\": \"hasRole\",     \"outputs\": [       {         \"internalType\": \"bool\",         \"name\": \"\",         \"type\": \"bool\"       }     ],     \"stateMutability\": \"view\",     \"type\": \"function\"   },   {     \"inputs\": [       {         \"internalType\": \"string\",         \"name\": \"projectID\",         \"type\": \"string\"       },       {         \"internalType\": \"string\",         \"name\": \"marketplaceID\",         \"type\": \"string\"       },       {         \"internalType\": \"address\",         \"name\": \"creator\",         \"type\": \"address\"       },       {         \"internalType\": \"address\",         \"name\": \"updater\",         \"type\": \"address\"       },       {         \"internalType\": \"address\",         \"name\": \"treasury\",         \"type\": \"address\"       },       {         \"internalType\": \"bool\",         \"name\": \"isWhitelistingEnable\",         \"type\": \"bool\"       },       {         \"internalType\": \"uint256\",         \"name\": \"chainsafeFeePercent\",         \"type\": \"uint256\"       },       {         \"internalType\": \"uint256\",         \"name\": \"maxPercent\",         \"type\": \"uint256\"       }     ],     \"name\": \"initialize\",     \"outputs\": [],     \"stateMutability\": \"nonpayable\",     \"type\": \"function\"   },   {     \"inputs\": [       {         \"internalType\": \"address\",         \"name\": \"token\",         \"type\": \"address\"       }     ],     \"name\": \"isNftToken\",     \"outputs\": [       {         \"internalType\": \"bool\",         \"name\": \"\",         \"type\": \"bool\"       }     ],     \"stateMutability\": \"view\",     \"type\": \"function\"   },   {     \"inputs\": [       {         \"internalType\": \"uint256\",         \"name\": \"itemId\",         \"type\": \"uint256\"       }     ],     \"name\": \"itemById\",     \"outputs\": [       {         \"components\": [           {             \"internalType\": \"address\",             \"name\": \"nftContract\",             \"type\": \"address\"           },           {             \"internalType\": \"uint256\",             \"name\": \"tokenId\",             \"type\": \"uint256\"           },           {             \"internalType\": \"address\",             \"name\": \"seller\",             \"type\": \"address\"           },           {             \"internalType\": \"uint256\",             \"name\": \"price\",             \"type\": \"uint256\"           },           {             \"internalType\": \"uint256\",             \"name\": \"deadline\",             \"type\": \"uint256\"           }         ],         \"internalType\": \"struct Marketplace.MarketItem\",         \"name\": \"\",         \"type\": \"tuple\"       }     ],     \"stateMutability\": \"view\",     \"type\": \"function\"   },   {     \"inputs\": [       {         \"internalType\": \"address\",         \"name\": \"nftContract\",         \"type\": \"address\"       },       {         \"internalType\": \"uint256\",         \"name\": \"tokenId\",         \"type\": \"uint256\"       },       {         \"internalType\": \"uint256\",         \"name\": \"price\",         \"type\": \"uint256\"       },       {         \"internalType\": \"uint256\",         \"name\": \"deadline\",         \"type\": \"uint256\"       }     ],     \"name\": \"listItem\",     \"outputs\": [],     \"stateMutability\": \"nonpayable\",     \"type\": \"function\"   },   {     \"inputs\": [       {         \"internalType\": \"address[]\",         \"name\": \"nftContracts\",         \"type\": \"address[]\"       },       {         \"internalType\": \"uint256[]\",         \"name\": \"tokenIds\",         \"type\": \"uint256[]\"       },       {         \"internalType\": \"uint256[]\",         \"name\": \"amounts\",         \"type\": \"uint256[]\"       },       {         \"internalType\": \"uint256[]\",         \"name\": \"prices\",         \"type\": \"uint256[]\"       },       {         \"internalType\": \"uint256[]\",         \"name\": \"deadlines\",         \"type\": \"uint256[]\"       }     ],     \"name\": \"listItems\",     \"outputs\": [],     \"stateMutability\": \"nonpayable\",     \"type\": \"function\"   },   {     \"inputs\": [],     \"name\": \"marketplaceID\",     \"outputs\": [       {         \"internalType\": \"string\",         \"name\": \"\",         \"type\": \"string\"       }     ],     \"stateMutability\": \"view\",     \"type\": \"function\"   },   {     \"inputs\": [],     \"name\": \"maxFeePercent\",     \"outputs\": [       {         \"internalType\": \"uint256\",         \"name\": \"\",         \"type\": \"uint256\"       }     ],     \"stateMutability\": \"view\",     \"type\": \"function\"   },   {     \"inputs\": [       {         \"internalType\": \"uint256\",         \"name\": \"id\",         \"type\": \"uint256\"       }     ],     \"name\": \"nftToken\",     \"outputs\": [       {         \"internalType\": \"address\",         \"name\": \"token\",         \"type\": \"address\"       }     ],     \"stateMutability\": \"view\",     \"type\": \"function\"   },   {     \"inputs\": [       {         \"internalType\": \"address\",         \"name\": \"operator\",         \"type\": \"address\"       },       {         \"internalType\": \"address\",         \"name\": \"from\",         \"type\": \"address\"       },       {         \"internalType\": \"uint256[]\",         \"name\": \"ids\",         \"type\": \"uint256[]\"       },       {         \"internalType\": \"uint256[]\",         \"name\": \"values\",         \"type\": \"uint256[]\"       },       {         \"internalType\": \"bytes\",         \"name\": \"data\",         \"type\": \"bytes\"       }     ],     \"name\": \"onERC1155BatchReceived\",     \"outputs\": [       {         \"internalType\": \"bytes4\",         \"name\": \"\",         \"type\": \"bytes4\"       }     ],     \"stateMutability\": \"view\",     \"type\": \"function\"   },   {     \"inputs\": [       {         \"internalType\": \"address\",         \"name\": \"operator\",         \"type\": \"address\"       },       {         \"internalType\": \"address\",         \"name\": \"from\",         \"type\": \"address\"       },       {         \"internalType\": \"uint256\",         \"name\": \"id\",         \"type\": \"uint256\"       },       {         \"internalType\": \"uint256\",         \"name\": \"value\",         \"type\": \"uint256\"       },       {         \"internalType\": \"bytes\",         \"name\": \"data\",         \"type\": \"bytes\"       }     ],     \"name\": \"onERC1155Received\",     \"outputs\": [       {         \"internalType\": \"bytes4\",         \"name\": \"\",         \"type\": \"bytes4\"       }     ],     \"stateMutability\": \"view\",     \"type\": \"function\"   },   {     \"inputs\": [       {         \"internalType\": \"address\",         \"name\": \"operator\",         \"type\": \"address\"       },       {         \"internalType\": \"address\",         \"name\": \"from\",         \"type\": \"address\"       },       {         \"internalType\": \"uint256\",         \"name\": \"id\",         \"type\": \"uint256\"       },       {         \"internalType\": \"bytes\",         \"name\": \"data\",         \"type\": \"bytes\"       }     ],     \"name\": \"onERC721Received\",     \"outputs\": [       {         \"internalType\": \"bytes4\",         \"name\": \"\",         \"type\": \"bytes4\"       }     ],     \"stateMutability\": \"view\",     \"type\": \"function\"   },   {     \"inputs\": [],     \"name\": \"projectID\",     \"outputs\": [       {         \"internalType\": \"string\",         \"name\": \"\",         \"type\": \"string\"       }     ],     \"stateMutability\": \"view\",     \"type\": \"function\"   },   {     \"inputs\": [       {         \"internalType\": \"uint256\",         \"name\": \"itemId\",         \"type\": \"uint256\"       }     ],     \"name\": \"purchaseItem\",     \"outputs\": [],     \"stateMutability\": \"payable\",     \"type\": \"function\"   },   {     \"inputs\": [       {         \"internalType\": \"address\",         \"name\": \"feeReceiver\",         \"type\": \"address\"       }     ],     \"name\": \"removeFeeReceiver\",     \"outputs\": [],     \"stateMutability\": \"nonpayable\",     \"type\": \"function\"   },   {     \"inputs\": [       {         \"internalType\": \"bytes32\",         \"name\": \"role\",         \"type\": \"bytes32\"       },       {         \"internalType\": \"address\",         \"name\": \"account\",         \"type\": \"address\"       }     ],     \"name\": \"renounceRole\",     \"outputs\": [],     \"stateMutability\": \"nonpayable\",     \"type\": \"function\"   },   {     \"inputs\": [       {         \"internalType\": \"bytes32\",         \"name\": \"role\",         \"type\": \"bytes32\"       },       {         \"internalType\": \"address\",         \"name\": \"account\",         \"type\": \"address\"       }     ],     \"name\": \"revokeRole\",     \"outputs\": [],     \"stateMutability\": \"nonpayable\",     \"type\": \"function\"   },   {     \"inputs\": [       {         \"internalType\": \"address\",         \"name\": \"feeReceiver\",         \"type\": \"address\"       },       {         \"internalType\": \"uint256\",         \"name\": \"feePercent\",         \"type\": \"uint256\"       }     ],     \"name\": \"setFeeReceiver\",     \"outputs\": [],     \"stateMutability\": \"nonpayable\",     \"type\": \"function\"   },   {     \"inputs\": [       {         \"internalType\": \"uint256\",         \"name\": \"feePercent\",         \"type\": \"uint256\"       }     ],     \"name\": \"setMaxFeePercent\",     \"outputs\": [],     \"stateMutability\": \"nonpayable\",     \"type\": \"function\"   },   {     \"inputs\": [       {         \"internalType\": \"bytes4\",         \"name\": \"interfaceId\",         \"type\": \"bytes4\"       }     ],     \"name\": \"supportsInterface\",     \"outputs\": [       {         \"internalType\": \"bool\",         \"name\": \"\",         \"type\": \"bool\"       }     ],     \"stateMutability\": \"view\",     \"type\": \"function\"   },   {     \"inputs\": [],     \"name\": \"totalFeePercent\",     \"outputs\": [       {         \"internalType\": \"uint256\",         \"name\": \"feePercent\",         \"type\": \"uint256\"       }     ],     \"stateMutability\": \"view\",     \"type\": \"function\"   },   {     \"inputs\": [],     \"name\": \"totalListings\",     \"outputs\": [       {         \"internalType\": \"uint256\",         \"name\": \"\",         \"type\": \"uint256\"       }     ],     \"stateMutability\": \"view\",     \"type\": \"function\"   },   {     \"inputs\": [       {         \"internalType\": \"address\",         \"name\": \"treasury\",         \"type\": \"address\"       },       {         \"internalType\": \"uint256\",         \"name\": \"feePercent\",         \"type\": \"uint256\"       }     ],     \"name\": \"updateChainSafeTreasury\",     \"outputs\": [],     \"stateMutability\": \"nonpayable\",     \"type\": \"function\"   },   {     \"inputs\": [       {         \"internalType\": \"address\",         \"name\": \"user\",         \"type\": \"address\"       }     ],     \"name\": \"usersListingIds\",     \"outputs\": [       {         \"internalType\": \"uint256[]\",         \"name\": \"\",         \"type\": \"uint256[]\"       }     ],     \"stateMutability\": \"view\",     \"type\": \"function\"   },   {     \"inputs\": [       {         \"internalType\": \"address\",         \"name\": \"user\",         \"type\": \"address\"       }     ],     \"name\": \"usersListings\",     \"outputs\": [       {         \"components\": [           {             \"internalType\": \"address\",             \"name\": \"nftContract\",             \"type\": \"address\"           },           {             \"internalType\": \"uint256\",             \"name\": \"tokenId\",             \"type\": \"uint256\"           },           {             \"internalType\": \"address\",             \"name\": \"seller\",             \"type\": \"address\"           },           {             \"internalType\": \"uint256\",             \"name\": \"price\",             \"type\": \"uint256\"           },           {             \"internalType\": \"uint256\",             \"name\": \"deadline\",             \"type\": \"uint256\"           }         ],         \"internalType\": \"struct Marketplace.MarketItem[]\",         \"name\": \"\",         \"type\": \"tuple[]\"       }     ],     \"stateMutability\": \"view\",     \"type\": \"function\"   },   {     \"inputs\": [       {         \"internalType\": \"address[]\",         \"name\": \"nftAddresses\",         \"type\": \"address[]\"       }     ],     \"name\": \"whitelistNFTContracts\",     \"outputs\": [],     \"stateMutability\": \"nonpayable\",     \"type\": \"function\"   },   {     \"inputs\": [],     \"name\": \"whitelistingEnable\",     \"outputs\": [       {         \"internalType\": \"bool\",         \"name\": \"\",         \"type\": \"bool\"       }     ],     \"stateMutability\": \"view\",     \"type\": \"function\"   },   {     \"stateMutability\": \"payable\",     \"type\": \"receive\"   } ]";

        public string ContractAddress { get; set; }

        public IContractBuilder ContractBuilder { get; set; }

        public Contract OriginalContract { get; set; }

        public string WebSocketUrl { get; set; }

        public bool Subscribed { get; set; }

        private StreamingWebSocketClient _webSocketClient;

        #region Methods

        public async Task<byte[]> CREATOR_ROLE()
        {
            var response = await OriginalContract.Call<byte[]>("CREATOR_ROLE", new object[]
            {
            });

            return response;
        }


        public async Task<byte[]> DEFAULT_ADMIN_ROLE()
        {
            var response = await OriginalContract.Call<byte[]>("DEFAULT_ADMIN_ROLE", new object[]
            {
            });

            return response;
        }


        public async Task<byte[]> UPDATER_ROLE()
        {
            var response = await OriginalContract.Call<byte[]>("UPDATER_ROLE", new object[]
            {
            });

            return response;
        }


        public async Task<(BigInteger feePercent, BigInteger feeCollected)> FeeReceiverDetails(string address)
        {
            var response = await OriginalContract.Call("_feeReceiverDetails", new object[]
            {
            });

            return ((BigInteger)response[0], (BigInteger)response[1]);
        }


        public async Task<MarketItem[]> ActiveItems()
        {
            var response = await OriginalContract.Call<MarketItem[]>("activeItems", new object[]
            {
            });

            return response;
        }


        public async Task BlacklistNFTContracts(string[] nftAddresses)
        {
            var response = await OriginalContract.Send("blacklistNFTContracts", new object[]
            {
                nftAddresses
            });
        }

        public async Task<TransactionReceipt> BlacklistNFTContractsWithReceipt(string[] nftAddresses)
        {
            var response = await OriginalContract.SendWithReceipt("blacklistNFTContracts", new object[]
            {
                nftAddresses
            });

            return response.receipt;
        }

        public async Task CancelExpiredListings(BigInteger[] itemIds)
        {
            var response = await OriginalContract.Send("cancelExpiredListings", new object[]
            {
                itemIds
            });
        }

        public async Task<TransactionReceipt> CancelExpiredListingsWithReceipt(BigInteger[] itemIds)
        {
            var response = await OriginalContract.SendWithReceipt("cancelExpiredListings", new object[]
            {
                itemIds
            });

            return response.receipt;
        }

        public async Task CancelListing(BigInteger itemId)
        {
            var response = await OriginalContract.Send("cancelListing", new object[]
            {
                itemId
            });
        }

        public async Task<TransactionReceipt> CancelListingWithReceipt(BigInteger itemId)
        {
            var response = await OriginalContract.SendWithReceipt("cancelListing", new object[]
            {
                itemId
            });

            return response.receipt;
        }

        public async Task<string> ChainsafeTreasury()
        {
            var response = await OriginalContract.Call<string>("chainsafeTreasury", new object[]
            {
            });

            return response;
        }


        public async Task ClaimFee(string receiver)
        {
            var response = await OriginalContract.Send("claimFee", new object[]
            {
                receiver
            });
        }

        public async Task<TransactionReceipt> ClaimFeeWithReceipt(string receiver)
        {
            var response = await OriginalContract.SendWithReceipt("claimFee", new object[]
            {
                receiver
            });

            return response.receipt;
        }

        public async Task EnableWhitelisting(bool isEnable)
        {
            var response = await OriginalContract.Send("enableWhitelisting", new object[]
            {
                isEnable
            });
        }

        public async Task<TransactionReceipt> EnableWhitelistingWithReceipt(bool isEnable)
        {
            var response = await OriginalContract.SendWithReceipt("enableWhitelisting", new object[]
            {
                isEnable
            });

            return response.receipt;
        }

        public async Task<BigInteger[]> ExpiredListingIds(BigInteger from, BigInteger to)
        {
            var response = await OriginalContract.Call<BigInteger[]>("expiredListingIds", new object[]
            {
                from, to
            });

            return response;
        }


        public async Task<BigInteger> FeeCollectedByReceiver(string feeReceiver)
        {
            var response = await OriginalContract.Call<BigInteger>("feeCollectedByReceiver", new object[]
            {
                feeReceiver
            });

            return response;
        }


        public async Task<(string feeReceiver, BigInteger feePercent)> FeeReceiver(BigInteger id)
        {
            var response = await OriginalContract.Call("feeReceiver", new object[]
            {
                id
            });

            return ((string)response[0], (BigInteger)response[1]);
        }


        public async Task<BigInteger> FeeReceiversNumber()
        {
            var response = await OriginalContract.Call<BigInteger>("feeReceiversNumber", new object[]
            {
            });

            return response;
        }


        public async Task<byte[]> GetRoleAdmin(byte[] role)
        {
            var response = await OriginalContract.Call<byte[]>("getRoleAdmin", new object[]
            {
                role
            });

            return response;
        }


        public async Task GrantRole(byte[] role, string account)
        {
            var response = await OriginalContract.Send("grantRole", new object[]
            {
                role, account
            });
        }

        public async Task<TransactionReceipt> GrantRoleWithReceipt(byte[] role, string account)
        {
            var response = await OriginalContract.SendWithReceipt("grantRole", new object[]
            {
                role, account
            });

            return response.receipt;
        }

        public async Task<bool> HasRole(byte[] role, string account)
        {
            var response = await OriginalContract.Call<bool>("hasRole", new object[]
            {
                role, account
            });

            return response;
        }


        public async Task Initialize(string projectID, string marketplaceID, string creator, string updater,
            string treasury, bool isWhitelistingEnable, BigInteger chainsafeFeePercent, BigInteger maxPercent)
        {
            var response = await OriginalContract.Send("initialize", new object[]
            {
                projectID, marketplaceID, creator, updater, treasury, isWhitelistingEnable, chainsafeFeePercent,
                maxPercent
            });
        }

        public async Task<TransactionReceipt> InitializeWithReceipt(string projectID, string marketplaceID,
            string creator, string updater, string treasury, bool isWhitelistingEnable, BigInteger chainsafeFeePercent,
            BigInteger maxPercent)
        {
            var response = await OriginalContract.SendWithReceipt("initialize", new object[]
            {
                projectID, marketplaceID, creator, updater, treasury, isWhitelistingEnable, chainsafeFeePercent,
                maxPercent
            });

            return response.receipt;
        }

        public async Task<bool> IsNftToken(string token)
        {
            var response = await OriginalContract.Call<bool>("isNftToken", new object[]
            {
                token
            });

            return response;
        }


        public async Task<MarketItem> ItemById(BigInteger itemId)
        {
            var response = await OriginalContract.Call<MarketItem>("itemById", new object[]
            {
                itemId
            });

            return response;
        }


        public async Task ListItem(string nftContract, BigInteger tokenId, BigInteger price, BigInteger deadline)
        {
            var response = await OriginalContract.Send("listItem", new object[]
            {
                nftContract, tokenId, price, deadline
            });
        }

        public async Task<TransactionReceipt> ListItemWithReceipt(string nftContract, BigInteger tokenId,
            BigInteger price, BigInteger deadline)
        {
            var response = await OriginalContract.SendWithReceipt("listItem", new object[]
            {
                nftContract, tokenId, price, deadline
            });

            return response.receipt;
        }

        public async Task ListItems(string[] nftContracts, BigInteger[] tokenIds, BigInteger[] amounts,
            BigInteger[] prices, BigInteger[] deadlines)
        {
            var response = await OriginalContract.Send("listItems", new object[]
            {
                nftContracts, tokenIds, amounts, prices, deadlines
            });
        }

        public async Task<TransactionReceipt> ListItemsWithReceipt(string[] nftContracts, BigInteger[] tokenIds,
            BigInteger[] amounts, BigInteger[] prices, BigInteger[] deadlines)
        {
            var response = await OriginalContract.SendWithReceipt("listItems", new object[]
            {
                nftContracts, tokenIds, amounts, prices, deadlines
            });

            return response.receipt;
        }

        public async Task<string> MarketplaceID()
        {
            var response = await OriginalContract.Call<string>("marketplaceID", new object[]
            {
            });

            return response;
        }


        public async Task<BigInteger> MaxFeePercent()
        {
            var response = await OriginalContract.Call<BigInteger>("maxFeePercent", new object[]
            {
            });

            return response;
        }


        public async Task<string> NftToken(BigInteger id)
        {
            var response = await OriginalContract.Call<string>("nftToken", new object[]
            {
                id
            });

            return response;
        }


        public async Task<byte[]> OnERC1155BatchReceived(string @operator, string from, BigInteger[] ids,
            BigInteger[] values, byte[] data)
        {
            var response = await OriginalContract.Call<byte[]>("onERC1155BatchReceived", new object[]
            {
                @operator, from, ids, values, data
            });

            return response;
        }


        public async Task<byte[]> OnERC1155Received(string @operator, string from, BigInteger id, BigInteger value,
            byte[] data)
        {
            var response = await OriginalContract.Call<byte[]>("onERC1155Received", new object[]
            {
                @operator, from, id, value, data
            });

            return response;
        }


        public async Task<byte[]> OnERC721Received(string @operator, string from, BigInteger id, byte[] data)
        {
            var response = await OriginalContract.Call<byte[]>("onERC721Received", new object[]
            {
                @operator, from, id, data
            });

            return response;
        }


        public async Task<string> ProjectID()
        {
            var response = await OriginalContract.Call<string>("projectID", new object[]
            {
            });

            return response;
        }


        public async Task PurchaseItem(BigInteger itemId)
        {
            var response = await OriginalContract.Send("purchaseItem", new object[]
            {
                itemId
            });
        }

        public async Task<TransactionReceipt> PurchaseItemWithReceipt(BigInteger itemId)
        {
            var response = await OriginalContract.SendWithReceipt("purchaseItem", new object[]
            {
                itemId
            });

            return response.receipt;
        }

        public async Task RemoveFeeReceiver(string feeReceiver)
        {
            var response = await OriginalContract.Send("removeFeeReceiver", new object[]
            {
                feeReceiver
            });
        }

        public async Task<TransactionReceipt> RemoveFeeReceiverWithReceipt(string feeReceiver)
        {
            var response = await OriginalContract.SendWithReceipt("removeFeeReceiver", new object[]
            {
                feeReceiver
            });

            return response.receipt;
        }

        public async Task RenounceRole(byte[] role, string account)
        {
            var response = await OriginalContract.Send("renounceRole", new object[]
            {
                role, account
            });
        }

        public async Task<TransactionReceipt> RenounceRoleWithReceipt(byte[] role, string account)
        {
            var response = await OriginalContract.SendWithReceipt("renounceRole", new object[]
            {
                role, account
            });

            return response.receipt;
        }

        public async Task RevokeRole(byte[] role, string account)
        {
            var response = await OriginalContract.Send("revokeRole", new object[]
            {
                role, account
            });
        }

        public async Task<TransactionReceipt> RevokeRoleWithReceipt(byte[] role, string account)
        {
            var response = await OriginalContract.SendWithReceipt("revokeRole", new object[]
            {
                role, account
            });

            return response.receipt;
        }

        public async Task SetFeeReceiver(string feeReceiver, BigInteger feePercent)
        {
            var response = await OriginalContract.Send("setFeeReceiver", new object[]
            {
                feeReceiver, feePercent
            });
        }

        public async Task<TransactionReceipt> SetFeeReceiverWithReceipt(string feeReceiver, BigInteger feePercent)
        {
            var response = await OriginalContract.SendWithReceipt("setFeeReceiver", new object[]
            {
                feeReceiver, feePercent
            });

            return response.receipt;
        }

        public async Task SetMaxFeePercent(BigInteger feePercent)
        {
            var response = await OriginalContract.Send("setMaxFeePercent", new object[]
            {
                feePercent
            });
        }

        public async Task<TransactionReceipt> SetMaxFeePercentWithReceipt(BigInteger feePercent)
        {
            var response = await OriginalContract.SendWithReceipt("setMaxFeePercent", new object[]
            {
                feePercent
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


        public async Task<BigInteger> TotalFeePercent()
        {
            var response = await OriginalContract.Call<BigInteger>("totalFeePercent", new object[]
            {
            });

            return response;
        }


        public async Task<BigInteger> TotalListings()
        {
            var response = await OriginalContract.Call<BigInteger>("totalListings", new object[]
            {
            });

            return response;
        }


        public async Task UpdateChainSafeTreasury(string treasury, BigInteger feePercent)
        {
            var response = await OriginalContract.Send("updateChainSafeTreasury", new object[]
            {
                treasury, feePercent
            });
        }

        public async Task<TransactionReceipt> UpdateChainSafeTreasuryWithReceipt(string treasury, BigInteger feePercent)
        {
            var response = await OriginalContract.SendWithReceipt("updateChainSafeTreasury", new object[]
            {
                treasury, feePercent
            });

            return response.receipt;
        }

        public async Task<BigInteger[]> UsersListingIds(string user)
        {
            var response = await OriginalContract.Call<BigInteger[]>("usersListingIds", new object[]
            {
                user
            });

            return response;
        }


        public async Task<MarketItem[]> UsersListings(string user)
        {
            var response = await OriginalContract.Call<MarketItem[]>("usersListings", new object[]
            {
                user
            });

            return response;
        }


        public async Task WhitelistNFTContracts(string[] nftAddresses)
        {
            var response = await OriginalContract.Send("whitelistNFTContracts", new object[]
            {
                nftAddresses
            });
        }

        public async Task<TransactionReceipt> WhitelistNFTContractsWithReceipt(string[] nftAddresses)
        {
            var response = await OriginalContract.SendWithReceipt("whitelistNFTContracts", new object[]
            {
                nftAddresses
            });

            return response.receipt;
        }

        public async Task<bool> WhitelistingEnable()
        {
            var response = await OriginalContract.Call<bool>("whitelistingEnable", new object[]
            {
            });

            return response;
        }

        #endregion


        #region Event Classes

        public partial class ChainSafeFeeUpdatedEventDTO : ChainSafeFeeUpdatedEventDTOBase
        {
        }

        [Event("ChainSafeFeeUpdated")]
        public class ChainSafeFeeUpdatedEventDTOBase : IEventDTO
        {
            [Parameter("address", "treasury", 0, false)]
            public virtual string Treasury { get; set; }

            [Parameter("uint256", "feePercent", 1, false)]
            public virtual BigInteger FeePercent { get; set; }
        }

        private EthLogsObservableSubscription eventChainSafeFeeUpdated;
        public event Action<ChainSafeFeeUpdatedEventDTO> OnChainSafeFeeUpdated;

        public partial class FeeClaimedEventDTO : FeeClaimedEventDTOBase
        {
        }

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

        private EthLogsObservableSubscription eventFeeClaimed;
        public event Action<FeeClaimedEventDTO> OnFeeClaimed;

        public partial class FeeReceiverRemovedEventDTO : FeeReceiverRemovedEventDTOBase
        {
        }

        [Event("FeeReceiverRemoved")]
        public class FeeReceiverRemovedEventDTOBase : IEventDTO
        {
            [Parameter("address", "feeReceiver", 0, false)]
            public virtual string FeeReceiver { get; set; }
        }

        private EthLogsObservableSubscription eventFeeReceiverRemoved;
        public event Action<FeeReceiverRemovedEventDTO> OnFeeReceiverRemoved;

        public partial class FeeReceiverSetEventDTO : FeeReceiverSetEventDTOBase
        {
        }

        [Event("FeeReceiverSet")]
        public class FeeReceiverSetEventDTOBase : IEventDTO
        {
            [Parameter("address", "feeReceiver", 0, false)]
            public virtual string FeeReceiver { get; set; }

            [Parameter("uint256", "feePercent", 1, false)]
            public virtual BigInteger FeePercent { get; set; }
        }

        private EthLogsObservableSubscription eventFeeReceiverSet;
        public event Action<FeeReceiverSetEventDTO> OnFeeReceiverSet;

        public partial class InitializedEventDTO : InitializedEventDTOBase
        {
        }

        [Event("Initialized")]
        public class InitializedEventDTOBase : IEventDTO
        {
            [Parameter("uint8", "version", 0, false)]
            public virtual byte Version { get; set; }
        }

        private EthLogsObservableSubscription eventInitialized;
        public event Action<InitializedEventDTO> OnInitialized;

        public partial class ItemCancelledEventDTO : ItemCancelledEventDTOBase
        {
        }

        [Event("ItemCancelled")]
        public class ItemCancelledEventDTOBase : IEventDTO
        {
            [Parameter("uint256", "itemId", 0, false)]
            public virtual BigInteger ItemId { get; set; }

            [Parameter("address", "owner", 1, false)]
            public virtual string Owner { get; set; }
        }

        private EthLogsObservableSubscription eventItemCancelled;
        public event Action<ItemCancelledEventDTO> OnItemCancelled;

        public partial class ItemListedEventDTO : ItemListedEventDTOBase
        {
        }

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

        private EthLogsObservableSubscription eventItemListed;
        public event Action<ItemListedEventDTO> OnItemListed;

        public partial class ItemSoldEventDTO : ItemSoldEventDTOBase
        {
        }

        [Event("ItemSold")]
        public class ItemSoldEventDTOBase : IEventDTO
        {
            [Parameter("uint256", "itemId", 0, false)]
            public virtual BigInteger ItemId { get; set; }

            [Parameter("address", "buyer", 1, false)]
            public virtual string Buyer { get; set; }
        }

        private EthLogsObservableSubscription eventItemSold;
        public event Action<ItemSoldEventDTO> OnItemSold;

        public partial class MaxFeeUpdatedEventDTO : MaxFeeUpdatedEventDTOBase
        {
        }

        [Event("MaxFeeUpdated")]
        public class MaxFeeUpdatedEventDTOBase : IEventDTO
        {
            [Parameter("uint256", "feePercent", 0, false)]
            public virtual BigInteger FeePercent { get; set; }
        }

        private EthLogsObservableSubscription eventMaxFeeUpdated;
        public event Action<MaxFeeUpdatedEventDTO> OnMaxFeeUpdated;

        public partial class NFTBlacklistedEventDTO : NFTBlacklistedEventDTOBase
        {
        }

        [Event("NFTBlacklisted")]
        public class NFTBlacklistedEventDTOBase : IEventDTO
        {
            [Parameter("address[]", "nftAddresses", 0, false)]
            public virtual string[] NftAddresses { get; set; }
        }

        private EthLogsObservableSubscription eventNFTBlacklisted;
        public event Action<NFTBlacklistedEventDTO> OnNFTBlacklisted;

        public partial class NFTWhitelistedEventDTO : NFTWhitelistedEventDTOBase
        {
        }

        [Event("NFTWhitelisted")]
        public class NFTWhitelistedEventDTOBase : IEventDTO
        {
            [Parameter("address[]", "nftAddresses", 0, false)]
            public virtual string[] NftAddresses { get; set; }
        }

        private EthLogsObservableSubscription eventNFTWhitelisted;
        public event Action<NFTWhitelistedEventDTO> OnNFTWhitelisted;

        public partial class RoleAdminChangedEventDTO : RoleAdminChangedEventDTOBase
        {
        }

        [Event("RoleAdminChanged")]
        public class RoleAdminChangedEventDTOBase : IEventDTO
        {
            [Parameter("bytes32", "role", 0, true)]
            public virtual byte[] Role { get; set; }

            [Parameter("bytes32", "previousAdminRole", 1, true)]
            public virtual byte[] PreviousAdminRole { get; set; }

            [Parameter("bytes32", "newAdminRole", 2, true)]
            public virtual byte[] NewAdminRole { get; set; }
        }

        private EthLogsObservableSubscription eventRoleAdminChanged;
        public event Action<RoleAdminChangedEventDTO> OnRoleAdminChanged;

        public partial class RoleGrantedEventDTO : RoleGrantedEventDTOBase
        {
        }

        [Event("RoleGranted")]
        public class RoleGrantedEventDTOBase : IEventDTO
        {
            [Parameter("bytes32", "role", 0, true)]
            public virtual byte[] Role { get; set; }

            [Parameter("address", "account", 1, true)]
            public virtual string Account { get; set; }

            [Parameter("address", "sender", 2, true)]
            public virtual string Sender { get; set; }
        }

        private EthLogsObservableSubscription eventRoleGranted;
        public event Action<RoleGrantedEventDTO> OnRoleGranted;

        public partial class RoleRevokedEventDTO : RoleRevokedEventDTOBase
        {
        }

        [Event("RoleRevoked")]
        public class RoleRevokedEventDTOBase : IEventDTO
        {
            [Parameter("bytes32", "role", 0, true)]
            public virtual byte[] Role { get; set; }

            [Parameter("address", "account", 1, true)]
            public virtual string Account { get; set; }

            [Parameter("address", "sender", 2, true)]
            public virtual string Sender { get; set; }
        }

        private EthLogsObservableSubscription eventRoleRevoked;
        public event Action<RoleRevokedEventDTO> OnRoleRevoked;

        public partial class WhitelistingStatusUpdatedEventDTO : WhitelistingStatusUpdatedEventDTOBase
        {
        }

        [Event("WhitelistingStatusUpdated")]
        public class WhitelistingStatusUpdatedEventDTOBase : IEventDTO
        {
            [Parameter("bool", "isEnabled", 0, false)]
            public virtual bool IsEnabled { get; set; }
        }

        private EthLogsObservableSubscription eventWhitelistingStatusUpdated;
        public event Action<WhitelistingStatusUpdatedEventDTO> OnWhitelistingStatusUpdated;

        #endregion

        #region Interface Implemented Methods

        public async ValueTask DisposeAsync()
        {
            if (string.IsNullOrEmpty(WebSocketUrl))
                return;
            if (!Subscribed)
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
            await eventNFTBlacklisted.UnsubscribeAsync();
            OnNFTBlacklisted = null;
            await eventNFTWhitelisted.UnsubscribeAsync();
            OnNFTWhitelisted = null;
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
            if (Subscribed)
                return;

            if (string.IsNullOrEmpty(WebSocketUrl))
            {
                Debug.LogWarning($"WebSocketUrl is not set for this class. Event Subscriptions will not work.");
                return;
            }

            _webSocketClient ??= new StreamingWebSocketClient(WebSocketUrl);
            await _webSocketClient.StartAsync();
            Subscribed = true;

            var filterChainSafeFeeUpdatedEvent = Event<ChainSafeFeeUpdatedEventDTO>.GetEventABI().CreateFilterInput();
            eventChainSafeFeeUpdated = new EthLogsObservableSubscription(_webSocketClient);

            eventChainSafeFeeUpdated.GetSubscriptionDataResponsesAsObservable().Subscribe(log =>
            {
                try
                {
                    var decoded = Event<ChainSafeFeeUpdatedEventDTO>.DecodeEvent(log);
                    if (decoded != null) OnChainSafeFeeUpdated?.Invoke(decoded.Event);
                }
                catch (Exception ex)
                {
                    Debug.LogError("Log Address: " + log.Address + " is not a standard transfer log:" + ex.Message);
                }
            });

            await eventChainSafeFeeUpdated.SubscribeAsync(filterChainSafeFeeUpdatedEvent);
            var filterFeeClaimedEvent = Event<FeeClaimedEventDTO>.GetEventABI().CreateFilterInput();
            eventFeeClaimed = new EthLogsObservableSubscription(_webSocketClient);

            eventFeeClaimed.GetSubscriptionDataResponsesAsObservable().Subscribe(log =>
            {
                try
                {
                    var decoded = Event<FeeClaimedEventDTO>.DecodeEvent(log);
                    if (decoded != null) OnFeeClaimed?.Invoke(decoded.Event);
                }
                catch (Exception ex)
                {
                    Debug.LogError("Log Address: " + log.Address + " is not a standard transfer log:" + ex.Message);
                }
            });

            await eventFeeClaimed.SubscribeAsync(filterFeeClaimedEvent);
            var filterFeeReceiverRemovedEvent = Event<FeeReceiverRemovedEventDTO>.GetEventABI().CreateFilterInput();
            eventFeeReceiverRemoved = new EthLogsObservableSubscription(_webSocketClient);

            eventFeeReceiverRemoved.GetSubscriptionDataResponsesAsObservable().Subscribe(log =>
            {
                try
                {
                    var decoded = Event<FeeReceiverRemovedEventDTO>.DecodeEvent(log);
                    if (decoded != null) OnFeeReceiverRemoved?.Invoke(decoded.Event);
                }
                catch (Exception ex)
                {
                    Debug.LogError("Log Address: " + log.Address + " is not a standard transfer log:" + ex.Message);
                }
            });

            await eventFeeReceiverRemoved.SubscribeAsync(filterFeeReceiverRemovedEvent);
            var filterFeeReceiverSetEvent = Event<FeeReceiverSetEventDTO>.GetEventABI().CreateFilterInput();
            eventFeeReceiverSet = new EthLogsObservableSubscription(_webSocketClient);

            eventFeeReceiverSet.GetSubscriptionDataResponsesAsObservable().Subscribe(log =>
            {
                try
                {
                    var decoded = Event<FeeReceiverSetEventDTO>.DecodeEvent(log);
                    if (decoded != null) OnFeeReceiverSet?.Invoke(decoded.Event);
                }
                catch (Exception ex)
                {
                    Debug.LogError("Log Address: " + log.Address + " is not a standard transfer log:" + ex.Message);
                }
            });

            await eventFeeReceiverSet.SubscribeAsync(filterFeeReceiverSetEvent);
            var filterInitializedEvent = Event<InitializedEventDTO>.GetEventABI().CreateFilterInput();
            eventInitialized = new EthLogsObservableSubscription(_webSocketClient);

            eventInitialized.GetSubscriptionDataResponsesAsObservable().Subscribe(log =>
            {
                try
                {
                    var decoded = Event<InitializedEventDTO>.DecodeEvent(log);
                    if (decoded != null) OnInitialized?.Invoke(decoded.Event);
                }
                catch (Exception ex)
                {
                    Debug.LogError("Log Address: " + log.Address + " is not a standard transfer log:" + ex.Message);
                }
            });

            await eventInitialized.SubscribeAsync(filterInitializedEvent);
            var filterItemCancelledEvent = Event<ItemCancelledEventDTO>.GetEventABI().CreateFilterInput();
            eventItemCancelled = new EthLogsObservableSubscription(_webSocketClient);

            eventItemCancelled.GetSubscriptionDataResponsesAsObservable().Subscribe(log =>
            {
                try
                {
                    var decoded = Event<ItemCancelledEventDTO>.DecodeEvent(log);
                    if (decoded != null) OnItemCancelled?.Invoke(decoded.Event);
                }
                catch (Exception ex)
                {
                    Debug.LogError("Log Address: " + log.Address + " is not a standard transfer log:" + ex.Message);
                }
            });

            await eventItemCancelled.SubscribeAsync(filterItemCancelledEvent);
            var filterItemListedEvent = Event<ItemListedEventDTO>.GetEventABI().CreateFilterInput();
            eventItemListed = new EthLogsObservableSubscription(_webSocketClient);

            eventItemListed.GetSubscriptionDataResponsesAsObservable().Subscribe(log =>
            {
                try
                {
                    var decoded = Event<ItemListedEventDTO>.DecodeEvent(log);
                    if (decoded != null) OnItemListed?.Invoke(decoded.Event);
                }
                catch (Exception ex)
                {
                    Debug.LogError("Log Address: " + log.Address + " is not a standard transfer log:" + ex.Message);
                }
            });

            await eventItemListed.SubscribeAsync(filterItemListedEvent);
            var filterItemSoldEvent = Event<ItemSoldEventDTO>.GetEventABI().CreateFilterInput();
            eventItemSold = new EthLogsObservableSubscription(_webSocketClient);

            eventItemSold.GetSubscriptionDataResponsesAsObservable().Subscribe(log =>
            {
                try
                {
                    var decoded = Event<ItemSoldEventDTO>.DecodeEvent(log);
                    if (decoded != null) OnItemSold?.Invoke(decoded.Event);
                }
                catch (Exception ex)
                {
                    Debug.LogError("Log Address: " + log.Address + " is not a standard transfer log:" + ex.Message);
                }
            });

            await eventItemSold.SubscribeAsync(filterItemSoldEvent);
            var filterMaxFeeUpdatedEvent = Event<MaxFeeUpdatedEventDTO>.GetEventABI().CreateFilterInput();
            eventMaxFeeUpdated = new EthLogsObservableSubscription(_webSocketClient);

            eventMaxFeeUpdated.GetSubscriptionDataResponsesAsObservable().Subscribe(log =>
            {
                try
                {
                    var decoded = Event<MaxFeeUpdatedEventDTO>.DecodeEvent(log);
                    if (decoded != null) OnMaxFeeUpdated?.Invoke(decoded.Event);
                }
                catch (Exception ex)
                {
                    Debug.LogError("Log Address: " + log.Address + " is not a standard transfer log:" + ex.Message);
                }
            });

            await eventMaxFeeUpdated.SubscribeAsync(filterMaxFeeUpdatedEvent);
            var filterNFTBlacklistedEvent = Event<NFTBlacklistedEventDTO>.GetEventABI().CreateFilterInput();
            eventNFTBlacklisted = new EthLogsObservableSubscription(_webSocketClient);

            eventNFTBlacklisted.GetSubscriptionDataResponsesAsObservable().Subscribe(log =>
            {
                try
                {
                    var decoded = Event<NFTBlacklistedEventDTO>.DecodeEvent(log);
                    if (decoded != null) OnNFTBlacklisted?.Invoke(decoded.Event);
                }
                catch (Exception ex)
                {
                    Debug.LogError("Log Address: " + log.Address + " is not a standard transfer log:" + ex.Message);
                }
            });

            await eventNFTBlacklisted.SubscribeAsync(filterNFTBlacklistedEvent);
            var filterNFTWhitelistedEvent = Event<NFTWhitelistedEventDTO>.GetEventABI().CreateFilterInput();
            eventNFTWhitelisted = new EthLogsObservableSubscription(_webSocketClient);

            eventNFTWhitelisted.GetSubscriptionDataResponsesAsObservable().Subscribe(log =>
            {
                try
                {
                    var decoded = Event<NFTWhitelistedEventDTO>.DecodeEvent(log);
                    if (decoded != null) OnNFTWhitelisted?.Invoke(decoded.Event);
                }
                catch (Exception ex)
                {
                    Debug.LogError("Log Address: " + log.Address + " is not a standard transfer log:" + ex.Message);
                }
            });

            await eventNFTWhitelisted.SubscribeAsync(filterNFTWhitelistedEvent);
            var filterRoleAdminChangedEvent = Event<RoleAdminChangedEventDTO>.GetEventABI().CreateFilterInput();
            eventRoleAdminChanged = new EthLogsObservableSubscription(_webSocketClient);

            eventRoleAdminChanged.GetSubscriptionDataResponsesAsObservable().Subscribe(log =>
            {
                try
                {
                    var decoded = Event<RoleAdminChangedEventDTO>.DecodeEvent(log);
                    if (decoded != null) OnRoleAdminChanged?.Invoke(decoded.Event);
                }
                catch (Exception ex)
                {
                    Debug.LogError("Log Address: " + log.Address + " is not a standard transfer log:" + ex.Message);
                }
            });

            await eventRoleAdminChanged.SubscribeAsync(filterRoleAdminChangedEvent);
            var filterRoleGrantedEvent = Event<RoleGrantedEventDTO>.GetEventABI().CreateFilterInput();
            eventRoleGranted = new EthLogsObservableSubscription(_webSocketClient);

            eventRoleGranted.GetSubscriptionDataResponsesAsObservable().Subscribe(log =>
            {
                try
                {
                    var decoded = Event<RoleGrantedEventDTO>.DecodeEvent(log);
                    if (decoded != null) OnRoleGranted?.Invoke(decoded.Event);
                }
                catch (Exception ex)
                {
                    Debug.LogError("Log Address: " + log.Address + " is not a standard transfer log:" + ex.Message);
                }
            });

            await eventRoleGranted.SubscribeAsync(filterRoleGrantedEvent);
            var filterRoleRevokedEvent = Event<RoleRevokedEventDTO>.GetEventABI().CreateFilterInput();
            eventRoleRevoked = new EthLogsObservableSubscription(_webSocketClient);

            eventRoleRevoked.GetSubscriptionDataResponsesAsObservable().Subscribe(log =>
            {
                try
                {
                    var decoded = Event<RoleRevokedEventDTO>.DecodeEvent(log);
                    if (decoded != null) OnRoleRevoked?.Invoke(decoded.Event);
                }
                catch (Exception ex)
                {
                    Debug.LogError("Log Address: " + log.Address + " is not a standard transfer log:" + ex.Message);
                }
            });

            await eventRoleRevoked.SubscribeAsync(filterRoleRevokedEvent);
            var filterWhitelistingStatusUpdatedEvent =
                Event<WhitelistingStatusUpdatedEventDTO>.GetEventABI().CreateFilterInput();
            eventWhitelistingStatusUpdated = new EthLogsObservableSubscription(_webSocketClient);

            eventWhitelistingStatusUpdated.GetSubscriptionDataResponsesAsObservable().Subscribe(log =>
            {
                try
                {
                    var decoded = Event<WhitelistingStatusUpdatedEventDTO>.DecodeEvent(log);
                    if (decoded != null) OnWhitelistingStatusUpdated?.Invoke(decoded.Event);
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
        public Task<TransactionRequest> PrepareTransactionRequest(string method, object[] parameters, bool isReadCall = false,
            TransactionRequest overwrite = null)
        {
            return OriginalContract.PrepareTransactionRequest(method, parameters, isReadCall, overwrite);
        }

        #endregion
    }

    public class MarketItem
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