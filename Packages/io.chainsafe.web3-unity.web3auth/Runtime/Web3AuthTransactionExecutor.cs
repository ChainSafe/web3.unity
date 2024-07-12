using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChainSafe.Gaming.Evm.Providers;
using ChainSafe.Gaming.Evm.Transactions;
using ChainSafe.Gaming.InProcessSigner;
using ChainSafe.Gaming.InProcessTransactionExecutor;
using ChainSafe.Gaming.Web3;
using ChainSafe.GamingSdk.Web3Auth;

/// <summary>
/// Send Transaction for Web3Auth wallet.
/// </summary>
public class Web3AuthTransactionExecutor : InProcessTransactionExecutor, IWeb3AuthTransactionHandler
{
    public event Action<TransactionRequest> OnTransactionRequested;
        
    public event Action<TransactionResponse> OnTransactionConfirmed;
        
    private readonly Dictionary<string, (TransactionRequest request, TaskCompletionSource<TransactionResponse> response)> _transactionPool = new();
    
    public Web3AuthTransactionExecutor(IAccountProvider accountProvider, IRpcProvider rpcProvider) : base(accountProvider, rpcProvider)
    {
    }

    public override Task<TransactionResponse> SendTransaction(TransactionRequest transaction)
    {
        transaction.Id = Guid.NewGuid().ToString();
        
        var tcs = new TaskCompletionSource<TransactionResponse>();
        
        // Add transaction to pool.
        _transactionPool.Add(transaction.Id, (transaction, tcs));
            
        OnTransactionRequested?.Invoke(transaction);
            
        return tcs.Task;
    }
    
    public async void TransactionApproved(string transactionId)
    {
        if (!_transactionPool.TryGetValue(transactionId, out var transaction))
        {
            throw new Web3Exception("Transaction not found in pool.");
        }
        
        var response = await base.SendTransaction(transaction.request);
            
        transaction.response.SetResult(response);

        OnTransactionConfirmed?.Invoke(response);
        
        _transactionPool.Remove(transactionId);
    }
        
    public void TransactionDeclined(string transactionId)
    {
        if (!_transactionPool.TryGetValue(transactionId, out var transaction))
        {
            throw new Web3Exception("Transaction not found in pool.");
        }
        
        transaction.response.SetCanceled();

        _transactionPool.Remove(transactionId);
    }
}
