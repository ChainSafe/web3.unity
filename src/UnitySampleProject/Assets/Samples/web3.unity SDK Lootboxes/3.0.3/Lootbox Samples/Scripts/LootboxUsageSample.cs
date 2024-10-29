using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;
using ChainSafe.Gaming.Evm.Transactions;
using Nethereum.Hex.HexTypes;
using Nethereum.ABI.FunctionEncoding.Attributes;
using UnityEngine;
using ChainSafe.Gaming.RPC.Events;



namespace ChainSafe.Gaming.Evm.Contracts.Custom
{
    public partial class LootboxUsageSample : ICustomContract
    {
        public string Address => OriginalContract.Address;
       
        public string ABI => "[     {         \"inputs\": [],         \"name\": \"AcceptingOnlyLINK\",         \"type\": \"error\"     },     {         \"inputs\": [             {                 \"internalType\": \"bytes32\",                 \"name\": \"role\",                 \"type\": \"bytes32\"             }         ],         \"name\": \"AccessDenied\",         \"type\": \"error\"     },     {         \"inputs\": [             {                 \"internalType\": \"uint256\",                 \"name\": \"value\",                 \"type\": \"uint256\"             }         ],         \"name\": \"AmountPerUnitOverflow\",         \"type\": \"error\"     },     {         \"inputs\": [             {                 \"internalType\": \"address\",                 \"name\": \"token\",                 \"type\": \"address\"             },             {                 \"internalType\": \"uint256\",                 \"name\": \"tokenId\",                 \"type\": \"uint256\"             }         ],         \"name\": \"DepositStateCorruption\",         \"type\": \"error\"     },     {         \"inputs\": [],         \"name\": \"EndOfService\",         \"type\": \"error\"     },     {         \"inputs\": [],         \"name\": \"InsufficientFee\",         \"type\": \"error\"     },     {         \"inputs\": [],         \"name\": \"InsufficientGas\",         \"type\": \"error\"     },     {         \"inputs\": [],         \"name\": \"InsufficientPayment\",         \"type\": \"error\"     },     {         \"inputs\": [             {                 \"internalType\": \"uint256\",                 \"name\": \"supply\",                 \"type\": \"uint256\"             },             {                 \"internalType\": \"uint256\",                 \"name\": \"requested\",                 \"type\": \"uint256\"             }         ],         \"name\": \"InsufficientSupply\",         \"type\": \"error\"     },     {         \"inputs\": [],         \"name\": \"InvalidLength\",         \"type\": \"error\"     },     {         \"inputs\": [             {                 \"internalType\": \"int256\",                 \"name\": \"value\",                 \"type\": \"int256\"             }         ],         \"name\": \"InvalidLinkPrice\",         \"type\": \"error\"     },     {         \"inputs\": [],         \"name\": \"InvalidLootboxType\",         \"type\": \"error\"     },     {         \"inputs\": [             {                 \"internalType\": \"uint256\",                 \"name\": \"requestId\",                 \"type\": \"uint256\"             }         ],         \"name\": \"InvalidRequestAllocation\",         \"type\": \"error\"     },     {         \"inputs\": [],         \"name\": \"InvalidTokenAmount\",         \"type\": \"error\"     },     {         \"inputs\": [             {                 \"internalType\": \"address\",                 \"name\": \"token\",                 \"type\": \"address\"             }         ],         \"name\": \"InventoryStateCorruption\",         \"type\": \"error\"     },     {         \"inputs\": [             {                 \"internalType\": \"enum LootboxInterface.RewardType\",                 \"name\": \"oldType\",                 \"type\": \"uint8\"             },             {                 \"internalType\": \"enum LootboxInterface.RewardType\",                 \"name\": \"newType\",                 \"type\": \"uint8\"             }         ],         \"name\": \"ModifiedRewardType\",         \"type\": \"error\"     },     {         \"inputs\": [],         \"name\": \"NoTokens\",         \"type\": \"error\"     },     {         \"inputs\": [],         \"name\": \"NothingToClaim\",         \"type\": \"error\"     },     {         \"inputs\": [],         \"name\": \"NothingToRecover\",         \"type\": \"error\"     },     {         \"inputs\": [],         \"name\": \"OnlyThis\",         \"type\": \"error\"     },     {         \"inputs\": [             {                 \"internalType\": \"uint256\",                 \"name\": \"requestId\",                 \"type\": \"uint256\"             }         ],         \"name\": \"PendingOpenRequest\",         \"type\": \"error\"     },     {         \"inputs\": [             {                 \"internalType\": \"address\",                 \"name\": \"token\",                 \"type\": \"address\"             }         ],         \"name\": \"RewardWithdrawalDenied\",         \"type\": \"error\"     },     {         \"inputs\": [             {                 \"internalType\": \"address\",                 \"name\": \"from\",                 \"type\": \"address\"             }         ],         \"name\": \"SupplyDenied\",         \"type\": \"error\"     },     {         \"inputs\": [             {                 \"internalType\": \"uint256\",                 \"name\": \"supply\",                 \"type\": \"uint256\"             },             {                 \"internalType\": \"uint256\",                 \"name\": \"unitsToGet\",                 \"type\": \"uint256\"             }         ],         \"name\": \"SupplyExceeded\",         \"type\": \"error\"     },     {         \"inputs\": [             {                 \"internalType\": \"address\",                 \"name\": \"token\",                 \"type\": \"address\"             }         ],         \"name\": \"TokenDenied\",         \"type\": \"error\"     },     {         \"inputs\": [             {                 \"internalType\": \"uint256\",                 \"name\": \"currentPrice\",                 \"type\": \"uint256\"             }         ],         \"name\": \"UnexpectedPrice\",         \"type\": \"error\"     },     {         \"inputs\": [             {                 \"internalType\": \"enum LootboxInterface.RewardType\",                 \"name\": \"rewardType\",                 \"type\": \"uint8\"             }         ],         \"name\": \"UnexpectedRewardType\",         \"type\": \"error\"     },     {         \"inputs\": [             {                 \"internalType\": \"uint256\",                 \"name\": \"value\",                 \"type\": \"uint256\"             }         ],         \"name\": \"UnitsOverflow\",         \"type\": \"error\"     },     {         \"inputs\": [],         \"name\": \"Unsupported\",         \"type\": \"error\"     },     {         \"inputs\": [],         \"name\": \"ViewCallFailed\",         \"type\": \"error\"     },     {         \"inputs\": [],         \"name\": \"ZeroAmount\",         \"type\": \"error\"     },     {         \"inputs\": [             {                 \"internalType\": \"address\",                 \"name\": \"token\",                 \"type\": \"address\"             },             {                 \"internalType\": \"uint256\",                 \"name\": \"id\",                 \"type\": \"uint256\"             }         ],         \"name\": \"ZeroSupply\",         \"type\": \"error\"     },     {         \"anonymous\": false,         \"inputs\": [             {                 \"indexed\": false,                 \"internalType\": \"address\",                 \"name\": \"opener\",                 \"type\": \"address\"             },             {                 \"indexed\": false,                 \"internalType\": \"address\",                 \"name\": \"token\",                 \"type\": \"address\"             },             {                 \"indexed\": false,                 \"internalType\": \"uint256\",                 \"name\": \"tokenId\",                 \"type\": \"uint256\"             },             {                 \"indexed\": false,                 \"internalType\": \"uint256\",                 \"name\": \"amount\",                 \"type\": \"uint256\"             }         ],         \"name\": \"Allocated\",         \"type\": \"event\"     },     {         \"anonymous\": false,         \"inputs\": [             {                 \"indexed\": false,                 \"internalType\": \"address\",                 \"name\": \"token\",                 \"type\": \"address\"             },             {                 \"indexed\": false,                 \"internalType\": \"uint256\",                 \"name\": \"tokenId\",                 \"type\": \"uint256\"             },             {                 \"indexed\": false,                 \"internalType\": \"uint256\",                 \"name\": \"amountPerUnit\",                 \"type\": \"uint256\"             },             {                 \"indexed\": false,                 \"internalType\": \"uint256\",                 \"name\": \"newSupply\",                 \"type\": \"uint256\"             }         ],         \"name\": \"AmountPerUnitSet\",         \"type\": \"event\"     },     {         \"anonymous\": false,         \"inputs\": [             {                 \"indexed\": true,                 \"internalType\": \"address\",                 \"name\": \"account\",                 \"type\": \"address\"             },             {                 \"indexed\": true,                 \"internalType\": \"address\",                 \"name\": \"operator\",                 \"type\": \"address\"             },             {                 \"indexed\": false,                 \"internalType\": \"bool\",                 \"name\": \"approved\",                 \"type\": \"bool\"             }         ],         \"name\": \"ApprovalForAll\",         \"type\": \"event\"     },     {         \"anonymous\": false,         \"inputs\": [             {                 \"indexed\": false,                 \"internalType\": \"address\",                 \"name\": \"opener\",                 \"type\": \"address\"             },             {                 \"indexed\": false,                 \"internalType\": \"uint256\",                 \"name\": \"requestId\",                 \"type\": \"uint256\"             }         ],         \"name\": \"BoxesRecovered\",         \"type\": \"event\"     },     {         \"anonymous\": false,         \"inputs\": [             {                 \"indexed\": false,                 \"internalType\": \"address\",                 \"name\": \"caller\",                 \"type\": \"address\"             }         ],         \"name\": \"EmergencyModeEnabled\",         \"type\": \"event\"     },     {         \"anonymous\": false,         \"inputs\": [             {                 \"indexed\": false,                 \"internalType\": \"address\",                 \"name\": \"token\",                 \"type\": \"address\"             },             {                 \"indexed\": false,                 \"internalType\": \"enum LootboxInterface.RewardType\",                 \"name\": \"tokenType\",                 \"type\": \"uint8\"             },             {                 \"indexed\": false,                 \"internalType\": \"address\",                 \"name\": \"to\",                 \"type\": \"address\"             },             {                 \"indexed\": false,                 \"internalType\": \"uint256[]\",                 \"name\": \"ids\",                 \"type\": \"uint256[]\"             },             {                 \"indexed\": false,                 \"internalType\": \"uint256[]\",                 \"name\": \"amounts\",                 \"type\": \"uint256[]\"             }         ],         \"name\": \"EmergencyWithdrawal\",         \"type\": \"event\"     },     {         \"anonymous\": false,         \"inputs\": [             {                 \"indexed\": false,                 \"internalType\": \"uint256\",                 \"name\": \"requestId\",                 \"type\": \"uint256\"             },             {                 \"indexed\": false,                 \"internalType\": \"bytes\",                 \"name\": \"reason\",                 \"type\": \"bytes\"             }         ],         \"name\": \"OpenRequestFailed\",         \"type\": \"event\"     },     {         \"anonymous\": false,         \"inputs\": [             {                 \"indexed\": false,                 \"internalType\": \"uint256\",                 \"name\": \"requestId\",                 \"type\": \"uint256\"             },             {                 \"indexed\": false,                 \"internalType\": \"uint256\",                 \"name\": \"randomness\",                 \"type\": \"uint256\"             }         ],         \"name\": \"OpenRequestFulfilled\",         \"type\": \"event\"     },     {         \"anonymous\": false,         \"inputs\": [             {                 \"indexed\": false,                 \"internalType\": \"address\",                 \"name\": \"opener\",                 \"type\": \"address\"             },             {                 \"indexed\": false,                 \"internalType\": \"uint256\",                 \"name\": \"unitsToGet\",                 \"type\": \"uint256\"             },             {                 \"indexed\": false,                 \"internalType\": \"uint256\",                 \"name\": \"requestId\",                 \"type\": \"uint256\"             }         ],         \"name\": \"OpenRequested\",         \"type\": \"event\"     },     {         \"anonymous\": false,         \"inputs\": [             {                 \"indexed\": false,                 \"internalType\": \"address\",                 \"name\": \"account\",                 \"type\": \"address\"             }         ],         \"name\": \"Paused\",         \"type\": \"event\"     },     {         \"anonymous\": false,         \"inputs\": [             {                 \"indexed\": false,                 \"internalType\": \"uint256\",                 \"name\": \"newPrice\",                 \"type\": \"uint256\"             }         ],         \"name\": \"PriceUpdated\",         \"type\": \"event\"     },     {         \"anonymous\": false,         \"inputs\": [             {                 \"indexed\": false,                 \"internalType\": \"address\",                 \"name\": \"opener\",                 \"type\": \"address\"             },             {                 \"indexed\": false,                 \"internalType\": \"address\",                 \"name\": \"token\",                 \"type\": \"address\"             },             {                 \"indexed\": false,                 \"internalType\": \"uint256\",                 \"name\": \"tokenId\",                 \"type\": \"uint256\"             },             {                 \"indexed\": false,                 \"internalType\": \"uint256\",                 \"name\": \"amount\",                 \"type\": \"uint256\"             }         ],         \"name\": \"RewardsClaimed\",         \"type\": \"event\"     },     {         \"anonymous\": false,         \"inputs\": [             {                 \"indexed\": true,                 \"internalType\": \"bytes32\",                 \"name\": \"role\",                 \"type\": \"bytes32\"             },             {                 \"indexed\": true,                 \"internalType\": \"bytes32\",                 \"name\": \"previousAdminRole\",                 \"type\": \"bytes32\"             },             {                 \"indexed\": true,                 \"internalType\": \"bytes32\",                 \"name\": \"newAdminRole\",                 \"type\": \"bytes32\"             }         ],         \"name\": \"RoleAdminChanged\",         \"type\": \"event\"     },     {         \"anonymous\": false,         \"inputs\": [             {                 \"indexed\": true,                 \"internalType\": \"bytes32\",                 \"name\": \"role\",                 \"type\": \"bytes32\"             },             {                 \"indexed\": true,                 \"internalType\": \"address\",                 \"name\": \"account\",                 \"type\": \"address\"             },             {                 \"indexed\": true,                 \"internalType\": \"address\",                 \"name\": \"sender\",                 \"type\": \"address\"             }         ],         \"name\": \"RoleGranted\",         \"type\": \"event\"     },     {         \"anonymous\": false,         \"inputs\": [             {                 \"indexed\": true,                 \"internalType\": \"bytes32\",                 \"name\": \"role\",                 \"type\": \"bytes32\"             },             {                 \"indexed\": true,                 \"internalType\": \"address\",                 \"name\": \"account\",                 \"type\": \"address\"             },             {                 \"indexed\": true,                 \"internalType\": \"address\",                 \"name\": \"sender\",                 \"type\": \"address\"             }         ],         \"name\": \"RoleRevoked\",         \"type\": \"event\"     },     {         \"anonymous\": false,         \"inputs\": [             {                 \"indexed\": false,                 \"internalType\": \"address\",                 \"name\": \"buyer\",                 \"type\": \"address\"             },             {                 \"indexed\": false,                 \"internalType\": \"uint256\",                 \"name\": \"amount\",                 \"type\": \"uint256\"             },             {                 \"indexed\": false,                 \"internalType\": \"uint256\",                 \"name\": \"payment\",                 \"type\": \"uint256\"             }         ],         \"name\": \"Sold\",         \"type\": \"event\"     },     {         \"anonymous\": false,         \"inputs\": [             {                 \"indexed\": false,                 \"internalType\": \"address\",                 \"name\": \"supplier\",                 \"type\": \"address\"             }         ],         \"name\": \"SupplierAdded\",         \"type\": \"event\"     },     {         \"anonymous\": false,         \"inputs\": [             {                 \"indexed\": false,                 \"internalType\": \"address\",                 \"name\": \"supplier\",                 \"type\": \"address\"             }         ],         \"name\": \"SupplierRemoved\",         \"type\": \"event\"     },     {         \"anonymous\": false,         \"inputs\": [             {                 \"indexed\": false,                 \"internalType\": \"address\",                 \"name\": \"token\",                 \"type\": \"address\"             }         ],         \"name\": \"TokenAdded\",         \"type\": \"event\"     },     {         \"anonymous\": false,         \"inputs\": [             {                 \"indexed\": true,                 \"internalType\": \"address\",                 \"name\": \"operator\",                 \"type\": \"address\"             },             {                 \"indexed\": true,                 \"internalType\": \"address\",                 \"name\": \"from\",                 \"type\": \"address\"             },             {                 \"indexed\": true,                 \"internalType\": \"address\",                 \"name\": \"to\",                 \"type\": \"address\"             },             {                 \"indexed\": false,                 \"internalType\": \"uint256[]\",                 \"name\": \"ids\",                 \"type\": \"uint256[]\"             },             {                 \"indexed\": false,                 \"internalType\": \"uint256[]\",                 \"name\": \"values\",                 \"type\": \"uint256[]\"             }         ],         \"name\": \"TransferBatch\",         \"type\": \"event\"     },     {         \"anonymous\": false,         \"inputs\": [             {                 \"indexed\": true,                 \"internalType\": \"address\",                 \"name\": \"operator\",                 \"type\": \"address\"             },             {                 \"indexed\": true,                 \"internalType\": \"address\",                 \"name\": \"from\",                 \"type\": \"address\"             },             {                 \"indexed\": true,                 \"internalType\": \"address\",                 \"name\": \"to\",                 \"type\": \"address\"             },             {                 \"indexed\": false,                 \"internalType\": \"uint256\",                 \"name\": \"id\",                 \"type\": \"uint256\"             },             {                 \"indexed\": false,                 \"internalType\": \"uint256\",                 \"name\": \"value\",                 \"type\": \"uint256\"             }         ],         \"name\": \"TransferSingle\",         \"type\": \"event\"     },     {         \"anonymous\": false,         \"inputs\": [             {                 \"indexed\": false,                 \"internalType\": \"string\",                 \"name\": \"value\",                 \"type\": \"string\"             },             {                 \"indexed\": true,                 \"internalType\": \"uint256\",                 \"name\": \"id\",                 \"type\": \"uint256\"             }         ],         \"name\": \"URI\",         \"type\": \"event\"     },     {         \"anonymous\": false,         \"inputs\": [             {                 \"indexed\": false,                 \"internalType\": \"address\",                 \"name\": \"account\",                 \"type\": \"address\"             }         ],         \"name\": \"Unpaused\",         \"type\": \"event\"     },     {         \"anonymous\": false,         \"inputs\": [             {                 \"indexed\": false,                 \"internalType\": \"address\",                 \"name\": \"token\",                 \"type\": \"address\"             },             {                 \"indexed\": false,                 \"internalType\": \"address\",                 \"name\": \"to\",                 \"type\": \"address\"             },             {                 \"indexed\": false,                 \"internalType\": \"uint256\",                 \"name\": \"amount\",                 \"type\": \"uint256\"             }         ],         \"name\": \"Withdraw\",         \"type\": \"event\"     },     {         \"inputs\": [],         \"name\": \"DEFAULT_ADMIN_ROLE\",         \"outputs\": [             {                 \"internalType\": \"bytes32\",                 \"name\": \"\",                 \"type\": \"bytes32\"             }         ],         \"stateMutability\": \"view\",         \"type\": \"function\"     },     {         \"inputs\": [],         \"name\": \"FACTORY\",         \"outputs\": [             {                 \"internalType\": \"contract ILootboxFactory\",                 \"name\": \"\",                 \"type\": \"address\"             }         ],         \"stateMutability\": \"view\",         \"type\": \"function\"     },     {         \"inputs\": [],         \"name\": \"LINK_ETH_FEED\",         \"outputs\": [             {                 \"internalType\": \"contract AggregatorV3Interface\",                 \"name\": \"\",                 \"type\": \"address\"             }         ],         \"stateMutability\": \"view\",         \"type\": \"function\"     },     {         \"inputs\": [],         \"name\": \"MINTER_ROLE\",         \"outputs\": [             {                 \"internalType\": \"bytes32\",                 \"name\": \"\",                 \"type\": \"bytes32\"             }         ],         \"stateMutability\": \"view\",         \"type\": \"function\"     },     {         \"inputs\": [],         \"name\": \"PAUSER_ROLE\",         \"outputs\": [             {                 \"internalType\": \"bytes32\",                 \"name\": \"\",                 \"type\": \"bytes32\"             }         ],         \"stateMutability\": \"view\",         \"type\": \"function\"     },     {         \"inputs\": [             {                 \"internalType\": \"uint256\",                 \"name\": \"_requestId\",                 \"type\": \"uint256\"             },             {                 \"internalType\": \"uint256\",                 \"name\": \"_randomness\",                 \"type\": \"uint256\"             }         ],         \"name\": \"_allocateRewards\",         \"outputs\": [],         \"stateMutability\": \"nonpayable\",         \"type\": \"function\"     },     {         \"inputs\": [             {                 \"internalType\": \"address[]\",                 \"name\": \"_suppliers\",                 \"type\": \"address[]\"             }         ],         \"name\": \"addSuppliers\",         \"outputs\": [],         \"stateMutability\": \"nonpayable\",         \"type\": \"function\"     },     {         \"inputs\": [             {                 \"internalType\": \"address[]\",                 \"name\": \"_tokens\",                 \"type\": \"address[]\"             }         ],         \"name\": \"addTokens\",         \"outputs\": [],         \"stateMutability\": \"nonpayable\",         \"type\": \"function\"     },     {         \"inputs\": [             {                 \"internalType\": \"address\",                 \"name\": \"account\",                 \"type\": \"address\"             },             {                 \"internalType\": \"uint256\",                 \"name\": \"id\",                 \"type\": \"uint256\"             }         ],         \"name\": \"balanceOf\",         \"outputs\": [             {                 \"internalType\": \"uint256\",                 \"name\": \"\",                 \"type\": \"uint256\"             }         ],         \"stateMutability\": \"view\",         \"type\": \"function\"     },     {         \"inputs\": [             {                 \"internalType\": \"address[]\",                 \"name\": \"accounts\",                 \"type\": \"address[]\"             },             {                 \"internalType\": \"uint256[]\",                 \"name\": \"ids\",                 \"type\": \"uint256[]\"             }         ],         \"name\": \"balanceOfBatch\",         \"outputs\": [             {                 \"internalType\": \"uint256[]\",                 \"name\": \"\",                 \"type\": \"uint256[]\"             }         ],         \"stateMutability\": \"view\",         \"type\": \"function\"     },     {         \"inputs\": [             {                 \"internalType\": \"address\",                 \"name\": \"account\",                 \"type\": \"address\"             },             {                 \"internalType\": \"uint256\",                 \"name\": \"id\",                 \"type\": \"uint256\"             },             {                 \"internalType\": \"uint256\",                 \"name\": \"value\",                 \"type\": \"uint256\"             }         ],         \"name\": \"burn\",         \"outputs\": [],         \"stateMutability\": \"nonpayable\",         \"type\": \"function\"     },     {         \"inputs\": [             {                 \"internalType\": \"address\",                 \"name\": \"account\",                 \"type\": \"address\"             },             {                 \"internalType\": \"uint256[]\",                 \"name\": \"ids\",                 \"type\": \"uint256[]\"             },             {                 \"internalType\": \"uint256[]\",                 \"name\": \"values\",                 \"type\": \"uint256[]\"             }         ],         \"name\": \"burnBatch\",         \"outputs\": [],         \"stateMutability\": \"nonpayable\",         \"type\": \"function\"     },     {         \"inputs\": [             {                 \"internalType\": \"uint256\",                 \"name\": \"_amount\",                 \"type\": \"uint256\"             },             {                 \"internalType\": \"uint256\",                 \"name\": \"_maxPrice\",                 \"type\": \"uint256\"             }         ],         \"name\": \"buy\",         \"outputs\": [],         \"stateMutability\": \"payable\",         \"type\": \"function\"     },     {         \"inputs\": [             {                 \"internalType\": \"uint32\",                 \"name\": \"_gas\",                 \"type\": \"uint32\"             },             {                 \"internalType\": \"uint256\",                 \"name\": \"_gasPriceInWei\",                 \"type\": \"uint256\"             },             {                 \"internalType\": \"uint256\",                 \"name\": \"_units\",                 \"type\": \"uint256\"             }         ],         \"name\": \"calculateOpenPrice\",         \"outputs\": [             {                 \"internalType\": \"uint256\",                 \"name\": \"\",                 \"type\": \"uint256\"             }         ],         \"stateMutability\": \"view\",         \"type\": \"function\"     },     {         \"inputs\": [             {                 \"internalType\": \"address\",                 \"name\": \"_opener\",                 \"type\": \"address\"             }         ],         \"name\": \"canClaimRewards\",         \"outputs\": [             {                 \"internalType\": \"bool\",                 \"name\": \"\",                 \"type\": \"bool\"             }         ],         \"stateMutability\": \"view\",         \"type\": \"function\"     },     {         \"inputs\": [             {                 \"internalType\": \"address\",                 \"name\": \"_opener\",                 \"type\": \"address\"             }         ],         \"name\": \"claimRewards\",         \"outputs\": [],         \"stateMutability\": \"nonpayable\",         \"type\": \"function\"     },     {         \"inputs\": [             {                 \"internalType\": \"address\",                 \"name\": \"_token\",                 \"type\": \"address\"             },             {                 \"internalType\": \"enum LootboxInterface.RewardType\",                 \"name\": \"_type\",                 \"type\": \"uint8\"             },             {                 \"internalType\": \"address\",                 \"name\": \"_to\",                 \"type\": \"address\"             },             {                 \"internalType\": \"uint256[]\",                 \"name\": \"_ids\",                 \"type\": \"uint256[]\"             },             {                 \"internalType\": \"uint256[]\",                 \"name\": \"_amounts\",                 \"type\": \"uint256[]\"             }         ],         \"name\": \"emergencyWithdraw\",         \"outputs\": [],         \"stateMutability\": \"nonpayable\",         \"type\": \"function\"     },     {         \"inputs\": [],         \"name\": \"getAllowedTokenTypes\",         \"outputs\": [             {                 \"internalType\": \"enum LootboxInterface.RewardType[]\",                 \"name\": \"result\",                 \"type\": \"uint8[]\"             }         ],         \"stateMutability\": \"view\",         \"type\": \"function\"     },     {         \"inputs\": [],         \"name\": \"getAllowedTokens\",         \"outputs\": [             {                 \"internalType\": \"address[]\",                 \"name\": \"\",                 \"type\": \"address[]\"             }         ],         \"stateMutability\": \"view\",         \"type\": \"function\"     },     {         \"inputs\": [],         \"name\": \"getAvailableSupply\",         \"outputs\": [             {                 \"internalType\": \"uint256\",                 \"name\": \"\",                 \"type\": \"uint256\"             }         ],         \"stateMutability\": \"view\",         \"type\": \"function\"     },     {         \"inputs\": [],         \"name\": \"getInventory\",         \"outputs\": [             {                 \"components\": [                     {                         \"internalType\": \"address\",                         \"name\": \"rewardToken\",                         \"type\": \"address\"                     },                     {                         \"internalType\": \"enum LootboxInterface.RewardType\",                         \"name\": \"rewardType\",                         \"type\": \"uint8\"                     },                     {                         \"internalType\": \"uint256\",                         \"name\": \"units\",                         \"type\": \"uint256\"                     },                     {                         \"internalType\": \"uint256\",                         \"name\": \"amountPerUnit\",                         \"type\": \"uint256\"                     },                     {                         \"internalType\": \"uint256\",                         \"name\": \"balance\",                         \"type\": \"uint256\"                     },                     {                         \"components\": [                             {                                 \"internalType\": \"uint256\",                                 \"name\": \"id\",                                 \"type\": \"uint256\"                             },                             {                                 \"internalType\": \"uint256\",                                 \"name\": \"units\",                                 \"type\": \"uint256\"                             },                             {                                 \"internalType\": \"uint256\",                                 \"name\": \"amountPerUnit\",                                 \"type\": \"uint256\"                             },                             {                                 \"internalType\": \"uint256\",                                 \"name\": \"balance\",                                 \"type\": \"uint256\"                             }                         ],                         \"internalType\": \"struct LootboxInterface.ExtraRewardInfo[]\",                         \"name\": \"extra\",                         \"type\": \"tuple[]\"                     }                 ],                 \"internalType\": \"struct LootboxInterface.RewardView[]\",                 \"name\": \"result\",                 \"type\": \"tuple[]\"             },             {                 \"components\": [                     {                         \"internalType\": \"address\",                         \"name\": \"rewardToken\",                         \"type\": \"address\"                     },                     {                         \"internalType\": \"enum LootboxInterface.RewardType\",                         \"name\": \"rewardType\",                         \"type\": \"uint8\"                     },                     {                         \"internalType\": \"uint256\",                         \"name\": \"units\",                         \"type\": \"uint256\"                     },                     {                         \"internalType\": \"uint256\",                         \"name\": \"amountPerUnit\",                         \"type\": \"uint256\"                     },                     {                         \"internalType\": \"uint256\",                         \"name\": \"balance\",                         \"type\": \"uint256\"                     },                     {                         \"components\": [                             {                                 \"internalType\": \"uint256\",                                 \"name\": \"id\",                                 \"type\": \"uint256\"                             },                             {                                 \"internalType\": \"uint256\",                                 \"name\": \"units\",                                 \"type\": \"uint256\"                             },                             {                                 \"internalType\": \"uint256\",                                 \"name\": \"amountPerUnit\",                                 \"type\": \"uint256\"                             },                             {                                 \"internalType\": \"uint256\",                                 \"name\": \"balance\",                                 \"type\": \"uint256\"                             }                         ],                         \"internalType\": \"struct LootboxInterface.ExtraRewardInfo[]\",                         \"name\": \"extra\",                         \"type\": \"tuple[]\"                     }                 ],                 \"internalType\": \"struct LootboxInterface.RewardView[]\",                 \"name\": \"leftoversResult\",                 \"type\": \"tuple[]\"             }         ],         \"stateMutability\": \"view\",         \"type\": \"function\"     },     {         \"inputs\": [],         \"name\": \"getLink\",         \"outputs\": [             {                 \"internalType\": \"address\",                 \"name\": \"\",                 \"type\": \"address\"             }         ],         \"stateMutability\": \"view\",         \"type\": \"function\"     },     {         \"inputs\": [],         \"name\": \"getLinkPrice\",         \"outputs\": [             {                 \"internalType\": \"uint256\",                 \"name\": \"\",                 \"type\": \"uint256\"             }         ],         \"stateMutability\": \"view\",         \"type\": \"function\"     },     {         \"inputs\": [],         \"name\": \"getLootboxTypes\",         \"outputs\": [             {                 \"internalType\": \"uint256[]\",                 \"name\": \"\",                 \"type\": \"uint256[]\"             }         ],         \"stateMutability\": \"view\",         \"type\": \"function\"     },     {         \"inputs\": [             {                 \"internalType\": \"address\",                 \"name\": \"_opener\",                 \"type\": \"address\"             }         ],         \"name\": \"getOpenerRequestDetails\",         \"outputs\": [             {                 \"components\": [                     {                         \"internalType\": \"address\",                         \"name\": \"opener\",                         \"type\": \"address\"                     },                     {                         \"internalType\": \"uint96\",                         \"name\": \"unitsToGet\",                         \"type\": \"uint96\"                     },                     {                         \"internalType\": \"uint256[]\",                         \"name\": \"lootIds\",                         \"type\": \"uint256[]\"                     },                     {                         \"internalType\": \"uint256[]\",                         \"name\": \"lootAmounts\",                         \"type\": \"uint256[]\"                     }                 ],                 \"internalType\": \"struct LootboxInterface.Request\",                 \"name\": \"request\",                 \"type\": \"tuple\"             }         ],         \"stateMutability\": \"view\",         \"type\": \"function\"     },     {         \"inputs\": [],         \"name\": \"getPrice\",         \"outputs\": [             {                 \"internalType\": \"uint256\",                 \"name\": \"\",                 \"type\": \"uint256\"             }         ],         \"stateMutability\": \"view\",         \"type\": \"function\"     },     {         \"inputs\": [             {                 \"internalType\": \"bytes32\",                 \"name\": \"role\",                 \"type\": \"bytes32\"             }         ],         \"name\": \"getRoleAdmin\",         \"outputs\": [             {                 \"internalType\": \"bytes32\",                 \"name\": \"\",                 \"type\": \"bytes32\"             }         ],         \"stateMutability\": \"view\",         \"type\": \"function\"     },     {         \"inputs\": [             {                 \"internalType\": \"bytes32\",                 \"name\": \"role\",                 \"type\": \"bytes32\"             },             {                 \"internalType\": \"uint256\",                 \"name\": \"index\",                 \"type\": \"uint256\"             }         ],         \"name\": \"getRoleMember\",         \"outputs\": [             {                 \"internalType\": \"address\",                 \"name\": \"\",                 \"type\": \"address\"             }         ],         \"stateMutability\": \"view\",         \"type\": \"function\"     },     {         \"inputs\": [             {                 \"internalType\": \"bytes32\",                 \"name\": \"role\",                 \"type\": \"bytes32\"             }         ],         \"name\": \"getRoleMemberCount\",         \"outputs\": [             {                 \"internalType\": \"uint256\",                 \"name\": \"\",                 \"type\": \"uint256\"             }         ],         \"stateMutability\": \"view\",         \"type\": \"function\"     },     {         \"inputs\": [],         \"name\": \"getSuppliers\",         \"outputs\": [             {                 \"internalType\": \"address[]\",                 \"name\": \"\",                 \"type\": \"address[]\"             }         ],         \"stateMutability\": \"view\",         \"type\": \"function\"     },     {         \"inputs\": [],         \"name\": \"getVRFV2Wrapper\",         \"outputs\": [             {                 \"internalType\": \"address\",                 \"name\": \"\",                 \"type\": \"address\"             }         ],         \"stateMutability\": \"view\",         \"type\": \"function\"     },     {         \"inputs\": [             {                 \"internalType\": \"bytes32\",                 \"name\": \"role\",                 \"type\": \"bytes32\"             },             {                 \"internalType\": \"address\",                 \"name\": \"account\",                 \"type\": \"address\"             }         ],         \"name\": \"grantRole\",         \"outputs\": [],         \"stateMutability\": \"nonpayable\",         \"type\": \"function\"     },     {         \"inputs\": [             {                 \"internalType\": \"bytes32\",                 \"name\": \"role\",                 \"type\": \"bytes32\"             },             {                 \"internalType\": \"address\",                 \"name\": \"account\",                 \"type\": \"address\"             }         ],         \"name\": \"hasRole\",         \"outputs\": [             {                 \"internalType\": \"bool\",                 \"name\": \"\",                 \"type\": \"bool\"             }         ],         \"stateMutability\": \"view\",         \"type\": \"function\"     },     {         \"inputs\": [             {                 \"internalType\": \"address\",                 \"name\": \"account\",                 \"type\": \"address\"             },             {                 \"internalType\": \"address\",                 \"name\": \"operator\",                 \"type\": \"address\"             }         ],         \"name\": \"isApprovedForAll\",         \"outputs\": [             {                 \"internalType\": \"bool\",                 \"name\": \"\",                 \"type\": \"bool\"             }         ],         \"stateMutability\": \"view\",         \"type\": \"function\"     },     {         \"inputs\": [],         \"name\": \"isEmergencyMode\",         \"outputs\": [             {                 \"internalType\": \"bool\",                 \"name\": \"\",                 \"type\": \"bool\"             }         ],         \"stateMutability\": \"view\",         \"type\": \"function\"     },     {         \"inputs\": [             {                 \"internalType\": \"address\",                 \"name\": \"to\",                 \"type\": \"address\"             },             {                 \"internalType\": \"uint256\",                 \"name\": \"id\",                 \"type\": \"uint256\"             },             {                 \"internalType\": \"uint256\",                 \"name\": \"amount\",                 \"type\": \"uint256\"             },             {                 \"internalType\": \"bytes\",                 \"name\": \"data\",                 \"type\": \"bytes\"             }         ],         \"name\": \"mint\",         \"outputs\": [],         \"stateMutability\": \"nonpayable\",         \"type\": \"function\"     },     {         \"inputs\": [             {                 \"internalType\": \"address\",                 \"name\": \"to\",                 \"type\": \"address\"             },             {                 \"internalType\": \"uint256[]\",                 \"name\": \"ids\",                 \"type\": \"uint256[]\"             },             {                 \"internalType\": \"uint256[]\",                 \"name\": \"amounts\",                 \"type\": \"uint256[]\"             },             {                 \"internalType\": \"bytes\",                 \"name\": \"data\",                 \"type\": \"bytes\"             }         ],         \"name\": \"mintBatch\",         \"outputs\": [],         \"stateMutability\": \"nonpayable\",         \"type\": \"function\"     },     {         \"inputs\": [             {                 \"internalType\": \"address[]\",                 \"name\": \"_tos\",                 \"type\": \"address[]\"             },             {                 \"internalType\": \"uint256[]\",                 \"name\": \"_lootboxTypes\",                 \"type\": \"uint256[]\"             },             {                 \"internalType\": \"uint256[]\",                 \"name\": \"_amounts\",                 \"type\": \"uint256[]\"             }         ],         \"name\": \"mintToMany\",         \"outputs\": [],         \"stateMutability\": \"nonpayable\",         \"type\": \"function\"     },     {         \"inputs\": [             {                 \"internalType\": \"address\",                 \"name\": \"\",                 \"type\": \"address\"             },             {                 \"internalType\": \"address\",                 \"name\": \"\",                 \"type\": \"address\"             },             {                 \"internalType\": \"uint256[]\",                 \"name\": \"\",                 \"type\": \"uint256[]\"             },             {                 \"internalType\": \"uint256[]\",                 \"name\": \"\",                 \"type\": \"uint256[]\"             },             {                 \"internalType\": \"bytes\",                 \"name\": \"\",                 \"type\": \"bytes\"             }         ],         \"name\": \"onERC1155BatchReceived\",         \"outputs\": [             {                 \"internalType\": \"bytes4\",                 \"name\": \"\",                 \"type\": \"bytes4\"             }         ],         \"stateMutability\": \"nonpayable\",         \"type\": \"function\"     },     {         \"inputs\": [             {                 \"internalType\": \"address\",                 \"name\": \"\",                 \"type\": \"address\"             },             {                 \"internalType\": \"address\",                 \"name\": \"\",                 \"type\": \"address\"             },             {                 \"internalType\": \"uint256\",                 \"name\": \"\",                 \"type\": \"uint256\"             },             {                 \"internalType\": \"uint256\",                 \"name\": \"\",                 \"type\": \"uint256\"             },             {                 \"internalType\": \"bytes\",                 \"name\": \"\",                 \"type\": \"bytes\"             }         ],         \"name\": \"onERC1155Received\",         \"outputs\": [             {                 \"internalType\": \"bytes4\",                 \"name\": \"\",                 \"type\": \"bytes4\"             }         ],         \"stateMutability\": \"nonpayable\",         \"type\": \"function\"     },     {         \"inputs\": [             {                 \"internalType\": \"address\",                 \"name\": \"\",                 \"type\": \"address\"             },             {                 \"internalType\": \"address\",                 \"name\": \"\",                 \"type\": \"address\"             },             {                 \"internalType\": \"uint256\",                 \"name\": \"\",                 \"type\": \"uint256\"             },             {                 \"internalType\": \"bytes\",                 \"name\": \"\",                 \"type\": \"bytes\"             }         ],         \"name\": \"onERC721Received\",         \"outputs\": [             {                 \"internalType\": \"bytes4\",                 \"name\": \"\",                 \"type\": \"bytes4\"             }         ],         \"stateMutability\": \"nonpayable\",         \"type\": \"function\"     },     {         \"inputs\": [             {                 \"internalType\": \"uint32\",                 \"name\": \"_gas\",                 \"type\": \"uint32\"             },             {                 \"internalType\": \"uint256[]\",                 \"name\": \"_lootIds\",                 \"type\": \"uint256[]\"             },             {                 \"internalType\": \"uint256[]\",                 \"name\": \"_lootAmounts\",                 \"type\": \"uint256[]\"             }         ],         \"name\": \"open\",         \"outputs\": [],         \"stateMutability\": \"payable\",         \"type\": \"function\"     },     {         \"inputs\": [             {                 \"internalType\": \"address\",                 \"name\": \"\",                 \"type\": \"address\"             }         ],         \"name\": \"openerRequests\",         \"outputs\": [             {                 \"internalType\": \"uint256\",                 \"name\": \"\",                 \"type\": \"uint256\"             }         ],         \"stateMutability\": \"view\",         \"type\": \"function\"     },     {         \"inputs\": [],         \"name\": \"pause\",         \"outputs\": [],         \"stateMutability\": \"nonpayable\",         \"type\": \"function\"     },     {         \"inputs\": [],         \"name\": \"paused\",         \"outputs\": [             {                 \"internalType\": \"bool\",                 \"name\": \"\",                 \"type\": \"bool\"             }         ],         \"stateMutability\": \"view\",         \"type\": \"function\"     },     {         \"inputs\": [             {                 \"internalType\": \"uint256\",                 \"name\": \"_requestId\",                 \"type\": \"uint256\"             },             {                 \"internalType\": \"uint256[]\",                 \"name\": \"_randomWords\",                 \"type\": \"uint256[]\"             }         ],         \"name\": \"rawFulfillRandomWords\",         \"outputs\": [],         \"stateMutability\": \"nonpayable\",         \"type\": \"function\"     },     {         \"inputs\": [             {                 \"internalType\": \"address\",                 \"name\": \"_opener\",                 \"type\": \"address\"             }         ],         \"name\": \"recoverBoxes\",         \"outputs\": [],         \"stateMutability\": \"nonpayable\",         \"type\": \"function\"     },     {         \"inputs\": [             {                 \"internalType\": \"address[]\",                 \"name\": \"_suppliers\",                 \"type\": \"address[]\"             }         ],         \"name\": \"removeSuppliers\",         \"outputs\": [],         \"stateMutability\": \"nonpayable\",         \"type\": \"function\"     },     {         \"inputs\": [             {                 \"internalType\": \"bytes32\",                 \"name\": \"role\",                 \"type\": \"bytes32\"             },             {                 \"internalType\": \"address\",                 \"name\": \"account\",                 \"type\": \"address\"             }         ],         \"name\": \"renounceRole\",         \"outputs\": [],         \"stateMutability\": \"nonpayable\",         \"type\": \"function\"     },     {         \"inputs\": [             {                 \"internalType\": \"bytes32\",                 \"name\": \"role\",                 \"type\": \"bytes32\"             },             {                 \"internalType\": \"address\",                 \"name\": \"account\",                 \"type\": \"address\"             }         ],         \"name\": \"revokeRole\",         \"outputs\": [],         \"stateMutability\": \"nonpayable\",         \"type\": \"function\"     },     {         \"inputs\": [             {                 \"internalType\": \"address\",                 \"name\": \"from\",                 \"type\": \"address\"             },             {                 \"internalType\": \"address\",                 \"name\": \"to\",                 \"type\": \"address\"             },             {                 \"internalType\": \"uint256[]\",                 \"name\": \"ids\",                 \"type\": \"uint256[]\"             },             {                 \"internalType\": \"uint256[]\",                 \"name\": \"amounts\",                 \"type\": \"uint256[]\"             },             {                 \"internalType\": \"bytes\",                 \"name\": \"data\",                 \"type\": \"bytes\"             }         ],         \"name\": \"safeBatchTransferFrom\",         \"outputs\": [],         \"stateMutability\": \"nonpayable\",         \"type\": \"function\"     },     {         \"inputs\": [             {                 \"internalType\": \"address\",                 \"name\": \"from\",                 \"type\": \"address\"             },             {                 \"internalType\": \"address\",                 \"name\": \"to\",                 \"type\": \"address\"             },             {                 \"internalType\": \"uint256\",                 \"name\": \"id\",                 \"type\": \"uint256\"             },             {                 \"internalType\": \"uint256\",                 \"name\": \"amount\",                 \"type\": \"uint256\"             },             {                 \"internalType\": \"bytes\",                 \"name\": \"data\",                 \"type\": \"bytes\"             }         ],         \"name\": \"safeTransferFrom\",         \"outputs\": [],         \"stateMutability\": \"nonpayable\",         \"type\": \"function\"     },     {         \"inputs\": [             {                 \"internalType\": \"address[]\",                 \"name\": \"_tokens\",                 \"type\": \"address[]\"             },             {                 \"internalType\": \"uint256[]\",                 \"name\": \"_ids\",                 \"type\": \"uint256[]\"             },             {                 \"internalType\": \"uint256[]\",                 \"name\": \"_amountsPerUnit\",                 \"type\": \"uint256[]\"             }         ],         \"name\": \"setAmountsPerUnit\",         \"outputs\": [],         \"stateMutability\": \"nonpayable\",         \"type\": \"function\"     },     {         \"inputs\": [             {                 \"internalType\": \"address\",                 \"name\": \"operator\",                 \"type\": \"address\"             },             {                 \"internalType\": \"bool\",                 \"name\": \"approved\",                 \"type\": \"bool\"             }         ],         \"name\": \"setApprovalForAll\",         \"outputs\": [],         \"stateMutability\": \"nonpayable\",         \"type\": \"function\"     },     {         \"inputs\": [             {                 \"internalType\": \"uint256\",                 \"name\": \"_newPrice\",                 \"type\": \"uint256\"             }         ],         \"name\": \"setPrice\",         \"outputs\": [],         \"stateMutability\": \"nonpayable\",         \"type\": \"function\"     },     {         \"inputs\": [             {                 \"internalType\": \"string\",                 \"name\": \"_baseURI\",                 \"type\": \"string\"             }         ],         \"name\": \"setURI\",         \"outputs\": [],         \"stateMutability\": \"nonpayable\",         \"type\": \"function\"     },     {         \"inputs\": [             {                 \"internalType\": \"address\",                 \"name\": \"_from\",                 \"type\": \"address\"             }         ],         \"name\": \"supplyAllowed\",         \"outputs\": [             {                 \"internalType\": \"bool\",                 \"name\": \"\",                 \"type\": \"bool\"             }         ],         \"stateMutability\": \"view\",         \"type\": \"function\"     },     {         \"inputs\": [             {                 \"internalType\": \"bytes4\",                 \"name\": \"interfaceId\",                 \"type\": \"bytes4\"             }         ],         \"name\": \"supportsInterface\",         \"outputs\": [             {                 \"internalType\": \"bool\",                 \"name\": \"\",                 \"type\": \"bool\"             }         ],         \"stateMutability\": \"view\",         \"type\": \"function\"     },     {         \"inputs\": [             {                 \"internalType\": \"address\",                 \"name\": \"_token\",                 \"type\": \"address\"             }         ],         \"name\": \"tokenAllowed\",         \"outputs\": [             {                 \"internalType\": \"bool\",                 \"name\": \"\",                 \"type\": \"bool\"             }         ],         \"stateMutability\": \"view\",         \"type\": \"function\"     },     {         \"inputs\": [],         \"name\": \"unitsMinted\",         \"outputs\": [             {                 \"internalType\": \"uint256\",                 \"name\": \"\",                 \"type\": \"uint256\"             }         ],         \"stateMutability\": \"view\",         \"type\": \"function\"     },     {         \"inputs\": [],         \"name\": \"unitsRequested\",         \"outputs\": [             {                 \"internalType\": \"uint256\",                 \"name\": \"\",                 \"type\": \"uint256\"             }         ],         \"stateMutability\": \"view\",         \"type\": \"function\"     },     {         \"inputs\": [],         \"name\": \"unitsSupply\",         \"outputs\": [             {                 \"internalType\": \"uint256\",                 \"name\": \"\",                 \"type\": \"uint256\"             }         ],         \"stateMutability\": \"view\",         \"type\": \"function\"     },     {         \"inputs\": [],         \"name\": \"unpause\",         \"outputs\": [],         \"stateMutability\": \"nonpayable\",         \"type\": \"function\"     },     {         \"inputs\": [             {                 \"internalType\": \"uint256\",                 \"name\": \"\",                 \"type\": \"uint256\"             }         ],         \"name\": \"uri\",         \"outputs\": [             {                 \"internalType\": \"string\",                 \"name\": \"\",                 \"type\": \"string\"             }         ],         \"stateMutability\": \"view\",         \"type\": \"function\"     },     {         \"inputs\": [],         \"name\": \"viewCall\",         \"outputs\": [],         \"stateMutability\": \"nonpayable\",         \"type\": \"function\"     },     {         \"inputs\": [             {                 \"internalType\": \"address\",                 \"name\": \"_token\",                 \"type\": \"address\"             },             {                 \"internalType\": \"address payable\",                 \"name\": \"_to\",                 \"type\": \"address\"             },             {                 \"internalType\": \"uint256\",                 \"name\": \"_amount\",                 \"type\": \"uint256\"             }         ],         \"name\": \"withdraw\",         \"outputs\": [],         \"stateMutability\": \"nonpayable\",         \"type\": \"function\"     } ]";
        
        public string ContractAddress { get; set; }
        
        public IEventManager EventManager { get; set; }

        public Contract OriginalContract { get; set; }
                
        public bool Subscribed { get; set; }

        
        #region Methods

        public async Task<byte[]> DEFAULT_ADMIN_ROLE( TransactionRequest transactionOverwrite=null) 
        {
            var response = await OriginalContract.Call<byte[]>("DEFAULT_ADMIN_ROLE", new object [] {
                
            }, transactionOverwrite);
            
            return response;
        }


        public async Task<string> FACTORY( TransactionRequest transactionOverwrite=null) 
        {
            var response = await OriginalContract.Call<string>("FACTORY", new object [] {
                
            }, transactionOverwrite);
            
            return response;
        }


        public async Task<string> LINK_ETH_FEED( TransactionRequest transactionOverwrite=null) 
        {
            var response = await OriginalContract.Call<string>("LINK_ETH_FEED", new object [] {
                
            }, transactionOverwrite);
            
            return response;
        }


        public async Task<byte[]> MINTER_ROLE( TransactionRequest transactionOverwrite=null) 
        {
            var response = await OriginalContract.Call<byte[]>("MINTER_ROLE", new object [] {
                
            }, transactionOverwrite);
            
            return response;
        }


        public async Task<byte[]> PAUSER_ROLE( TransactionRequest transactionOverwrite=null) 
        {
            var response = await OriginalContract.Call<byte[]>("PAUSER_ROLE", new object [] {
                
            }, transactionOverwrite);
            
            return response;
        }


        public async Task AllocateRewards(BigInteger _requestId, BigInteger _randomness, TransactionRequest transactionOverwrite=null) 
        {
            var response = await OriginalContract.Send("_allocateRewards", new object [] {
                _requestId, _randomness
            }, transactionOverwrite);
            
            
        }
        public async Task<TransactionReceipt> AllocateRewardsWithReceipt(BigInteger _requestId, BigInteger _randomness, TransactionRequest transactionOverwrite=null) 
        {
            var response = await OriginalContract.SendWithReceipt("_allocateRewards", new object [] {
                _requestId, _randomness
            }, transactionOverwrite);
            
            return response.receipt;
        }

        public async Task AddSuppliers(string[] _suppliers, TransactionRequest transactionOverwrite=null) 
        {
            var response = await OriginalContract.Send("addSuppliers", new object [] {
                _suppliers
            }, transactionOverwrite);
            
            
        }
        public async Task<TransactionReceipt> AddSuppliersWithReceipt(string[] _suppliers, TransactionRequest transactionOverwrite=null) 
        {
            var response = await OriginalContract.SendWithReceipt("addSuppliers", new object [] {
                _suppliers
            }, transactionOverwrite);
            
            return response.receipt;
        }

        public async Task AddTokens(string[] _tokens, TransactionRequest transactionOverwrite=null) 
        {
            var response = await OriginalContract.Send("addTokens", new object [] {
                _tokens
            }, transactionOverwrite);
            
            
        }
        public async Task<TransactionReceipt> AddTokensWithReceipt(string[] _tokens, TransactionRequest transactionOverwrite=null) 
        {
            var response = await OriginalContract.SendWithReceipt("addTokens", new object [] {
                _tokens
            }, transactionOverwrite);
            
            return response.receipt;
        }

        public async Task<BigInteger> BalanceOf(string account, BigInteger id, TransactionRequest transactionOverwrite=null) 
        {
            var response = await OriginalContract.Call<BigInteger>("balanceOf", new object [] {
                account, id
            }, transactionOverwrite);
            
            return response;
        }


        public async Task<List<BigInteger>> BalanceOfBatch(string[] accounts, List<BigInteger> ids, TransactionRequest transactionOverwrite=null) 
        {
            var response = await OriginalContract.Call<List<BigInteger>>("balanceOfBatch", new object [] {
                accounts, ids
            }, transactionOverwrite);
            
            return new List<BigInteger>(response);
        }


        public async Task Burn(string account, BigInteger id, BigInteger value, TransactionRequest transactionOverwrite=null) 
        {
            var response = await OriginalContract.Send("burn", new object [] {
                account, id, value
            }, transactionOverwrite);
            
            
        }
        public async Task<TransactionReceipt> BurnWithReceipt(string account, BigInteger id, BigInteger value, TransactionRequest transactionOverwrite=null) 
        {
            var response = await OriginalContract.SendWithReceipt("burn", new object [] {
                account, id, value
            }, transactionOverwrite);
            
            return response.receipt;
        }

        public async Task BurnBatch(string account, BigInteger[] ids, BigInteger[] values, TransactionRequest transactionOverwrite=null) 
        {
            var response = await OriginalContract.Send("burnBatch", new object [] {
                account, ids, values
            }, transactionOverwrite);
            
            
        }
        public async Task<TransactionReceipt> BurnBatchWithReceipt(string account, BigInteger[] ids, BigInteger[] values, TransactionRequest transactionOverwrite=null) 
        {
            var response = await OriginalContract.SendWithReceipt("burnBatch", new object [] {
                account, ids, values
            }, transactionOverwrite);
            
            return response.receipt;
        }

        public async Task Buy(BigInteger _amount, BigInteger _maxPrice, TransactionRequest transactionOverwrite=null) 
        {
            var response = await OriginalContract.Send("buy", new object [] {
                _amount, _maxPrice
            }, transactionOverwrite);
            
            
        }
        public async Task<TransactionReceipt> BuyWithReceipt(BigInteger _amount, BigInteger _maxPrice, TransactionRequest transactionOverwrite=null) 
        {
            var response = await OriginalContract.SendWithReceipt("buy", new object [] {
                _amount, _maxPrice
            }, transactionOverwrite);
            
            return response.receipt;
        }

        public async Task<BigInteger> CalculateOpenPrice(BigInteger _gas, BigInteger _gasPriceInWei, BigInteger _units, TransactionRequest transactionOverwrite=null) 
        {
            var response = await OriginalContract.Call<BigInteger>("calculateOpenPrice", new object [] {
                _gas, _gasPriceInWei, _units
            }, transactionOverwrite);
            
            return response;
        }


        public async Task<bool> CanClaimRewards(string _opener, TransactionRequest transactionOverwrite=null) 
        {
            var response = await OriginalContract.Call<bool>("canClaimRewards", new object [] {
                _opener
            }, transactionOverwrite);
            
            return response;
        }


        public async Task ClaimRewards(string _opener, TransactionRequest transactionOverwrite=null) 
        {
            var response = await OriginalContract.Send("claimRewards", new object [] {
                _opener
            }, transactionOverwrite);
            
            
        }
        public async Task<TransactionReceipt> ClaimRewardsWithReceipt(string _opener, TransactionRequest transactionOverwrite=null) 
        {
            var response = await OriginalContract.SendWithReceipt("claimRewards", new object [] {
                _opener
            }, transactionOverwrite);
            
            return response.receipt;
        }

        public async Task EmergencyWithdraw(string _token, BigInteger _type, string _to, BigInteger[] _ids, BigInteger[] _amounts, TransactionRequest transactionOverwrite=null) 
        {
            var response = await OriginalContract.Send("emergencyWithdraw", new object [] {
                _token, _type, _to, _ids, _amounts
            }, transactionOverwrite);
            
            
        }
        public async Task<TransactionReceipt> EmergencyWithdrawWithReceipt(string _token, BigInteger _type, string _to, BigInteger[] _ids, BigInteger[] _amounts, TransactionRequest transactionOverwrite=null) 
        {
            var response = await OriginalContract.SendWithReceipt("emergencyWithdraw", new object [] {
                _token, _type, _to, _ids, _amounts
            }, transactionOverwrite);
            
            return response.receipt;
        }

        public async Task<BigInteger[]> GetAllowedTokenTypes( TransactionRequest transactionOverwrite=null) 
        {
            var response = await OriginalContract.Call<BigInteger[]>("getAllowedTokenTypes", new object [] {
                
            }, transactionOverwrite);
            
            return response;
        }


        public async Task<string[]> GetAllowedTokens( TransactionRequest transactionOverwrite=null) 
        {
            var response = await OriginalContract.Call<string[]>("getAllowedTokens", new object [] {
                
            }, transactionOverwrite);
            
            return response;
        }


        public async Task<BigInteger> GetAvailableSupply( TransactionRequest transactionOverwrite=null) 
        {
            var response = await OriginalContract.Call<BigInteger>("getAvailableSupply", new object [] {
                
            }, transactionOverwrite);
            
            return response;
        }


        public async Task<(Tuple[] result, Tuple[] leftoversResult)> GetInventory( TransactionRequest transactionOverwrite=null) 
        {
            var response = await OriginalContract.Call("getInventory", new object [] {
                
            }, transactionOverwrite);
            
            return ((Tuple[])response[0], (Tuple[])response[1]);
        }


        public async Task<string> GetLink( TransactionRequest transactionOverwrite=null) 
        {
            var response = await OriginalContract.Call<string>("getLink", new object [] {
                
            }, transactionOverwrite);
            
            return response;
        }


        public async Task<BigInteger> GetLinkPrice( TransactionRequest transactionOverwrite=null) 
        {
            var response = await OriginalContract.Call<BigInteger>("getLinkPrice", new object [] {
                
            }, transactionOverwrite);
            
            return response;
        }


        public async Task<List<BigInteger>> GetLootboxTypes( TransactionRequest transactionOverwrite=null) 
        {
            var response = await OriginalContract.Call<List<BigInteger>>("getLootboxTypes", new object [] {
                
            }, transactionOverwrite);
            
            return response;
        }


        public async Task<Tuple> GetOpenerRequestDetails(string _opener, TransactionRequest transactionOverwrite=null) 
        {
            var response = await OriginalContract.Call<Tuple>("getOpenerRequestDetails", new object [] {
                _opener
            }, transactionOverwrite);
            
            return response;
        }


        public async Task<BigInteger> GetPrice( TransactionRequest transactionOverwrite=null) 
        {
            var response = await OriginalContract.Call<BigInteger>("getPrice", new object [] {
                
            }, transactionOverwrite);
            
            return response;
        }


        public async Task<byte[]> GetRoleAdmin(byte[] role, TransactionRequest transactionOverwrite=null) 
        {
            var response = await OriginalContract.Call<byte[]>("getRoleAdmin", new object [] {
                role
            }, transactionOverwrite);
            
            return response;
        }


        public async Task<string> GetRoleMember(byte[] role, BigInteger index, TransactionRequest transactionOverwrite=null) 
        {
            var response = await OriginalContract.Call<string>("getRoleMember", new object [] {
                role, index
            }, transactionOverwrite);
            
            return response;
        }


        public async Task<BigInteger> GetRoleMemberCount(byte[] role, TransactionRequest transactionOverwrite=null) 
        {
            var response = await OriginalContract.Call<BigInteger>("getRoleMemberCount", new object [] {
                role
            }, transactionOverwrite);
            
            return response;
        }


        public async Task<string[]> GetSuppliers( TransactionRequest transactionOverwrite=null) 
        {
            var response = await OriginalContract.Call<string[]>("getSuppliers", new object [] {
                
            }, transactionOverwrite);
            
            return response;
        }


        public async Task<string> GetVRFV2Wrapper( TransactionRequest transactionOverwrite=null) 
        {
            var response = await OriginalContract.Call<string>("getVRFV2Wrapper", new object [] {
                
            }, transactionOverwrite);
            
            return response;
        }


        public async Task GrantRole(byte[] role, string account, TransactionRequest transactionOverwrite=null) 
        {
            var response = await OriginalContract.Send("grantRole", new object [] {
                role, account
            }, transactionOverwrite);
            
            
        }
        public async Task<TransactionReceipt> GrantRoleWithReceipt(byte[] role, string account, TransactionRequest transactionOverwrite=null) 
        {
            var response = await OriginalContract.SendWithReceipt("grantRole", new object [] {
                role, account
            }, transactionOverwrite);
            
            return response.receipt;
        }

        public async Task<bool> HasRole(byte[] role, string account, TransactionRequest transactionOverwrite=null) 
        {
            var response = await OriginalContract.Call<bool>("hasRole", new object [] {
                role, account
            }, transactionOverwrite);
            
            return response;
        }


        public async Task<bool> IsApprovedForAll(string account, string @operator, TransactionRequest transactionOverwrite=null) 
        {
            var response = await OriginalContract.Call<bool>("isApprovedForAll", new object [] {
                account, @operator
            }, transactionOverwrite);
            
            return response;
        }


        public async Task<bool> IsEmergencyMode( TransactionRequest transactionOverwrite=null) 
        {
            var response = await OriginalContract.Call<bool>("isEmergencyMode", new object [] {
                
            }, transactionOverwrite);
            
            return response;
        }


        public async Task Mint(string to, BigInteger id, BigInteger amount, byte[] data, TransactionRequest transactionOverwrite=null) 
        {
            var response = await OriginalContract.Send("mint", new object [] {
                to, id, amount, data
            }, transactionOverwrite);
            
            
        }
        public async Task<TransactionReceipt> MintWithReceipt(string to, BigInteger id, BigInteger amount, byte[] data, TransactionRequest transactionOverwrite=null) 
        {
            var response = await OriginalContract.SendWithReceipt("mint", new object [] {
                to, id, amount, data
            }, transactionOverwrite);
            
            return response.receipt;
        }

        public async Task MintBatch(string to, BigInteger[] ids, BigInteger[] amounts, byte[] data, TransactionRequest transactionOverwrite=null) 
        {
            var response = await OriginalContract.Send("mintBatch", new object [] {
                to, ids, amounts, data
            }, transactionOverwrite);
            
            
        }
        public async Task<TransactionReceipt> MintBatchWithReceipt(string to, BigInteger[] ids, BigInteger[] amounts, byte[] data, TransactionRequest transactionOverwrite=null) 
        {
            var response = await OriginalContract.SendWithReceipt("mintBatch", new object [] {
                to, ids, amounts, data
            }, transactionOverwrite);
            
            return response.receipt;
        }

        public async Task MintToMany(string[] _tos, BigInteger[] _lootboxTypes, BigInteger[] _amounts, TransactionRequest transactionOverwrite=null) 
        {
            var response = await OriginalContract.Send("mintToMany", new object [] {
                _tos, _lootboxTypes, _amounts
            }, transactionOverwrite);
            
            
        }
        public async Task<TransactionReceipt> MintToManyWithReceipt(string[] _tos, BigInteger[] _lootboxTypes, BigInteger[] _amounts, TransactionRequest transactionOverwrite=null) 
        {
            var response = await OriginalContract.SendWithReceipt("mintToMany", new object [] {
                _tos, _lootboxTypes, _amounts
            }, transactionOverwrite);
            
            return response.receipt;
        }

        public async Task<byte[]> OnERC1155BatchReceived(string param1, string param2, BigInteger[] param3, BigInteger[] param4, byte[] param5, TransactionRequest transactionOverwrite=null) 
        {
            var response = await OriginalContract.Send<byte[]>("onERC1155BatchReceived", new object [] {
                param1, param2, param3, param4, param5
            }, transactionOverwrite);
            
            return response;
        }
        public async Task<(byte[] , TransactionReceipt receipt)> OnERC1155BatchReceivedWithReceipt(string param1, string param2, BigInteger[] param3, BigInteger[] param4, byte[] param5, TransactionRequest transactionOverwrite=null) 
        {
            var response = await OriginalContract.SendWithReceipt<byte[]>("onERC1155BatchReceived", new object [] {
                param1, param2, param3, param4, param5
            }, transactionOverwrite);
            
            return (response.response, response.receipt);
        }

        public async Task<byte[]> OnERC1155Received(string param1, string param2, BigInteger param3, BigInteger param4, byte[] param5, TransactionRequest transactionOverwrite=null) 
        {
            var response = await OriginalContract.Send<byte[]>("onERC1155Received", new object [] {
                param1, param2, param3, param4, param5
            }, transactionOverwrite);
            
            return response;
        }
        public async Task<(byte[] , TransactionReceipt receipt)> OnERC1155ReceivedWithReceipt(string param1, string param2, BigInteger param3, BigInteger param4, byte[] param5, TransactionRequest transactionOverwrite=null) 
        {
            var response = await OriginalContract.SendWithReceipt<byte[]>("onERC1155Received", new object [] {
                param1, param2, param3, param4, param5
            }, transactionOverwrite);
            
            return (response.response, response.receipt);
        }

        public async Task<byte[]> OnERC721Received(string param1, string param2, BigInteger param3, byte[] param4, TransactionRequest transactionOverwrite=null) 
        {
            var response = await OriginalContract.Send<byte[]>("onERC721Received", new object [] {
                param1, param2, param3, param4
            }, transactionOverwrite);
            
            return response;
        }
        public async Task<(byte[] , TransactionReceipt receipt)> OnERC721ReceivedWithReceipt(string param1, string param2, BigInteger param3, byte[] param4, TransactionRequest transactionOverwrite=null) 
        {
            var response = await OriginalContract.SendWithReceipt<byte[]>("onERC721Received", new object [] {
                param1, param2, param3, param4
            }, transactionOverwrite);
            
            return (response.response, response.receipt);
        }

        public async Task Open(BigInteger _gas, BigInteger[] _lootIds, BigInteger[] _lootAmounts, TransactionRequest transactionOverwrite=null) 
        {
            var response = await OriginalContract.Send("open", new object [] {
                _gas, _lootIds, _lootAmounts
            }, transactionOverwrite);
            
            
        }
        public async Task<TransactionReceipt> OpenWithReceipt(BigInteger _gas, BigInteger[] _lootIds, BigInteger[] _lootAmounts, TransactionRequest transactionOverwrite=null) 
        {
            var response = await OriginalContract.SendWithReceipt("open", new object [] {
                _gas, _lootIds, _lootAmounts
            }, transactionOverwrite);
            
            return response.receipt;
        }

        public async Task<BigInteger> OpenerRequests(string param1, TransactionRequest transactionOverwrite=null) 
        {
            var response = await OriginalContract.Call<BigInteger>("openerRequests", new object [] {
                param1
            }, transactionOverwrite);
            
            return response;
        }


        public async Task Pause( TransactionRequest transactionOverwrite=null) 
        {
            var response = await OriginalContract.Send("pause", new object [] {
                
            }, transactionOverwrite);
            
            
        }
        public async Task<TransactionReceipt> PauseWithReceipt( TransactionRequest transactionOverwrite=null) 
        {
            var response = await OriginalContract.SendWithReceipt("pause", new object [] {
                
            }, transactionOverwrite);
            
            return response.receipt;
        }

        public async Task<bool> Paused( TransactionRequest transactionOverwrite=null) 
        {
            var response = await OriginalContract.Call<bool>("paused", new object [] {
                
            }, transactionOverwrite);
            
            return response;
        }


        public async Task RawFulfillRandomWords(BigInteger _requestId, BigInteger[] _randomWords, TransactionRequest transactionOverwrite=null) 
        {
            var response = await OriginalContract.Send("rawFulfillRandomWords", new object [] {
                _requestId, _randomWords
            }, transactionOverwrite);
            
            
        }
        public async Task<TransactionReceipt> RawFulfillRandomWordsWithReceipt(BigInteger _requestId, BigInteger[] _randomWords, TransactionRequest transactionOverwrite=null) 
        {
            var response = await OriginalContract.SendWithReceipt("rawFulfillRandomWords", new object [] {
                _requestId, _randomWords
            }, transactionOverwrite);
            
            return response.receipt;
        }

        public async Task RecoverBoxes(string _opener, TransactionRequest transactionOverwrite=null) 
        {
            var response = await OriginalContract.Send("recoverBoxes", new object [] {
                _opener
            }, transactionOverwrite);
            
            
        }
        public async Task<TransactionReceipt> RecoverBoxesWithReceipt(string _opener, TransactionRequest transactionOverwrite=null) 
        {
            var response = await OriginalContract.SendWithReceipt("recoverBoxes", new object [] {
                _opener
            }, transactionOverwrite);
            
            return response.receipt;
        }

        public async Task RemoveSuppliers(string[] _suppliers, TransactionRequest transactionOverwrite=null) 
        {
            var response = await OriginalContract.Send("removeSuppliers", new object [] {
                _suppliers
            }, transactionOverwrite);
            
            
        }
        public async Task<TransactionReceipt> RemoveSuppliersWithReceipt(string[] _suppliers, TransactionRequest transactionOverwrite=null) 
        {
            var response = await OriginalContract.SendWithReceipt("removeSuppliers", new object [] {
                _suppliers
            }, transactionOverwrite);
            
            return response.receipt;
        }

        public async Task RenounceRole(byte[] role, string account, TransactionRequest transactionOverwrite=null) 
        {
            var response = await OriginalContract.Send("renounceRole", new object [] {
                role, account
            }, transactionOverwrite);
            
            
        }
        public async Task<TransactionReceipt> RenounceRoleWithReceipt(byte[] role, string account, TransactionRequest transactionOverwrite=null) 
        {
            var response = await OriginalContract.SendWithReceipt("renounceRole", new object [] {
                role, account
            }, transactionOverwrite);
            
            return response.receipt;
        }

        public async Task RevokeRole(byte[] role, string account, TransactionRequest transactionOverwrite=null) 
        {
            var response = await OriginalContract.Send("revokeRole", new object [] {
                role, account
            }, transactionOverwrite);
            
            
        }
        public async Task<TransactionReceipt> RevokeRoleWithReceipt(byte[] role, string account, TransactionRequest transactionOverwrite=null) 
        {
            var response = await OriginalContract.SendWithReceipt("revokeRole", new object [] {
                role, account
            }, transactionOverwrite);
            
            return response.receipt;
        }

        public async Task SafeBatchTransferFrom(string from, string to, BigInteger[] ids, BigInteger[] amounts, byte[] data, TransactionRequest transactionOverwrite=null) 
        {
            var response = await OriginalContract.Send("safeBatchTransferFrom", new object [] {
                from, to, ids, amounts, data
            }, transactionOverwrite);
            
            
        }
        public async Task<TransactionReceipt> SafeBatchTransferFromWithReceipt(string from, string to, BigInteger[] ids, BigInteger[] amounts, byte[] data, TransactionRequest transactionOverwrite=null) 
        {
            var response = await OriginalContract.SendWithReceipt("safeBatchTransferFrom", new object [] {
                from, to, ids, amounts, data
            }, transactionOverwrite);
            
            return response.receipt;
        }

        public async Task SafeTransferFrom(string from, string to, BigInteger id, BigInteger amount, byte[] data, TransactionRequest transactionOverwrite=null) 
        {
            var response = await OriginalContract.Send("safeTransferFrom", new object [] {
                from, to, id, amount, data
            }, transactionOverwrite);
            
            
        }
        public async Task<TransactionReceipt> SafeTransferFromWithReceipt(string from, string to, BigInteger id, BigInteger amount, byte[] data, TransactionRequest transactionOverwrite=null) 
        {
            var response = await OriginalContract.SendWithReceipt("safeTransferFrom", new object [] {
                from, to, id, amount, data
            }, transactionOverwrite);
            
            return response.receipt;
        }

        public async Task SetAmountsPerUnit(string[] _tokens, BigInteger[] _ids, BigInteger[] _amountsPerUnit, TransactionRequest transactionOverwrite=null) 
        {
            var response = await OriginalContract.Send("setAmountsPerUnit", new object [] {
                _tokens, _ids, _amountsPerUnit
            }, transactionOverwrite);
            
            
        }
        public async Task<TransactionReceipt> SetAmountsPerUnitWithReceipt(string[] _tokens, BigInteger[] _ids, BigInteger[] _amountsPerUnit, TransactionRequest transactionOverwrite=null) 
        {
            var response = await OriginalContract.SendWithReceipt("setAmountsPerUnit", new object [] {
                _tokens, _ids, _amountsPerUnit
            }, transactionOverwrite);
            
            return response.receipt;
        }

        public async Task SetApprovalForAll(string @operator, bool approved, TransactionRequest transactionOverwrite=null) 
        {
            var response = await OriginalContract.Send("setApprovalForAll", new object [] {
                @operator, approved
            }, transactionOverwrite);
            
            
        }
        public async Task<TransactionReceipt> SetApprovalForAllWithReceipt(string @operator, bool approved, TransactionRequest transactionOverwrite=null) 
        {
            var response = await OriginalContract.SendWithReceipt("setApprovalForAll", new object [] {
                @operator, approved
            }, transactionOverwrite);
            
            return response.receipt;
        }

        public async Task SetPrice(BigInteger _newPrice, TransactionRequest transactionOverwrite=null) 
        {
            var response = await OriginalContract.Send("setPrice", new object [] {
                _newPrice
            }, transactionOverwrite);
            
            
        }
        public async Task<TransactionReceipt> SetPriceWithReceipt(BigInteger _newPrice, TransactionRequest transactionOverwrite=null) 
        {
            var response = await OriginalContract.SendWithReceipt("setPrice", new object [] {
                _newPrice
            }, transactionOverwrite);
            
            return response.receipt;
        }

        public async Task SetURI(string _baseURI, TransactionRequest transactionOverwrite=null) 
        {
            var response = await OriginalContract.Send("setURI", new object [] {
                _baseURI
            }, transactionOverwrite);
            
            
        }
        public async Task<TransactionReceipt> SetURIWithReceipt(string _baseURI, TransactionRequest transactionOverwrite=null) 
        {
            var response = await OriginalContract.SendWithReceipt("setURI", new object [] {
                _baseURI
            }, transactionOverwrite);
            
            return response.receipt;
        }

        public async Task<bool> SupplyAllowed(string _from, TransactionRequest transactionOverwrite=null) 
        {
            var response = await OriginalContract.Call<bool>("supplyAllowed", new object [] {
                _from
            }, transactionOverwrite);
            
            return response;
        }


        public async Task<bool> SupportsInterface(byte[] interfaceId, TransactionRequest transactionOverwrite=null) 
        {
            var response = await OriginalContract.Call<bool>("supportsInterface", new object [] {
                interfaceId
            }, transactionOverwrite);
            
            return response;
        }


        public async Task<bool> TokenAllowed(string _token, TransactionRequest transactionOverwrite=null) 
        {
            var response = await OriginalContract.Call<bool>("tokenAllowed", new object [] {
                _token
            }, transactionOverwrite);
            
            return response;
        }


        public async Task<BigInteger> UnitsMinted( TransactionRequest transactionOverwrite=null) 
        {
            var response = await OriginalContract.Call<BigInteger>("unitsMinted", new object [] {
                
            }, transactionOverwrite);
            
            return response;
        }


        public async Task<BigInteger> UnitsRequested( TransactionRequest transactionOverwrite=null) 
        {
            var response = await OriginalContract.Call<BigInteger>("unitsRequested", new object [] {
                
            }, transactionOverwrite);
            
            return response;
        }


        public async Task<BigInteger> UnitsSupply( TransactionRequest transactionOverwrite=null) 
        {
            var response = await OriginalContract.Call<BigInteger>("unitsSupply", new object [] {
                
            }, transactionOverwrite);
            
            return response;
        }


        public async Task Unpause( TransactionRequest transactionOverwrite=null) 
        {
            var response = await OriginalContract.Send("unpause", new object [] {
                
            }, transactionOverwrite);
            
            
        }
        public async Task<TransactionReceipt> UnpauseWithReceipt( TransactionRequest transactionOverwrite=null) 
        {
            var response = await OriginalContract.SendWithReceipt("unpause", new object [] {
                
            }, transactionOverwrite);
            
            return response.receipt;
        }

        public async Task<string> Uri(BigInteger param1, TransactionRequest transactionOverwrite=null) 
        {
            var response = await OriginalContract.Call<string>("uri", new object [] {
                param1
            }, transactionOverwrite);
            
            return response;
        }


        public async Task ViewCall( TransactionRequest transactionOverwrite=null) 
        {
            var response = await OriginalContract.Send("viewCall", new object [] {
                
            }, transactionOverwrite);
            
            
        }
        public async Task<TransactionReceipt> ViewCallWithReceipt( TransactionRequest transactionOverwrite=null) 
        {
            var response = await OriginalContract.SendWithReceipt("viewCall", new object [] {
                
            }, transactionOverwrite);
            
            return response.receipt;
        }

        public async Task Withdraw(string _token, string _to, BigInteger _amount, TransactionRequest transactionOverwrite=null) 
        {
            var response = await OriginalContract.Send("withdraw", new object [] {
                _token, _to, _amount
            }, transactionOverwrite);
            
            
        }
        public async Task<TransactionReceipt> WithdrawWithReceipt(string _token, string _to, BigInteger _amount, TransactionRequest transactionOverwrite=null) 
        {
            var response = await OriginalContract.SendWithReceipt("withdraw", new object [] {
                _token, _to, _amount
            }, transactionOverwrite);
            
            return response.receipt;
        }


        #endregion
        
        
        #region Event Classes

        public partial class AllocatedEventDTO : AllocatedEventDTOBase { }
        
        [Event("Allocated")]
        public class AllocatedEventDTOBase : IEventDTO
        {
                    [Parameter("address", "opener", 0, false)]
        public virtual string Opener { get; set; }
        [Parameter("address", "token", 1, false)]
        public virtual string Token { get; set; }
        [Parameter("uint256", "tokenId", 2, false)]
        public virtual BigInteger TokenId { get; set; }
        [Parameter("uint256", "amount", 3, false)]
        public virtual BigInteger Amount { get; set; }

        }
    
        public event Action<AllocatedEventDTO> OnAllocated;
        
        private void Allocated(AllocatedEventDTO allocated)
        {
            OnAllocated?.Invoke(allocated);
        }

        public partial class AmountPerUnitSetEventDTO : AmountPerUnitSetEventDTOBase { }
        
        [Event("AmountPerUnitSet")]
        public class AmountPerUnitSetEventDTOBase : IEventDTO
        {
                    [Parameter("address", "token", 0, false)]
        public virtual string Token { get; set; }
        [Parameter("uint256", "tokenId", 1, false)]
        public virtual BigInteger TokenId { get; set; }
        [Parameter("uint256", "amountPerUnit", 2, false)]
        public virtual BigInteger AmountPerUnit { get; set; }
        [Parameter("uint256", "newSupply", 3, false)]
        public virtual BigInteger NewSupply { get; set; }

        }
    
        public event Action<AmountPerUnitSetEventDTO> OnAmountPerUnitSet;
        
        private void AmountPerUnitSet(AmountPerUnitSetEventDTO amountPerUnitSet)
        {
            OnAmountPerUnitSet?.Invoke(amountPerUnitSet);
        }

        public partial class ApprovalForAllEventDTO : ApprovalForAllEventDTOBase { }
        
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

        public partial class BoxesRecoveredEventDTO : BoxesRecoveredEventDTOBase { }
        
        [Event("BoxesRecovered")]
        public class BoxesRecoveredEventDTOBase : IEventDTO
        {
                    [Parameter("address", "opener", 0, false)]
        public virtual string Opener { get; set; }
        [Parameter("uint256", "requestId", 1, false)]
        public virtual BigInteger RequestId { get; set; }

        }
    
        public event Action<BoxesRecoveredEventDTO> OnBoxesRecovered;
        
        private void BoxesRecovered(BoxesRecoveredEventDTO boxesRecovered)
        {
            OnBoxesRecovered?.Invoke(boxesRecovered);
        }

        public partial class EmergencyModeEnabledEventDTO : EmergencyModeEnabledEventDTOBase { }
        
        [Event("EmergencyModeEnabled")]
        public class EmergencyModeEnabledEventDTOBase : IEventDTO
        {
                    [Parameter("address", "caller", 0, false)]
        public virtual string Caller { get; set; }

        }
    
        public event Action<EmergencyModeEnabledEventDTO> OnEmergencyModeEnabled;
        
        private void EmergencyModeEnabled(EmergencyModeEnabledEventDTO emergencyModeEnabled)
        {
            OnEmergencyModeEnabled?.Invoke(emergencyModeEnabled);
        }

        public partial class EmergencyWithdrawalEventDTO : EmergencyWithdrawalEventDTOBase { }
        
        [Event("EmergencyWithdrawal")]
        public class EmergencyWithdrawalEventDTOBase : IEventDTO
        {
                    [Parameter("address", "token", 0, false)]
        public virtual string Token { get; set; }
        [Parameter("uint8", "tokenType", 1, false)]
        public virtual BigInteger TokenType { get; set; }
        [Parameter("address", "to", 2, false)]
        public virtual string To { get; set; }
        [Parameter("uint256[]", "ids", 3, false)]
        public virtual BigInteger[] Ids { get; set; }
        [Parameter("uint256[]", "amounts", 4, false)]
        public virtual BigInteger[] Amounts { get; set; }

        }
    
        public event Action<EmergencyWithdrawalEventDTO> OnEmergencyWithdrawal;
        
        private void EmergencyWithdrawal(EmergencyWithdrawalEventDTO emergencyWithdrawal)
        {
            OnEmergencyWithdrawal?.Invoke(emergencyWithdrawal);
        }

        public partial class OpenRequestFailedEventDTO : OpenRequestFailedEventDTOBase { }
        
        [Event("OpenRequestFailed")]
        public class OpenRequestFailedEventDTOBase : IEventDTO
        {
                    [Parameter("uint256", "requestId", 0, false)]
        public virtual BigInteger RequestId { get; set; }
        [Parameter("bytes", "reason", 1, false)]
        public virtual byte[] Reason { get; set; }

        }
    
        public event Action<OpenRequestFailedEventDTO> OnOpenRequestFailed;
        
        private void OpenRequestFailed(OpenRequestFailedEventDTO openRequestFailed)
        {
            OnOpenRequestFailed?.Invoke(openRequestFailed);
        }

        public partial class OpenRequestFulfilledEventDTO : OpenRequestFulfilledEventDTOBase { }
        
        [Event("OpenRequestFulfilled")]
        public class OpenRequestFulfilledEventDTOBase : IEventDTO
        {
                    [Parameter("uint256", "requestId", 0, false)]
        public virtual BigInteger RequestId { get; set; }
        [Parameter("uint256", "randomness", 1, false)]
        public virtual BigInteger Randomness { get; set; }

        }
    
        public event Action<OpenRequestFulfilledEventDTO> OnOpenRequestFulfilled;
        
        private void OpenRequestFulfilled(OpenRequestFulfilledEventDTO openRequestFulfilled)
        {
            OnOpenRequestFulfilled?.Invoke(openRequestFulfilled);
        }

        public partial class OpenRequestedEventDTO : OpenRequestedEventDTOBase { }
        
        [Event("OpenRequested")]
        public class OpenRequestedEventDTOBase : IEventDTO
        {
                    [Parameter("address", "opener", 0, false)]
        public virtual string Opener { get; set; }
        [Parameter("uint256", "unitsToGet", 1, false)]
        public virtual BigInteger UnitsToGet { get; set; }
        [Parameter("uint256", "requestId", 2, false)]
        public virtual BigInteger RequestId { get; set; }

        }
    
        public event Action<OpenRequestedEventDTO> OnOpenRequested;
        
        private void OpenRequested(OpenRequestedEventDTO openRequested)
        {
            OnOpenRequested?.Invoke(openRequested);
        }

        public partial class PausedEventDTO : PausedEventDTOBase { }
        
        [Event("Paused")]
        public class PausedEventDTOBase : IEventDTO
        {
                    [Parameter("address", "account", 0, false)]
        public virtual string Account { get; set; }

        }
    
        public event Action<PausedEventDTO> OnPaused;
        
        private void Paused(PausedEventDTO paused)
        {
            OnPaused?.Invoke(paused);
        }

        public partial class PriceUpdatedEventDTO : PriceUpdatedEventDTOBase { }
        
        [Event("PriceUpdated")]
        public class PriceUpdatedEventDTOBase : IEventDTO
        {
                    [Parameter("uint256", "newPrice", 0, false)]
        public virtual BigInteger NewPrice { get; set; }

        }
    
        public event Action<PriceUpdatedEventDTO> OnPriceUpdated;
        
        private void PriceUpdated(PriceUpdatedEventDTO priceUpdated)
        {
            OnPriceUpdated?.Invoke(priceUpdated);
        }

        public partial class RewardsClaimedEventDTO : RewardsClaimedEventDTOBase { }
        
        [Event("RewardsClaimed")]
        public class RewardsClaimedEventDTOBase : IEventDTO
        {
                    [Parameter("address", "opener", 0, false)]
        public virtual string Opener { get; set; }
        [Parameter("address", "token", 1, false)]
        public virtual string Token { get; set; }
        [Parameter("uint256", "tokenId", 2, false)]
        public virtual BigInteger TokenId { get; set; }
        [Parameter("uint256", "amount", 3, false)]
        public virtual BigInteger Amount { get; set; }

        }
    
        public event Action<RewardsClaimedEventDTO> OnRewardsClaimed;
        
        private void RewardsClaimed(RewardsClaimedEventDTO rewardsClaimed)
        {
            OnRewardsClaimed?.Invoke(rewardsClaimed);
        }

        public partial class RoleAdminChangedEventDTO : RoleAdminChangedEventDTOBase { }
        
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
    
        public event Action<RoleAdminChangedEventDTO> OnRoleAdminChanged;
        
        private void RoleAdminChanged(RoleAdminChangedEventDTO roleAdminChanged)
        {
            OnRoleAdminChanged?.Invoke(roleAdminChanged);
        }

        public partial class RoleGrantedEventDTO : RoleGrantedEventDTOBase { }
        
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
    
        public event Action<RoleGrantedEventDTO> OnRoleGranted;
        
        private void RoleGranted(RoleGrantedEventDTO roleGranted)
        {
            OnRoleGranted?.Invoke(roleGranted);
        }

        public partial class RoleRevokedEventDTO : RoleRevokedEventDTOBase { }
        
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
    
        public event Action<RoleRevokedEventDTO> OnRoleRevoked;
        
        private void RoleRevoked(RoleRevokedEventDTO roleRevoked)
        {
            OnRoleRevoked?.Invoke(roleRevoked);
        }

        public partial class SoldEventDTO : SoldEventDTOBase { }
        
        [Event("Sold")]
        public class SoldEventDTOBase : IEventDTO
        {
                    [Parameter("address", "buyer", 0, false)]
        public virtual string Buyer { get; set; }
        [Parameter("uint256", "amount", 1, false)]
        public virtual BigInteger Amount { get; set; }
        [Parameter("uint256", "payment", 2, false)]
        public virtual BigInteger Payment { get; set; }

        }
    
        public event Action<SoldEventDTO> OnSold;
        
        private void Sold(SoldEventDTO sold)
        {
            OnSold?.Invoke(sold);
        }

        public partial class SupplierAddedEventDTO : SupplierAddedEventDTOBase { }
        
        [Event("SupplierAdded")]
        public class SupplierAddedEventDTOBase : IEventDTO
        {
                    [Parameter("address", "supplier", 0, false)]
        public virtual string Supplier { get; set; }

        }
    
        public event Action<SupplierAddedEventDTO> OnSupplierAdded;
        
        private void SupplierAdded(SupplierAddedEventDTO supplierAdded)
        {
            OnSupplierAdded?.Invoke(supplierAdded);
        }

        public partial class SupplierRemovedEventDTO : SupplierRemovedEventDTOBase { }
        
        [Event("SupplierRemoved")]
        public class SupplierRemovedEventDTOBase : IEventDTO
        {
                    [Parameter("address", "supplier", 0, false)]
        public virtual string Supplier { get; set; }

        }
    
        public event Action<SupplierRemovedEventDTO> OnSupplierRemoved;
        
        private void SupplierRemoved(SupplierRemovedEventDTO supplierRemoved)
        {
            OnSupplierRemoved?.Invoke(supplierRemoved);
        }

        public partial class TokenAddedEventDTO : TokenAddedEventDTOBase { }
        
        [Event("TokenAdded")]
        public class TokenAddedEventDTOBase : IEventDTO
        {
                    [Parameter("address", "token", 0, false)]
        public virtual string Token { get; set; }

        }
    
        public event Action<TokenAddedEventDTO> OnTokenAdded;
        
        private void TokenAdded(TokenAddedEventDTO tokenAdded)
        {
            OnTokenAdded?.Invoke(tokenAdded);
        }

        public partial class TransferBatchEventDTO : TransferBatchEventDTOBase { }
        
        [Event("TransferBatch")]
        public class TransferBatchEventDTOBase : IEventDTO
        {
                    [Parameter("address", "operator", 0, true)]
        public virtual string Operator { get; set; }
        [Parameter("address", "from", 1, true)]
        public virtual string From { get; set; }
        [Parameter("address", "to", 2, true)]
        public virtual string To { get; set; }
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

        public partial class TransferSingleEventDTO : TransferSingleEventDTOBase { }
        
        [Event("TransferSingle")]
        public class TransferSingleEventDTOBase : IEventDTO
        {
                    [Parameter("address", "operator", 0, true)]
        public virtual string Operator { get; set; }
        [Parameter("address", "from", 1, true)]
        public virtual string From { get; set; }
        [Parameter("address", "to", 2, true)]
        public virtual string To { get; set; }
        [Parameter("uint256", "id", 3, false)]
        public virtual BigInteger Id { get; set; }
        [Parameter("uint256", "value", 4, false)]
        public virtual BigInteger Value { get; set; }

        }
    
        public event Action<TransferSingleEventDTO> OnTransferSingle;
        
        private void TransferSingle(TransferSingleEventDTO transferSingle)
        {
            OnTransferSingle?.Invoke(transferSingle);
        }

        public partial class URIEventDTO : URIEventDTOBase { }
        
        [Event("URI")]
        public class URIEventDTOBase : IEventDTO
        {
                    [Parameter("string", "value", 0, false)]
        public virtual string Value { get; set; }
        [Parameter("uint256", "id", 1, true)]
        public virtual BigInteger Id { get; set; }

        }
    
        public event Action<URIEventDTO> OnURI;
        
        private void URI(URIEventDTO uRI)
        {
            OnURI?.Invoke(uRI);
        }

        public partial class UnpausedEventDTO : UnpausedEventDTOBase { }
        
        [Event("Unpaused")]
        public class UnpausedEventDTOBase : IEventDTO
        {
                    [Parameter("address", "account", 0, false)]
        public virtual string Account { get; set; }

        }
    
        public event Action<UnpausedEventDTO> OnUnpaused;
        
        private void Unpaused(UnpausedEventDTO unpaused)
        {
            OnUnpaused?.Invoke(unpaused);
        }

        public partial class WithdrawEventDTO : WithdrawEventDTOBase { }
        
        [Event("Withdraw")]
        public class WithdrawEventDTOBase : IEventDTO
        {
                    [Parameter("address", "token", 0, false)]
        public virtual string Token { get; set; }
        [Parameter("address", "to", 1, false)]
        public virtual string To { get; set; }
        [Parameter("uint256", "amount", 2, false)]
        public virtual BigInteger Amount { get; set; }

        }
    
        public event Action<WithdrawEventDTO> OnWithdraw;
        
        private void Withdraw(WithdrawEventDTO withdraw)
        {
            OnWithdraw?.Invoke(withdraw);
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

			await EventManager.Unsubscribe<AllocatedEventDTO>(Allocated, ContractAddress);
			OnAllocated = null;
			await EventManager.Unsubscribe<AmountPerUnitSetEventDTO>(AmountPerUnitSet, ContractAddress);
			OnAmountPerUnitSet = null;
			await EventManager.Unsubscribe<ApprovalForAllEventDTO>(ApprovalForAll, ContractAddress);
			OnApprovalForAll = null;
			await EventManager.Unsubscribe<BoxesRecoveredEventDTO>(BoxesRecovered, ContractAddress);
			OnBoxesRecovered = null;
			await EventManager.Unsubscribe<EmergencyModeEnabledEventDTO>(EmergencyModeEnabled, ContractAddress);
			OnEmergencyModeEnabled = null;
			await EventManager.Unsubscribe<EmergencyWithdrawalEventDTO>(EmergencyWithdrawal, ContractAddress);
			OnEmergencyWithdrawal = null;
			await EventManager.Unsubscribe<OpenRequestFailedEventDTO>(OpenRequestFailed, ContractAddress);
			OnOpenRequestFailed = null;
			await EventManager.Unsubscribe<OpenRequestFulfilledEventDTO>(OpenRequestFulfilled, ContractAddress);
			OnOpenRequestFulfilled = null;
			await EventManager.Unsubscribe<OpenRequestedEventDTO>(OpenRequested, ContractAddress);
			OnOpenRequested = null;
			await EventManager.Unsubscribe<PausedEventDTO>(Paused, ContractAddress);
			OnPaused = null;
			await EventManager.Unsubscribe<PriceUpdatedEventDTO>(PriceUpdated, ContractAddress);
			OnPriceUpdated = null;
			await EventManager.Unsubscribe<RewardsClaimedEventDTO>(RewardsClaimed, ContractAddress);
			OnRewardsClaimed = null;
			await EventManager.Unsubscribe<RoleAdminChangedEventDTO>(RoleAdminChanged, ContractAddress);
			OnRoleAdminChanged = null;
			await EventManager.Unsubscribe<RoleGrantedEventDTO>(RoleGranted, ContractAddress);
			OnRoleGranted = null;
			await EventManager.Unsubscribe<RoleRevokedEventDTO>(RoleRevoked, ContractAddress);
			OnRoleRevoked = null;
			await EventManager.Unsubscribe<SoldEventDTO>(Sold, ContractAddress);
			OnSold = null;
			await EventManager.Unsubscribe<SupplierAddedEventDTO>(SupplierAdded, ContractAddress);
			OnSupplierAdded = null;
			await EventManager.Unsubscribe<SupplierRemovedEventDTO>(SupplierRemoved, ContractAddress);
			OnSupplierRemoved = null;
			await EventManager.Unsubscribe<TokenAddedEventDTO>(TokenAdded, ContractAddress);
			OnTokenAdded = null;
			await EventManager.Unsubscribe<TransferBatchEventDTO>(TransferBatch, ContractAddress);
			OnTransferBatch = null;
			await EventManager.Unsubscribe<TransferSingleEventDTO>(TransferSingle, ContractAddress);
			OnTransferSingle = null;
			await EventManager.Unsubscribe<URIEventDTO>(URI, ContractAddress);
			OnURI = null;
			await EventManager.Unsubscribe<UnpausedEventDTO>(Unpaused, ContractAddress);
			OnUnpaused = null;
			await EventManager.Unsubscribe<WithdrawEventDTO>(Withdraw, ContractAddress);
			OnWithdraw = null;

            
            
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

                await EventManager.Subscribe<AllocatedEventDTO>(Allocated, ContractAddress);
                await EventManager.Subscribe<AmountPerUnitSetEventDTO>(AmountPerUnitSet, ContractAddress);
                await EventManager.Subscribe<ApprovalForAllEventDTO>(ApprovalForAll, ContractAddress);
                await EventManager.Subscribe<BoxesRecoveredEventDTO>(BoxesRecovered, ContractAddress);
                await EventManager.Subscribe<EmergencyModeEnabledEventDTO>(EmergencyModeEnabled, ContractAddress);
                await EventManager.Subscribe<EmergencyWithdrawalEventDTO>(EmergencyWithdrawal, ContractAddress);
                await EventManager.Subscribe<OpenRequestFailedEventDTO>(OpenRequestFailed, ContractAddress);
                await EventManager.Subscribe<OpenRequestFulfilledEventDTO>(OpenRequestFulfilled, ContractAddress);
                await EventManager.Subscribe<OpenRequestedEventDTO>(OpenRequested, ContractAddress);
                await EventManager.Subscribe<PausedEventDTO>(Paused, ContractAddress);
                await EventManager.Subscribe<PriceUpdatedEventDTO>(PriceUpdated, ContractAddress);
                await EventManager.Subscribe<RewardsClaimedEventDTO>(RewardsClaimed, ContractAddress);
                await EventManager.Subscribe<RoleAdminChangedEventDTO>(RoleAdminChanged, ContractAddress);
                await EventManager.Subscribe<RoleGrantedEventDTO>(RoleGranted, ContractAddress);
                await EventManager.Subscribe<RoleRevokedEventDTO>(RoleRevoked, ContractAddress);
                await EventManager.Subscribe<SoldEventDTO>(Sold, ContractAddress);
                await EventManager.Subscribe<SupplierAddedEventDTO>(SupplierAdded, ContractAddress);
                await EventManager.Subscribe<SupplierRemovedEventDTO>(SupplierRemoved, ContractAddress);
                await EventManager.Subscribe<TokenAddedEventDTO>(TokenAdded, ContractAddress);
                await EventManager.Subscribe<TransferBatchEventDTO>(TransferBatch, ContractAddress);
                await EventManager.Subscribe<TransferSingleEventDTO>(TransferSingle, ContractAddress);
                await EventManager.Subscribe<URIEventDTO>(URI, ContractAddress);
                await EventManager.Subscribe<UnpausedEventDTO>(Unpaused, ContractAddress);
                await EventManager.Subscribe<WithdrawEventDTO>(Withdraw, ContractAddress);
    
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

public class Tuple
{
            [Parameter("address", "rewardToken", 0, false)]
        public virtual string RewardToken { get; set; }
        [Parameter("uint8", "rewardType", 1, false)]
        public virtual BigInteger RewardType { get; set; }
        [Parameter("uint256", "units", 2, false)]
        public virtual BigInteger Units { get; set; }
        [Parameter("uint256", "amountPerUnit", 3, false)]
        public virtual BigInteger AmountPerUnit { get; set; }
        [Parameter("uint256", "balance", 4, false)]
        public virtual BigInteger Balance { get; set; }
        [Parameter("tuple[]", "extra", 5, false)]
        public virtual Tuple[] Extra { get; set; }

}
}
