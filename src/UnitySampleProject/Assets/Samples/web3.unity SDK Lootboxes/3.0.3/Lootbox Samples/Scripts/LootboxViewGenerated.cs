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
    public partial class LootboxViewGenerated : ICustomContract
    {
        public string Address => OriginalContract.Address;
       
        public string ABI => "[ { \"inputs\": [ { \"internalType\": \"address\", \"name\": \"_link\", \"type\": \"address\" }, { \"internalType\": \"address\", \"name\": \"_vrfV2Wrapper\", \"type\": \"address\" }, { \"internalType\": \"address payable\", \"name\": \"_factory\", \"type\": \"address\" } ], \"stateMutability\": \"nonpayable\", \"type\": \"constructor\" }, { \"inputs\": [], \"name\": \"AccessControlBadConfirmation\", \"type\": \"error\" }, { \"inputs\": [ { \"internalType\": \"address\", \"name\": \"account\", \"type\": \"address\" }, { \"internalType\": \"bytes32\", \"name\": \"neededRole\", \"type\": \"bytes32\" } ], \"name\": \"AccessControlUnauthorizedAccount\", \"type\": \"error\" }, { \"inputs\": [ { \"internalType\": \"address\", \"name\": \"sender\", \"type\": \"address\" }, { \"internalType\": \"uint256\", \"name\": \"balance\", \"type\": \"uint256\" }, { \"internalType\": \"uint256\", \"name\": \"needed\", \"type\": \"uint256\" }, { \"internalType\": \"uint256\", \"name\": \"tokenId\", \"type\": \"uint256\" } ], \"name\": \"ERC1155InsufficientBalance\", \"type\": \"error\" }, { \"inputs\": [ { \"internalType\": \"address\", \"name\": \"approver\", \"type\": \"address\" } ], \"name\": \"ERC1155InvalidApprover\", \"type\": \"error\" }, { \"inputs\": [ { \"internalType\": \"uint256\", \"name\": \"idsLength\", \"type\": \"uint256\" }, { \"internalType\": \"uint256\", \"name\": \"valuesLength\", \"type\": \"uint256\" } ], \"name\": \"ERC1155InvalidArrayLength\", \"type\": \"error\" }, { \"inputs\": [ { \"internalType\": \"address\", \"name\": \"operator\", \"type\": \"address\" } ], \"name\": \"ERC1155InvalidOperator\", \"type\": \"error\" }, { \"inputs\": [ { \"internalType\": \"address\", \"name\": \"receiver\", \"type\": \"address\" } ], \"name\": \"ERC1155InvalidReceiver\", \"type\": \"error\" }, { \"inputs\": [ { \"internalType\": \"address\", \"name\": \"sender\", \"type\": \"address\" } ], \"name\": \"ERC1155InvalidSender\", \"type\": \"error\" }, { \"inputs\": [ { \"internalType\": \"address\", \"name\": \"operator\", \"type\": \"address\" }, { \"internalType\": \"address\", \"name\": \"owner\", \"type\": \"address\" } ], \"name\": \"ERC1155MissingApprovalForAll\", \"type\": \"error\" }, { \"inputs\": [], \"name\": \"EnforcedPause\", \"type\": \"error\" }, { \"inputs\": [], \"name\": \"ExpectedPause\", \"type\": \"error\" }, { \"inputs\": [ { \"internalType\": \"int256\", \"name\": \"value\", \"type\": \"int256\" } ], \"name\": \"InvalidLinkPrice\", \"type\": \"error\" }, { \"anonymous\": false, \"inputs\": [ { \"indexed\": true, \"internalType\": \"address\", \"name\": \"account\", \"type\": \"address\" }, { \"indexed\": true, \"internalType\": \"address\", \"name\": \"operator\", \"type\": \"address\" }, { \"indexed\": false, \"internalType\": \"bool\", \"name\": \"approved\", \"type\": \"bool\" } ], \"name\": \"ApprovalForAll\", \"type\": \"event\" }, { \"anonymous\": false, \"inputs\": [ { \"indexed\": false, \"internalType\": \"address\", \"name\": \"account\", \"type\": \"address\" } ], \"name\": \"Paused\", \"type\": \"event\" }, { \"anonymous\": false, \"inputs\": [ { \"indexed\": true, \"internalType\": \"bytes32\", \"name\": \"role\", \"type\": \"bytes32\" }, { \"indexed\": true, \"internalType\": \"bytes32\", \"name\": \"previousAdminRole\", \"type\": \"bytes32\" }, { \"indexed\": true, \"internalType\": \"bytes32\", \"name\": \"newAdminRole\", \"type\": \"bytes32\" } ], \"name\": \"RoleAdminChanged\", \"type\": \"event\" }, { \"anonymous\": false, \"inputs\": [ { \"indexed\": true, \"internalType\": \"bytes32\", \"name\": \"role\", \"type\": \"bytes32\" }, { \"indexed\": true, \"internalType\": \"address\", \"name\": \"account\", \"type\": \"address\" }, { \"indexed\": true, \"internalType\": \"address\", \"name\": \"sender\", \"type\": \"address\" } ], \"name\": \"RoleGranted\", \"type\": \"event\" }, { \"anonymous\": false, \"inputs\": [ { \"indexed\": true, \"internalType\": \"bytes32\", \"name\": \"role\", \"type\": \"bytes32\" }, { \"indexed\": true, \"internalType\": \"address\", \"name\": \"account\", \"type\": \"address\" }, { \"indexed\": true, \"internalType\": \"address\", \"name\": \"sender\", \"type\": \"address\" } ], \"name\": \"RoleRevoked\", \"type\": \"event\" }, { \"anonymous\": false, \"inputs\": [ { \"indexed\": true, \"internalType\": \"address\", \"name\": \"operator\", \"type\": \"address\" }, { \"indexed\": true, \"internalType\": \"address\", \"name\": \"from\", \"type\": \"address\" }, { \"indexed\": true, \"internalType\": \"address\", \"name\": \"to\", \"type\": \"address\" }, { \"indexed\": false, \"internalType\": \"uint256[]\", \"name\": \"ids\", \"type\": \"uint256[]\" }, { \"indexed\": false, \"internalType\": \"uint256[]\", \"name\": \"values\", \"type\": \"uint256[]\" } ], \"name\": \"TransferBatch\", \"type\": \"event\" }, { \"anonymous\": false, \"inputs\": [ { \"indexed\": true, \"internalType\": \"address\", \"name\": \"operator\", \"type\": \"address\" }, { \"indexed\": true, \"internalType\": \"address\", \"name\": \"from\", \"type\": \"address\" }, { \"indexed\": true, \"internalType\": \"address\", \"name\": \"to\", \"type\": \"address\" }, { \"indexed\": false, \"internalType\": \"uint256\", \"name\": \"id\", \"type\": \"uint256\" }, { \"indexed\": false, \"internalType\": \"uint256\", \"name\": \"value\", \"type\": \"uint256\" } ], \"name\": \"TransferSingle\", \"type\": \"event\" }, { \"anonymous\": false, \"inputs\": [ { \"indexed\": false, \"internalType\": \"string\", \"name\": \"value\", \"type\": \"string\" }, { \"indexed\": true, \"internalType\": \"uint256\", \"name\": \"id\", \"type\": \"uint256\" } ], \"name\": \"URI\", \"type\": \"event\" }, { \"anonymous\": false, \"inputs\": [ { \"indexed\": false, \"internalType\": \"address\", \"name\": \"account\", \"type\": \"address\" } ], \"name\": \"Unpaused\", \"type\": \"event\" }, { \"inputs\": [], \"name\": \"DEFAULT_ADMIN_ROLE\", \"outputs\": [ { \"internalType\": \"bytes32\", \"name\": \"\", \"type\": \"bytes32\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [], \"name\": \"FACTORY\", \"outputs\": [ { \"internalType\": \"contract ILootboxFactory\", \"name\": \"\", \"type\": \"address\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [], \"name\": \"LINK\", \"outputs\": [ { \"internalType\": \"address\", \"name\": \"\", \"type\": \"address\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [], \"name\": \"LINK_ETH_FEED\", \"outputs\": [ { \"internalType\": \"contract AggregatorV3Interface\", \"name\": \"\", \"type\": \"address\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [], \"name\": \"MINTER_ROLE\", \"outputs\": [ { \"internalType\": \"bytes32\", \"name\": \"\", \"type\": \"bytes32\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [], \"name\": \"PAUSER_ROLE\", \"outputs\": [ { \"internalType\": \"bytes32\", \"name\": \"\", \"type\": \"bytes32\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [], \"name\": \"VRF_V2_WRAPPER\", \"outputs\": [ { \"internalType\": \"contract VRFV2WrapperInterface\", \"name\": \"\", \"type\": \"address\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [ { \"internalType\": \"address\", \"name\": \"account\", \"type\": \"address\" }, { \"internalType\": \"uint256\", \"name\": \"id\", \"type\": \"uint256\" } ], \"name\": \"balanceOf\", \"outputs\": [ { \"internalType\": \"uint256\", \"name\": \"\", \"type\": \"uint256\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [ { \"internalType\": \"address[]\", \"name\": \"accounts\", \"type\": \"address[]\" }, { \"internalType\": \"uint256[]\", \"name\": \"ids\", \"type\": \"uint256[]\" } ], \"name\": \"balanceOfBatch\", \"outputs\": [ { \"internalType\": \"uint256[]\", \"name\": \"\", \"type\": \"uint256[]\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [ { \"internalType\": \"address\", \"name\": \"account\", \"type\": \"address\" }, { \"internalType\": \"uint256\", \"name\": \"id\", \"type\": \"uint256\" }, { \"internalType\": \"uint256\", \"name\": \"value\", \"type\": \"uint256\" } ], \"name\": \"burn\", \"outputs\": [], \"stateMutability\": \"nonpayable\", \"type\": \"function\" }, { \"inputs\": [ { \"internalType\": \"address\", \"name\": \"account\", \"type\": \"address\" }, { \"internalType\": \"uint256[]\", \"name\": \"ids\", \"type\": \"uint256[]\" }, { \"internalType\": \"uint256[]\", \"name\": \"values\", \"type\": \"uint256[]\" } ], \"name\": \"burnBatch\", \"outputs\": [], \"stateMutability\": \"nonpayable\", \"type\": \"function\" }, { \"inputs\": [ { \"internalType\": \"uint32\", \"name\": \"\", \"type\": \"uint32\" }, { \"internalType\": \"uint256\", \"name\": \"\", \"type\": \"uint256\" }, { \"internalType\": \"uint256\", \"name\": \"_units\", \"type\": \"uint256\" } ], \"name\": \"calculateOpenPrice\", \"outputs\": [ { \"internalType\": \"uint256\", \"name\": \"\", \"type\": \"uint256\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [ { \"internalType\": \"address\", \"name\": \"_opener\", \"type\": \"address\" } ], \"name\": \"canClaimRewards\", \"outputs\": [ { \"internalType\": \"bool\", \"name\": \"\", \"type\": \"bool\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [], \"name\": \"getAllowedTokenTypes\", \"outputs\": [ { \"internalType\": \"enum LootboxView.RewardType[]\", \"name\": \"result\", \"type\": \"uint8[]\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [], \"name\": \"getAllowedTokens\", \"outputs\": [ { \"internalType\": \"address[]\", \"name\": \"\", \"type\": \"address[]\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [], \"name\": \"getAvailableSupply\", \"outputs\": [ { \"internalType\": \"uint256\", \"name\": \"\", \"type\": \"uint256\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [], \"name\": \"getInventory\", \"outputs\": [ { \"components\": [ { \"internalType\": \"address\", \"name\": \"rewardToken\", \"type\": \"address\" }, { \"internalType\": \"enum LootboxView.RewardType\", \"name\": \"rewardType\", \"type\": \"uint8\" }, { \"internalType\": \"uint256\", \"name\": \"units\", \"type\": \"uint256\" }, { \"internalType\": \"uint256\", \"name\": \"amountPerUnit\", \"type\": \"uint256\" }, { \"internalType\": \"uint256\", \"name\": \"balance\", \"type\": \"uint256\" }, { \"components\": [ { \"internalType\": \"uint256\", \"name\": \"id\", \"type\": \"uint256\" }, { \"internalType\": \"uint256\", \"name\": \"units\", \"type\": \"uint256\" }, { \"internalType\": \"uint256\", \"name\": \"amountPerUnit\", \"type\": \"uint256\" }, { \"internalType\": \"uint256\", \"name\": \"balance\", \"type\": \"uint256\" } ], \"internalType\": \"struct LootboxView.ExtraRewardInfo[]\", \"name\": \"extra\", \"type\": \"tuple[]\" } ], \"internalType\": \"struct LootboxView.RewardView[]\", \"name\": \"result\", \"type\": \"tuple[]\" }, { \"components\": [ { \"internalType\": \"address\", \"name\": \"rewardToken\", \"type\": \"address\" }, { \"internalType\": \"enum LootboxView.RewardType\", \"name\": \"rewardType\", \"type\": \"uint8\" }, { \"internalType\": \"uint256\", \"name\": \"units\", \"type\": \"uint256\" }, { \"internalType\": \"uint256\", \"name\": \"amountPerUnit\", \"type\": \"uint256\" }, { \"internalType\": \"uint256\", \"name\": \"balance\", \"type\": \"uint256\" }, { \"components\": [ { \"internalType\": \"uint256\", \"name\": \"id\", \"type\": \"uint256\" }, { \"internalType\": \"uint256\", \"name\": \"units\", \"type\": \"uint256\" }, { \"internalType\": \"uint256\", \"name\": \"amountPerUnit\", \"type\": \"uint256\" }, { \"internalType\": \"uint256\", \"name\": \"balance\", \"type\": \"uint256\" } ], \"internalType\": \"struct LootboxView.ExtraRewardInfo[]\", \"name\": \"extra\", \"type\": \"tuple[]\" } ], \"internalType\": \"struct LootboxView.RewardView[]\", \"name\": \"leftoversResult\", \"type\": \"tuple[]\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [], \"name\": \"getLink\", \"outputs\": [ { \"internalType\": \"address\", \"name\": \"\", \"type\": \"address\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [], \"name\": \"getLinkPrice\", \"outputs\": [ { \"internalType\": \"uint256\", \"name\": \"\", \"type\": \"uint256\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [], \"name\": \"getLootboxTypes\", \"outputs\": [ { \"internalType\": \"uint256[]\", \"name\": \"\", \"type\": \"uint256[]\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [ { \"internalType\": \"address\", \"name\": \"_opener\", \"type\": \"address\" } ], \"name\": \"getOpenerRequestDetails\", \"outputs\": [ { \"components\": [ { \"internalType\": \"address\", \"name\": \"opener\", \"type\": \"address\" }, { \"internalType\": \"uint96\", \"name\": \"unitsToGet\", \"type\": \"uint96\" }, { \"internalType\": \"uint256[]\", \"name\": \"lootIds\", \"type\": \"uint256[]\" }, { \"internalType\": \"uint256[]\", \"name\": \"lootAmounts\", \"type\": \"uint256[]\" } ], \"internalType\": \"struct LootboxView.Request\", \"name\": \"request\", \"type\": \"tuple\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [], \"name\": \"getPrice\", \"outputs\": [ { \"internalType\": \"uint256\", \"name\": \"\", \"type\": \"uint256\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [ { \"internalType\": \"bytes32\", \"name\": \"role\", \"type\": \"bytes32\" } ], \"name\": \"getRoleAdmin\", \"outputs\": [ { \"internalType\": \"bytes32\", \"name\": \"\", \"type\": \"bytes32\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [], \"name\": \"getSuppliers\", \"outputs\": [ { \"internalType\": \"address[]\", \"name\": \"\", \"type\": \"address[]\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [], \"name\": \"getVRFV2Wrapper\", \"outputs\": [ { \"internalType\": \"address\", \"name\": \"\", \"type\": \"address\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [ { \"internalType\": \"bytes32\", \"name\": \"role\", \"type\": \"bytes32\" }, { \"internalType\": \"address\", \"name\": \"account\", \"type\": \"address\" } ], \"name\": \"grantRole\", \"outputs\": [], \"stateMutability\": \"nonpayable\", \"type\": \"function\" }, { \"inputs\": [ { \"internalType\": \"bytes32\", \"name\": \"role\", \"type\": \"bytes32\" }, { \"internalType\": \"address\", \"name\": \"account\", \"type\": \"address\" } ], \"name\": \"hasRole\", \"outputs\": [ { \"internalType\": \"bool\", \"name\": \"\", \"type\": \"bool\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [ { \"internalType\": \"address\", \"name\": \"account\", \"type\": \"address\" }, { \"internalType\": \"address\", \"name\": \"operator\", \"type\": \"address\" } ], \"name\": \"isApprovedForAll\", \"outputs\": [ { \"internalType\": \"bool\", \"name\": \"\", \"type\": \"bool\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [], \"name\": \"isEmergencyMode\", \"outputs\": [ { \"internalType\": \"bool\", \"name\": \"\", \"type\": \"bool\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [ { \"internalType\": \"address\", \"name\": \"account\", \"type\": \"address\" }, { \"internalType\": \"uint256\", \"name\": \"id\", \"type\": \"uint256\" }, { \"internalType\": \"uint256\", \"name\": \"amount\", \"type\": \"uint256\" }, { \"internalType\": \"bytes\", \"name\": \"data\", \"type\": \"bytes\" } ], \"name\": \"mint\", \"outputs\": [], \"stateMutability\": \"nonpayable\", \"type\": \"function\" }, { \"inputs\": [ { \"internalType\": \"address\", \"name\": \"to\", \"type\": \"address\" }, { \"internalType\": \"uint256[]\", \"name\": \"ids\", \"type\": \"uint256[]\" }, { \"internalType\": \"uint256[]\", \"name\": \"amounts\", \"type\": \"uint256[]\" }, { \"internalType\": \"bytes\", \"name\": \"data\", \"type\": \"bytes\" } ], \"name\": \"mintBatch\", \"outputs\": [], \"stateMutability\": \"nonpayable\", \"type\": \"function\" }, { \"inputs\": [ { \"internalType\": \"address\", \"name\": \"\", \"type\": \"address\" }, { \"internalType\": \"address\", \"name\": \"\", \"type\": \"address\" }, { \"internalType\": \"uint256[]\", \"name\": \"\", \"type\": \"uint256[]\" }, { \"internalType\": \"uint256[]\", \"name\": \"\", \"type\": \"uint256[]\" }, { \"internalType\": \"bytes\", \"name\": \"\", \"type\": \"bytes\" } ], \"name\": \"onERC1155BatchReceived\", \"outputs\": [ { \"internalType\": \"bytes4\", \"name\": \"\", \"type\": \"bytes4\" } ], \"stateMutability\": \"nonpayable\", \"type\": \"function\" }, { \"inputs\": [ { \"internalType\": \"address\", \"name\": \"\", \"type\": \"address\" }, { \"internalType\": \"address\", \"name\": \"\", \"type\": \"address\" }, { \"internalType\": \"uint256\", \"name\": \"\", \"type\": \"uint256\" }, { \"internalType\": \"uint256\", \"name\": \"\", \"type\": \"uint256\" }, { \"internalType\": \"bytes\", \"name\": \"\", \"type\": \"bytes\" } ], \"name\": \"onERC1155Received\", \"outputs\": [ { \"internalType\": \"bytes4\", \"name\": \"\", \"type\": \"bytes4\" } ], \"stateMutability\": \"nonpayable\", \"type\": \"function\" }, { \"inputs\": [ { \"internalType\": \"address\", \"name\": \"\", \"type\": \"address\" }, { \"internalType\": \"address\", \"name\": \"\", \"type\": \"address\" }, { \"internalType\": \"uint256\", \"name\": \"\", \"type\": \"uint256\" }, { \"internalType\": \"bytes\", \"name\": \"\", \"type\": \"bytes\" } ], \"name\": \"onERC721Received\", \"outputs\": [ { \"internalType\": \"bytes4\", \"name\": \"\", \"type\": \"bytes4\" } ], \"stateMutability\": \"nonpayable\", \"type\": \"function\" }, { \"inputs\": [ { \"internalType\": \"address\", \"name\": \"\", \"type\": \"address\" } ], \"name\": \"openerRequests\", \"outputs\": [ { \"internalType\": \"uint256\", \"name\": \"\", \"type\": \"uint256\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [], \"name\": \"pause\", \"outputs\": [], \"stateMutability\": \"nonpayable\", \"type\": \"function\" }, { \"inputs\": [], \"name\": \"paused\", \"outputs\": [ { \"internalType\": \"bool\", \"name\": \"\", \"type\": \"bool\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [ { \"internalType\": \"bytes32\", \"name\": \"role\", \"type\": \"bytes32\" }, { \"internalType\": \"address\", \"name\": \"callerConfirmation\", \"type\": \"address\" } ], \"name\": \"renounceRole\", \"outputs\": [], \"stateMutability\": \"nonpayable\", \"type\": \"function\" }, { \"inputs\": [ { \"internalType\": \"bytes32\", \"name\": \"role\", \"type\": \"bytes32\" }, { \"internalType\": \"address\", \"name\": \"account\", \"type\": \"address\" } ], \"name\": \"revokeRole\", \"outputs\": [], \"stateMutability\": \"nonpayable\", \"type\": \"function\" }, { \"inputs\": [ { \"internalType\": \"address\", \"name\": \"from\", \"type\": \"address\" }, { \"internalType\": \"address\", \"name\": \"to\", \"type\": \"address\" }, { \"internalType\": \"uint256[]\", \"name\": \"ids\", \"type\": \"uint256[]\" }, { \"internalType\": \"uint256[]\", \"name\": \"values\", \"type\": \"uint256[]\" }, { \"internalType\": \"bytes\", \"name\": \"data\", \"type\": \"bytes\" } ], \"name\": \"safeBatchTransferFrom\", \"outputs\": [], \"stateMutability\": \"nonpayable\", \"type\": \"function\" }, { \"inputs\": [ { \"internalType\": \"address\", \"name\": \"from\", \"type\": \"address\" }, { \"internalType\": \"address\", \"name\": \"to\", \"type\": \"address\" }, { \"internalType\": \"uint256\", \"name\": \"id\", \"type\": \"uint256\" }, { \"internalType\": \"uint256\", \"name\": \"value\", \"type\": \"uint256\" }, { \"internalType\": \"bytes\", \"name\": \"data\", \"type\": \"bytes\" } ], \"name\": \"safeTransferFrom\", \"outputs\": [], \"stateMutability\": \"nonpayable\", \"type\": \"function\" }, { \"inputs\": [ { \"internalType\": \"address\", \"name\": \"operator\", \"type\": \"address\" }, { \"internalType\": \"bool\", \"name\": \"approved\", \"type\": \"bool\" } ], \"name\": \"setApprovalForAll\", \"outputs\": [], \"stateMutability\": \"nonpayable\", \"type\": \"function\" }, { \"inputs\": [ { \"internalType\": \"address\", \"name\": \"_from\", \"type\": \"address\" } ], \"name\": \"supplyAllowed\", \"outputs\": [ { \"internalType\": \"bool\", \"name\": \"\", \"type\": \"bool\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [ { \"internalType\": \"bytes4\", \"name\": \"\", \"type\": \"bytes4\" } ], \"name\": \"supportsInterface\", \"outputs\": [ { \"internalType\": \"bool\", \"name\": \"\", \"type\": \"bool\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [ { \"internalType\": \"address\", \"name\": \"_token\", \"type\": \"address\" } ], \"name\": \"tokenAllowed\", \"outputs\": [ { \"internalType\": \"bool\", \"name\": \"\", \"type\": \"bool\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [], \"name\": \"unitsMinted\", \"outputs\": [ { \"internalType\": \"uint256\", \"name\": \"\", \"type\": \"uint256\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [], \"name\": \"unitsRequested\", \"outputs\": [ { \"internalType\": \"uint256\", \"name\": \"\", \"type\": \"uint256\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [], \"name\": \"unitsSupply\", \"outputs\": [ { \"internalType\": \"uint256\", \"name\": \"\", \"type\": \"uint256\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [], \"name\": \"unpause\", \"outputs\": [], \"stateMutability\": \"nonpayable\", \"type\": \"function\" }, { \"inputs\": [ { \"internalType\": \"uint256\", \"name\": \"\", \"type\": \"uint256\" } ], \"name\": \"uri\", \"outputs\": [ { \"internalType\": \"string\", \"name\": \"\", \"type\": \"string\" } ], \"stateMutability\": \"view\", \"type\": \"function\" } ]";
        
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


        public async Task<string> LINK( TransactionRequest transactionOverwrite=null) 
        {
            var response = await OriginalContract.Call<string>("LINK", new object [] {
                
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


        public async Task<string> VRF_V2_WRAPPER( TransactionRequest transactionOverwrite=null) 
        {
            var response = await OriginalContract.Call<string>("VRF_V2_WRAPPER", new object [] {
                
            }, transactionOverwrite);
            
            return response;
        }


        public async Task<BigInteger> BalanceOf(string account, BigInteger id, TransactionRequest transactionOverwrite=null) 
        {
            var response = await OriginalContract.Call<BigInteger>("balanceOf", new object [] {
                account, id
            }, transactionOverwrite);
            
            return response;
        }


        public async Task<BigInteger[]> BalanceOfBatch(string[] accounts, BigInteger[] ids, TransactionRequest transactionOverwrite=null) 
        {
            var response = await OriginalContract.Call<BigInteger[]>("balanceOfBatch", new object [] {
                accounts, ids
            }, transactionOverwrite);
            
            return response;
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

        public async Task<BigInteger> CalculateOpenPrice(BigInteger param1, BigInteger param2, BigInteger _units, TransactionRequest transactionOverwrite=null) 
        {
            var response = await OriginalContract.Call<BigInteger>("calculateOpenPrice", new object [] {
                param1, param2, _units
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


        public async Task<BigInteger[]> GetLootboxTypes( TransactionRequest transactionOverwrite=null) 
        {
            var response = await OriginalContract.Call<BigInteger[]>("getLootboxTypes", new object [] {
                
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


        public async Task Mint(string account, BigInteger id, BigInteger amount, byte[] data, TransactionRequest transactionOverwrite=null) 
        {
            var response = await OriginalContract.Send("mint", new object [] {
                account, id, amount, data
            }, transactionOverwrite);
            
            
        }
        public async Task<TransactionReceipt> MintWithReceipt(string account, BigInteger id, BigInteger amount, byte[] data, TransactionRequest transactionOverwrite=null) 
        {
            var response = await OriginalContract.SendWithReceipt("mint", new object [] {
                account, id, amount, data
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


        public async Task RenounceRole(byte[] role, string callerConfirmation, TransactionRequest transactionOverwrite=null) 
        {
            var response = await OriginalContract.Send("renounceRole", new object [] {
                role, callerConfirmation
            }, transactionOverwrite);
            
            
        }
        public async Task<TransactionReceipt> RenounceRoleWithReceipt(byte[] role, string callerConfirmation, TransactionRequest transactionOverwrite=null) 
        {
            var response = await OriginalContract.SendWithReceipt("renounceRole", new object [] {
                role, callerConfirmation
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

        public async Task SafeBatchTransferFrom(string from, string to, BigInteger[] ids, BigInteger[] values, byte[] data, TransactionRequest transactionOverwrite=null) 
        {
            var response = await OriginalContract.Send("safeBatchTransferFrom", new object [] {
                from, to, ids, values, data
            }, transactionOverwrite);
            
            
        }
        public async Task<TransactionReceipt> SafeBatchTransferFromWithReceipt(string from, string to, BigInteger[] ids, BigInteger[] values, byte[] data, TransactionRequest transactionOverwrite=null) 
        {
            var response = await OriginalContract.SendWithReceipt("safeBatchTransferFrom", new object [] {
                from, to, ids, values, data
            }, transactionOverwrite);
            
            return response.receipt;
        }

        public async Task SafeTransferFrom(string from, string to, BigInteger id, BigInteger value, byte[] data, TransactionRequest transactionOverwrite=null) 
        {
            var response = await OriginalContract.Send("safeTransferFrom", new object [] {
                from, to, id, value, data
            }, transactionOverwrite);
            
            
        }
        public async Task<TransactionReceipt> SafeTransferFromWithReceipt(string from, string to, BigInteger id, BigInteger value, byte[] data, TransactionRequest transactionOverwrite=null) 
        {
            var response = await OriginalContract.SendWithReceipt("safeTransferFrom", new object [] {
                from, to, id, value, data
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

        public async Task<bool> SupplyAllowed(string _from, TransactionRequest transactionOverwrite=null) 
        {
            var response = await OriginalContract.Call<bool>("supplyAllowed", new object [] {
                _from
            }, transactionOverwrite);
            
            return response;
        }


        public async Task<bool> SupportsInterface(byte[] param1, TransactionRequest transactionOverwrite=null) 
        {
            var response = await OriginalContract.Call<bool>("supportsInterface", new object [] {
                param1
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



        #endregion
        
        
        #region Event Classes

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

			await EventManager.Unsubscribe<ApprovalForAllEventDTO>(ApprovalForAll, ContractAddress);
			OnApprovalForAll = null;
			await EventManager.Unsubscribe<PausedEventDTO>(Paused, ContractAddress);
			OnPaused = null;
			await EventManager.Unsubscribe<RoleAdminChangedEventDTO>(RoleAdminChanged, ContractAddress);
			OnRoleAdminChanged = null;
			await EventManager.Unsubscribe<RoleGrantedEventDTO>(RoleGranted, ContractAddress);
			OnRoleGranted = null;
			await EventManager.Unsubscribe<RoleRevokedEventDTO>(RoleRevoked, ContractAddress);
			OnRoleRevoked = null;
			await EventManager.Unsubscribe<TransferBatchEventDTO>(TransferBatch, ContractAddress);
			OnTransferBatch = null;
			await EventManager.Unsubscribe<TransferSingleEventDTO>(TransferSingle, ContractAddress);
			OnTransferSingle = null;
			await EventManager.Unsubscribe<URIEventDTO>(URI, ContractAddress);
			OnURI = null;
			await EventManager.Unsubscribe<UnpausedEventDTO>(Unpaused, ContractAddress);
			OnUnpaused = null;

            
            
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

                await EventManager.Subscribe<ApprovalForAllEventDTO>(ApprovalForAll, ContractAddress);
                await EventManager.Subscribe<PausedEventDTO>(Paused, ContractAddress);
                await EventManager.Subscribe<RoleAdminChangedEventDTO>(RoleAdminChanged, ContractAddress);
                await EventManager.Subscribe<RoleGrantedEventDTO>(RoleGranted, ContractAddress);
                await EventManager.Subscribe<RoleRevokedEventDTO>(RoleRevoked, ContractAddress);
                await EventManager.Subscribe<TransferBatchEventDTO>(TransferBatch, ContractAddress);
                await EventManager.Subscribe<TransferSingleEventDTO>(TransferSingle, ContractAddress);
                await EventManager.Subscribe<URIEventDTO>(URI, ContractAddress);
                await EventManager.Subscribe<UnpausedEventDTO>(Unpaused, ContractAddress);
    
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
