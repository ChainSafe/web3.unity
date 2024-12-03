using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// Sprite handler class to assist with lazy sprite loading and data responses.
/// </summary>
public class SpriteHandler : MonoBehaviour
{
    private static Dictionary<string, Sprite> _spritesDict = new();
    
    /// <summary>
    /// Gets the image sprite from the model URI.
    /// </summary>
    /// <param name="model">Item data model</param>
    /// <returns>A 2D sprite.</returns>
    public static async Task<Sprite> GetSprite(ItemData model)
    {
        Sprite sprite = null;
        string imageUrl = (string)model.itemImage;
        if (_spritesDict.TryGetValue(imageUrl, out sprite)) return sprite;
        var unityWebRequest = UnityWebRequestTexture.GetTexture(model.itemImage);
        await unityWebRequest.SendWebRequest();
        if (unityWebRequest.error != null)
        {
            Debug.LogError("There was an error getting the texture " + unityWebRequest.error);
            return null;
        }

        var myTexture = ((DownloadHandlerTexture)unityWebRequest.downloadHandler).texture;
        sprite = Sprite.Create(myTexture, new Rect(0, 0, myTexture.width, myTexture.height), Vector2.one * 0.5f);
        _spritesDict[imageUrl] = sprite;
        return sprite;
    }
}
