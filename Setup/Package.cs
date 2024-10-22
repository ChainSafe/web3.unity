using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Setup.Utils;

namespace Setup
{
    /// <summary>
    /// Unity package.json model for UPM.
    /// https://docs.unity3d.com/Manual/upm-manifestPkg.html.
    /// </summary>
    public class Package
    {
        /// <summary>
        /// Path of package from root of repository.
        /// </summary>
        [JsonIgnore]
        public string Path { get; private set; }

        [JsonProperty("name"), JsonIgnoreSerialization]
        public string Name { get; private set; }
        
        [JsonProperty("displayName"), JsonIgnoreSerialization]
        public string DisplayName { get; private set; }
        
        [JsonProperty("version")]
        public string Version { get; private set; }
    
        [JsonProperty("dependencies", NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<string, string> Dependencies { get; private set; }

        [JsonProperty("testables", NullValueHandling = NullValueHandling.Ignore), JsonIgnoreSerialization]
        public string[] Testables { get; private set; }
    
        [JsonProperty("samples", NullValueHandling = NullValueHandling.Ignore), JsonIgnoreSerialization]
        public Sample[] Samples { get; private set; }

        /// <summary>
        /// Path of package.json file.
        /// </summary>
        [JsonIgnore]
        public string FilePath { get; private set; }
        
        // For Json Deserialize.
        public Package()
        {
            
        }
        
        public Package(string path)
        {
            Path = path;
            
            FilePath = System.IO.Path.Combine(path, "package.json");
        }

        /// <summary>
        /// Sets Package Version.
        /// </summary>
        /// <param name="version">Version string.</param>
        /// <exception cref="Exception">Exception thrown if version format doesn't match guidelines https://semver.org/.</exception>
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

        /// <summary>
        /// Save altered package to <see cref="Path"/>.
        /// </summary>
        public void Save()
        {
            string json = File.ReadAllText(FilePath);
            
            JObject jObject = JObject.Parse(json);

            jObject.Merge(
                JObject.Parse(JsonConvert.SerializeObject(this,
                    new JsonSerializerSettings { ContractResolver = new JsonPropertiesResolver() })),
                new JsonMergeSettings { MergeArrayHandling = MergeArrayHandling.Merge });
            
            File.WriteAllText(FilePath, jObject.ToString(Formatting.Indented));
        }

        public bool HasSamples()
        {
            return Samples != null && Samples.Length > 0;
        }
    }

    public struct Sample
    {
        [JsonProperty("displayName")]
        public string DisplayName { get; private set; }
        
        [JsonProperty("path")]
        public string Path { get; private set; }
    }
}