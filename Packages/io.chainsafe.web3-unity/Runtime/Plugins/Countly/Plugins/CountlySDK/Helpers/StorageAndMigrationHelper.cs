using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using iBoxDB.LocalServer;
using Newtonsoft.Json;
using Plugins.CountlySDK.Models;
using Plugins.CountlySDK.Persistance.Dao;
using Plugins.CountlySDK.Persistance.Entities;
using Plugins.CountlySDK.Persistance.Repositories;
using Plugins.CountlySDK.Persistance.Repositories.Impls;
using Plugins.iBoxDB;
using UnityEngine;
using System.Web;
using System.Text;
using UnityEngine.Networking;
using Plugins.CountlySDK.Enums;
using Plugins.CountlySDK.Persistance;

namespace Plugins.CountlySDK.Helpers
{
    public enum EntityType
    {
        ViewEvents, NonViewEvents, Requests, ViewEventSegments, NonViewEventSegments, Configs, EventNumberInSameSessions
    }

    internal class StorageAndMigrationHelper
    {
        private DB _db;
        internal int CurrentVersion = 0;
        private const long _dbNumber = 3;
        internal readonly int SchemaVersion = 3;
        public const string key_from_2_to_3_custom_id_set = "2_3_custom_id_set";

        private CountlyLogHelper _logHelper;
        private readonly RequestBuilder _requestBuilder;
        internal SegmentDao EventSegmentDao { get; private set; }

        internal Dao<ConfigEntity> ConfigDao { get; private set; }
        internal Dao<RequestEntity> RequestDao { get; private set; }
        internal Dao<EventEntity> EventDao { get; private set; }
        internal Dao<EventNumberInSameSessionEntity> EventNrInSameSessionDao { get; private set; }

        internal RequestRepository RequestRepo { get; private set; }
        internal NonViewEventRepository EventRepo { get; private set; }
        internal ViewEventRepository ViewRepo { get; private set; }

        internal StorageAndMigrationHelper(CountlyLogHelper logHelper, RequestBuilder requestBuilder)
        {
            _logHelper = logHelper;
            _requestBuilder = requestBuilder;

            if (FirstLaunchAppHelper.IsFirstLaunchApp) {
                CurrentVersion = SchemaVersion;
                PlayerPrefs.SetInt(Constants.SchemaVersion, SchemaVersion);
            } else {
                CurrentVersion = PlayerPrefs.GetInt(Constants.SchemaVersion, 0);
            }
        }

        /// <summary>
        /// Create database and tables
        /// </summary>
        private DB BuildDatabase(long dbNumber)
        {
            DB.Root(Application.persistentDataPath);
            DB db = new DB(dbNumber);

            db.GetConfig().EnsureTable<SegmentEntity>(EntityType.Configs.ToString(), "Id");
            db.GetConfig().EnsureTable<RequestEntity>(EntityType.Requests.ToString(), "Id");
            db.GetConfig().EnsureTable<EventEntity>(EntityType.NonViewEvents.ToString(), "Id");
            db.GetConfig().EnsureTable<SegmentEntity>(EntityType.NonViewEventSegments.ToString(), "Id");

            if (CurrentVersion < 1) {
                db.GetConfig().EnsureTable<EventEntity>(EntityType.ViewEvents.ToString(), "Id");
                db.GetConfig().EnsureTable<SegmentEntity>(EntityType.ViewEventSegments.ToString(), "Id");
                db.GetConfig().EnsureTable<EventNumberInSameSessionEntity>(EntityType.EventNumberInSameSessions.ToString(), "Id");
            }

            return db;
        }

        /// <summary>
        /// Open database connection and initialize data access objects.
        /// </summary>
        internal void OpenDB()
        {
            _logHelper.Debug("[CountlyStorageHelper] OpenDB");

            _db = BuildDatabase(_dbNumber);
            DB.AutoBox auto = _db.Open();

            _logHelper.Debug("[CountlyStorageHelper] OpenDB path: " + Application.persistentDataPath + "/db3.box");

            EventSegmentDao = new SegmentDao(auto, EntityType.NonViewEventSegments.ToString(), _logHelper);

            ConfigDao = new Dao<ConfigEntity>(auto, EntityType.Configs.ToString(), _logHelper);
            RequestDao = new Dao<RequestEntity>(auto, EntityType.Requests.ToString(), _logHelper);
            EventDao = new Dao<EventEntity>(auto, EntityType.NonViewEvents.ToString(), _logHelper);

            if (CurrentVersion < 1) {
                EventNrInSameSessionDao = new Dao<EventNumberInSameSessionEntity>(auto, EntityType.EventNumberInSameSessions.ToString(), _logHelper);

                Dao<EventEntity> viewDao = new Dao<EventEntity>(auto, EntityType.ViewEvents.ToString(), _logHelper);
                SegmentDao viewSegmentDao = new SegmentDao(auto, EntityType.ViewEventSegments.ToString(), _logHelper);

                ViewRepo = new ViewEventRepository(viewDao, viewSegmentDao, _logHelper);
                ViewRepo.Initialize();
            }

            RequestRepo = new RequestRepository(RequestDao, _logHelper);
            EventRepo = new NonViewEventRepository(EventDao, EventSegmentDao, _logHelper);

            EventRepo.Initialize();
            RequestRepo.Initialize();

        }

        /// <summary>
        /// Close database connection.
        /// </summary>
        internal void CloseDB()
        {
            _logHelper.Debug("[CountlyStorageHelper] CloseDB");

            _db.Close();
        }

        /// <summary>
        /// Migrate database schema.
        /// </summary>
        internal void RunMigration(IDictionary<string, object> migrationParams)
        {
            _logHelper.Verbose("[CountlyStorageHelper] RunMigration : currentVersion = " + CurrentVersion);

            /*
             * Schema Version = 1 :
             * - Deletion of the data in the “EventNumberInSameSessionEntity” table
             * - Copy data of 'Views Repository(Entity Dao, Segment Dao)' into Event Repository(Entity Dao, Segment Dao)'.
            */
            if (CurrentVersion == 0) {
                Migration_EventNumberInSameSessionEntityDataRemoval();
                Migration_CopyViewDataIntoEventData();

                CurrentVersion = 1;
                PlayerPrefs.SetInt(Constants.SchemaVersion, CurrentVersion);
            }

            if (CurrentVersion == 1) {
                Migration_MigrateOldRequests();
                CurrentVersion = 2;
                PlayerPrefs.SetInt(Constants.SchemaVersion, CurrentVersion);
            }

            if (CurrentVersion == 2) {
                bool customIdProvided = (bool)migrationParams[key_from_2_to_3_custom_id_set];
                Migration_GuessTheDeviceIDType(customIdProvided);
                CurrentVersion = 3;
                PlayerPrefs.SetInt(Constants.SchemaVersion, CurrentVersion);
            }
        }

        /// <summary>
        /// Helper for migration from version 0 to version 1.
        /// Deletion Of data in the 'EventNumberInSameSessionEntity' table.
        /// </summary>
        private void Migration_EventNumberInSameSessionEntityDataRemoval()
        {
            EventNrInSameSessionDao.RemoveAll();
            _logHelper.Verbose("[CountlyStorageHelper] Migration_EventNumberInSameSessionEntityDataRemoval");
        }

        /// <summary>
        /// Helper for migration from version 0 to version 1.
        /// Copy data of 'Views Repository(Entity Dao, Segment Dao)' into Event Repository(Entity Dao, Segment Dao)'.
        /// </summary>
        private void Migration_CopyViewDataIntoEventData()
        {
            while (ViewRepo.Count > 0) {
                EventRepo.Enqueue(ViewRepo.Dequeue());
            }
            _logHelper.Verbose("[CountlyStorageHelper] Migration_CopyViewDataIntoEventData");

        }

        /// <summary>
        /// Helper for migration from version 1 to version 2.
        /// Go through all requests and remove the checksum param set there.
        /// Checksum would now be computed just before sending the request
        /// </summary>
        private void Migration_MigrateOldRequests()
        {
            //get all stored requests
            CountlyRequestModel[] requestModels = RequestRepo.Models.ToArray();
            foreach (CountlyRequestModel request in requestModels) {
                //go through all of them
                if (request.RequestData == null) {
                    // if we have no request data then that means that all of the info is in the request URL
                    // start by parsing all the params from the URL.
                    // remove the checksum and then write the request back as a string
                    int index = request.RequestUrl.IndexOf('?');
                    string uri = request.RequestUrl.Substring(index);
                    NameValueCollection collection = HttpUtility.ParseQueryString(uri);

                    Dictionary<string, object> queryParams = collection.AllKeys.ToDictionary(t => t, t => (object)collection[t]);
                    queryParams.Remove("checksum256");
                    string data = _requestBuilder.BuildQueryString(queryParams);

                    request.RequestUrl = null;
                    request.RequestData = data;
                } else {
                    // if we don't have request data then that means that all of the request params are in the request data field
                    // deserialize the values, remove the checksum and then combine them all into a single array which should then be the replacement
                    Dictionary<string, object> requestData = JsonConvert.DeserializeObject<Dictionary<string, object>>(request.RequestData);
                    requestData.Remove("checksum256");

                    request.RequestUrl = null;
                    request.RequestData = _requestBuilder.BuildQueryString(requestData);
                }

                bool result = RequestRepo.Update(request);
                if (!result) {
                    _logHelper.Warning("[CountlyStorageHelper] Migration_MigrateOldRequests: updating the request failed");
                    //we failed to update the old request,

                    RequestRepo.DeleteEntry(request);
                }
            }
            _logHelper.Verbose("[CountlyStorageHelper] Migration_MigrateOldRequests");

            //refresh internal memory represensation for future migrations
            RequestRepo.RefreshMemoryCache();
        }

        private void Migration_GuessTheDeviceIDType(bool customIdProvided)
        {
            if (customIdProvided) {
                PlayerPrefs.SetInt(Constants.DeviceIDTypeKey, (int)DeviceIdType.DeveloperProvided);
            } else {
                PlayerPrefs.SetInt(Constants.DeviceIDTypeKey, (int)DeviceIdType.SDKGenerated);
            }
        }

        internal void ClearDBData()
        {
            EventRepo.Clear();
            RequestRepo.Clear();
            ConfigDao.RemoveAll();
        }
    }
}
