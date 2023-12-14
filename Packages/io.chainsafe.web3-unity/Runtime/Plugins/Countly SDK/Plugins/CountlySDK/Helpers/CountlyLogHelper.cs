using Plugins.CountlySDK.Models;

public class CountlyLogHelper
{
    private const string TAG = "[Countly]";
    private readonly CountlyConfiguration _configuration;
    internal CountlyLogHelper(CountlyConfiguration configuration)
    {
        _configuration = configuration;
    }

    internal void Info(string message)
    {
        if (_configuration.EnableConsoleLogging) {
            UnityEngine.Debug.Log("[Info]" + TAG + message);
        }

    }

    internal void Debug(string message)
    {
        if (_configuration.EnableConsoleLogging) {
            UnityEngine.Debug.Log("[Debug]" + TAG + message);
        }

    }

    internal void Verbose(string message)
    {
        if (_configuration.EnableConsoleLogging) {
            UnityEngine.Debug.Log("[Verbose]" + TAG + message);
        }

    }

    internal void Error(string message)
    {
        if (_configuration.EnableConsoleLogging) {
            UnityEngine.Debug.LogError("[Error]" + TAG + message);
        }
    }

    internal void Warning(string message)
    {
        if (_configuration.EnableConsoleLogging) {
            UnityEngine.Debug.LogWarning("[Warning]" + TAG + message);
        }
    }

}
