using System.Threading.Tasks;
using WalletConnectSharp.Storage;

namespace ChainSafe.Gaming.WalletConnect.Storage
{
    public interface IDataStorage
    {
        Task<LocalData> LoadLocalData();

        Task SaveLocalData(LocalData localData);

        void ClearLocalData();

        FileSystemStorage BuildStorage(bool sessionStored);
    }
}