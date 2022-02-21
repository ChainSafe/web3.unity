using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;

public class ImportNFTTextureIPFS : MonoBehaviour
{

    [Serializable]
    public class Attribute
    {
        public string trait_type { get; set; }
        public string value { get; set; }
    }
    [Serializable]
    public class Response
    {
        public string image { get; set; }
        public List<Attribute> attributes { get; set; }
    }
    // Create a game object and attach it.
    public GameObject gameObject;
    async void Start()
    {
        string chain = "ethereum";
        string network = "mainnet";
        // BAYC contract address
        string contract = "0xbc4ca0eda7647a8ab7c2061c2e118a18a936f13d";
        string tokenId = "4671";

        // fetch uri from chain
        string uri = await ERC721.URI(chain, network, contract, tokenId);
        print("uri: " + uri);

        if (uri.StartsWith("ipfs://"))
        {
            uri = uri.Replace("ipfs://", "https://ipfs.io/ipfs/");
        }
        print("URI: " + uri);
        // fetch json from uri

        UnityWebRequest webRequest = UnityWebRequest.Get(uri);
        await webRequest.SendWebRequest();
        Response data = JsonConvert.DeserializeObject<Response>(System.Text.Encoding.UTF8.GetString(webRequest.downloadHandler.data));
        print("Data: " + data.image);

        // parse json to get image uri
        string imageUri = data.image;
        if (imageUri.StartsWith("ipfs://"))
        {
            imageUri = imageUri.Replace("ipfs://", "https://ipfs.io/ipfs/");
        }
        print("imageUri: " + imageUri);
        print("Attibutes: " + data.attributes[0].trait_type);
        print("Attibutes: " + data.attributes[1].trait_type);
        print("Attibutes: " + data.attributes[2].trait_type);


        // fetch image and display in game
        UnityWebRequest textureRequest = UnityWebRequestTexture.GetTexture(imageUri);
        await textureRequest.SendWebRequest();
        this.gameObject.GetComponent<Renderer>().material.mainTexture = ((DownloadHandlerTexture)textureRequest.downloadHandler).texture;
    }
}
