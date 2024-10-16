using Setup.Utils;

namespace Setup;

/// <summary>
/// Git helper class.
/// </summary>
public static class Git
{
    private static bool _configured;
    
    public static void Add(string path)
    {
        $"git add \"{path}\" -f".RunWithBash();
    }
    
    public static void Commit(string message, string[] tags = null)
    {
        if (!_configured)
        {
            Configure();
        }
        
        $"git commit -m \"{message} [skip ci]\"".RunWithBash();

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
        "git push -f".RunWithBash();

        if (tags != null)
        {
            foreach (string tag in tags)
            {
                if (!string.IsNullOrEmpty(tag))
                {
                    $"git push origin \"{tag}\"".RunWithBash();
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
        $"git tag \"{tag}\"".RunWithBash();
    }
    
    private static void Configure()
    {
        "git config user.email $git_email".RunWithBash();
        "git config user.name $git_actor".RunWithBash();

        _configured = true;
    }
}