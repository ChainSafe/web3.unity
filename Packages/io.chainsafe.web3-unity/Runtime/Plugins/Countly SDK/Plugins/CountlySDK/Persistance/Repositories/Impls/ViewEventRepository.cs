using Plugins.CountlySDK.Models;
using Plugins.CountlySDK.Persistance.Dao;
using Plugins.CountlySDK.Persistance.Entities;
using Plugins.iBoxDB;
using UnityEngine;

namespace Plugins.CountlySDK.Persistance.Repositories.Impls
{
    public class ViewEventRepository : AbstractEventRepository
    {

        public ViewEventRepository(Dao<EventEntity> dao, SegmentDao segmentDao, CountlyLogHelper log) : base(dao, segmentDao, log)
        {
        }

        protected override bool ValidateModelBeforeEnqueue(CountlyEventModel model)
        {
            Log.Debug("[ViewEventRepository] Validate model: \n" + model);
            return model.Key.Equals(CountlyEventModel.ViewEvent);
        }
    }
}