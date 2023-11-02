using System;
using System.Buffers.Text;
using System.Numerics;
using System.Threading.Tasks;
using ChainSafe.Gaming.Web3.Environment;

namespace LootBoxes.Chainlink
{
    public class Erc1155MetaData
    {
        public string Name { get; set; }
        public string Image { get; set; }
    }

    public class Erc1155MetaDataReader
    {
        private readonly IHttpClient httpClient;

        public Erc1155MetaDataReader(IHttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<Erc1155MetaData> Fetch(string uri, BigInteger tokenId)
        {
            if (uri.StartsWith("data:application/json"))
            {
                return DecodeUri(uri);
            }

            if (uri.StartsWith("ipfs://"))
            {
                return FetchIpfs(uri);
            }

            if (uri.Contains("{id}"))
            {
                uri = uri.Replace("{id}", tokenId.ToString());
            }



            var response = await httpClient.Get<Erc1155MetaData>(uri);
            return response.AssertSuccess();
        }

        private Erc1155MetaData DecodeUri(string uri)
        {
            /*
             * TODO Handle URI:
             * data:application/json;base64,eyJu...fV19
             */

            throw new NotImplementedException();
        }

        private Erc1155MetaData FetchIpfs(string uri)
        {
            throw new NotImplementedException();
        }
    }
}