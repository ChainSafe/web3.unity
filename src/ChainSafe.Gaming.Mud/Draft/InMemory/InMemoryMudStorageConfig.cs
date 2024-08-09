using System.Numerics;

namespace ChainSafe.Gaming.Mud.Draft
{
    public class InMemoryMudStorageConfig : IInMemoryMudStorageConfig
    {
        public BigInteger FromBlockNumber { get; set; }
    }
}