using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Assists with populating data for the NFT modal.
/// </summary>
public class NftModal : MonoBehaviour
{
    #region Fields

    [SerializeField] private TextMeshProUGUI typeText, nameText, idText, descriptionText, amountText;
    [SerializeField] private Image image;
    [SerializeField] private Button closeButton;

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
        EventManager.Instance.ToggleNftData += PopulateData;
    }

    /// <summary>
    /// Unsubscribes from the event to free up memory.
    /// </summary>
    private void OnDisable()
    {
        EventManager.Instance.ToggleNftData -= PopulateData;
    }

    /// <summary>
    /// Populates & displays object data.
    /// </summary>
    /// <param name="model"></param>
    public async void PopulateData(ItemData model)
    {
        typeText.text = $"Type: {model.itemType}";
        nameText.text = $"Name: {model.itemName}";
        idText.text = model.itemId == null ? "" : $"#{model.itemId}";
        amountText.text = $"Amount: {model.itemAmount}";
        descriptionText.text = model.itemDescription ?? "";
        if (model.itemImage != null) image.sprite = await GetSprite(model);
    }

    /// <summary>
    /// Gets the image sprite from the model URI.
    /// </summary>
    /// <param name="model">Item data model.</param>
    /// <returns>A 2D sprite.</returns>
    private async Task<Sprite> GetSprite(ItemData model)
    {
        var sprite = await SpriteHandler.GetSprite(model);
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