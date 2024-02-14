namespace ChainSafe.Gaming.SygmaClient.Types
{
    public class Handler
    {
        public Handler(ResourceType type, string address)
        {
            Type = type;
            Address = address;
        }

        public ResourceType Type { get; }

        public string Address { get; }
    }
}