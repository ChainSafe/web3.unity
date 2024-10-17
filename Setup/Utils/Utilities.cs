using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace Setup.Utils;

public static class Utilities
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
    
    public static void AddRunnable(this List<IRunnable> runnableList, IRunnable runnable)
    {
        if (runnableList.Contains(runnable))
        {
            throw new Exception($"Duplicate command {runnable.GetType().Name}");
        }
        
        runnableList.Add(runnable);
    }

    public static void CopyDirectory(string source, string destination, bool copyAsNew = true, bool overwriteFile = true)
    {
        DirectoryInfo sourceDirectory = new DirectoryInfo(source);
        
        DirectoryInfo destinationDirectory = new DirectoryInfo(destination);

        if (copyAsNew)
        {
            if (destinationDirectory.Exists)
            {
                destinationDirectory.Delete(true);
            }
            
            Directory.CreateDirectory(destination);
        }
        
        DirectoryInfo[] subDirectories = sourceDirectory.GetDirectories();

        foreach (FileInfo file in sourceDirectory.GetFiles())
        {
            file.CopyTo(Path.Combine(destination, file.Name), overwriteFile);
        }

        // Copy Recursively
        foreach (DirectoryInfo directory in subDirectories)
        {
            CopyDirectory(directory.FullName, Path.Combine(destination, directory.Name));
        }
    }
}