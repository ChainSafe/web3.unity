namespace Plugins.CountlySDK.Persistance.Entities
{
    public class ConfigEntity : IEntity
    {
        public long Id;
        public string Json;

        public long GetId()
        {
            return Id;
        }
    }
}