using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChainSafe.Gaming.Evm.Providers;
using ChainSafe.Gaming.Evm.Transactions;
using ChainSafe.Gaming.InProcessSigner;
using ChainSafe.Gaming.InProcessTransactionExecutor;
using ChainSafe.GamingSdk.Web3Auth;

/// <summary>
/// Send Transaction for Web3Auth wallet.
/// </summary>
public class Web3AuthTransactionExecutor : InProcessTransactionExecutor, IWeb3AuthTransactionHandler
{
    public event Action<(string id, TransactionRequest request)> OnTransactionRequested;
        
    public event Action<TransactionResponse> OnTransactionConfirmed;
        
    private readonly Dictionary<string, (TransactionRequest request, TaskCompletionSource<TransactionResponse> response)> _transactionPool = new();
    
    public Web3AuthTransactionExecutor(AccountProvider accountProvider, IRpcProvider rpcProvider) : base(accountProvider, rpcProvider)
    {
    }

    public override Task<TransactionResponse> SendTransaction(TransactionRequest transaction)
    {
        string id = Guid.NewGuid().ToString();

        var tcs = new TaskCompletionSource<TransactionResponse>();
        
        // Add transaction to pool.
        _transactionPool.Add(id, (transaction, tcs));
            
        OnTransactionRequested?.Invoke((id, transaction));
            
        return tcs.Task;
    }
    
    public async void TransactionApproved(string transactionId)
    {
        var pair = _transactionPool.Single(t => t.Key == transactionId);
            
        var response = await base.SendTransaction(pair.Value.request);
            
        pair.Value.response.SetResult(response);

        OnTransactionConfirmed?.Invoke(response);
    }
        
    public void TransactionDeclined(string transactionId)
    {
        var pair = _transactionPool.Single(t => t.Key == transactionId);
            
        pair.Value.response.SetCanceled();
    }
}
