using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Management.Automation;

namespace Setup;

public static class Utils
{
    public static void RunWithPowerShell(this string command)
    {
        using (PowerShell powershell = PowerShell.Create())
        {
            powershell.AddScript(@command).Invoke();
        }
    }
}