using System.Collections.Generic;
using CommandLine;
using Setup.Utils;

namespace Setup;

[Verb("do", true, HelpText = "Runs default options.")]
public class DefaultOptions
{
    [Option('s', "sync_dependencies", Required = false, Default = false, HelpText = "Generate and copy dependencies to Sample Project.")]
    public bool SyncDependencies { get; set; }
    
    [Option('d', "duplicate_samples", Required = false, Default = false, HelpText = "Duplicate samples in Sample Project into package samples.")]
    public bool DuplicateSamples { get; set; }
    
    [Option("deploy", Required = false, HelpText = "Version to release.")]
    public string Release { get; set; }
    
    [Option('g', "git", Required = false, Default = true, HelpText = "Enable Git.")]
    public bool? EnableGit { get; set; }

    public List<IRunnable> GetRunnableList()
    {
        List<IRunnable> runnableList = new List<IRunnable>();
        
        if (SyncDependencies)
        {
            runnableList.Add(new SyncDependencies());
        }

        if (DuplicateSamples)
        {
            runnableList.Add(new DuplicateSamples());
        }

        if (!string.IsNullOrEmpty(Release))
        {
            runnableList.Add(new Release(Release));
        }

        EnableGit ??= false;
        
        runnableList.Add(new Git(EnableGit.Value));

        return runnableList;
    }
}