using System;
using System.Collections.Generic;
using System.Linq;
using Setup.Utils;

namespace Setup;

/// <summary>
/// Git helper class.
/// </summary>
public static class Git
{
    private static bool _configured;

    public static bool Enabled { get; private set; } = true;

    public static void Configure(string configuration)
    {
        switch (configuration)
        {
            case "enabled":
                Enabled = true;
                break;
            case "disabled":
                Enabled = false;
                break;
            default:
                throw new Exception($"-git can't configure {configuration}");
        }
    }
    
    public static void Add(string path)
    {
        $"git add \"{path}\" -f".Run();
    }
    
    public static void Commit(string message, string[] tags = null)
    {
        if (!_configured)
        {
            Configure();
        }

        // Checks if there are any changes to commit before committing
        $"git diff-index --cached --quiet HEAD || git commit -m \"{message} [skip ci]\"".Run();

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
        "git push -f".Run();

        if (tags != null)
        {
            foreach (string tag in tags)
            {
                if (!string.IsNullOrEmpty(tag))
                {
                    $"git push origin \"{tag}\"".Run();
                }
            }
        }
    }

    public static void CommitAndPush(string message, string[] tags = null)
    {
        Commit(message, tags);
        
        Push(tags);
    }
    
    public static void Tag(string tag)
    {
        $"git tag \"{tag}\"".Run();
    }
    
    private static void Configure()
    {
        "git config user.email $git_email".Run();
        "git config user.name $git_actor".Run();

        _configured = true;
    }
}