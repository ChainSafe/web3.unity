using System.Collections.Generic;

namespace Setup;

/// <summary>
/// Release.
/// </summary>
public class Release
{
    private readonly Package[] _packages;
    
    private readonly string _version;
    
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
            package.SetVersion(_version);

            package.Save();
            
            Git.Add(package.Path);
            
            tags.Add($"{package.Name}/{_version}");
        }
        
        Git.CommitAndPush($"Release {_version}", tags.ToArray());
    }
}