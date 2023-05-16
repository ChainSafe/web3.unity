using System;
using System.Threading.Tasks;
using ChainSafe.GamingWeb3.Environment;
using Web3Unity.Scripts.Library.Ethers.Unity;

namespace ChainSafe.GamingSdk.Evm.Unity
{
    public class UnityDispatcherAdapter : IMainThreadRunner
    {
        private readonly Dispatcher dispatcher;

        public UnityDispatcherAdapter()
        {
            dispatcher = Dispatcher.Initialize();
        }

        public void Enqueue(Action action) => dispatcher.Enqueue(action);

        public Task EnqueueTask(Func<Task> task) => dispatcher.EnqueueTask(task);

        public Task<T> EnqueueTask<T>(Func<Task<T>> task) => dispatcher.EnqueueTask(task);
    }
}