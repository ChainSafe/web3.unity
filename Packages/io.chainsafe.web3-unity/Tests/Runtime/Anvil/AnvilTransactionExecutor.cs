using ChainSafe.Gaming.Evm.Providers;
using ChainSafe.Gaming.InProcessSigner;
using ChainSafe.Gaming.InProcessTransactionExecutor;

public class AnvilTransactionExecutor : InProcessTransactionExecutor
{
    public AnvilTransactionExecutor(IAccountProvider accountProvider, IRpcProvider rpcProvider) : base(accountProvider, rpcProvider)
    {
    }
}
