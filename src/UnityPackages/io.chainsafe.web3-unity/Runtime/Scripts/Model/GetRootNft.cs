using System;
using System.Collections.Generic;

namespace ChainSafe.Gaming.UnityPackage.Model
{
    [Serializable]
    public class Properties
    {
        public List<string> additionalFiles;
    }

    [Serializable]
    public class RootGetNFT
    {
        public string description;
        public string image;
        public string name;
        public Properties properties;
    }
}