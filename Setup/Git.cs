namespace Setup;

public static class Git
{
    public static void Add(string path)
    {
        $"git add \"{path}\" -f".RunWithBash();
    }
    
    public static void Commit(string message)
    {
        $"git commit -m \"{message}\"".RunWithBash();
    }
    
    public static void Push()
    {
        "git push".RunWithBash();
    }
}