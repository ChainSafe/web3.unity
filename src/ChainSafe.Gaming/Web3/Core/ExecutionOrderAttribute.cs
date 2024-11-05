using System;

namespace ChainSafe.Gaming.Web3.Core
{
    /// <summary>
    /// ExecutionOrder for <see cref="ILifecycleParticipant"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class ExecutionOrderAttribute : Attribute
    {
        public ExecutionOrderAttribute(int order)
        {
            Order = order;
        }

        public int Order { get; }
    }
}