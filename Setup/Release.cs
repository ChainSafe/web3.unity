using System;

namespace Setup;

public class Release
{
    private readonly Package[] _packages;
    
    private readonly string _version;
    
    public Release(string version)
    {
        _packages = Setup.Packages.ToArray();

        _version = version;
    }
    
    public void Run()
    {
        foreach (Package package in _packages)
        {
            package.SetVersion(_version);

            package.Save();
            
            Git.Add(package.Path);
        }
        
        string[] tags = Array.ConvertAll(_packages, p => $"{p.Name}/{_version}");
        
        Git.Commit($"Release {_version}", tags);
        
        Git.Push(tags);
    }
}