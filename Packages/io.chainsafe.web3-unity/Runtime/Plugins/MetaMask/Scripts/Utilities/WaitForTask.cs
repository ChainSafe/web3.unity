using System.Threading.Tasks;
using UnityEngine;

namespace MetaMask.Unity.Utils
{
    public class WaitForTask<T> : CustomYieldInstruction
    {
        private Task<T> _task;

        public override bool keepWaiting
        {
            get
            {
                return !_task.IsCompleted;
            }
        }

        public T Result
        {
            get
            {
                return _task.Result;
            }
        }

        public WaitForTask(Task<T> task)
        {
            _task = task;
        }
    }
}