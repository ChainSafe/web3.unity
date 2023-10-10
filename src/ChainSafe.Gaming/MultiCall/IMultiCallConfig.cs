using System.Collections.Generic;

namespace ChainSafe.Gaming.MultiCall
{
    public interface IMultiCallConfig
    {
        public IReadOnlyDictionary<string, string> CustomNetworks { get; }
    }
}