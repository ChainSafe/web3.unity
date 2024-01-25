namespace Plugins.CountlySDK.Persistance.Entities
{
    public class EventEntity : IEntity
    {
        public long Id;
        public string Json;

        public long GetId()
        {
            return Id;
        }
    }
}
