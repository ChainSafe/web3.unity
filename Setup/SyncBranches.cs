using System;
using System.Collections.Generic;
using System.IO;
using CommandLine;
using Newtonsoft.Json;
using Setup.Utils;

namespace Setup;

/// <summary>
/// Sync branches (merge).
/// </summary>
[Verb("sync", HelpText = "Sync branches.")]
public class SyncBranches : IRunnable
{
    public int Order => 0;
    
    public IEnumerable<string> CommandLineArgument { get; set; }

    private readonly string[] _excludedPaths;

    [Option('b', "base", Required = true, HelpText = "Base branch.")]
    public string Base { get; set; }
    
    [Option('t', "target", Required = true, HelpText = "Target branch.")]
    public string Target { get; set; }
    
    [Option('s', "skip_ci", Required = false, Default = true, HelpText = "Disable/Skip CI triggers for sync/merge.")]
    public bool? SkipCi { get; set; }
    
    public SyncBranches()
    {
        Dependency[] dependencies = JsonConvert.DeserializeObject<Dependency[]>(File.ReadAllText("dependencies.json"));

        _excludedPaths = Array.ConvertAll(dependencies, d => $"../{d.Path}");
    }
    
    public void Run()
    {
        SkipCi ??= false;
        
        Console.WriteLine($"Syncing {Target} branch to {Base} branch...");
        
        Git.Fetch(Base);
        
        Git.Checkout(Base);
        
        Git.Fetch(Target);
        
        Git.Checkout(Target);
        
        foreach (string path in _excludedPaths)
        {
            Git.Checkout(Base, path);
            
            Git.Add(path);
        }
        
        Git.Commit("Exclude Paths - Auto Commit", skipCi: SkipCi.Value, allowEmpty: true);

        string message = $"Sync to {Base} - Auto Commit";

        if (SkipCi.Value)
        {
            message += " [skip ci]";
        }
        
        Git.Merge(Base, message);
        
        Git.Push();
        
        Console.WriteLine($"Synced {Target} branch to {Base} branch completed.");
    }
}