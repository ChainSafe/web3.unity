using System.Numerics;

namespace ChainSafe.Gaming.Mud.Storages.InMemory
{
    public class InMemoryMudStorageConfig : IInMemoryMudStorageConfig
    {
        public BigInteger FromBlockNumber { get; set; }
    }
}