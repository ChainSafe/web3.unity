namespace Setup;

public static class Git
{
    public static void Add(string path)
    {
        $"git add \"{path}\" -f".RunWithPowerShell();
    }
    
    public static void Commit(string message)
    {
        $"git commit -m \"{message}\"".RunWithPowerShell();
    }
    
    public static void Push()
    {
        "git push".RunWithPowerShell();
    }
}