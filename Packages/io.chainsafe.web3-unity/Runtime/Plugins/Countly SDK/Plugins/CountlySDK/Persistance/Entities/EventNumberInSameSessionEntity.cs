namespace Plugins.CountlySDK.Persistance.Entities
{
    public class EventNumberInSameSessionEntity : IEntity
    {
        public long Id;
        public string EventKey;
        public int Number;

        public long GetId()
        {
            return Id;
        }
    }
}