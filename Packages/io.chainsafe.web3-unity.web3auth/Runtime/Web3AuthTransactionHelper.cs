using System.Threading.Tasks;
using UnityEngine;

namespace ChainSafe.GamingSdk.Web3Auth
{
    public static class Web3AuthTransactionHelper
    {
        public static TaskCompletionSource<bool> TransactionAcceptedTcs { get; set; }

        public static bool Working { get; set; }

        public static async Task WaitForTransactionAsync()
        {
            Debug.Log("Waiting for Web3AuthWallet tx confirmation");
            TransactionAcceptedTcs = new TaskCompletionSource<bool>();
            Working = true;
            await TransactionAcceptedTcs.Task;
        }

        public static void AcceptTransaction()
        {
            Debug.Log("Accepting Web3AuthWallet transaction");
            TransactionAcceptedTcs.SetResult(true);
            Working = false;
            TransactionAcceptedTcs = null;
        }

        public static void RejectTransaction()
        {
            Debug.Log("Rejecting Web3AuthWallet transaction");
            TransactionAcceptedTcs.SetResult(false);
            Working = false;
        }
    }
}