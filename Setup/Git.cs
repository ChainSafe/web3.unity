namespace Setup;

public static class Git
{
    private static bool _configured;
    
    public static void Add(string path)
    {
        $"git add \"{path}\" -f".RunWithBash();
    }
    
    public static void Commit(string message)
    {
        if (!_configured)
        {
            Configure();
        }
        
        $"git commit -m \"{message}\"".RunWithBash();
    }
    
    public static void Push()
    {
        "git push".RunWithBash();
    }

    private static void Configure()
    {
        "git config user.email ${{ github.actor }}@users.noreply.github.com".RunWithBash();
        "git config user.name ${{ github.actor }}".RunWithBash();

        _configured = true;
    }
}