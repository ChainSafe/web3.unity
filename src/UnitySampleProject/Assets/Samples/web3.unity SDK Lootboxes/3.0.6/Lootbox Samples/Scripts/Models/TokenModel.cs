using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Token model class to help with deserializing uri json responses from nft metadata.
/// </summary>
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