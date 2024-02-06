using System;
using System.Text;
using iBoxDB.LocalServer;
using Plugins.CountlySDK.Models;
using Plugins.CountlySDK.Persistance.Entities;
using Plugins.iBoxDB;
using UnityEngine;

namespace Plugins.CountlySDK.Persistance.Dao
{
    public class SegmentDao : Dao<SegmentEntity>
    {
        private readonly CountlyLogHelper Log;
        private readonly StringBuilder _stringBuilder = new StringBuilder();

        public SegmentDao(AutoBox auto, string table, CountlyLogHelper log) : base(auto, table, log)
        {
            Log = log;
        }


        public SegmentEntity GetByEventId(long eventId)
        {

            Log.Debug("[SegmentDao] GetByEventId: eventId = " + eventId);

            _stringBuilder.Clear();

            string ql = _stringBuilder.Append("from ").Append(Table).Append(" where EventId==?").ToString();

            try {
                System.Collections.Generic.List<SegmentEntity> entities = Auto.Select<SegmentEntity>(ql, eventId);
                if (entities.Count > 1) {
                    throw new ArgumentException("Only one or zero segment can be assigned to entity with id " + eventId + ". "
                                                + entities.Count + " segments found.");
                }

                if (entities.Count == 0) {
                    return null;
                }

                return entities[0];
            } catch (Exception ex) {
                Log.Debug("[SegmentDao] GetByEventId: Couldn't complete db operation, [" + ex.Message + "]");
                return null;
            }
        }

    }
}