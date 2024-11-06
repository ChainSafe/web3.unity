using System.Collections.Generic;
using UnityEngine;

public class TokenModel : MonoBehaviour
{
    public class Token
    {
        public string description { get; set; }
        public string image { get; set; }
        public string name { get; set; }
        public string tokenType { get; set; }
        public Metadata metadata { get; set; }
    }

    public class Metadata
    {
        public List<Attribute> attributes { get; set; }
    }

    public class Attribute
    {
        public string trait_type { get; set; }
    }
}
