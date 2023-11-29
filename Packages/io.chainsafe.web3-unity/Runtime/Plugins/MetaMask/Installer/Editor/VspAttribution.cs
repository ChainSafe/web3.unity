using System;

using UnityEditor;

using UnityEngine.Analytics;

namespace UnityEngine.VspAttribution.MetaMask
{
    public static class VspAttribution
    {
        private const int k_VersionId = 4;
        private const int k_MaxEventsPerHour = 10;
        private const int k_MaxNumberOfElements = 1000;
        private const string k_VendorKey = "unity.vsp-attribution";
        private const string k_EventName = "vspAttribution";

        private static bool RegisterEvent()
        {
#if UNITY_EDITOR
            AnalyticsResult result = EditorAnalytics.RegisterEventWithLimit(k_EventName, k_MaxEventsPerHour,
                k_MaxNumberOfElements, k_VendorKey, k_VersionId);
#else // IF !UNITY_EDITOR
			AnalyticsResult result = Analytics.Analytics.RegisterEvent(k_EventName, k_MaxEventsPerHour,
				k_MaxNumberOfElements, k_VendorKey, k_VersionId);
#endif

            bool isResultOk = result == AnalyticsResult.Ok;
            return isResultOk;
        }

        [Serializable]
        private struct VspAttributionData
        {
            public string actionName;
            public string partnerName;
            public string customerUid;
            public string extra;
        }

        /// <summary>
        /// Registers and attempts to send a VSP Attribution event.
        /// </summary>
        /// <param name="actionName">Name of the action, identifying a place this event was called from.</param>
        /// <param name="partnerName">Identifiable Verified Solutions Partner name.</param>
        /// <param name="customerUid">Unique identifier of the customer using Partner's Verified Solution.</param>
        public static AnalyticsResult SendAttributionEvent(string actionName, string partnerName, string customerUid)
        {
            try
            {
#if UNITY_EDITOR
                // Are Editor Analytics enabled ? (Preferences)
                // The event shouldn't be able to report if this is disabled but if we know we're not going to report
                // lets early out and not spend time gathering all the data
                bool isEditorAnalyticsEnabled = EditorAnalytics.enabled;

                if (!isEditorAnalyticsEnabled)
                    return AnalyticsResult.AnalyticsDisabled;
#else // IF !UNITY_EDITOR
				bool isRuntimeAnalyticsEnabled = Analytics.Analytics.enabled;
				
				if (!isRuntimeAnalyticsEnabled)
					return AnalyticsResult.AnalyticsDisabled;
				
				if (!Debug.isDebugBuild)
					return AnalyticsResult.UnsupportedPlatform;
#endif

                // Can an event be registered?
                bool isEventRegistered = RegisterEvent();

                if (!isEventRegistered)
                    return AnalyticsResult.InvalidData;

                // Create an expected data object
                var eventData = new VspAttributionData
                {
                    actionName = actionName,
                    partnerName = partnerName,
                    customerUid = customerUid,
                    extra = "{}"
                };

#if UNITY_EDITOR
                // Send the Attribution Event
                var eventResult = EditorAnalytics.SendEventWithLimit(k_EventName, eventData, k_VersionId);
#else // IF !UNITY_EDITOR
				var eventResult = Analytics.Analytics.SendEvent(k_EventName, eventData, k_VersionId);
#endif
                return eventResult;
            }
            catch
            {
                // Fail silently
                return AnalyticsResult.AnalyticsDisabled;
            }
        }
    }
}