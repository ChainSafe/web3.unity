using System.Collections.Generic;
using System.IO;
using System.Linq;
using CommandLine;
using Newtonsoft.Json;
using Setup.Utils;

namespace Setup
{
    internal class Setup
    {
        public const string SampleProjectPath = "../src/UnitySampleProject";
        
        public static readonly List<Package> Packages = new List<Package>();
        
        // dotnet run -release:3.0.0 -duplicate_samples -sync_dependencies;
        static void Main(string[] args)
        {
            InitializePackages();
            
            List<IRunnable> runnableList = GetRunnableList(args);

            runnableList = runnableList.OrderBy(r => r.Order).ToList();
            
            foreach (IRunnable runnable in runnableList)
            {
                runnable.Run();
            }
        }

        /// <summary>
        /// Initialize packages from packages.json in root of this project.
        /// </summary>
        private static void InitializePackages()
        {
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
        
        /// <summary>
        /// Get runnable list based on passed arguments.
        /// </summary>
        /// <param name="args">Arguments passed.</param>
        /// <returns>Runnable List.</returns>
        private static List<IRunnable> GetRunnableList(string[] args)
        {
            List<IRunnable> runnableList = new List<IRunnable>();
            
            Parser.Default.ParseArguments<DefaultOptions, SyncBranches>(args)
                .WithParsed(options =>
                {
                    switch (options)
                    {
                        case DefaultOptions defaultOptions:
                            runnableList.AddRange(defaultOptions.GetRunnableList());
                            break;
                        case SyncBranches syncBranches:
                            runnableList.Add(syncBranches);
                            runnableList.Add(new Git(true));
                            break;
                    }
                });

            return runnableList;
        }
    }
}