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
            string sourceAddress,
            string destinationAddress,
            uint destinationChainId,
            ResourceType resourceType,
            string tokenId,
            HexBigInteger amount = null,
            string destinationProviderUrl = "");

        public Task<TransactionResponse> Transfer(SygmaTransferParams transferParams);

        public Task<Transfer<Fungible>> CreateFungibleTransfer(
            string sourceAddress,
            string destinationAddress,
            uint destinationChainId,
            ResourceType resourceType,
            HexBigInteger amount,
            string destinationProviderUrl = "");

        public Task<EvmFee> Fee(Transfer transfer);

        public Task<TransactionRequest> BuildApprovals(Transfer transfer);

        public Task<TransactionRequest> BuildTransferTransaction(Transfer transfer, EvmFee fee);

        public Task<TransferStatus> TransferStatusData(Environment environment, string transactionHash);
    }
}