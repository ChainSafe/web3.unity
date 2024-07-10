using System.Threading.Tasks;
using System;
using ChainSafe.Gaming.Evm.Transactions;
using Nethereum.Hex.HexTypes;
using ChainSafe.Gaming.Evm.Contracts;
using System.Numerics;

namespace ChainSafe.Gaming.Evm.Contracts.Custom
{
    public class TestContract : ICustomContract
    {
        public string ABI =>
            "[   {     \"inputs\": [],     \"stateMutability\": \"nonpayable\",     \"type\": \"constructor\"   },   {     \"inputs\": [],     \"name\": \"AlreadySameStatus\",     \"type\": \"error\"   },   {     \"inputs\": [],     \"name\": \"AmountInvalid\",     \"type\": \"error\"   },   {     \"inputs\": [],     \"name\": \"CanNotModify\",     \"type\": \"error\"   },   {     \"inputs\": [],     \"name\": \"DeadlineInvalid\",     \"type\": \"error\"   },   {     \"inputs\": [],     \"name\": \"EtherTransferFailed\",     \"type\": \"error\"   },   {     \"inputs\": [],     \"name\": \"FeeReceiverInvalid\",     \"type\": \"error\"   },   {     \"inputs\": [],     \"name\": \"IncorrectAmountSupplied\",     \"type\": \"error\"   },   {     \"inputs\": [],     \"name\": \"IncorrectLength\",     \"type\": \"error\"   },   {     \"inputs\": [],     \"name\": \"ItemExpired\",     \"type\": \"error\"   },   {     \"inputs\": [],     \"name\": \"ItemIdInvalid\",     \"type\": \"error\"   },   {     \"inputs\": [],     \"name\": \"MaxFeeInvalid\",     \"type\": \"error\"   },   {     \"inputs\": [],     \"name\": \"NFTAlreadyWhitelisted\",     \"type\": \"error\"   },   {     \"inputs\": [],     \"name\": \"NftTokenInvalid\",     \"type\": \"error\"   },   {     \"inputs\": [],     \"name\": \"NotEnoughBalance\",     \"type\": \"error\"   },   {     \"inputs\": [],     \"name\": \"NotExpired\",     \"type\": \"error\"   },   {     \"inputs\": [],     \"name\": \"OperatorInvalid\",     \"type\": \"error\"   },   {     \"inputs\": [],     \"name\": \"TotalFeePercentInvalid\",     \"type\": \"error\"   },   {     \"inputs\": [],     \"name\": \"Unauthorized\",     \"type\": \"error\"   },   {     \"inputs\": [],     \"name\": \"WhitelistingDisabled\",     \"type\": \"error\"   },   {     \"inputs\": [],     \"name\": \"ZeroAddress\",     \"type\": \"error\"   },   {     \"inputs\": [],     \"name\": \"ZeroFeePercent\",     \"type\": \"error\"   },   {     \"inputs\": [],     \"name\": \"ZeroPrice\",     \"type\": \"error\"   },   {     \"anonymous\": false,     \"inputs\": [       {         \"indexed\": false,         \"internalType\": \"address\",         \"name\": \"treasury\",         \"type\": \"address\"       },       {         \"indexed\": false,         \"internalType\": \"uint256\",         \"name\": \"feePercent\",         \"type\": \"uint256\"       }     ],     \"name\": \"ChainSafeFeeUpdated\",     \"type\": \"event\"   },   {     \"anonymous\": false,     \"inputs\": [       {         \"indexed\": false,         \"internalType\": \"address\",         \"name\": \"feeCollector\",         \"type\": \"address\"       },       {         \"indexed\": false,         \"internalType\": \"address\",         \"name\": \"receiver\",         \"type\": \"address\"       },       {         \"indexed\": false,         \"internalType\": \"uint256\",         \"name\": \"amount\",         \"type\": \"uint256\"       }     ],     \"name\": \"FeeClaimed\",     \"type\": \"event\"   },   {     \"anonymous\": false,     \"inputs\": [       {         \"indexed\": false,         \"internalType\": \"address\",         \"name\": \"feeReceiver\",         \"type\": \"address\"       }     ],     \"name\": \"FeeReceiverRemoved\",     \"type\": \"event\"   },   {     \"anonymous\": false,     \"inputs\": [       {         \"indexed\": false,         \"internalType\": \"address\",         \"name\": \"feeReceiver\",         \"type\": \"address\"       },       {         \"indexed\": false,         \"internalType\": \"uint256\",         \"name\": \"feePercent\",         \"type\": \"uint256\"       }     ],     \"name\": \"FeeReceiverSet\",     \"type\": \"event\"   },   {     \"anonymous\": false,     \"inputs\": [       {         \"indexed\": false,         \"internalType\": \"uint8\",         \"name\": \"version\",         \"type\": \"uint8\"       }     ],     \"name\": \"Initialized\",     \"type\": \"event\"   },   {     \"anonymous\": false,     \"inputs\": [       {         \"indexed\": false,         \"internalType\": \"uint256\",         \"name\": \"itemId\",         \"type\": \"uint256\"       },       {         \"indexed\": false,         \"internalType\": \"address\",         \"name\": \"owner\",         \"type\": \"address\"       }     ],     \"name\": \"ItemCancelled\",     \"type\": \"event\"   },   {     \"anonymous\": false,     \"inputs\": [       {         \"indexed\": false,         \"internalType\": \"address\",         \"name\": \"nftContract\",         \"type\": \"address\"       },       {         \"indexed\": false,         \"internalType\": \"uint256\",         \"name\": \"tokenId\",         \"type\": \"uint256\"       },       {         \"indexed\": false,         \"internalType\": \"uint256\",         \"name\": \"itemId\",         \"type\": \"uint256\"       },       {         \"indexed\": false,         \"internalType\": \"address\",         \"name\": \"seller\",         \"type\": \"address\"       },       {         \"indexed\": false,         \"internalType\": \"uint256\",         \"name\": \"price\",         \"type\": \"uint256\"       },       {         \"indexed\": false,         \"internalType\": \"uint256\",         \"name\": \"deadline\",         \"type\": \"uint256\"       }     ],     \"name\": \"ItemListed\",     \"type\": \"event\"   },   {     \"anonymous\": false,     \"inputs\": [       {         \"indexed\": false,         \"internalType\": \"uint256\",         \"name\": \"itemId\",         \"type\": \"uint256\"       },       {         \"indexed\": false,         \"internalType\": \"address\",         \"name\": \"buyer\",         \"type\": \"address\"       }     ],     \"name\": \"ItemSold\",     \"type\": \"event\"   },   {     \"anonymous\": false,     \"inputs\": [       {         \"indexed\": false,         \"internalType\": \"uint256\",         \"name\": \"feePercent\",         \"type\": \"uint256\"       }     ],     \"name\": \"MaxFeeUpdated\",     \"type\": \"event\"   },   {     \"anonymous\": false,     \"inputs\": [       {         \"indexed\": false,         \"internalType\": \"address[]\",         \"name\": \"nftAddresses\",         \"type\": \"address[]\"       }     ],     \"name\": \"NFTBlacklisted\",     \"type\": \"event\"   },   {     \"anonymous\": false,     \"inputs\": [       {         \"indexed\": false,         \"internalType\": \"address[]\",         \"name\": \"nftAddresses\",         \"type\": \"address[]\"       }     ],     \"name\": \"NFTWhitelisted\",     \"type\": \"event\"   },   {     \"anonymous\": false,     \"inputs\": [       {         \"indexed\": true,         \"internalType\": \"bytes32\",         \"name\": \"role\",         \"type\": \"bytes32\"       },       {         \"indexed\": true,         \"internalType\": \"bytes32\",         \"name\": \"previousAdminRole\",         \"type\": \"bytes32\"       },       {         \"indexed\": true,         \"internalType\": \"bytes32\",         \"name\": \"newAdminRole\",         \"type\": \"bytes32\"       }     ],     \"name\": \"RoleAdminChanged\",     \"type\": \"event\"   },   {     \"anonymous\": false,     \"inputs\": [       {         \"indexed\": true,         \"internalType\": \"bytes32\",         \"name\": \"role\",         \"type\": \"bytes32\"       },       {         \"indexed\": true,         \"internalType\": \"address\",         \"name\": \"account\",         \"type\": \"address\"       },       {         \"indexed\": true,         \"internalType\": \"address\",         \"name\": \"sender\",         \"type\": \"address\"       }     ],     \"name\": \"RoleGranted\",     \"type\": \"event\"   },   {     \"anonymous\": false,     \"inputs\": [       {         \"indexed\": true,         \"internalType\": \"bytes32\",         \"name\": \"role\",         \"type\": \"bytes32\"       },       {         \"indexed\": true,         \"internalType\": \"address\",         \"name\": \"account\",         \"type\": \"address\"       },       {         \"indexed\": true,         \"internalType\": \"address\",         \"name\": \"sender\",         \"type\": \"address\"       }     ],     \"name\": \"RoleRevoked\",     \"type\": \"event\"   },   {     \"anonymous\": false,     \"inputs\": [       {         \"indexed\": false,         \"internalType\": \"bool\",         \"name\": \"isEnabled\",         \"type\": \"bool\"       }     ],     \"name\": \"WhitelistingStatusUpdated\",     \"type\": \"event\"   },   {     \"inputs\": [],     \"name\": \"CREATOR_ROLE\",     \"outputs\": [       {         \"internalType\": \"bytes32\",         \"name\": \"\",         \"type\": \"bytes32\"       }     ],     \"stateMutability\": \"view\",     \"type\": \"function\"   },   {     \"inputs\": [],     \"name\": \"DEFAULT_ADMIN_ROLE\",     \"outputs\": [       {         \"internalType\": \"bytes32\",         \"name\": \"\",         \"type\": \"bytes32\"       }     ],     \"stateMutability\": \"view\",     \"type\": \"function\"   },   {     \"inputs\": [],     \"name\": \"UPDATER_ROLE\",     \"outputs\": [       {         \"internalType\": \"bytes32\",         \"name\": \"\",         \"type\": \"bytes32\"       }     ],     \"stateMutability\": \"view\",     \"type\": \"function\"   },   {     \"inputs\": [       {         \"internalType\": \"address\",         \"name\": \"\",         \"type\": \"address\"       }     ],     \"name\": \"_feeReceiverDetails\",     \"outputs\": [       {         \"internalType\": \"uint256\",         \"name\": \"feePercent\",         \"type\": \"uint256\"       },       {         \"internalType\": \"uint256\",         \"name\": \"feeCollected\",         \"type\": \"uint256\"       }     ],     \"stateMutability\": \"view\",     \"type\": \"function\"   },   {     \"inputs\": [],     \"name\": \"activeItems\",     \"outputs\": [       {         \"components\": [           {             \"internalType\": \"address\",             \"name\": \"nftContract\",             \"type\": \"address\"           },           {             \"internalType\": \"uint256\",             \"name\": \"tokenId\",             \"type\": \"uint256\"           },           {             \"internalType\": \"address\",             \"name\": \"seller\",             \"type\": \"address\"           },           {             \"internalType\": \"uint256\",             \"name\": \"price\",             \"type\": \"uint256\"           },           {             \"internalType\": \"uint256\",             \"name\": \"deadline\",             \"type\": \"uint256\"           }         ],         \"internalType\": \"struct Marketplace.MarketItem[]\",         \"name\": \"\",         \"type\": \"tuple[]\"       }     ],     \"stateMutability\": \"view\",     \"type\": \"function\"   },   {     \"inputs\": [       {         \"internalType\": \"address[]\",         \"name\": \"nftAddresses\",         \"type\": \"address[]\"       }     ],     \"name\": \"blacklistNFTContracts\",     \"outputs\": [],     \"stateMutability\": \"nonpayable\",     \"type\": \"function\"   },   {     \"inputs\": [       {         \"internalType\": \"uint256[]\",         \"name\": \"itemIds\",         \"type\": \"uint256[]\"       }     ],     \"name\": \"cancelExpiredListings\",     \"outputs\": [],     \"stateMutability\": \"nonpayable\",     \"type\": \"function\"   },   {     \"inputs\": [       {         \"internalType\": \"uint256\",         \"name\": \"itemId\",         \"type\": \"uint256\"       }     ],     \"name\": \"cancelListing\",     \"outputs\": [],     \"stateMutability\": \"nonpayable\",     \"type\": \"function\"   },   {     \"inputs\": [],     \"name\": \"chainsafeTreasury\",     \"outputs\": [       {         \"internalType\": \"address\",         \"name\": \"\",         \"type\": \"address\"       }     ],     \"stateMutability\": \"view\",     \"type\": \"function\"   },   {     \"inputs\": [       {         \"internalType\": \"address\",         \"name\": \"receiver\",         \"type\": \"address\"       }     ],     \"name\": \"claimFee\",     \"outputs\": [],     \"stateMutability\": \"nonpayable\",     \"type\": \"function\"   },   {     \"inputs\": [       {         \"internalType\": \"bool\",         \"name\": \"isEnable\",         \"type\": \"bool\"       }     ],     \"name\": \"enableWhitelisting\",     \"outputs\": [],     \"stateMutability\": \"nonpayable\",     \"type\": \"function\"   },   {     \"inputs\": [       {         \"internalType\": \"uint256\",         \"name\": \"from\",         \"type\": \"uint256\"       },       {         \"internalType\": \"uint256\",         \"name\": \"to\",         \"type\": \"uint256\"       }     ],     \"name\": \"expiredListingIds\",     \"outputs\": [       {         \"internalType\": \"uint256[]\",         \"name\": \"\",         \"type\": \"uint256[]\"       }     ],     \"stateMutability\": \"view\",     \"type\": \"function\"   },   {     \"inputs\": [       {         \"internalType\": \"address\",         \"name\": \"feeReceiver\",         \"type\": \"address\"       }     ],     \"name\": \"feeCollectedByReceiver\",     \"outputs\": [       {         \"internalType\": \"uint256\",         \"name\": \"\",         \"type\": \"uint256\"       }     ],     \"stateMutability\": \"view\",     \"type\": \"function\"   },   {     \"inputs\": [       {         \"internalType\": \"uint256\",         \"name\": \"id\",         \"type\": \"uint256\"       }     ],     \"name\": \"feeReceiver\",     \"outputs\": [       {         \"internalType\": \"address\",         \"name\": \"feeReceiver\",         \"type\": \"address\"       },       {         \"internalType\": \"uint256\",         \"name\": \"feePercent\",         \"type\": \"uint256\"       }     ],     \"stateMutability\": \"view\",     \"type\": \"function\"   },   {     \"inputs\": [],     \"name\": \"feeReceiversNumber\",     \"outputs\": [       {         \"internalType\": \"uint256\",         \"name\": \"\",         \"type\": \"uint256\"       }     ],     \"stateMutability\": \"view\",     \"type\": \"function\"   },   {     \"inputs\": [       {         \"internalType\": \"bytes32\",         \"name\": \"role\",         \"type\": \"bytes32\"       }     ],     \"name\": \"getRoleAdmin\",     \"outputs\": [       {         \"internalType\": \"bytes32\",         \"name\": \"\",         \"type\": \"bytes32\"       }     ],     \"stateMutability\": \"view\",     \"type\": \"function\"   },   {     \"inputs\": [       {         \"internalType\": \"bytes32\",         \"name\": \"role\",         \"type\": \"bytes32\"       },       {         \"internalType\": \"address\",         \"name\": \"account\",         \"type\": \"address\"       }     ],     \"name\": \"grantRole\",     \"outputs\": [],     \"stateMutability\": \"nonpayable\",     \"type\": \"function\"   },   {     \"inputs\": [       {         \"internalType\": \"bytes32\",         \"name\": \"role\",         \"type\": \"bytes32\"       },       {         \"internalType\": \"address\",         \"name\": \"account\",         \"type\": \"address\"       }     ],     \"name\": \"hasRole\",     \"outputs\": [       {         \"internalType\": \"bool\",         \"name\": \"\",         \"type\": \"bool\"       }     ],     \"stateMutability\": \"view\",     \"type\": \"function\"   },   {     \"inputs\": [       {         \"internalType\": \"string\",         \"name\": \"projectID\",         \"type\": \"string\"       },       {         \"internalType\": \"string\",         \"name\": \"marketplaceID\",         \"type\": \"string\"       },       {         \"internalType\": \"address\",         \"name\": \"creator\",         \"type\": \"address\"       },       {         \"internalType\": \"address\",         \"name\": \"updater\",         \"type\": \"address\"       },       {         \"internalType\": \"address\",         \"name\": \"treasury\",         \"type\": \"address\"       },       {         \"internalType\": \"bool\",         \"name\": \"isWhitelistingEnable\",         \"type\": \"bool\"       },       {         \"internalType\": \"uint256\",         \"name\": \"chainsafeFeePercent\",         \"type\": \"uint256\"       },       {         \"internalType\": \"uint256\",         \"name\": \"maxPercent\",         \"type\": \"uint256\"       }     ],     \"name\": \"initialize\",     \"outputs\": [],     \"stateMutability\": \"nonpayable\",     \"type\": \"function\"   },   {     \"inputs\": [       {         \"internalType\": \"address\",         \"name\": \"token\",         \"type\": \"address\"       }     ],     \"name\": \"isNftToken\",     \"outputs\": [       {         \"internalType\": \"bool\",         \"name\": \"\",         \"type\": \"bool\"       }     ],     \"stateMutability\": \"view\",     \"type\": \"function\"   },   {     \"inputs\": [       {         \"internalType\": \"uint256\",         \"name\": \"itemId\",         \"type\": \"uint256\"       }     ],     \"name\": \"itemById\",     \"outputs\": [       {         \"components\": [           {             \"internalType\": \"address\",             \"name\": \"nftContract\",             \"type\": \"address\"           },           {             \"internalType\": \"uint256\",             \"name\": \"tokenId\",             \"type\": \"uint256\"           },           {             \"internalType\": \"address\",             \"name\": \"seller\",             \"type\": \"address\"           },           {             \"internalType\": \"uint256\",             \"name\": \"price\",             \"type\": \"uint256\"           },           {             \"internalType\": \"uint256\",             \"name\": \"deadline\",             \"type\": \"uint256\"           }         ],         \"internalType\": \"struct Marketplace.MarketItem\",         \"name\": \"\",         \"type\": \"tuple\"       }     ],     \"stateMutability\": \"view\",     \"type\": \"function\"   },   {     \"inputs\": [       {         \"internalType\": \"address\",         \"name\": \"nftContract\",         \"type\": \"address\"       },       {         \"internalType\": \"uint256\",         \"name\": \"tokenId\",         \"type\": \"uint256\"       },       {         \"internalType\": \"uint256\",         \"name\": \"price\",         \"type\": \"uint256\"       },       {         \"internalType\": \"uint256\",         \"name\": \"deadline\",         \"type\": \"uint256\"       }     ],     \"name\": \"listItem\",     \"outputs\": [],     \"stateMutability\": \"nonpayable\",     \"type\": \"function\"   },   {     \"inputs\": [       {         \"internalType\": \"address[]\",         \"name\": \"nftContracts\",         \"type\": \"address[]\"       },       {         \"internalType\": \"uint256[]\",         \"name\": \"tokenIds\",         \"type\": \"uint256[]\"       },       {         \"internalType\": \"uint256[]\",         \"name\": \"amounts\",         \"type\": \"uint256[]\"       },       {         \"internalType\": \"uint256[]\",         \"name\": \"prices\",         \"type\": \"uint256[]\"       },       {         \"internalType\": \"uint256[]\",         \"name\": \"deadlines\",         \"type\": \"uint256[]\"       }     ],     \"name\": \"listItems\",     \"outputs\": [],     \"stateMutability\": \"nonpayable\",     \"type\": \"function\"   },   {     \"inputs\": [],     \"name\": \"marketplaceID\",     \"outputs\": [       {         \"internalType\": \"string\",         \"name\": \"\",         \"type\": \"string\"       }     ],     \"stateMutability\": \"view\",     \"type\": \"function\"   },   {     \"inputs\": [],     \"name\": \"maxFeePercent\",     \"outputs\": [       {         \"internalType\": \"uint256\",         \"name\": \"\",         \"type\": \"uint256\"       }     ],     \"stateMutability\": \"view\",     \"type\": \"function\"   },   {     \"inputs\": [       {         \"internalType\": \"uint256\",         \"name\": \"id\",         \"type\": \"uint256\"       }     ],     \"name\": \"nftToken\",     \"outputs\": [       {         \"internalType\": \"address\",         \"name\": \"token\",         \"type\": \"address\"       }     ],     \"stateMutability\": \"view\",     \"type\": \"function\"   },   {     \"inputs\": [       {         \"internalType\": \"address\",         \"name\": \"operator\",         \"type\": \"address\"       },       {         \"internalType\": \"address\",         \"name\": \"from\",         \"type\": \"address\"       },       {         \"internalType\": \"uint256[]\",         \"name\": \"ids\",         \"type\": \"uint256[]\"       },       {         \"internalType\": \"uint256[]\",         \"name\": \"values\",         \"type\": \"uint256[]\"       },       {         \"internalType\": \"bytes\",         \"name\": \"data\",         \"type\": \"bytes\"       }     ],     \"name\": \"onERC1155BatchReceived\",     \"outputs\": [       {         \"internalType\": \"bytes4\",         \"name\": \"\",         \"type\": \"bytes4\"       }     ],     \"stateMutability\": \"view\",     \"type\": \"function\"   },   {     \"inputs\": [       {         \"internalType\": \"address\",         \"name\": \"operator\",         \"type\": \"address\"       },       {         \"internalType\": \"address\",         \"name\": \"from\",         \"type\": \"address\"       },       {         \"internalType\": \"uint256\",         \"name\": \"id\",         \"type\": \"uint256\"       },       {         \"internalType\": \"uint256\",         \"name\": \"value\",         \"type\": \"uint256\"       },       {         \"internalType\": \"bytes\",         \"name\": \"data\",         \"type\": \"bytes\"       }     ],     \"name\": \"onERC1155Received\",     \"outputs\": [       {         \"internalType\": \"bytes4\",         \"name\": \"\",         \"type\": \"bytes4\"       }     ],     \"stateMutability\": \"view\",     \"type\": \"function\"   },   {     \"inputs\": [       {         \"internalType\": \"address\",         \"name\": \"operator\",         \"type\": \"address\"       },       {         \"internalType\": \"address\",         \"name\": \"from\",         \"type\": \"address\"       },       {         \"internalType\": \"uint256\",         \"name\": \"id\",         \"type\": \"uint256\"       },       {         \"internalType\": \"bytes\",         \"name\": \"data\",         \"type\": \"bytes\"       }     ],     \"name\": \"onERC721Received\",     \"outputs\": [       {         \"internalType\": \"bytes4\",         \"name\": \"\",         \"type\": \"bytes4\"       }     ],     \"stateMutability\": \"view\",     \"type\": \"function\"   },   {     \"inputs\": [],     \"name\": \"projectID\",     \"outputs\": [       {         \"internalType\": \"string\",         \"name\": \"\",         \"type\": \"string\"       }     ],     \"stateMutability\": \"view\",     \"type\": \"function\"   },   {     \"inputs\": [       {         \"internalType\": \"uint256\",         \"name\": \"itemId\",         \"type\": \"uint256\"       }     ],     \"name\": \"purchaseItem\",     \"outputs\": [],     \"stateMutability\": \"payable\",     \"type\": \"function\"   },   {     \"inputs\": [       {         \"internalType\": \"address\",         \"name\": \"feeReceiver\",         \"type\": \"address\"       }     ],     \"name\": \"removeFeeReceiver\",     \"outputs\": [],     \"stateMutability\": \"nonpayable\",     \"type\": \"function\"   },   {     \"inputs\": [       {         \"internalType\": \"bytes32\",         \"name\": \"role\",         \"type\": \"bytes32\"       },       {         \"internalType\": \"address\",         \"name\": \"account\",         \"type\": \"address\"       }     ],     \"name\": \"renounceRole\",     \"outputs\": [],     \"stateMutability\": \"nonpayable\",     \"type\": \"function\"   },   {     \"inputs\": [       {         \"internalType\": \"bytes32\",         \"name\": \"role\",         \"type\": \"bytes32\"       },       {         \"internalType\": \"address\",         \"name\": \"account\",         \"type\": \"address\"       }     ],     \"name\": \"revokeRole\",     \"outputs\": [],     \"stateMutability\": \"nonpayable\",     \"type\": \"function\"   },   {     \"inputs\": [       {         \"internalType\": \"address\",         \"name\": \"feeReceiver\",         \"type\": \"address\"       },       {         \"internalType\": \"uint256\",         \"name\": \"feePercent\",         \"type\": \"uint256\"       }     ],     \"name\": \"setFeeReceiver\",     \"outputs\": [],     \"stateMutability\": \"nonpayable\",     \"type\": \"function\"   },   {     \"inputs\": [       {         \"internalType\": \"uint256\",         \"name\": \"feePercent\",         \"type\": \"uint256\"       }     ],     \"name\": \"setMaxFeePercent\",     \"outputs\": [],     \"stateMutability\": \"nonpayable\",     \"type\": \"function\"   },   {     \"inputs\": [       {         \"internalType\": \"bytes4\",         \"name\": \"interfaceId\",         \"type\": \"bytes4\"       }     ],     \"name\": \"supportsInterface\",     \"outputs\": [       {         \"internalType\": \"bool\",         \"name\": \"\",         \"type\": \"bool\"       }     ],     \"stateMutability\": \"view\",     \"type\": \"function\"   },   {     \"inputs\": [],     \"name\": \"totalFeePercent\",     \"outputs\": [       {         \"internalType\": \"uint256\",         \"name\": \"feePercent\",         \"type\": \"uint256\"       }     ],     \"stateMutability\": \"view\",     \"type\": \"function\"   },   {     \"inputs\": [],     \"name\": \"totalListings\",     \"outputs\": [       {         \"internalType\": \"uint256\",         \"name\": \"\",         \"type\": \"uint256\"       }     ],     \"stateMutability\": \"view\",     \"type\": \"function\"   },   {     \"inputs\": [       {         \"internalType\": \"address\",         \"name\": \"treasury\",         \"type\": \"address\"       },       {         \"internalType\": \"uint256\",         \"name\": \"feePercent\",         \"type\": \"uint256\"       }     ],     \"name\": \"updateChainSafeTreasury\",     \"outputs\": [],     \"stateMutability\": \"nonpayable\",     \"type\": \"function\"   },   {     \"inputs\": [       {         \"internalType\": \"address\",         \"name\": \"user\",         \"type\": \"address\"       }     ],     \"name\": \"usersListingIds\",     \"outputs\": [       {         \"internalType\": \"uint256[]\",         \"name\": \"\",         \"type\": \"uint256[]\"       }     ],     \"stateMutability\": \"view\",     \"type\": \"function\"   },   {     \"inputs\": [       {         \"internalType\": \"address\",         \"name\": \"user\",         \"type\": \"address\"       }     ],     \"name\": \"usersListings\",     \"outputs\": [       {         \"components\": [           {             \"internalType\": \"address\",             \"name\": \"nftContract\",             \"type\": \"address\"           },           {             \"internalType\": \"uint256\",             \"name\": \"tokenId\",             \"type\": \"uint256\"           },           {             \"internalType\": \"address\",             \"name\": \"seller\",             \"type\": \"address\"           },           {             \"internalType\": \"uint256\",             \"name\": \"price\",             \"type\": \"uint256\"           },           {             \"internalType\": \"uint256\",             \"name\": \"deadline\",             \"type\": \"uint256\"           }         ],         \"internalType\": \"struct Marketplace.MarketItem[]\",         \"name\": \"\",         \"type\": \"tuple[]\"       }     ],     \"stateMutability\": \"view\",     \"type\": \"function\"   },   {     \"inputs\": [       {         \"internalType\": \"address[]\",         \"name\": \"nftAddresses\",         \"type\": \"address[]\"       }     ],     \"name\": \"whitelistNFTContracts\",     \"outputs\": [],     \"stateMutability\": \"nonpayable\",     \"type\": \"function\"   },   {     \"inputs\": [],     \"name\": \"whitelistingEnable\",     \"outputs\": [       {         \"internalType\": \"bool\",         \"name\": \"\",         \"type\": \"bool\"       }     ],     \"stateMutability\": \"view\",     \"type\": \"function\"   },   {     \"stateMutability\": \"payable\",     \"type\": \"receive\"   } ]";

        public string ContractAddress { get; set; }

        public IContractBuilder ContractBuilder { get; set; }

        public Contract OriginalContract { get; set; }


        public async Task<byte[]> CREATOR_ROLE()
        {
            var response = await OriginalContract.Call("CREATOR_ROLE", new object[]
            {
            });

            return (byte[])response[0];
        }

        public async Task<byte[]> DEFAULT_ADMIN_ROLE()
        {
            var response = await OriginalContract.Call("DEFAULT_ADMIN_ROLE", new object[]
            {
            });

            return (byte[])response[0];
        }

        public async Task<byte[]> UPDATER_ROLE()
        {
            var response = await OriginalContract.Call("UPDATER_ROLE", new object[]
            {
            });

            return (byte[])response[0];
        }

        public async Task<(BigInteger feePercent, BigInteger feeCollected)> FeeReceiverDetails(string address)
        {
            var response = await OriginalContract.Call("_feeReceiverDetails", new object[]
            {
            });

            return ((BigInteger)response[0], (BigInteger)response[1]);
        }

        public async Task<object> ActiveItems()
        {
            var response = await OriginalContract.Call("activeItems", new object[]
            {
            });

            return (object)response[0];
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
            var response = await OriginalContract.Call("chainsafeTreasury", new object[]
            {
            });

            return (string)response[0];
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
            var response = await OriginalContract.Call("expiredListingIds", new object[]
            {
                from, to
            });

            return (BigInteger[])response[0];
        }

        public async Task<BigInteger> FeeCollectedByReceiver(string feeReceiver)
        {
            var response = await OriginalContract.Call("feeCollectedByReceiver", new object[]
            {
                feeReceiver
            });

            return (BigInteger)response[0];
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
            var response = await OriginalContract.Call("feeReceiversNumber", new object[]
            {
            });

            return (BigInteger)response[0];
        }

        public async Task<byte[]> GetRoleAdmin(byte[] role)
        {
            var response = await OriginalContract.Call("getRoleAdmin", new object[]
            {
                role
            });

            return (byte[])response[0];
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
            var response = await OriginalContract.Call("hasRole", new object[]
            {
                role, account
            });

            return (bool)response[0];
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
            var response = await OriginalContract.Call("isNftToken", new object[]
            {
                token
            });

            return (bool)response[0];
        }

        public async Task<object> ItemById(BigInteger itemId)
        {
            var response = await OriginalContract.Call("itemById", new object[]
            {
                itemId
            });

            return (object)response[0];
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
            var response = await OriginalContract.Call("marketplaceID", new object[]
            {
            });

            return (string)response[0];
        }

        public async Task<BigInteger> MaxFeePercent()
        {
            var response = await OriginalContract.Call("maxFeePercent", new object[]
            {
            });

            return (BigInteger)response[0];
        }

        public async Task<string> NftToken(BigInteger id)
        {
            var response = await OriginalContract.Call("nftToken", new object[]
            {
                id
            });

            return (string)response[0];
        }

        public async Task<byte[]> OnERC1155BatchReceived(string @operator, string from, BigInteger[] ids,
            BigInteger[] values, byte[] data)
        {
            var response = await OriginalContract.Call("onERC1155BatchReceived", new object[]
            {
                @operator, from, ids, values, data
            });

            return (byte[])response[0];
        }

        public async Task<byte[]> OnERC1155Received(string @operator, string from, BigInteger id, BigInteger value,
            byte[] data)
        {
            var response = await OriginalContract.Call("onERC1155Received", new object[]
            {
                @operator, from, id, value, data
            });

            return (byte[])response[0];
        }

        public async Task<byte[]> OnERC721Received(string @operator, string from, BigInteger id, byte[] data)
        {
            var response = await OriginalContract.Call("onERC721Received", new object[]
            {
                @operator, from, id, data
            });

            return (byte[])response[0];
        }

        public async Task<string> ProjectID()
        {
            var response = await OriginalContract.Call("projectID", new object[]
            {
            });

            return (string)response[0];
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
            var response = await OriginalContract.Call("supportsInterface", new object[]
            {
                interfaceId
            });

            return (bool)response[0];
        }

        public async Task<BigInteger> TotalFeePercent()
        {
            var response = await OriginalContract.Call("totalFeePercent", new object[]
            {
            });

            return (BigInteger)response[0];
        }

        public async Task<BigInteger> TotalListings()
        {
            var response = await OriginalContract.Call("totalListings", new object[]
            {
            });

            return (BigInteger)response[0];
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
            var response = await OriginalContract.Call("usersListingIds", new object[]
            {
                user
            });

            return (BigInteger[])response[0];
        }

        public async Task<object> UsersListings(string user)
        {
            var response = await OriginalContract.Call("usersListings", new object[]
            {
                user
            });

            return (object)response[0];
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
            var response = await OriginalContract.Call("whitelistingEnable", new object[]
            {
            });

            return (bool)response[0];
        }


        #region Interface Implemented Methods

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
            TransactionRequest overwrite = null)
        {
            return OriginalContract.PrepareTransactionRequest(method, parameters, overwrite);
        }

        #endregion
    }
}