using System.Collections.Generic;
using System.Threading.Tasks;
using ChainSafe.Gaming.LocalStorage;
using ChainSafe.Gaming.Web3.Environment;
using UnityEngine;

namespace ChainSafe.Gaming.UnityPackage
{
    public class WebDataStorage : DataStorage
    {
        public WebDataStorage(IEnumerable<IStorable> store, IOperatingSystemMediator osMediator, ILogWriter logWriter) : base(store, osMediator, logWriter)
        {
        }
    
        public override async Task Save<T>(T storable, bool createFile = true)
        {
            await base.Save(storable, createFile);

            // This forces the data to be saved in WebGL even when reloading WebPages.
#if UNITY_WEBGL && !UNITY_EDITOR
        PlayerPrefs.SetString("forceSave", string.Empty);
        
        PlayerPrefs.Save();
#endif
        }
    }
}
