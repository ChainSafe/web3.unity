using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChainSafe.Gaming.Evm.Providers;
using ChainSafe.Gaming.Evm.Transactions;
using ChainSafe.Gaming.InProcessSigner;
using ChainSafe.Gaming.InProcessTransactionExecutor;
using ChainSafe.GamingSdk.Web3Auth;

public class Web3AuthTransactionExecutor : InProcessTransactionExecutor, IWeb3AuthTransactionHandler
{
    public event Action<TransactionRequested> OnTransactionRequested;
        
    public event Action<TransactionConfirmed> OnTransactionConfirmed;
        
    private readonly Dictionary<TransactionRequested, TaskCompletionSource<TransactionResponse>> _transactionPool =
        new Dictionary<TransactionRequested, TaskCompletionSource<TransactionResponse>>();
    
    public Web3AuthTransactionExecutor(AccountProvider accountProvider, IRpcProvider rpcProvider) : base(accountProvider, rpcProvider)
    {
    }

    public override Task<TransactionResponse> SendTransaction(TransactionRequest transaction)
    {
        string id = Guid.NewGuid().ToString();

        var request = new TransactionRequested(id, transaction);
            
        var tcs = new TaskCompletionSource<TransactionResponse>();
            
        _transactionPool.Add(request, tcs);
            
        RequestTransaction(request);
            
        return tcs.Task;
    }
    
    public void RequestTransaction(TransactionRequested transactionRequested)
    {
        OnTransactionRequested?.Invoke(transactionRequested);
    }

    public void ConfirmTransaction(TransactionConfirmed transactionConfirmed)
    {
        OnTransactionConfirmed?.Invoke(transactionConfirmed);
    }

    public async void ApproveTransaction(TransactionApproved transactionApproved)
    {
        var pair = _transactionPool.Single(t => t.Key.Id == transactionApproved.Id);
            
        var response = await base.SendTransaction(pair.Key.Transaction);
            
        pair.Value.SetResult(response);

        ConfirmTransaction(new TransactionConfirmed(response));
    }
        
    public void DeclineTransaction(TransactionDeclined transactionDeclined)
    {
        var pair = _transactionPool.Single(t => t.Key.Id == transactionDeclined.Id);
            
        pair.Value.SetCanceled();
    }
}
