using ChainSafe.Gaming.LocalStorage;

namespace ChainSafe.Gaming.HyperPlay
{
    public interface IHyperPlayConfig
    {
        public string Url => "http://localhost:9680/rpc";

        public bool RememberSession { get; set; }
    }
}