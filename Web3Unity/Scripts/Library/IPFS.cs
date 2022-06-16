using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class IPFS
{
    private string _apiKey;
    private static readonly string host = "https://api.chainsafe.io";

    [System.Serializable]
    private class Response<T>
    {
        public T response;

        [System.Serializable]
        public struct Error
        {
            public int code;

            public string message;
            // public Array<Object> details;
        }
        
        public Error error;
    }

    [System.Serializable]
    public class GetFileInfoResponse
    {
        [System.Serializable]
        public class Content
        {
            public string cid;
        }

        public Content content;
    }

    public IPFS(string apiKey)
    {
        _apiKey = apiKey;
    }

    public async Task<string> Upload(string bucketId, string path, string filename, byte[] content, string contentType)
    {
        var formUpload = new List<IMultipartFormSection>
        {
            new MultipartFormDataSection("path=" + path),
            new MultipartFormFileSection("file", content, filename, contentType)
        };
        
        using var requestUpload = UnityWebRequest.Post(host + "/api/v1/bucket/" + bucketId + "/upload", formUpload);
        requestUpload.SetRequestHeader("Authorization", "Bearer " + _apiKey);
        await requestUpload.SendWebRequest();
        
        if (requestUpload.result != UnityWebRequest.Result.Success)
        {
            throw new WebException(requestUpload.error);
        }

        // var jsonFile ="{\"path\": \""+path+"/"+filename+"\", \"source\": \""+bucketId+"\"}";
        var jsonFile ="{\"path\": \""+filename+"\", \"source\": \""+bucketId+"\"}";

        using var requestFile = new UnityWebRequest(host + "/api/v1/bucket/" + bucketId + "/file", "POST");
        requestFile.SetRequestHeader("Authorization", "Bearer " + _apiKey);
        requestFile.SetRequestHeader("Content-Type", "application/json");
        requestFile.uploadHandler = new UploadHandlerRaw(new System.Text.UTF8Encoding().GetBytes(jsonFile));
        requestFile.downloadHandler = new DownloadHandlerBuffer();
        await requestFile.SendWebRequest();
        
        if (requestFile.result != UnityWebRequest.Result.Success)
        {
            throw new WebException(requestFile.error);
        }
        
        var data = JsonUtility.FromJson<GetFileInfoResponse>(requestFile.downloadHandler.text);
        return data.content.cid;
    }
}