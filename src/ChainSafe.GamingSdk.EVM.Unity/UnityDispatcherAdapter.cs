using System;
using System.Threading.Tasks;
using ChainSafe.GamingWeb3.Environment;
using Web3Unity.Scripts.Library.Ethers.Unity;

namespace ChainSafe.GamingSdk.Evm.Unity
{
    public class UnityDispatcherAdapter : IMainThreadRunner
    {
        private readonly Dispatcher _dispatcher;

        public UnityDispatcherAdapter()
        {
            _dispatcher = Dispatcher.Initialize();
        }

        public void Enqueue(Action action) => _dispatcher.Enqueue(action);
        public Task EnqueueTask(Func<Task> task) => _dispatcher.EnqueueTask(task);
        public Task<T> EnqueueTask<T>(Func<Task<T>> task) => _dispatcher.EnqueueTask(task);
    }
}