using iBoxDB.LocalServer;
using Plugins.CountlySDK.Models;
using Plugins.CountlySDK.Persistance.Entities;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Plugins.iBoxDB
{
    public class Dao<TEntity> where TEntity : class, IEntity, new()
    {
        protected readonly AutoBox Auto;
        protected readonly string Table;
        private readonly CountlyLogHelper Log;

        public Dao(AutoBox auto, string table, CountlyLogHelper log)
        {
            Auto = auto;
            Table = table;
            Log = log;
        }

        public bool Save(TEntity entity)
        {
            try {
                bool insertionResult = Auto.Insert(Table, entity);
                if (!insertionResult) {
                    Log.Info("[Dao] Save: Auto.Insert result: [" + insertionResult + "]");
                }
                return insertionResult;
            } catch (Exception ex) {
                Log.Error("[Dao] Save: Couldn't complete db operation, [" + ex.Message + "]");
            }

            return false;
        }

        public bool Update(TEntity entity)
        {
            try {
                bool updateResult = Auto.Update(Table, entity);
                if (!updateResult) {
                    Log.Info("[Dao] Update: Auto.Update result: [" + updateResult + "]");
                }
                return updateResult;
            } catch (Exception ex) {
                Log.Error("[Dao] Update: Couldn't complete db operation, [" + ex.Message + "]");
            }

            return false;
        }

        public List<TEntity> LoadAll()
        {
            List<TEntity> result = new List<TEntity>();
            try {
                result = Auto.Select<TEntity>("from " + Table + " order by Id asc");
                if (result.Count == 0) {
                    Log.Info("[Dao] LoadAll: Auto.Select result: [" + string.Join(", ", result) + "]");
                }
                return result;
            } catch (Exception ex) {
                Log.Error("[Dao] LoadAll: Couldn't complete db operation, [" + ex.Message + "]");
            }

            return result;
        }

        public void Remove(params object[] key)
        {
            try {
                bool deletionResult = Auto.Delete(Table, key);
                if (!deletionResult) {
                    Log.Info("[Dao] Remove: Auto.Delete result: [" + deletionResult + "]");
                }
            } catch (Exception ex) {
                Log.Error("[Dao] Remove: Couldn't complete db operation, [" + ex.Message + "]");
            }
        }

        public void RemoveAll()
        {
            try {
                List<TEntity> list = Auto.Select<TEntity>("from " + Table);
                if (list.Count == 0) {
                    Log.Info("[Dao] RemoveAll: Auto.Select result: [" + string.Join(", ", list) + "]");
                }
                foreach (TEntity entity in list) {
                    bool deletionResult = Auto.Delete(Table, entity.GetId());
                    if (!deletionResult) {
                        Log.Info("[Dao] RemoveAll: Auto.Delete result: [" + deletionResult + "]");
                    }
                }
            } catch (Exception ex) {
                Log.Error("[Dao] RemoveAll: Couldn't complete db operation, [" + ex.Message + "]");
            }
        }

        public long GenerateNewId()
        {
            long result;
            try {
                result = Auto.NewId();
                if (result < 0) {
                    Log.Info("[Dao] GenerateNewId: Auto.NewId result: [" + result + "]");
                }
                return result;
            } catch (Exception ex) {
                result = 0;
                Log.Error("[Dao] GenerateNewId: Couldn't complete db operation, [" + ex.Message + "]");
            }

            return result;
        }
    }
}
