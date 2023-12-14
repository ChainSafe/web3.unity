namespace Plugins.CountlySDK.Persistance.Entities
{
    public class SegmentEntity : IEntity
    {
        public long Id;
        public string Json;
        public long EventId;

        public long GetId()
        {
            return Id;
        }
    }
}
