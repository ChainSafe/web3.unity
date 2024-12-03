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