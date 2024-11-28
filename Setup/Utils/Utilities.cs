using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Management.Automation;
using System.Runtime.InteropServices;

namespace Setup.Utils;

public static class Utilities
{
    public static void Run(this string command)
    {
        Console.WriteLine($"Running Command : {command}");
        
        // Tried switch statement couldn't find a way to make it work
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            command.RunWithBash();
        }
        
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            command.RunWithPowershell();
        }

        else
        {
            throw new Exception($"[Unsupported OS] Can't run command: {command}");
        }
    }

    private static void RunWithPowershell(this string command)
    {
        using (PowerShell powerShell = PowerShell.Create())
        {
            var result = powerShell.AddScript($"{command} | Out-String").Invoke();
            
            // Output
            foreach (var line in result)
            {
                Console.WriteLine(line.ToString());
            }
            
            if (powerShell.HadErrors)
            {
                throw new Exception($"Error executing powershell command: {command}");
            }
        }
    }
    
    /// <summary>
    /// Runs commands with bash.
    /// </summary>
    /// <param name="command">Command to run.</param>
    /// <exception cref="Exception">If command fails.</exception>
    private static void RunWithBash( this string command)
    {
        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            Console.WriteLine($"Skipping bash command \"{command}\" on non-linux platform.");
            
            return;
        }
        
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
            throw new Exception($"Error executing bash command: {command} Exit Code: {process.ExitCode}");
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