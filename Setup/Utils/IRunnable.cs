namespace Setup.Utils;

public interface IRunnable
{
    public int Order { get; }
    
    public void Run();
}