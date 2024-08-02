using System.IO;
using System.Threading.Tasks;

namespace ChainSafe.Gaming.LocalStorage
{
    public interface ILocalStorage
    {
        Task Initialize();

        Task Save<T>(T storable, bool createFile = true)
            where T : IStorable;

        Task Load<T>(T storable)
            where T : IStorable;

        void Clear(IStorable storable);
    }
}