namespace MetaMask.IO
{

    /// <summary>
    /// An interface for implementing storage providers to be used by <see cref="MetaMaskDataManager"/>.
    /// </summary>
    public interface IMetaMaskPersistentStorage
    {

        /// <summary>
        /// Checks whether the given <paramref name="key"/> exists inside the storage or not.
        /// </summary>
        /// <param name="key">The key to check</param>
        /// <returns>Returns true if it exists, otherwise false.</returns>
        bool Exists(string key);

        /// <summary>
        /// Writes the <paramref name="data"/> to the <paramref name="key"/> inside the storage.
        /// </summary>
        /// <param name="key">The key</param>
        /// <param name="data">The data to write</param>
        void Write(string key, string data);

        /// <summary>
        /// Reads the data from the storage using the given <paramref name="key"/>.
        /// </summary>
        /// <param name="key">The key to read</param>
        /// <returns>Returns the data read</returns>
        string Read(string key);

        /// <summary>
        /// Delete the given <paramref name="key"/> inside the storage.
        /// </summary>
        /// <param name="key">The key to delete</param>
        void Delete(string key);

    }
}
