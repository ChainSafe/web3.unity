namespace Setup;

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
                    $"git tag {tag}".RunWithBash();
                }
            }
        }
    }
    
    public static void Push()
    {
        "git push".RunWithBash();
    }

    private static void Configure()
    {
        "git config user.email $git_email".RunWithBash();
        "git config user.name $git_actor".RunWithBash();

        _configured = true;
    }
}