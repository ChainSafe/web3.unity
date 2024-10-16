using System;
using System.Collections.Generic;
using System.Linq;

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