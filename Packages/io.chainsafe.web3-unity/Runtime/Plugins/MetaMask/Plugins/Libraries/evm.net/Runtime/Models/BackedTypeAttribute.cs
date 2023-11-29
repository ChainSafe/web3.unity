using System;

namespace evm.net.Models
{
    [AttributeUsage(AttributeTargets.Interface)]
    public class BackedTypeAttribute : Attribute
    {
        public Type ImplementationType { get; }

        public BackedTypeAttribute(Type implementationType)
        {
            this.ImplementationType = implementationType;
        }
    }
}