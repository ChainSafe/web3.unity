using ChainSafe.Gaming.Evm.Transactions;

namespace ChainSafe.GamingSdk.Web3Auth
{
    public struct TransactionRequested
    {
        public string Id { get; private set; }

        public TransactionRequest Transaction { get; private set; }

        public TransactionRequested(string id, TransactionRequest transaction)
        {
            Id = id;
            Transaction = transaction;
        }
    }

    public struct TransactionConfirmed
    {
        public TransactionResponse Transaction { get; private set; }

        public TransactionConfirmed(TransactionResponse transaction)
        {
            Transaction = transaction;
        }
    }

    public struct TransactionApproved
    {
        public string Id { get; private set; }

        public TransactionApproved(string id)
        {
            Id = id;
        }
    }

    public struct TransactionDeclined
    {
        public string Id { get; private set; }

        public TransactionDeclined(string id)
        {
            Id = id;
        }
    }
}