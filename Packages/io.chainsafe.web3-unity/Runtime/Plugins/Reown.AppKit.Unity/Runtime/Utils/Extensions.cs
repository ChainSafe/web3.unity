using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UIElements;

namespace Reown.AppKit.Unity.Utils
{
    public static class StringExtensions
    {
        // Regular
        public static string FontWeight400(this string value)
        {
            return $"<font-weight=\"400\">{value}</font-weight>";
        }

        // Medium
        public static string FontWeight500(this string value)
        {
            return $"<font-weight=\"500\">{value}</font-weight>";
        }

        // Semi-bold
        public static string FontWeight600(this string value)
        {
            return $"<font-weight=\"600\">{value}</font-weight>";
        }

        // Bold
        public static string FontWeight700(this string value)
        {
            return $"<font-weight=\"700\">{value}</font-weight>";
        }
        
        public static string AppendQueryString(this string path, IDictionary<string, string> queryParameters)
        {
            if (queryParameters == null || queryParameters.Count == 0)
            {
                return path;
            }

            var queryString = new StringBuilder();
            foreach (var param in queryParameters)
            {
                if (param.Value == default)
                    continue;
                
                if (queryString.Length > 0)
                    queryString.Append("&");

                queryString.Append($"{param.Key}={param.Value}");
            }

            return $"{path}?{queryString}";
        }

        public static string Truncate(this string str, int positions = 4)
        {
            if (string.IsNullOrWhiteSpace(str) || str.Length <= positions * 2)
                return str;

            var start = str[..positions];
            var end = str.Substring(str.Length - positions, positions);

            return $"{start}...{end}";
        }
    }

    public static class ScrollViewExtensions
    {
        public static void ForceUpdate(this ScrollView scrollView)
        {
            // https://forum.unity.com/threads/how-to-refresh-scrollview-scrollbars-to-reflect-changed-content-width-and-height.1260920/#post-8753383
            scrollView.schedule.Execute(() =>
            {
                var fakeOldRect = Rect.zero;
                var fakeNewRect = scrollView.layout;

                using var evt = GeometryChangedEvent.GetPooled(fakeOldRect, fakeNewRect);
                evt.target = scrollView.contentContainer;
                scrollView.contentContainer.SendEvent(evt);
            });
        }
    }
}