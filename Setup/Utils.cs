using System;
using System.Diagnostics;

namespace Setup;

public static class Utils
{
    public static void RunWithBash( this string cmd )
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

        Console.WriteLine($"Output for {cmd}: {result}");
        
        foreach (string line in result.Split('\n'))
        {
            Console.WriteLine(line);
        }
    }
}