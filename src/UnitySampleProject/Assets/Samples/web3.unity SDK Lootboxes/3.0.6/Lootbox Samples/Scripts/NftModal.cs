using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class NftModal : MonoBehaviour
{
    #region Fields

    [SerializeField] private TextMeshProUGUI typeText, nameText, idText, descriptionText, amountText;
    [SerializeField] private Image image;
    [SerializeField] private Button closeButton;
    private static Dictionary<string, Sprite> _spritesDict = new();

    #endregion

    #region Methods

    /// <summary>
    /// Initializes button & functions.
    /// </summary>
    private void Awake()
    {
        closeButton.onClick.AddListener(CloseNftModal);
    }

    /// <summary>
    /// Subscribes to events.
    /// </summary>
    private void OnEnable()
    {
        EventManager.ToggleNftModal += PopulateData;
    }

    /// <summary>
    /// Unsubscribes from the event to free up memory.
    /// </summary>
    private void OnDisable()
    {
        EventManager.ToggleNftModal -= PopulateData;
    }

    /// <summary>
    /// Populates & displays object data.
    /// </summary>
    /// <param name="model"></param>
    private async void PopulateData(ItemData model)
    {
        typeText.text = model.itemType;
        nameText.text = model.itemName;
        idText.text = model.itemId;
        descriptionText.text = model.itemDescription;
        amountText.text = model.itemAmount;
        image.sprite = await GetSprite(model);
    }

    /// <summary>
    /// Gets the image sprite from the model URI.
    /// </summary>
    /// <param name="model">Item data model</param>
    /// <returns>A 2D sprite.</returns>
    private async Task<Sprite> GetSprite(ItemData model)
    {
        Sprite sprite = null;
        string imageUrl = (string)model.itemImage;
        if (_spritesDict.TryGetValue(imageUrl, out sprite)) return sprite;
        var unityWebRequest = UnityWebRequestTexture.GetTexture(imageUrl);
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

    /// <summary>
    /// Closes the NFT modal.
    /// </summary>
    private void CloseNftModal()
    {
        gameObject.SetActive(false);
    }

    #endregion
}