using System.Threading.Tasks;
using WalletConnectSharp.Storage;

namespace ChainSafe.Gaming.WalletConnect.Storage
{
    /// <summary>
    /// Component responsible for storage of WalletConnect-related data.
    /// </summary>
    public interface IDataStorage
    {
        /// <summary>
        /// Loads local data for the WalletConnect integration.
        /// </summary>
        /// <returns>Local data for the WalletConnect integration.</returns>
        Task<LocalData> LoadLocalData();

        /// <summary>
        /// Saves local data for the WalletConnect integration.
        /// </summary>
        /// <param name="localData">The data.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task SaveLocalData(LocalData localData);

        /// <summary>
        /// Clears local data for the WalletConnect integration  from the disk.
        /// </summary>
        void ClearLocalData();

        /// <summary>
        /// Builds storage for WalletConnectCSharp.
        /// </summary>
        /// <param name="sessionStored">True if the session was already stored.</param>
        /// <returns>Storage object used by WalletConnectCSharp.</returns>
        FileSystemStorage BuildStorage(bool sessionStored);
    }
}