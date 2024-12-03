using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class LootboxItem : MonoBehaviour
{
    
    [SerializeField] private Image lootboxItemImage;
    [SerializeField] private TMP_Text itemType, itemName, itemId, itemAmount;
    [SerializeField] private Button button;
    private static Dictionary<string, Sprite> _spritesDict = new();
    
    public async Task Initialize(ItemData model)
    {
        if (model.itemImage != null) lootboxItemImage.sprite = await GetSprite(model);
        itemType.text = model.itemType;
        itemName.text = model.itemName ?? "";
        itemId.text = model.itemId == null ? "" : $"# {model.itemId}";
        itemAmount.text = $"Amount: {model.itemAmount}";
        button.onClick.AddListener(() => OpenNftModal(model));
    }

    private async Task<Sprite> GetSprite(ItemData model)
    {
        Sprite sprite = null;
        string imageUrl = (string)model.itemImage;
        Debug.Log($"ImageUrl: {imageUrl}");
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
    
    private void OpenNftModal(ItemData itemData)
    {
        EventManager.OnToggleNftModal(itemData);
    }
}
