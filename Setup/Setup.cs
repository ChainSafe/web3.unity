using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Setup.Utils;

namespace Setup
{
    internal class Setup
    {
        public const string SampleProjectPath = "../src/UnitySampleProject";
        
        public static readonly List<Package> Packages = new List<Package>();
        
        // dotnet run -release:3.0.0 -duplicate_samples -publish_dependencies;
        static void Main(string[] args)
        {
            InitializePackages();
            
            List<IRunnable> runnableList = GetRunnableList(args);

            runnableList = runnableList.OrderBy(r => r.Order).Append(new Git()).ToList();
            
            foreach (IRunnable runnable in runnableList)
            {
                runnable.Run();
            }
        }

        private static void InitializePackages()
        {
            // Initialize packages from file.
            string json = File.ReadAllText("packages.json");

            string[] paths = JsonConvert.DeserializeObject<string[]>(json);

            for (int i = 0; i < paths.Length; i++)
            {
                string path = $"../{paths[i]}";
                
                Package package = new Package(path);
                
                JsonConvert.PopulateObject(File.ReadAllText(package.FilePath), package);
                
                Packages.Add(package);
            }
        }
        
        private static List<IRunnable> GetRunnableList(string[] args)
        {
            List<IRunnable> runnableLit = new List<IRunnable>();
            
            // Parse arguments and Run operations based on that.
            foreach (var arg in args)
            {
                switch (arg)
                {
                    case not null when arg.StartsWith("-release"):
                        
                        string version = arg.Split(":")[1];
                        runnableLit.AddRunnable(new Release(version));
                        break;
                    
                    case "-duplicate_samples":
                        runnableLit.AddRunnable(new DuplicateSamples());
                        break;
                    
                    case "-sync_dependencies":
                        runnableLit.AddRunnable(new SyncDependencies());
                        break;
                    case "-unity_test":
                        runnableLit.AddRunnable(new UnityTests());
                        break;
                }
            }

            return runnableLit;
        }
    }
}