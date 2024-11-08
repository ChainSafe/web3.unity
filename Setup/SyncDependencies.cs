using System;
using System.IO;
using Newtonsoft.Json;
using Setup.Utils;

namespace Setup;

public class SyncDependencies : IRunnable
{
    public int Order => 0;

    private readonly Dependency[] _dependencies;

    private const string ProjectPath = "../src/ChainSafe.Gaming.Unity/";
    
    public SyncDependencies()
    {
        _dependencies = JsonConvert.DeserializeObject<Dependency[]>(File.ReadAllText("dependencies.json"));
    }
    
    public void Run()
    {
#if DEBUG
        string configuration = "Debug";
#else
        string configuration = "Release";
#endif
        $"dotnet publish \"{ProjectPath}ChainSafe.Gaming.Unity.csproj\" -c {configuration} /property:Unity=true".Run();
        
        string source = Path.Combine(ProjectPath, "bin", configuration, "netstandard2.1", "publish");
        
        foreach (Dependency dependency in _dependencies)
        {
            string[] namespaces = dependency.Namespaces;
            
            string destination = Path.Combine("../", dependency.Path);

            // Remove previous dependencies first
            if (Directory.Exists(destination))
            {
                DirectoryInfo directory = new DirectoryInfo(destination);

                foreach (var file in directory.GetFiles())
                {
                    if (file.Extension == ".dll" || file.Extension == ".pdb")
                    {
                        file.Delete();
                    }
                }
            }

            else
            {
                Directory.CreateDirectory(destination);
            }
            
            Console.WriteLine($"Copying Dependencies to {destination}...");
            
            foreach (string name in namespaces)
            {
                File.Copy(Path.Combine(source, $"{name}.dll"), Path.Combine(destination, $"{name}.dll"), true);
#if DEBUG
                string filePath = Path.Combine(source, $"{name}.pdb");
                
                if (File.Exists(filePath))
                {
                    File.Copy(filePath, Path.Combine(destination, $"{name}.pdb"), true);
                }
#endif
                Console.WriteLine(name);
            }
            
            Git.Add(destination);
        }
        
        Git.CommitAndPush("Sync Dependencies - Auto Commit", skipCi: false);
        
        Console.WriteLine("Dependencies Synced Successfully!");
    }
}