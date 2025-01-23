using System.Collections.Generic;
using System.Linq;
using ChainSafe.Gaming.Evm.Transactions;
using ChainSafe.Gaming.Web3;

namespace ChainSafe.Gaming.EmbeddedWallet
{
    /// <summary>
    /// Pool for managing embedded wallet transactions.
    /// </summary>
    public class TransactionPool
    {
        private readonly Dictionary<int, EmbeddedWalletTransaction> transactions = new();

        private int count = 0;

        private int Index => transactions.Keys.Min();

        /// <summary>
        /// Number of transactions in the pool.
        /// </summary>
        public int Count => transactions.Count;

        /// <summary>
        /// Add a new transaction to the pool.
        /// </summary>
        /// <param name="request">Initial Transaction Request.</param>
        /// <returns>Transaction for embedded wallet.</returns>
        public EmbeddedWalletTransaction Enqueue(TransactionRequest request)
        {
            var transaction = new EmbeddedWalletTransaction(request);

            transactions.Add(count++, transaction);

            return transaction;
        }

        /// <summary>
        /// Get the next (oldest) transaction in the pool.
        /// </summary>
        /// <returns>Transaction to get.</returns>
        public EmbeddedWalletTransaction Peek()
        {
            AssertTransaction();

            return transactions[Index];
        }

        /// <summary>
        /// Remove the next (oldest) transaction in the pool.
        /// </summary>
        /// <returns>Removed transaction.</returns>
        public EmbeddedWalletTransaction Dequeue()
        {
            AssertTransaction();

            var transaction = transactions[Index];

            transactions.Remove(Index);

            return transaction;
        }

        private void AssertTransaction()
        {
            if (Count.Equals(0))
            {
                throw new Web3Exception("Transaction pool empty.");
            }
        }
    }
}