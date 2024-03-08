using System.Collections.Generic;
using System.Threading.Tasks;
using ChainSafe.Gaming.Evm.Transactions;
using ChainSafe.Gaming.SygmaClient.Dto;
using Nethereum.Hex.HexTypes;

namespace ChainSafe.Gaming.SygmaClient.Types
{
    public interface ISygmaClient
    {
        public bool Initialize(Environment environment);

        public Task<Transfer<NonFungible>> CreateNonFungibleTransfer(
            NonFungibleTransferType type,
            string sourceAddress,
            uint destinationChainId,
            string destinationAddress,
            string resourceId,
            string tokenId,
            HexBigInteger amount = null,
            string destinationProviderUrl = "");

        public Task<Transfer<Fungible>> CreateFungibleTransfer(
            string sourceAddress,
            uint destinationChainId,
            string destinationAddress,
            string resourceId,
            HexBigInteger amount,
            string destinationProviderUrl = "");

        public Task<EvmFee> Fee<T>(Transfer<T> transfer)
            where T : TransferType;

        public Task<TransactionRequest> BuildApprovals<T>(Transfer<T> transfer, EvmFee fee, string tokenAddress)
            where T : TransferType;

        public Task<TransactionRequest> BuildTransferTransaction<T>(Transfer<T> transfer, EvmFee fee)
            where T : TransferType;

        public Task<TransferStatus> TransferStatusData(Environment environment, string transactionHash);
    }
}