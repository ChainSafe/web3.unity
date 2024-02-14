namespace ChainSafe.Gaming.SygmaClient.Types
{
    public enum ResourceType
    {
        Fungible,
        NonFungible,
        PermissionedGeneric,
        PermissionlessGeneric,
    }

    public class BaseResources
    {
        protected BaseResources(string resourceId, ResourceType type)
        {
            ResourceId = resourceId;
            Type = type;
            Native = null;
            Burnable = null;
            Symbol = string.Empty;
            Decimals = null;
        }

        public string ResourceId { get; }

        public ResourceType Type { get; }

        public bool? Native { get; set; }

        public bool? Burnable { get; set; }

        public string Symbol { get; set; }

        public int? Decimals { get; set; }
    }

    public class EvmResource : BaseResources
    {
        public EvmResource(
            string address, string resourceId, ResourceType type)
            : base(resourceId, type)
        {
            Address = address;
        }

        public string Address { get; }
    }
}