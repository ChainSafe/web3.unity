using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using Plugins.CountlySDK.Enums;
using Plugins.CountlySDK.Helpers;
using Plugins.CountlySDK.Models;
using UnityEngine;

namespace Plugins.CountlySDK
{
    public class CountlyUtils
    {
        private readonly Countly _countly;
        internal string ServerInputUrl { get; private set; }
        internal string ServerOutputUrl { get; private set; }

        public CountlyUtils(Countly countly)
        {
            _countly = countly;

            ServerInputUrl = _countly.Configuration.ServerUrl + "/i?";
            ServerOutputUrl = _countly.Configuration.ServerUrl + "/o/sdk?";
        }

        public static string GetUniqueDeviceId()
        {
            string uniqueID = SystemInfo.deviceUniqueIdentifier;
            if (uniqueID.Length > 5)
            {
                return "CLY_" + uniqueID;
            }
            else
            {
                return "CLY_" + SafeRandomVal();
            }
        }

        public static string GetAppVersion()
        {
            return Application.version;
        }

        /// <summary>
        /// Retrieves a dictionary of base parameters required for requests to the Countly server.
        /// </summary>
        /// <returns>
        /// A dictionary containing essential parameters, including "app_key," "device_id," "t," "sdk_name," "sdk_version," "av," and time-based metrics.
        /// </returns>
        public Dictionary<string, object> GetBaseParams()
        {
            Dictionary<string, object> baseParams = new Dictionary<string, object>
            {
                {"app_key", _countly.Configuration.AppKey},
                {"device_id", _countly.Device.DeviceId},
                {"t", Type()},
                {"sdk_name", Constants.SdkName},
                {"sdk_version", Constants.SdkVersion},
                {"av", GetAppVersion()}
            };

            // Add time-based metrics to the base parameters dictionary
            foreach (KeyValuePair<string, object> item in TimeMetricModel.GetTimeMetricModel())
            {
                baseParams.Add(item.Key, item.Value);
            }
            return baseParams;
        }

        /// <summary>
        /// Retrieves the set of parameters, "app_key" and "device_id,", required to be sent along with a remote configuration request.
        /// </summary>
        /// <returns>
        /// Dictionary containing the "app_key" and "device_id" parameters required for the remote config request.
        /// </returns>
        public Dictionary<string, object> GetAppKeyAndDeviceIdParams()
        {
            return new Dictionary<string, object>
            {
                {"app_key", _countly.Configuration.AppKey},
                {"device_id", _countly.Device.DeviceId}
            };
        }

        /// <summary>
        /// Checks whether a string is null, empty, or consists only of whitespace characters.
        /// Convenience call that combines both checks
        /// </summary>
        /// <param name="input">The string to be checked.</param>
        /// <returns>
        ///   <c>true</c> if the input string is null, empty, or contains only whitespace characters;
        ///   otherwise, <c>false</c>.
        /// </returns>
        public bool IsNullEmptyOrWhitespace(string input)
        {
            return string.IsNullOrEmpty(input) || string.IsNullOrWhiteSpace(input);
        }

        /// <summary>
        /// Checks whether a given picture URL is valid based on its format.
        /// </summary>
        /// <param name="pictureUrl">The URL of the picture to be validated.</param>
        /// <returns>
        ///   <c>true</c> if the picture URL is valid; otherwise, <c>false</c>.
        /// </returns>
        public bool IsPictureValid(string pictureUrl)
        {
            // Check if the provided url contains additional query parameters. Indicated by "?" 
            if (!string.IsNullOrEmpty(pictureUrl) && pictureUrl.Contains("?"))
            {
                // Remove the query string portion to isolate the file extension.
                pictureUrl = pictureUrl.Split(new[] { '?' }, StringSplitOptions.RemoveEmptyEntries)[0];
            }

            // Check the validity of the picture URL based on its file extension.
            return string.IsNullOrEmpty(pictureUrl)
                   || pictureUrl.EndsWith(".png")
                   || pictureUrl.EndsWith(".jpg")
                   || pictureUrl.EndsWith(".jpeg")
                   || pictureUrl.EndsWith(".gif");
        }

        /// <summary>
        /// Converts a byte array to a hexadecimal string representation.
        /// </summary>
        /// <param name="bytes">The byte array to be converted.</param>
        /// <returns>
        /// A hexadecimal string representation of the input byte array,
        /// or an empty string if the input byte array is <c>null</c>.
        /// </returns>
        public string GetStringFromBytes(byte[] bytes)
        {
            if (bytes == null)
            {
                // Always return a value as a fallback
                return "";
            }

            StringBuilder hex = new StringBuilder(bytes.Length * 2);

            // Iterate through each byte and convert to hexadecimal
            foreach (byte b in bytes)
            {
                hex.AppendFormat("{0:x2}", b);
            }

            return hex.ToString();
        }

        /// <summary>
        /// Removes dictionary keys exceeding a specified count
        /// </summary>
        /// <param name="segmentation"></param>
        /// <param name="maxCount"></param>
        /// <param name="prefix"></param>
        /// <param name="logger"></param>
        public void TruncateSegmentationValues(Dictionary<string, object>? segmentation, int maxCount, string prefix, CountlyLogHelper logger)
        {
            if (segmentation == null)
            {
                return;
            }

            List<string> keysToRemove = segmentation.Keys.Skip(maxCount).ToList();

            foreach (string key in keysToRemove)
            {
                logger.Warning($"{prefix}, Value exceeded the maximum segmentation count key:[{key}]");
                segmentation.Remove(key);
            }
        }

        /// <summary>
        /// Removes specific reserved keys from a dictionary of segmentation values
        /// </summary>
        /// <param name="segmentation"></param>
        /// <param name="reservedKeys"></param>
        /// <param name="messagePrefix"></param>
        /// <param name="logger"></param>
        public void RemoveReservedKeysFromSegmentation(Dictionary<string, object>? segmentation, string[] reservedKeys, string messagePrefix, CountlyLogHelper logger)
        {
            if (segmentation == null)
            {
                return;
            }

            foreach (string rKey in reservedKeys)
            {
                if (segmentation.ContainsKey(rKey))
                {
                    logger.Warning($"{messagePrefix} provided segmentation contains protected key [{rKey}]");
                    segmentation.Remove(rKey);
                }
            }
        }

        /// <summary>
        /// Returns the current timestamp in seconds using the Unix time format
        /// </summary>
        public int CurrentTimestampSeconds()
        {
            return (int)(DateTimeOffset.UtcNow.ToUnixTimeSeconds());
        }

        /// <summary>
        /// Creates a crypto-safe SHA-256 hashed random value.
        /// </summary>
        /// <returns>Randomly generated string</returns>
        public static string SafeRandomVal()
        {
            long timestamp = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            using (RandomNumberGenerator random = new RNGCryptoServiceProvider())
            {
                byte[] value = new byte[6];
                random.GetBytes(value);
                string b64Value = Convert.ToBase64String(value);
                return b64Value + timestamp;
            }
        }

        /// <summary>
        /// Removes unsupported data types from provided Dictionary
        /// </summary>
        /// <param name="data"></param>
        /// <param name="logger"></param>
        /// <returns>Returns true if any entry had been removed</returns>
        public bool RemoveUnsupportedDataTypes(Dictionary<string, object>? data, CountlyLogHelper? logger)
        {
            if (data == null)
            {
                return false;
            }

            List<string> keysToRemove = new List<string>();
            bool removed = false;

            foreach (var entry in data)
            {
                string key = entry.Key;
                object value = entry.Value;

                if (string.IsNullOrEmpty(key) || !(value is string || value is int || value is double || value is bool || value is float))
                {
                    // found unsupported data type or null key or value, add key to removal list
                    keysToRemove.Add(key);
                    removed = true;
                }
            }

            // Remove the keys marked for removal
            foreach (string key in keysToRemove)
            {
                data.Remove(key);
            }

            if (removed & logger != null)
            {
                logger.Warning("[Utils] Unsupported data types were removed from provided segmentation");
            }

            return removed;
        }

        /// <summary>
        /// Returns the device ID type of the current device ID.<br/>   
        /// 0 - developer provided<br/>
        /// 1 - SDK generated<br/>
        /// </summary>
        private int Type()
        {
            int type = 0;

            switch (_countly.Device.DeviceIdType)
            {
                case DeviceIdType.DeveloperProvided:
                    type = 0;
                    break;
                case DeviceIdType.SDKGenerated:
                    type = 1;
                    break;
                default:
                    break;
            }
            return type;
        }
    }
}
