using System;
using System.Linq;
using UnityEngine;

public static class SampleOutputUtil
{
    public static void PrintResult(string value, string sampleClassName, string sampleMethodName)
    {
        var msg = BuildResultMessage(value, sampleClassName, sampleMethodName);
        Debug.Log(msg);
    }

    public static string BuildResultMessage(string value, string sampleClassName, string sampleMethodName)
    {
        var processName = BuildProcessName(sampleClassName, sampleMethodName);
        var msg = $"{processName} executed successfully.\nOutput: {value}";
        return msg;
    }

    public static string BuildOutputValue(object[] dynamicResponse)
    {
        return string.Join(",\n", dynamicResponse.Select(o => o.ToString()));
    }

    private static string BuildProcessName(string sampleClassName, string sampleMethodName)
    {
        var classFormatted = FormatClassName(sampleClassName);
        return $"\"{classFormatted} - {sampleMethodName}\"";

        string FormatClassName(string className)
        {
            var redundantPartIndex = className.LastIndexOf("Sample", StringComparison.InvariantCulture);

            if (redundantPartIndex > 0)
            {
                className = className[..redundantPartIndex];
            }

            return className;
        }
    }
}