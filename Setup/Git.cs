using System;
using Setup.Utils;

namespace Setup;

/// <summary>
/// Git helper class.
/// </summary>
public class Git : IRunnable
{
    private static bool _configured;

    public static bool Enabled { get; private set; }

    public int Order => - 1;

    public Git(bool enabled)
    {
        Enabled = enabled;
    }
    
    public void Run()
    {
        Console.WriteLine($"Git {nameof(Enabled)} : {Enabled}");
        
        Execute("status");
    }

    private static void Execute(string command)
    {
        if (!Enabled)
        {
            Console.WriteLine($"Git disabled skipping command: {command}");
        }
        else
        {
            if (!_configured)
            {
                "git config user.email $git_email".Run();
                
                "git config user.name $git_actor".Run();

                _configured = true;
            }
        
            $"git {command}".Run();
        }
    }
    
    public static void Add(string path)
    {
        Execute($"add \"{path}\" -f");
    }
    
    public static void Commit(string message, string[] tags = null, bool skipCi = true, bool allowEmpty = false)
    {
        if (skipCi)
        {
            message += " [skip ci]";
        }

        Execute(allowEmpty
            ? $"commit --allow-empty -m \"{message}\""
            // Checks if there are any changes to commit before committing
            : $"diff-index --cached --quiet HEAD || git commit -m \"{message}\"");

        if (tags != null)
        {
            foreach (var tag in tags)
            {
                if (!string.IsNullOrEmpty(tag))
                {
                    Tag(tag);
                }
            }
        }
    }
    
    public static void Push(string[] tags = null)
    {
        Execute("push -f");

        if (tags != null)
        {
            foreach (string tag in tags)
            {
                if (!string.IsNullOrEmpty(tag))
                {
                    Execute($"push origin \"{tag}\"");
                }
            }
        }
    }

    public static void CommitAndPush(string message, string[] tags = null, bool skipCi = true)
    {
        Commit(message, tags, skipCi);
        
        Push(tags);
    }

    public static void Fetch(string branch)
    {
        Execute($"fetch origin {branch}");
    }
    
    public static void Checkout(string branch, string path = "")
    {
        string command = $"checkout {branch}";

        if (!string.IsNullOrEmpty(path))
        {
            command += $" \"{path}\"";
        }
        
        Execute(command);
    }
    
    public static void Merge(string branch, string message = "", bool allowUnrelatedHistories = true)
    {
        string command = $"merge {branch} --no-edit --commit --no-ff";

        if (!string.IsNullOrEmpty(message))
        {
            command += $" -m \"{message}\"";
        }
        
        if (allowUnrelatedHistories)
        {
            command += " --allow-unrelated-histories";
        }
        
        Execute(command);
    }
    
    public static void Tag(string tag)
    {
        Execute($"tag \"{tag}\"");
    }
}