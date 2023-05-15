using ChainSafe.GamingWeb3.Environment;
using UnityEngine;

namespace ChainSafe.GamingSdk.EVM.Unity
{
    public class UnitySettingsProvider : ISettingsProvider
    {
        public string DefaultRpcUrl => PlayerPrefs.GetString("RPC");
    }
}