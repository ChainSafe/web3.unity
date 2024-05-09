using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Web3Unity.Scripts.Library.IPFS;

namespace Web3Unity.Scripts.Prefabs
{
    public class IpfsUploadRequest
    {
        public string ApiKey { get; set; }
        public string Data { get; set; }
        public string BucketId { get; set; }
        public string Filename { get; set; }
        public Texture2D Image { get; set; }
        
        public class Attribute
        {
            public string display_type { get; set; }
            public string trait_type { get; set; }
            public string value { get; set; }
        }

        public class Metadata
        {
            public string description { get; set; }
            public string external_url { get; set; }
            public string image { get; set; }
            public string name { get; set; }
            public List<Attribute> attributes { get; set; }
        }
    }

    public class IpfsSample
    {
        public async Task<string> Upload(IpfsUploadRequest request)
        {
            var rawData = System.Text.Encoding.UTF8.GetBytes(request.Data);
            var ipfs = new Ipfs(request.ApiKey);
            var cid = await ipfs.Upload(request.BucketId, request.Filename, rawData, "application/octet-stream");
            return cid;
        }
        
    }
}