using System.Collections.Generic;
using System.IO;
using Setup.Utils;

namespace Setup;

/// <summary>
/// Release.
/// </summary>
public class Release : IRunnable
{
    private readonly Package[] _packages;
    
    private readonly string _version;
    
    public int Order => 2;
    
    public Release(string version)
    {
        _packages = Setup.Packages.ToArray();

        _version = version;
    }

    /// <summary>
    /// Bumps the version and makes a release.
    /// Commit and push with package.json versions bumped.
    /// Tag the commits so another CI (open-upm) can pick them up.
    /// </summary>
    public void Run()
    {
        List<string> tags = new List<string>();

        tags.Add(_version);
        
        foreach (Package package in _packages)
        {
            string previousVersion = package.Version;
            
            package.SetVersion(_version);

            package.Save();
            
            Git.Add(package.Path);
            
            tags.Add($"{package.Name}/{_version}");

            // Move samples to new directory with new version
            if (package.HasSamples())
            {
                string path = Path.Combine(Setup.SampleProjectPath, "Assets/Samples", package.DisplayName);
                    
                string source = Path.Combine(path, previousVersion);

                string destination = Path.Combine(path, _version);
                
                Directory.Move(source, destination);
                
                File.Move($"{source}.meta", $"{destination}.meta");
                    
                Git.Add(path);
            }
        }
        
        Git.CommitAndPush($"Release {_version}", tags.ToArray());
    }
}