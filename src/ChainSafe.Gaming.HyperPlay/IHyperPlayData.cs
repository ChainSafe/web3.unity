using ChainSafe.Gaming.LocalStorage;

namespace ChainSafe.Gaming.HyperPlay
{
    public interface IHyperPlayData : IStorable
    {
        public bool RememberSession { get; set; }

        public string SavedAccount { get; set; }
    }
}