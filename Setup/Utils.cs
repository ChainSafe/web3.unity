using System.Diagnostics;

namespace Setup;

public static class Utils
{
    public static string RunWithBash( this string cmd )
    {
        cmd = cmd.Replace( "\"", "\\\"" );

        Process process = new Process()
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "/bin/bash",
                Arguments = $"-c \"{cmd}\"",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true,
            }
        };
        
        process.Start();
        
        string result = process.StandardOutput.ReadToEnd();
        
        process.WaitForExit();
        
        return result;
    }
}