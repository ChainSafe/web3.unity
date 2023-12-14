using System.Collections.Generic;

namespace ChainSafe.Gaming.UnityPackage.Model
{
    public class Bridge
    {
        public string url { get; set; }
    }
    
    public class Ens
    {
        public string registry { get; set; }
    }
    
    public class Explorer
    {
        public string name { get; set; }
        public string url { get; set; }
        public string standard { get; set; }
        public string icon { get; set; }
    }
    
    public class Feature
    {
        public string name { get; set; }
    }
    
    public class NativeCurrency
    {
        public string name { get; set; }
        public string symbol { get; set; }
        public int decimals { get; set; }
    }
    
    public class Parent
    {
        public string type { get; set; }
        public string chain { get; set; }
        public List<Bridge> bridges { get; set; }
    }
    
    public class Root
    {
        public string name { get; set; }
        public string chain { get; set; }
        public string icon { get; set; }
        public List<string> rpc { get; set; }
        public List<Feature> features { get; set; }
        public List<string> faucets { get; set; }
        public NativeCurrency nativeCurrency { get; set; }
        public string infoURL { get; set; }
        public string shortName { get; set; }
        public object chainId { get; set; }
        public object networkId { get; set; }
        public long slip44 { get; set; }
        public Ens ens { get; set; }
        public List<Explorer> explorers { get; set; }
        public string title { get; set; }
        public string status { get; set; }
        public List<string> redFlags { get; set; }
        public Parent parent { get; set; }
    }
}