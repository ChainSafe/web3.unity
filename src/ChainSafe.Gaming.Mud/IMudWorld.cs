using System.Threading.Tasks;
using Nethereum.Mud;
using Nethereum.Mud.Contracts.World;

namespace ChainSafe.Gaming.Mud
{
    public interface IMudWorld
    {
        /// <summary>
        /// A Nethereum World Service. Use this if you need more control over the World.
        /// </summary>
        WorldService WorldService { get; }

        Task<TValue> Query<TRecord, TValue>()
            where TRecord : TableRecordSingleton<TValue>, new()
            where TValue : class, new();

        Task<TValue> Query<TRecord, TKey, TValue>(TKey key)
            where TRecord : TableRecord<TKey, TValue>, new()
            where TKey : class, new()
            where TValue : class, new();
    }
}