using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace Setup
{
    internal class Setup
    {
        public static readonly List<Package> Packages = new List<Package>();
        
        // dotnet run -release:3.0.0 -duplicate_samples -publish_dependencies;
        static void Main(string[] args)
        {
            // Initialize packages from file.
            string json = File.ReadAllText("packages.json");

            string[] paths = JsonConvert.DeserializeObject<string[]>(json);

            for (int i = 0; i < paths.Length; i++)
            {
                string path = $"../{paths[i]}";
                
                Package package = new Package(path);
                
                JsonConvert.PopulateObject(File.ReadAllText(path), package);
                
                Packages.Add(package);
            }
            
            // Parse arguments and Run operations based on that.
            // TODO: use runnable and runner to run multiple operations sort orders.
            foreach (var arg in args)
            {
                switch (arg)
                {
                    case not null when arg.StartsWith("-release"):
                        
                        string version = arg.Split(":")[1];
                        
                        Release release = new Release(version);
                        
                        release.Run();
                        
                        break;
                }
            }
        }
    }
}