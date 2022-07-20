using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Models
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