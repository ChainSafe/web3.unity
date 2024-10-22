using System;
using System.IO;
using Setup.Utils;

namespace Setup;

/// <summary>
/// Duplicates samples in Sample Project into package samples.
/// </summary>
public class DuplicateSamples : IRunnable
{
    private readonly Package[] _packages;
    
    public int Order => 1;
    
    public DuplicateSamples()
    {
        _packages = Setup.Packages.ToArray();
    }

    public void Run()
    {
        foreach (Package package in _packages)
        {
            if (package.HasSamples())
            {
                string path = Path.Combine(Setup.SampleProjectPath, "Assets/Samples", package.DisplayName);
                
                foreach (var sample in package.Samples)
                {
                    string source = Path.Combine(path, package.Version, sample.DisplayName);
                    
                    string destination = Path.Combine(package.Path, sample.Path);

                    Utilities.CopyDirectory(source, destination);
                    
                    Console.WriteLine($"Duplicated Sample {package.Name} - {sample.DisplayName} from {source} to {destination}");
                    
                    Git.Add(destination);
                }
            }
        }
        
        Git.CommitAndPush("Duplicated Samples");
        
        Console.WriteLine("Duplicated Samples Successfully!");
    }
}