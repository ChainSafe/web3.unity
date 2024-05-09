using UnityEngine;

namespace ChainSafe.Gaming.UnityPackage.Model
{
    public class IPFSUploadRequestModel
    {
        public string ApiKey { get; set; }
        public string Data { get; set; }
        public string BucketId { get; set; }
        public string Filename { get; set; }
        public string FileNameMetaData { get; set; }
        public string FileNameImage { get; set; }
        public Texture2D ImageTexture { get; set; }
        public string Display_type { get; set; }
        public string Trait_type { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
        public string External_url { get; set; }
        public string Image { get; set; }
        public string Name { get; set; }
    }
}