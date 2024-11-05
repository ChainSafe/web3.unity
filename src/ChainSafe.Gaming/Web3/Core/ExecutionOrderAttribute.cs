using System;

namespace ChainSafe.Gaming.Web3.Core
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ExecutionOrderAttribute : Attribute
    {
        public ExecutionOrderAttribute(int priority)
        {
            Priority = priority;
        }

        public int Priority { get; }
    }
}