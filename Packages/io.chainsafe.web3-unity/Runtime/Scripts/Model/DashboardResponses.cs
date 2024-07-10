namespace ChainSafe.Gaming.UnityPackage.Model
{
    public class CollectionResponses
    {
        public class Marketplace
        {
            public string id { get; set; }
            public string name { get; set; }
            public string description { get; set; }
            public string owner { get; set; }
            public int chain_id { get; set; }
            public string banner { get; set; }
            public string project_id { get; set; }
            public string contract_address { get; set; }
            public bool deployed { get; set; }
            public int created_at { get; set; }
            public int updated_at { get; set; }
        }

        public class Collections
        {
            public string id { get; set; }
            public string name { get; set; }
            public string description { get; set; }
            public string owner { get; set; }
            public int chain_id { get; set; }
            public string project_id { get; set; }
            public string contract_address { get; set; }
            public bool deployed { get; set; }
            public bool is_imported { get; set; }
            public string type { get; set; }
            public string logo { get; set; }
            public string banner { get; set; }
            public int created_at { get; set; }
            public int updated_at { get; set; }
        }
    }
}