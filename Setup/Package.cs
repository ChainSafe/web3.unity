using System.Collections.Generic;
using Newtonsoft.Json;

namespace Setup
{
    public struct Package
    {
        [JsonProperty("name")]
        public string Name { get; private set; }

        [JsonProperty("version")]
        public string Version { get; private set; }
    
        [JsonProperty("dependencies")]
        public Dictionary<string, string> Dependencies { get; private set; }

        [JsonProperty("testables")]
        public string[] Testables { get; private set; }
    
        [JsonProperty("samples")]
        public Sample[] Samples { get; private set; }
    }

    public struct Sample
    {
        [JsonProperty("path")]
        public string Path { get; private set; }
    }
}