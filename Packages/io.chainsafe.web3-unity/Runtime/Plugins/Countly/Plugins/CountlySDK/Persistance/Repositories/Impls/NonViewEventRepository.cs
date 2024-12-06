using Plugins.CountlySDK.Models;
using Plugins.CountlySDK.Persistance.Dao;
using Plugins.CountlySDK.Persistance.Entities;
using Plugins.iBoxDB;

namespace Plugins.CountlySDK.Persistance.Repositories.Impls
{
    public class NonViewEventRepository : AbstractEventRepository
    {
        public NonViewEventRepository(Dao<EventEntity> dao, SegmentDao segmentDao, CountlyLogHelper log) : base(dao, segmentDao, log)
        {
        }

        protected override bool ValidateModelBeforeEnqueue(CountlyEventModel model)
        {
            return true;
        }
    }
}