using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Plugins.CountlySDK;
using Plugins.CountlySDK.Models;
using UnityEngine;

internal class RequestBuilder
{

    internal RequestBuilder()
    {
    }

    /// <summary>
    /// Builds request by adding Base params into supplied queryParams parameters.
    /// The data is appended in the URL.
    /// </summary>
    /// <param name="queryParams"></param>
    /// <returns></returns>
    internal CountlyRequestModel BuildRequest(IDictionary<string, object> baseParams, IDictionary<string, object> queryParams)
    {
        //Metrics added to each request
        IDictionary<string, object> requestData = baseParams;
        foreach (KeyValuePair<string, object> item in queryParams) {
            requestData.Add(item.Key, item.Value);
        }

        string data = BuildQueryString(requestData);
        CountlyRequestModel requestModel = new CountlyRequestModel(null, data);

        return requestModel;
    }

    /// <summary>
    /// Builds query string using supplied queryParams parameters.
    /// </summary>
    /// <param name="queryParams"></param>
    /// <returns></returns>
    internal string BuildQueryString(IDictionary<string, object> queryParams)
    {
        //  Dictionary<string, object> queryParams = JsonConvert.DeserializeObject<Dictionary<string, object>>(data);
        StringBuilder requestStringBuilder = new StringBuilder();

        //Query params supplied for creating request
        foreach (KeyValuePair<string, object> item in queryParams) {
            if (!string.IsNullOrEmpty(item.Key) && item.Value != null) {
                requestStringBuilder.AppendFormat(requestStringBuilder.Length == 0 ? "{0}={1}" : "&{0}={1}", item.Key,
                    Convert.ToString(item.Value));
            }
        }

        string result = requestStringBuilder.ToString();

        return Uri.EscapeUriString(result);
    }
}
