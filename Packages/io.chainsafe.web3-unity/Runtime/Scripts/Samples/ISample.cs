using System;

namespace ChainSafe.Gaming
{
    public interface ISample
    {
        public string Title { get; }

        public string Description { get; }

        /// <summary>
        /// These are services that this sample depends on.
        /// If they're not bound/injected it'll not be generated.
        /// </summary>
        public Type[] DependentServiceTypes { get; }
    }
}
