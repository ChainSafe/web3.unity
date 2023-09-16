using System.Threading.Tasks;
using Web3Unity.Scripts.Prefabs;

namespace Samples.Behaviours.Ipfs
{
    public class IpfsUploadBehaviour : SampleBehaviour
    {
        public string apiKey = "YOUR_CHAINSAFE_STORE_API_KEY";
        public string data = "YOUR_DATA";
        public string bucketId = "BUCKET_ID";
        public string path = "/PATH";
        public string filename = "FILENAME.EXT";

        private IpfsSample logic;

        protected override void Awake()
        {
            logic = new IpfsSample();
        }

        protected override async Task ExecuteSample()
        {
            var cid = await logic.Upload(new IpfsUploadRequest
            {
                ApiKey = apiKey,
                Data = data,
                BucketId = bucketId,
                Path = path,
                Filename = filename
            });

            SampleOutputUtil.PrintResult(cid, nameof(IpfsSample), nameof(IpfsSample.Upload));
        }
    }
}