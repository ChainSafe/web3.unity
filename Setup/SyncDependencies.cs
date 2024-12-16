using System;
using System.IO;
using Newtonsoft.Json;
using Setup.Utils;

namespace Setup;

/// <summary>
/// Generate and copy dependencies to Sample Project.
/// </summary>
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
                string fileSource = Path.Combine(source, $"{name}.dll");
                
                string fileDestination = Path.Combine(destination, $"{name}.dll");
                
                File.Copy(fileSource, fileDestination, true);
                
                AddMetaFile(fileDestination);
#if DEBUG
                fileSource = fileSource.Replace(".dll", ".pdb");
                
                if (File.Exists(fileSource))
                {
                    fileDestination = fileDestination.Replace(".dll", ".pdb");
                    
                    File.Copy(fileSource, fileDestination, true);
                    
                    AddMetaFile(fileDestination);
                }
#endif
                Console.WriteLine(name);
            }
            
            Git.Add(destination);
        }
        
        Git.CommitAndPush("Sync Dependencies - Auto Commit", skipCi: false);
        
        Console.WriteLine("Dependencies Synced Successfully!");
    }

    private void AddMetaFile(string destination)
    {
        destination += ".meta";
        
        if (File.Exists(destination)) return;
        
        string text = File.ReadAllText("meta_file_template.txt");

        string guid = Guid.NewGuid().ToString();

        guid = guid.Replace("-", string.Empty);

        text = text.Replace("[[assetGuid]]", guid.Substring(0, 32));

        File.WriteAllText(destination, text);
    }
}