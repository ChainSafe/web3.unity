using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Setup.Utils;

namespace Setup
{
    public class Package
    {
        [JsonIgnore]
        public string Path { get; private set; }

        [JsonProperty("name")]
        public string Name { get; private set; }
        
        [JsonProperty("version")]
        public string Version { get; private set; }
    
        [JsonProperty("dependencies", NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<string, string> Dependencies { get; private set; }

        [JsonProperty("testables", NullValueHandling = NullValueHandling.Ignore)]
        public string[] Testables { get; private set; }
    
        [JsonProperty("samples", NullValueHandling = NullValueHandling.Ignore)]
        public Sample[] Samples { get; private set; }

        // For Json Deserialize.
        public Package()
        {
            
        }
        
        public Package(string path)
        {
            Path = path;
        }

        public void SetVersion(string version)
        {
            string[] increments = version.Split('.');
            
            if (increments.Length != 3
                || !increments.Any(i => int.TryParse(i, out _)
                                        || int.Parse(i) < 0
                                        || int.Parse(i) > 9))
            {
                throw new Exception("Incorrect Version Format https://semver.org/");
            }
            
            Version = version;

            string[] keys = Dependencies.Where(d => Setup.Packages.Any(p => p.Name == d.Key))
                .Select(d => d.Key)
                .ToArray();

            foreach (string key in keys)
            {
                Dependencies[key] = version;
            }
        }

        public void Save()
        {
            string json = File.ReadAllText(Path);
            
            JObject jObject = JObject.Parse(json);

            jObject.Merge(
                JObject.Parse(JsonConvert.SerializeObject(this,
                    new JsonSerializerSettings { ContractResolver = new JsonPropertiesResolver() })),
                new JsonMergeSettings { MergeArrayHandling = MergeArrayHandling.Merge });
            
            File.WriteAllText(Path, jObject.ToString(Formatting.Indented));
        }
    }

    public struct Sample
    {
        [JsonProperty("path")]
        public string Path { get; private set; }
    }
}