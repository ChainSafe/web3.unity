using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using Web3Unity.Scripts.Library.ETHEREUEM.EIP;

public class ImportNFTTextureExample : MonoBehaviour
{
    public class Response
    {
        public string image;
    }

    async void Start()
    {
        string contract = "0x162BA1d478948e0ab2d4B21dca2471982C1Fb797"; // gitleaks:allow
        string tokenId = "0x01559ae4021aee70424836ca173b6a4e647287d15cee8ac42d8c2d8d128927e5"; // gitleaks:allow

        // fetch uri from chain
        string uri = await ERC1155.URI(Web3Accessor.Web3, contract, tokenId);
        print("uri: " + uri);

        // fetch json from uri
        UnityWebRequest webRequest = UnityWebRequest.Get(uri);
        await webRequest.SendWebRequest();
        if (webRequest.result != UnityWebRequest.Result.Success)
        {
            throw new System.Exception(webRequest.error);
        }
        Response data = JsonUtility.FromJson<Response>(System.Text.Encoding.UTF8.GetString(webRequest.downloadHandler.data));

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
        gameObject.GetComponent<Renderer>().material.mainTexture = ((DownloadHandlerTexture)textureRequest.downloadHandler).texture;
    }
}
