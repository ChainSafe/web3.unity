using System;
using UnityEngine;

namespace ChainSafe.Gaming
{
    [AttributeUsage(AttributeTargets.Field)]
    public class DefaultAssetValueAttribute : PropertyAttribute
    {
        public string Path { get; private set; }

        public DefaultAssetValueAttribute(string path)
        {
            Path = path;
        }
    }
}
