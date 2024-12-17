using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Plugins.CountlySDK.Enums;
using Plugins.CountlySDK.Helpers;
using Plugins.CountlySDK.Models;
using UnityEngine;

namespace Plugins.CountlySDK.Services
{
    public class ViewCountlyService : AbstractBaseService, IViewModule, IViewIDProvider
    {
        private class ViewData
        {
            public string ViewID;
            public long ViewStartTimeSeconds; // If this is 0, the view is not started yet or was paused
            public string ViewName;
            public bool IsAutoStoppedView; // Views started with "startAutoStoppedView" would have this as "true".
            public bool IsAutoPaused; // This marks that this view automatically paused when going to the background
            public Dictionary<string, object> ViewSegmentation;
        }

        private string currentViewID;
        private string previousViewID;
        private readonly string viewEventKey = "[CLY]_view";

        readonly Dictionary<string, ViewData> viewDataMap = new Dictionary<string, ViewData>();
        readonly Dictionary<string, object> automaticViewSegmentation = new Dictionary<string, object>();

        internal bool _isFirstView = true;
        internal readonly EventCountlyService _eventService;
        internal readonly Countly _cly;
        internal readonly CountlyUtils _utils;

        internal ISafeIDGenerator safeViewIDGenerator;

        readonly string[] reservedSegmentationKeysViews = { "name", "visit", "start", "segment" };
        private readonly Dictionary<string, DateTime> _viewToLastViewStartTime = new Dictionary<string, DateTime>();

        internal ViewCountlyService(Countly countly, CountlyUtils utils, CountlyConfiguration configuration, CountlyLogHelper logHelper, EventCountlyService eventService, ConsentCountlyService consentService) : base(configuration, logHelper, consentService)
        {
            Log.Debug("[ViewCountlyService] Initializing.");
            _cly = countly;
            _utils = utils;
            eventService.viewIDProvider = this;
            _eventService = eventService;
            safeViewIDGenerator = configuration.SafeViewIDGenerator;
        }

        #region PublicAPI

        /// <summary>
        /// Returns the current ViewID
        /// </summary>
        public string GetCurrentViewId()
        {
            return currentViewID == null ? "" : currentViewID;
        }

        /// <summary>
        /// Returns the previous ViewID
        /// </summary>
        public string GetPreviousViewId()
        {
            return previousViewID == null ? "" : previousViewID;
        }

        /// <summary>
        /// Manually starts a view with the given name.
        /// It can be used to open multiple views in parallel.
        /// </summary>
        /// <param name="viewName">Name of the view</param>
        /// <returns>ViewId</returns>
        public string StartView(string viewName)
        {
            lock (LockObj) {
                Log.Info("[ViewCountlyService] StartView, vn[" + viewName + "]");
                return StartViewInternal(viewName, null, false);
            }
        }

        /// <summary>
        /// Manually starts a view with the given name.
        /// It can be used to open multiple views in parallel.
        /// </summary>
        /// <param name="viewName">Name of the view</param>
        /// <param name="viewSegmentation">Segmentation that will be added to the view, set 'null' if none should be added</param>
        /// <returns>ViewId</returns>
        public string StartView(string viewName, Dictionary<string, object> viewSegmentation)
        {
            lock (LockObj) {
                Log.Info("[ViewCountlyService] StartView, vn[" + viewName + "] sg[" + (viewSegmentation == null ? "null" : JsonConvert.SerializeObject(viewSegmentation, new JsonSerializerSettings { Error = (_, args) => { args.ErrorContext.Handled = true; } })) + "]");
                return StartViewInternal(viewName, viewSegmentation, false);
            }
        }

        /// <summary>
        /// Manually starts a view with the given name. Starting any other view or calling this again, 
        /// closes the one that's opened automatically. <br/>
        /// This ensures a 'one view at a time' flow.
        /// </summary>
        /// <param name="viewName">Name of the view</param>
        /// <returns>ViewId</returns>
        public string StartAutoStoppedView(string viewName)
        {
            lock (LockObj) {
                Log.Info("[ViewCountlyService] StartAutoStoppedView, vn[" + viewName + "]");
                return StartAutoStoppedView(viewName, null);
            }
        }

        /// <summary>
        /// Manually starts a view with the given name. Starting any other view or calling this again, 
        /// closes the one that's opened automatically. <br/>
        /// This ensures a 'one view at a time' flow.
        /// </summary>
        /// <param name="viewName">Name of the view</param>
        /// <param name="viewSegmentation">Segmentation that will be added to the view, set 'null' if none should be added</param>
        /// <returns>ViewId</returns>
        public string StartAutoStoppedView(string viewName, Dictionary<string, object> viewSegmentation)
        {
            lock (LockObj) {
                Log.Info("[ViewCountlyService] StartAutoStoppedView, vn[" + viewName + "] sg[" + (viewSegmentation == null ? "null" : JsonConvert.SerializeObject(viewSegmentation, new JsonSerializerSettings { Error = (_, args) => { args.ErrorContext.Handled = true; } })) + "]");
                return StartViewInternal(viewName, viewSegmentation, true);
            }
        }

        /// <summary>
        /// Stops a view with the given name if it is open
        /// If multiple views with same name are open, last opened view will be closed.
        /// </summary>
        /// <param name="viewName">Name of the view</param>
        public void StopViewWithName(string viewName)
        {
            lock (LockObj) {
                Log.Info("[ViewCountlyService] StopViewWithName, vn[" + viewName + "]");
                StopViewWithNameInternal(viewName, null);
            }
        }

        /// <summary>
        /// Stops a view with the given name if it is open
        /// If multiple views with same name are open, last opened view will be closed.
        /// </summary>
        /// <param name="viewName">Name of the view</param>
        /// <param name="viewSegmentation">Segmentation that will be added to the view, set 'null' if none should be added</param>
        public void StopViewWithName(string viewName, Dictionary<string, object> viewSegmentation)
        {
            lock (LockObj) {
                Log.Info("[ViewCountlyService] StopViewWithName, vn[" + viewName + "] sg[" + (viewSegmentation == null ? "null" : JsonConvert.SerializeObject(viewSegmentation, new JsonSerializerSettings { Error = (_, args) => { args.ErrorContext.Handled = true; } })) + "]");
                StopViewWithNameInternal(viewName, viewSegmentation);
            }
        }

        /// <summary>
        /// Stops a view with the given ID if it is open
        /// </summary>
        /// <param name="viewID">ID of the view</param>
        public void StopViewWithID(string viewID)
        {
            lock (LockObj) {
                Log.Info("[ViewCountlyService] StopViewWithID, vi[" + viewID + "]");
                StopViewWithIDInternal(viewID, null);
            }
        }

        /// <summary>
        /// Stops a view with the given ID if it is open
        /// </summary>
        /// <param name="viewID">ID of the view</param>
        /// <param name="viewSegmentation">Segmentation that will be added to the view, set 'null' if none should be added</param>
        public void StopViewWithID(string viewID, Dictionary<string, object> viewSegmentation)
        {
            lock (LockObj) {
                Log.Info("[ViewCountlyService] StopViewWithID, vi[" + viewID + "] sg[" + (viewSegmentation == null ? "null" : JsonConvert.SerializeObject(viewSegmentation, new JsonSerializerSettings { Error = (_, args) => { args.ErrorContext.Handled = true; } })) + "]");
                StopViewWithIDInternal(viewID, viewSegmentation);
            }
        }

        /// <summary>
        /// Pauses a view with the given ID
        /// </summary>
        /// <param name="viewID">ID of the view</param>
        public void PauseViewWithID(string viewID)
        {
            lock (LockObj) {
                Log.Info("[ViewCountlyService] PauseViewWithID, vi[" + viewID + "]");
                PauseViewWithIDInternal(viewID, false);
            }
        }

        /// <summary>
        /// Resumes a view with the given ID
        /// </summary>
        /// <param name="viewID">ID of the view</param>
        public void ResumeViewWithID(string viewID)
        {
            lock (LockObj) {
                Log.Info("[ViewCountlyService] ResumeViewWithID, vi[" + viewID + "]");
                ResumeViewWithIDInternal(viewID);
            }
        }

        /// <summary>
        /// Stops all views and records a segmentation if set
        /// </summary>
        /// <param name="viewSegmentation">Segmentation that will be added, set 'null' if none should be added</param>
        public void StopAllViews(Dictionary<string, object> viewSegmentation)
        {
            lock (LockObj) {
                Log.Info("[ViewCountlyService] StopAllViews, sg[" + (viewSegmentation == null ? "null" : JsonConvert.SerializeObject(viewSegmentation, new JsonSerializerSettings { Error = (_, args) => { args.ErrorContext.Handled = true; } })) + "]");
                StopAllViewsInternal(viewSegmentation);
            }
        }

        /// <summary>
        /// Set a segmentation to be recorded with all views
        /// </summary>
        /// <param name="viewSegmentation">Global View Segmentation</param>
        public void SetGlobalViewSegmentation(Dictionary<string, object> viewSegmentation)
        {
            lock (LockObj) {
                Log.Info("[ViewCountlyService] SetGlobalViewSegmentation, sg[" + (viewSegmentation == null ? "null" : JsonConvert.SerializeObject(viewSegmentation, new JsonSerializerSettings { Error = (_, args) => { args.ErrorContext.Handled = true; } })) + "]");
                SetGlobalViewSegmentationInternal(viewSegmentation);
            }
        }

        /// <summary>
        /// Updates the segmentation of a view with view id.
        /// </summary>
        /// <param name="viewID">ID of the view</param>
        /// <param name="viewSegmentation">Segmentation that will be added to the view</param>
        public void AddSegmentationToViewWithID(string viewID, Dictionary<string, object> viewSegmentation)
        {
            lock (LockObj) {
                Log.Info("[ViewCountlyService] AddSegmentationToViewWithID, for view ID: [" + viewID + "]");
                AddSegmentationToViewWithIDInternal(viewID, viewSegmentation);
            }
        }

        /// <summary>
        /// Updates the segmentation of a view with view name.
        /// If multiple views with same name are open, last opened view will be updated.
        /// </summary>
        /// <param name="viewName">Name of the view</param>
        /// <param name="viewSegmentation">Segmentation that will be added to the view</param>
        public void AddSegmentationToViewWithName(string viewName, Dictionary<string, object> viewSegmentation)
        {
            lock (LockObj) {
                Log.Info("[ViewCountlyService] AddSegmentationToViewWithName, for Name: [" + viewName + "]");
                AddSegmentationToViewWithNameInternal(viewName, viewSegmentation);
            }
        }

        /// <summary>
        /// Updates the global segmentation
        /// </summary>
        /// <param name="viewSegmentation">Segmentation that will be added to the view</param>
        public void UpdateGlobalViewSegmentation(Dictionary<string, object> viewSegmentation)
        {
            lock (LockObj) {
                Log.Info("[ViewCountlyService] UpdateGlobalViewSegmentation, sg[" + (viewSegmentation == null ? "null" : JsonConvert.SerializeObject(viewSegmentation, new JsonSerializerSettings { Error = (_, args) => { args.ErrorContext.Handled = true; } })) + "]");
                UpdateGlobalViewSegmentationInternal(viewSegmentation);
            }
        }
        #endregion
        #region InternalMethods
        /// <summary>
        /// Starts the view with given viewName, segmentation
        /// </summary>
        /// <param name="viewName">name of the view</param>
        /// <param name="customViewSegmentation">segmentation that will be added to the view, set 'null' if none should be added</param>
        /// <param name="viewShouldBeAutomaticallyStopped"></param>
        /// <returns></returns>
        private string StartViewInternal(string viewName, Dictionary<string, object> customViewSegmentation, bool viewShouldBeAutomaticallyStopped)
        {
            if (!_cly.IsSDKInitialized) {
                Log.Warning("[ViewCountlyService] StartViewInternal, Countly.Instance.Init() must be called before StartViewInternal");
                return null;
            }

            if(!_consentService.CheckConsentInternal(Consents.Views)) {
                Log.Debug("[ViewCountlyService] StartView, consent is not given, ignoring the request.");
                return null;
            }

            if (_utils.IsNullEmptyOrWhitespace(viewName)) {
                Log.Warning("[ViewCountlyService] StartViewInternal, trying to record view with null or empty view name, ignoring request");
                return null;
            }

            if (viewName.Length > _configuration.GetMaxKeyLength()) {
                Log.Verbose("[ViewCountlyService] StartViewInternal, max allowed key length is " + _configuration.GetMaxKeyLength());
                viewName = viewName.Substring(0, _configuration.GetMaxKeyLength());
            }

            _utils.TruncateSegmentationValues(customViewSegmentation, _configuration.GetMaxSegmentationValues(), "[ViewCountlyService] StartViewInternal, ", Log);
            _utils.RemoveReservedKeysFromSegmentation(customViewSegmentation, reservedSegmentationKeysViews, "[ViewCountlyService] StartViewInternal, ", Log);

            Log.Debug("[ViewCountlyService] StartViewInternal, recording view with name: [" + viewName + "], previous view ID:[" + currentViewID + "], custom view segmentation:[" + customViewSegmentation?.ToString() + "], first:[" + _isFirstView + "], autoStop:[" + viewShouldBeAutomaticallyStopped + "]");

            AutoCloseRequiredViews(false, null);

            ViewData currentViewData = new ViewData();
            currentViewData.ViewID = safeViewIDGenerator.GenerateValue();
            currentViewData.ViewName = viewName;
            currentViewData.ViewStartTimeSeconds = _utils.CurrentTimestampSeconds();
            currentViewData.IsAutoStoppedView = viewShouldBeAutomaticallyStopped;

            viewDataMap.Add(currentViewData.ViewID, currentViewData);
            previousViewID = currentViewID;
            currentViewID = currentViewData.ViewID;

            Dictionary<string, object> accumulatedEventSegm = new Dictionary<string, object>(automaticViewSegmentation);

            if (customViewSegmentation != null) {
                _utils.CopyDictionaryToDestination(accumulatedEventSegm, customViewSegmentation, Log);
            }

            Dictionary<string, object> viewSegmentation = CreateViewEventSegmentation(currentViewData, _isFirstView, true, accumulatedEventSegm);

            if (_isFirstView) {
                Log.Debug("[ViewCountlyService] StartViewInternal, recording view as the first one in the session. [" + viewName + "]");
                _isFirstView = false;
            }
            _ = _eventService.RecordEventInternal(viewEventKey, viewSegmentation, 1, 0, null, currentViewData.ViewID);

            return currentViewData.ViewID;
        }

        /// <summary>
        /// Stops a view with the given name if it was open.
        /// </summary>
        /// <param name="viewName">name of the view</param>
        /// <param name="customViewSegmentation"></param>
        private void StopViewWithNameInternal(string viewName, Dictionary<string, object> customViewSegmentation)
        {
            if (!_consentService.CheckConsentInternal(Consents.Views)) {
                Log.Debug("[ViewCountlyService] StopViewWithNameInternal, consent is not given, ignoring the request.");
                return;
            }

            if (_utils.IsNullEmptyOrWhitespace(viewName)) {
                Log.Warning("[ViewCountlyService] StopViewWithNameInternal, trying to record view with null or empty view name, ignoring request");
                return;
            }

            if (viewName.Length > _configuration.GetMaxKeyLength()) {
                Log.Verbose("[ViewCountlyService] StopViewWithNameInternal, max allowed key length is " + _configuration.GetMaxKeyLength());
                viewName = viewName.Substring(0, _configuration.GetMaxKeyLength());
            }

            customViewSegmentation = (Dictionary<string, object>)RemoveSegmentInvalidDataTypes(customViewSegmentation);

            string viewID = null;

            foreach (KeyValuePair<string, ViewData> entry in viewDataMap) {
                string key = entry.Key;
                ViewData vd = entry.Value;

                if (vd != null && viewName.Equals(vd.ViewName)) {
                    viewID = key;
                }
            }

            if (viewID == null) {
                Log.Warning("[ViewCountlyService] StopViewWithNameInternal, no view entry found with the provided name :[" + viewName + "]");
                return;
            }

            StopViewWithIDInternal(viewID, customViewSegmentation);
        }

        /// <summary>
        /// Closes given views or all views.
        /// </summary>
        /// <param name="closeAllViews"></param>
        /// <param name="customViewSegmentation"></param>
        private void AutoCloseRequiredViews(bool closeAllViews, Dictionary<string, object> customViewSegmentation)
        {
            if (!_consentService.CheckConsentInternal(Consents.Views)) {
                Log.Debug("[ViewCountlyService] AutoCloseRequiredViews, consent is not given, ignoring the request.");
                return;
            }

            Log.Debug("[ViewCountlyService] AutoCloseRequiredViews, closing required views.");
            List<string> viewsToRemove = new List<string>(1);

            foreach (var entry in viewDataMap) {
                ViewData vd = entry.Value;
                if (closeAllViews || vd.IsAutoStoppedView) {
                    viewsToRemove.Add(vd.ViewID);
                }
            }

            if (viewsToRemove.Count > 0) {
                Log.Debug("[ViewCountlyService] AutoCloseRequiredViews, about to close [" + viewsToRemove.Count + "] views");
            }

            _utils.RemoveReservedKeysFromSegmentation(customViewSegmentation, reservedSegmentationKeysViews, "[ViewCountlyService] AutoCloseRequiredViews, ", Log);

            for (int i = 0; i < viewsToRemove.Count; i++) {
                StopViewWithIDInternal(viewsToRemove[i], customViewSegmentation);
            }
        }

        /// <summary>
        /// Stops a view with the given ID if it was open.
        /// </summary>
        /// <param name="viewID">ID of the view</param>
        /// <param name="viewSegmentation">view segmentation</param>
        private void StopViewWithIDInternal(string viewID, Dictionary<string, object> customViewSegmentation)
        {
            if (!_consentService.CheckConsentInternal(Consents.Views)) {
                Log.Debug("[ViewCountlyService] StopViewWithIDInternal, consent is not given, ignoring the request.");
                return;
            }

            if (_utils.IsNullEmptyOrWhitespace(viewID)) {
                Log.Warning("[ViewCountlyService] StopViewWithIDInternal, trying to record view with null or empty view ID, ignoring request");
                return;
            }

            if (!viewDataMap.ContainsKey(viewID)) {
                Log.Warning("[ViewCountlyService] StopViewWithIDInternal, there is no view with the provided view id to close");
                return;
            }

            ViewData vd = viewDataMap[viewID];
            if (vd == null) {
                Log.Warning("[ViewCountlyService] StopViewWithIDInternal, view id:[" + viewID + "] has a 'null' value. This should not be happening");
                return;
            }
            
            customViewSegmentation = (Dictionary<string, object>)RemoveSegmentInvalidDataTypes(customViewSegmentation);

            Log.Debug("[ViewCountlyService] View [" + vd.ViewName + "], id:[" + vd.ViewID + "] is getting closed, reporting duration: [" + (_utils.CurrentTimestampSeconds() - vd.ViewStartTimeSeconds) + "] s, current timestamp: [" + _utils.CurrentTimestampSeconds() + "]");
            _utils.TruncateSegmentationValues(customViewSegmentation, _configuration.GetMaxSegmentationValues(), "[ViewCountlyService] StopViewWithIDInternal", Log);
            RecordViewEndEvent(vd, customViewSegmentation, "StopViewWithIDInternal");
            viewDataMap.Remove(vd.ViewID);
        }

        /// <summary>
        /// Pauses a view with the given ID.
        /// </summary>
        /// <param name="viewID"></param>
        /// <param name="pausedAutomatically"></param>
        private void PauseViewWithIDInternal(string viewID, bool pausedAutomatically)
        {
            if (!_consentService.CheckConsentInternal(Consents.Views)) {
                Log.Debug("[ViewCountlyService] PauseViewWithIDInternal, consent is not given, ignoring the request.");
                return;
            }

            if (_utils.IsNullEmptyOrWhitespace(viewID)) {
                Log.Warning("[ViewCountlyService] PauseViewWithIDInternal, trying to record view with null or empty view ID, ignoring request");
                return;
            }

            if (!viewDataMap.ContainsKey(viewID)) {
                Log.Warning("[ViewCountlyService] PauseViewWithIDInternal, there is no view with the provided view id to close");
                return;
            }

            ViewData vd = viewDataMap[viewID];
            if (vd == null) {
                Log.Warning("[ViewCountlyService] PauseViewWithIDInternal, view id:[" + viewID + "] has a 'null' value. This should not be happening, auto paused:[" + pausedAutomatically + "]");
                return;
            }

            Log.Debug("[ViewCountlyService] PauseViewWithIDInternal, pausing view for ID:[" + viewID + "], name:[" + vd.ViewName + "]");

            if (vd.ViewStartTimeSeconds == 0) {
                Log.Warning("[ViewCountlyService] PauseViewWithIDInternal, pausing a view that is already paused. ID:[" + viewID + "], name:[" + vd.ViewName + "]");
                return;
            }

            vd.IsAutoPaused = pausedAutomatically;
            RecordViewEndEvent(vd, null, "PauseViewWithIDInternal");
            vd.ViewStartTimeSeconds = 0;
        }

        /// <summary>
        /// Records event with given ViewData and filtered segmentation.
        /// </summary>
        /// <param name="vd"></param>
        /// <param name="filteredCustomViewSegmentation"></param>
        /// <param name="viewRecordingSource"></param>
        private void RecordViewEndEvent(ViewData vd, Dictionary<string, object> filteredCustomViewSegmentation, string viewRecordingSource)
        {
            long lastElapsedDurationSeconds = 0;

            if (vd.ViewStartTimeSeconds < 0) {
                Log.Warning("[ViewCountlyService] " + viewRecordingSource + ", view start time value is not normal: [" + vd.ViewStartTimeSeconds + "], ignoring that duration");
            } else if (vd.ViewStartTimeSeconds == 0) {
                Log.Info("[ViewCountlyService] " + viewRecordingSource + ", view is either paused or didn't run, ignoring start timestamp");
            } else {
                lastElapsedDurationSeconds = _utils.CurrentTimestampSeconds() - vd.ViewStartTimeSeconds;
            }

            if (vd.ViewName == null) {
                Log.Warning("[ViewCountlyService] StopViewWithIDInternal, view has no internal name, ignoring it");
                return;
            }

            Dictionary<string, object> accumulatedEventSegm = new Dictionary<string, object>(automaticViewSegmentation);
            if (filteredCustomViewSegmentation != null) {
                _utils.CopyDictionaryToDestination(accumulatedEventSegm, filteredCustomViewSegmentation, Log);
            }

            if (vd.ViewSegmentation != null) {
                _utils.CopyDictionaryToDestination(accumulatedEventSegm, vd.ViewSegmentation, Log);
            }

            long viewDurationSeconds = lastElapsedDurationSeconds;
            Dictionary<string, object> segments = CreateViewEventSegmentation(vd, false, false, accumulatedEventSegm);
            _ = _eventService.RecordEventInternal(CountlyEventModel.ViewEvent, segments, duration: viewDurationSeconds, eventIDOverride: vd.ViewID);
        }

        /// <summary>
        /// Creates view event segmentation
        /// </summary>
        /// <param name="vd"></param>
        /// <param name="firstView"></param>
        /// <param name="visit"></param>
        /// <param name="customViewSegmentation"></param>
        /// <returns></returns>
        private Dictionary<string, object> CreateViewEventSegmentation(ViewData vd, bool firstView, bool visit, Dictionary<string, object> customViewSegmentation)
        {
            Dictionary<string, object> viewSegmentation = new Dictionary<string, object>();
            if (customViewSegmentation != null) {
                _utils.CopyDictionaryToDestination(viewSegmentation, customViewSegmentation, Log);
            }

            viewSegmentation.Add("name", vd.ViewName);
            if (visit) {
                viewSegmentation.Add("visit", 1);
            }
            if (firstView) {
                viewSegmentation.Add("start", 1);
            }
            viewSegmentation.Add("segment", _configuration.metricHelper.OS);

            return viewSegmentation;
        }

        /// <summary>
        /// Resumes a paused view with the given ID. 
        /// </summary>
        /// <param name="viewID"></param>
        private void ResumeViewWithIDInternal(string viewID)
        {
            if (!_consentService.CheckConsentInternal(Consents.Views)) {
                Log.Debug("[ViewCountlyService] ResumeViewWithIDInternal, consent is not given, ignoring the request.");
                return;
            }

            if (_utils.IsNullEmptyOrWhitespace(viewID)) {
                Log.Warning("[ViewCountlyService] ResumeViewWithIDInternal, trying to record view with null or empty view ID, ignoring request");
                return;
            }

            if (!viewDataMap.ContainsKey(viewID)) {
                Log.Warning("[ViewCountlyService] ResumeViewWithIDInternal, there is no view with the provided view id to close");
                return;
            }

            ViewData vd = viewDataMap[viewID];
            if (vd == null) {
                Log.Warning("[ViewCountlyService] ResumeViewWithIDInternal, view id:[" + viewID + "] has a 'null' value. This should not be happening");
                return;
            }

            Log.Debug("[ViewCountlyService] ResumeViewWithIDInternal, resuming view for ID:[" + viewID + "], name:[" + vd.ViewName + "]");

            if (vd.ViewStartTimeSeconds > 0) {
                Log.Warning("[ViewCountlyService] ResumeViewWithIDInternal, resuming a view that is already running. ID:[" + viewID + "], name:[" + vd.ViewName + "]");
                return;
            }

            vd.ViewStartTimeSeconds = _utils.CurrentTimestampSeconds();
            vd.IsAutoPaused = false;
        }

        /// <summary>
        /// Stops all open views and records a segmentation if set.
        /// </summary>
        /// <param name="viewSegmentation"></param>
        private void StopAllViewsInternal(Dictionary<string, object> viewSegmentation)
        {
            if (!_consentService.CheckConsentInternal(Consents.Views)) {
                Log.Debug("[ViewCountlyService] StopAllViewsInternal, consent is not given, ignoring the request.");
                return;
            }

            Log.Debug("[ViewCountlyService] StopAllViewsInternal");

            viewSegmentation = (Dictionary<string, object>)RemoveSegmentInvalidDataTypes(viewSegmentation);

            AutoCloseRequiredViews(true, viewSegmentation);
        }

        /// <summary>
        /// Set a segmentation to be recorded with all views
        /// </summary>
        /// <param name="viewSegmentation"></param>
        private void SetGlobalViewSegmentationInternal(Dictionary<string, object> viewSegmentation)
        {
            if (!_consentService.CheckConsentInternal(Consents.Views)) {
                Log.Debug("[ViewCountlyService] SetGlobalViewSegmentation, consent is not given, ignoring the request.");
                return;
            }

            Log.Debug("[ViewCountlyService] SetGlobalViewSegmentationInternal, with[" + (viewSegmentation == null ? "null" : viewSegmentation.Count.ToString()) + "] entries");
            automaticViewSegmentation.Clear();

            if (viewSegmentation != null) {
                _utils.RemoveReservedKeysFromSegmentation(viewSegmentation, reservedSegmentationKeysViews, "[ViewCountlyService] SetGlobalViewSegmentationInternal, ", Log);
                viewSegmentation = (Dictionary<string, object>)RemoveSegmentInvalidDataTypes(viewSegmentation);
                _utils.CopyDictionaryToDestination(automaticViewSegmentation, viewSegmentation, Log);
            }
        }

        /// <summary>
        /// Updates the segmentation of a view.
        /// </summary>
        /// <param name="viewID"></param>
        /// <param name="viewSegmentation"></param>
        private void AddSegmentationToViewWithIDInternal(string viewID, Dictionary<string, object> viewSegmentation)
        {
            if (!_consentService.CheckConsentInternal(Consents.Views)) {
                Log.Debug("[ViewCountlyService] AddSegmentationToViewWithIDInternal, consent is not given, ignoring the request.");
                return;
            }

            if (_utils.IsNullEmptyOrWhitespace(viewID) || viewSegmentation == null) {
                Log.Warning("[ViewsCountlyService] AddSegmentationToViewWithIDInternal, null or empty parameters provided");
                return;
            }

            if (!viewDataMap.ContainsKey(viewID)) {
                Log.Warning("[ViewsCountlyService] AddSegmentationToViewWithIDInternal, there is no view with the provided view id");
                return;
            }

            ViewData vd = viewDataMap[viewID];
            if (vd == null) {
                Log.Warning("[ViewsCountlyService] AddSegmentationToViewWithIDInternal, view id:[" + viewID + "] has a 'null' view data. This should not be happening");
                return;
            }

            _utils.TruncateSegmentationValues(viewSegmentation, _cly.Configuration.GetMaxSegmentationValues(), "[ViewsCountlyService] AddSegmentationToViewWithIDInternal", Log);
            _utils.RemoveReservedKeysFromSegmentation(viewSegmentation, reservedSegmentationKeysViews, "[ViewsCountlyService] AddSegmentationToViewWithIDInternal, ", Log);

            if (vd.ViewSegmentation == null) {
                vd.ViewSegmentation = new Dictionary<string, object>(viewSegmentation);
            } else {
                _utils.CopyDictionaryToDestination(vd.ViewSegmentation, vd.ViewSegmentation, Log);
            }
        }

        /// <summary>
        /// Updates the segmentation of a view.
        /// </summary>
        /// <param name="viewName"></param>
        /// <param name="viewSegmentation"></param>
        private void AddSegmentationToViewWithNameInternal(string viewName, Dictionary<string, object> viewSegmentation)
        {
            if (!_consentService.CheckConsentInternal(Consents.Views)) {
                Log.Debug("[ViewCountlyService] AddSegmentationToViewWithNameInternal, consent is not given, ignoring the request.");
                return;
            }

            string viewID = null;

            foreach (var entry in viewDataMap) {
                string key = entry.Key;
                ViewData vd = entry.Value;

                if (vd != null && viewName != null && viewName.Equals(vd.ViewName)) {
                    viewID = key;
                }
            }

            if (viewID == null) {
                Log.Warning("[ViewsCountlyService] AddSegmentationToViewWithNameInternal, no view entry found with the provided name :[" + viewName + "]");
                return;
            }

            Log.Info("[ViewsCountlyService] AddSegmentationToViewWithNameInternal, will add segmentation for view: [" + viewName + "] with ID:[" + viewID + "]");

            AddSegmentationToViewWithIDInternal(viewID, viewSegmentation);
        }

        /// <summary>
        /// Updates the global segmentation
        /// </summary>
        /// <param name="viewSegmentation"></param>
        private void UpdateGlobalViewSegmentationInternal(Dictionary<string, object> viewSegmentation)
        {
            if (!_consentService.CheckConsentInternal(Consents.Views)) {
                Log.Debug("[ViewCountlyService] UpdateGlobalViewSegmentationInternal, consent is not given, ignoring the request.");
                return;
            }

            if (viewSegmentation == null) {
                Log.Warning("[ViewCountlyService] UpdateGlobalViewSegmentationInternal, when updating segmentation values, they can't be 'null'. Ignoring request.");
                return;
            }

            viewSegmentation = (Dictionary<string, object>)RemoveSegmentInvalidDataTypes(viewSegmentation);
            
            _utils.RemoveReservedKeysFromSegmentation(viewSegmentation, reservedSegmentationKeysViews, "[ViewsCountlyService] UpdateGlobalViewSegmentationInternal, ", Log);
            _utils.CopyDictionaryToDestination(automaticViewSegmentation, viewSegmentation, Log);
        }
        #endregion
        #region Deprecated Methods
        /// <summary>
        /// Records the opening of a view. This method is deprecated.
        /// </summary>
        /// <remarks>
        /// This method must be used in conjunction with <see cref="RecordCloseViewAsync"/>.
        /// Do not use with <see cref="StopView"/> as it will not function correctly.
        /// Please use <see cref="StartView"/> and <see cref="StopView"/> for new implementations.
        /// </remarks>
        /// <param name="name">The name of the view to open.</param>
        /// <param name="segmentation">Optional segmentation data for the view.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        [Obsolete("RecordOpenViewAsync(string name, IDictionary<string, object> segmentation = null) is deprecated and will be removed in the future. Please use StartView instead.")]
        public async Task RecordOpenViewAsync(string name, IDictionary<string, object> segmentation = null)
        {
            if (!_consentService.CheckConsentInternal(Consents.Views)) {
                Log.Debug("[ViewCountlyService] RecordOpenViewAsync, consent is not given, ignoring the request.");
                return;
            }

            lock (LockObj) {
                Log.Info("[ViewCountlyService] RecordOpenViewAsync: name = " + name);
                StartViewInternal(name, (Dictionary<string, object>)segmentation, false);
            }

            await Task.CompletedTask;
        }

        /// <summary>
        /// Records the closing of a view. This method is deprecated.
        /// </summary>
        /// <remarks>
        /// This method should only be used to close views that were opened using <see cref="RecordOpenViewAsync"/>.
        /// Do not use to close views started with <see cref="StartView"/>.
        /// </remarks>
        /// <param name="name">The name of the view to close.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        [Obsolete("RecordCloseViewAsync(string name) is deprecated and will be removed in the future. Please use StopView instead.")]
        public async Task RecordCloseViewAsync(string name)
        {
            if (!_consentService.CheckConsentInternal(Consents.Views)) {
                Log.Debug("[ViewCountlyService] RecordCloseViewAsync, consent is not given, ignoring the request.");
                return;
            }

            lock (LockObj) {
                Log.Info("[ViewCountlyService] RecordCloseViewAsync: name = " + name);
                StopViewWithNameInternal(name, null);
            }

            await Task.CompletedTask;
        }

        /// <summary>
        /// Reports a particular action with the specified details.
        /// </summary>
        /// <remarks>
        /// <para>This method is deprecated and will be removed in a future release. There is no direct replacement for this method.</para>
        /// <para>Consider re-evaluating the need for this functionality or implementing a custom solution as needed.</para>
        /// </remarks>
        /// <param name="type">The type of action.</param>
        /// <param name="x">The x-coordinate.</param>
        /// <param name="y">The y-coordinate.</param>
        /// <param name="width">The width of the screen.</param>
        /// <param name="height">The height of the screen.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        [Obsolete("ReportActionAsync(string type, int x, int y, int width, int height) is deprecated and will be removed in the future.")]
        public async Task ReportActionAsync(string type, int x, int y, int width, int height)
        {
            lock (LockObj) {
                Log.Info("[ViewCountlyService] ReportActionAsync : type = " + type + ", x = " + x + ", y = " + y + ", width = " + width + ", height = " + height);

                if (!_consentService.CheckConsentInternal(Consents.Views)) {
                    Log.Debug("[ViewCountlyService] ReportActionAsync, consent is not given, ignoring the request.");
                    return;
                }

                IDictionary<string, object> segmentation = new Dictionary<string, object>()
                {
                    {"type", type},
                    {"x", x},
                    {"y", y},
                    {"width", width},
                    {"height", height},
                };
                CountlyEventModel currentView = new CountlyEventModel(CountlyEventModel.ViewActionEvent, segmentation);
                _ = _eventService.RecordEventAsync(currentView);
            }
            await Task.CompletedTask;
        }

        #region override Methods
        internal override void DeviceIdChanged(string deviceId, bool merged)
        {
            if (!merged) {
                _isFirstView = true;
            }
        }
        #endregion

        #endregion
    }
}