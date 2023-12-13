using Plugins.CountlySDK;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CrashReporting : MonoBehaviour
{
    public async void CustomCrashLogs()
    {
        Dictionary<string, object> seg = new Dictionary<string, object>{
                { "Time Spent", "1234455"},
                { "Retry Attempts", "10"}
            };
        await Countly.Instance.CrashReports.SendCrashReportAsync("Exception", "Stacktrace", seg);


    }
    public void RecordUnhandledException()
    {
        throw new NullReferenceException("Test Unhandled Exception");
    }

    public async void RecordHandledException()
    {
        try {

            throw new DivideByZeroException();
        } catch (Exception ex) {
            Dictionary<string, object> seg = new Dictionary<string, object>{
                { "Fail Message", "Please retry"},
                { "Retry Left", "2"}
            };

            await Countly.Instance.CrashReports.SendCrashReportAsync(ex.Message, ex.StackTrace, seg, nonfatal: true);
        }

    }

    public void AddBreadCrumb()
    {
        Countly.Instance.CrashReports.AddBreadcrumbs("Breadcrumb 1");
    }
}
