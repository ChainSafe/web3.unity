namespace Setup.Utils;

/// <summary>
/// Runnable objects added to run queue.
/// </summary>
public interface IRunnable
{
    /// <summary>
    /// Run order.
    /// </summary>
    public int Order { get; }
    
    public void Run();
}