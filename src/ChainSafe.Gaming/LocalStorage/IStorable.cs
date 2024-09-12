namespace ChainSafe.Gaming.LocalStorage
{
    /// <summary>
    /// Storable data for local storage.
    /// </summary>
    public interface IStorable
    {
        /// <summary>
        /// Path to store the data.
        /// </summary>
        public string StoragePath { get; }

        /// <summary>
        /// Load data on initialize.
        /// </summary>
        public bool LoadOnInitialize { get; }
    }
}