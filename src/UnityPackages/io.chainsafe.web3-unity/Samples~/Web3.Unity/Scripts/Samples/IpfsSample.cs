using System.Threading.Tasks;
using Web3Unity.Scripts.Library.IPFS;

namespace Web3Unity.Scripts.Prefabs
{
    public class IpfsUploadRequest
    {
        public string ApiKey { get; set; }
        public string Data { get; set; }
        public string BucketId { get; set; }
        public string Path { get; set; }
        public string Filename { get; set; }
    }

    public class IpfsSample
    {
        public async Task<string> Upload(IpfsUploadRequest request)
        {
            var rawData = System.Text.Encoding.UTF8.GetBytes(request.Data);
            var ipfs = new Ipfs(request.ApiKey);
            var cid = await ipfs.Upload(request.BucketId, request.Path, request.Filename, rawData, "application/octet-stream");
            return cid;
        }
    }
}