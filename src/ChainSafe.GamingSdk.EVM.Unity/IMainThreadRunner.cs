using System;
using System.Threading.Tasks;

namespace ChainSafe.GamingSdk.Evm.Unity
{
    public interface IMainThreadRunner
    {
        public void Enqueue(Action action);

        public Task EnqueueTask(Func<Task> task);

        public Task<T> EnqueueTask<T>(Func<Task<T>> task);
    }
}