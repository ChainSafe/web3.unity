using System.Collections.Generic;
using System.Linq;
using ChainSafe.Gaming.Evm.Transactions;
using ChainSafe.Gaming.Web3;

namespace ChainSafe.Gaming.EmbeddedWallet
{
    public class TransactionPool
    {
        private readonly Dictionary<int, EmbeddedWalletTransaction> transactions = new();

        private int count = 0;

        private int Index => transactions.Keys.Min();

        public int Count => transactions.Count;

        public EmbeddedWalletTransaction Enqueue(TransactionRequest request)
        {
            var transaction = new EmbeddedWalletTransaction(request);

            transactions.Add(count++, transaction);

            return transaction;
        }

        public EmbeddedWalletTransaction Peek()
        {
            return transactions[Index];
        }

        public EmbeddedWalletTransaction Dequeue()
        {
            var transaction = transactions[Index];

            transactions.Remove(Index);

            return transaction;
        }

        public void AssertTransaction(int index)
        {
            if (!transactions.ContainsKey(index))
            {
                throw new Web3Exception("Transaction not found.");
            }
        }
    }
}