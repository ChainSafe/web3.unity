using System;
using System.IO;

namespace MetaMask.Unity
{

    public static class MetaMaskUnityAnalytics
    {

        private const string PartnerName = "MetaMask";
        private const string AnalyticsFileName = "analytics";
        private static string MetaMaskUnityUserDirectoryPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "MetaMask");

        private static Guid Guid { get; set; }

        static MetaMaskUnityAnalytics()
        {
            Initialize();
        }

        public static void LogEvent(string data)
        {
            try
            {
                UnityEngine.VspAttribution.MetaMask.VspAttribution.SendAttributionEvent(data, PartnerName, Guid.ToString());
            }
            catch (Exception)
            {
            }
        }

        public static void Initialize()
        {
            try
            {
                if (!Directory.Exists(MetaMaskUnityUserDirectoryPath))
                {
                    Directory.CreateDirectory(MetaMaskUnityUserDirectoryPath);
                }
                string fullPath = Path.Combine(MetaMaskUnityUserDirectoryPath, AnalyticsFileName);
                Guid? guid = new Guid?();
                if (File.Exists(fullPath))
                {
                    guid = ExtractGuidFrom(fullPath);
                }
                SetUserGuid(guid, fullPath);
            }
            catch (Exception)
            {
            }
        }

        private static void SetUserGuid(Guid? guid, string analyticsFilePath)
        {
            if (guid.HasValue)
            {
                Guid = guid.Value;
            }
            else
            {
                Guid = Guid.NewGuid();
                File.WriteAllText(analyticsFilePath, Guid.ToString());
            }
        }

        private static Guid? ExtractGuidFrom(string analyticsFilePath)
        {
            Guid result;
            return !Guid.TryParse(File.ReadAllText(analyticsFilePath), out result) ? new Guid?() : new Guid?(result);
        }

    }

}