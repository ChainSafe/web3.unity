using System.Collections.Generic;

namespace ChainSafe.Gaming.UnityPackage.Model
{
    public struct NativeCurrency
    {
        public string symbol { get; set; }
    }

    public struct Explorer
    {
        public string name { get; set; }
        public string url { get; set; }
        public string standard { get; set; }
    }

    public struct Root
    {
        public string name { get; set; }
        public string chain { get; set; }
        public List<string> rpc { get; set; }
        public NativeCurrency nativeCurrency { get; set; }
        public object chainId { get; set; }
        public List<Explorer> explorers { get; set; }
        public bool allowCustomValues { get; set; }
    }
}