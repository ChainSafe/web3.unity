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

        public Task<Transfer<T>> CreateTransfer<T>(string sourceAddress, uint destinationChainId, string destinationAddress, string assetResourceId, HexBigInteger amount)
            where T : TransferType;

        public Task<EvmFee> Fee<T>(Transfer<T> transfer)
            where T : TransferType;

        public Task<List<Transaction>> BuildApprovals<T>(Transfer<T> transfer, EvmFee fee)
            where T : TransferType;

        public Task<Transaction> BuildTransferTransaction<T>(Transfer<T> transfer, EvmFee fee)
            where T : TransferType;

        public Task<TransferStatus> TransferStatusData(Environment environment, string transactionHash);
    }
}