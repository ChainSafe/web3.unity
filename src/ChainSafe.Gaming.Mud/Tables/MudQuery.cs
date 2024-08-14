namespace ChainSafe.Gaming.Mud.Tables
{
    public class MudQuery
    {
        private MudQuery()
        {
        }

        public bool FindWithKey { get; private set; }

        public object[] KeyFilter { get; private set; }

        public static MudQuery All { get; } = new();

        public static MudQuery ByKey(object[] key)
        {
            return new MudQuery
            {
                FindWithKey = true,
                KeyFilter = key,
            };
        }
    }
}