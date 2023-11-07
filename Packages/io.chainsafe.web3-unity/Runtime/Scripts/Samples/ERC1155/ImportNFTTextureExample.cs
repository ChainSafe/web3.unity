using Scripts.EVM.Token;
using UnityEngine;
using UnityEngine.Networking;

namespace ChainSafe.Gaming.UnityPackage
{
    public class ImportNFTTextureExample : MonoBehaviour
    {
        // Response class
        public class Response
        {
            public string image;
        }
        
        // Fields
        private Erc1155 erc1155;
    
        // Initializes the protocol class
        public void Awake()
        {
            erc1155 = new Erc1155(Web3Accessor.Web3);
        }
        
        async void Start()
        {
            string contract = "0x0288B4F1389ED7b3d3f9C7B73d4408235c0CBbc6";
            string tokenId = "0";

            // fetch uri from chain
            string uri = await erc1155.Uri(contract, tokenId);
            print("uri: " + uri);

            // fetch json from uri
            UnityWebRequest webRequest = UnityWebRequest.Get(uri);
            await webRequest.SendWebRequest();
            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                throw new System.Exception(webRequest.error);
            }

            Response data =
                JsonUtility.FromJson<Response>(System.Text.Encoding.UTF8.GetString(webRequest.downloadHandler.data));

            // parse json to get image uri
            string imageUri = data.image;
            print("imageUri: " + imageUri);
            if (imageUri.StartsWith("ipfs://"))
            {
                imageUri = imageUri.Replace("ipfs://", "https://ipfs.io/ipfs/");
            }

            Debug.Log("Revised URI: " + imageUri);
            // fetch image and display in game
            UnityWebRequest textureRequest = UnityWebRequestTexture.GetTexture(imageUri);
            await textureRequest.SendWebRequest();
            gameObject.GetComponent<Renderer>().material.mainTexture =
                ((DownloadHandlerTexture)textureRequest.downloadHandler).texture;
        }
    }
}
