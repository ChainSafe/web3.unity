using System.Collections.Generic;

namespace ChainSafe.Gaming.UnityPackage.Model
{
    public class NftTokenModel
    {
        public class Root
        {
            public int page_number { get; set; }
            public int page_size { get; set; }
            public int total { get; set; }
            public string cursor { get; set; }
            public List<Token> tokens { get; set; }
        }

        public class Token
        {
            public string token_id { get; set; }
            public string chain_id { get; set; }
            public string project_id { get; set; }
            public string collection_id { get; set; }
            public string token_type { get; set; }
            public string contract_address { get; set; }
            public string supply { get; set; }
            public string uri { get; set; }
            public Metadata metadata { get; set; }
        }
        
        public class Metadata
        {
            public List<Attribute> attributes { get; set; }
        }
        
        public class Attribute
        {
            public string trait_type { get; set; }
            public string value { get; set; }
        }
    }
}