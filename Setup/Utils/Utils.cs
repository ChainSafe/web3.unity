using System;
using System.Diagnostics;

namespace Setup.Utils;

public static class Utils
{
    /// <summary>
    /// Runs commands with bash.
    /// </summary>
    /// <param name="command">Command to run.</param>
    /// <exception cref="Exception">If command fails.</exception>
    public static void RunWithBash( this string command)
    {
        command = command.Replace( "\"", "\\\"" );

        Process process = new Process()
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "/bin/bash",
                Arguments = $"-c \"{command}\"",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true,
            }
        };
        
        process.Start();
        
        string result = process.StandardOutput.ReadToEnd();
        
        process.WaitForExit();

        foreach (string line in result.Split('\n'))
        {
            Console.WriteLine(line);
        }

        if (process.ExitCode != 0)
        {
            throw new Exception($"Error executing bash command {command}");
        }
    }
}