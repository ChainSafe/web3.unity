namespace ChainSafe.Gaming.LocalStorage
{
    public interface IStorable
    {
        public string StoragePath { get; }

        public bool LoadOnInitialize { get; }
    }
}