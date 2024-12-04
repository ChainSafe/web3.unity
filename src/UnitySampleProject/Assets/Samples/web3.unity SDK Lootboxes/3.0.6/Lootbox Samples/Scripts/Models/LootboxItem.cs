using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Initializes lootbox data and ties it to an object.
/// </summary>
public class LootboxItem : MonoBehaviour
{
    #region Fields

    [SerializeField] private Image lootboxItemImage;
    [SerializeField] private TMP_Text itemType, itemName, itemId, itemAmount;
    [SerializeField] private Button button;

    #endregion

    #region Methods

    /// <summary>
    /// Initializes data.
    /// </summary>
    /// <param name="model">ItemData model to initialize from.</param>
    public async Task Initialize(ItemData model)
    {
        itemType.text = model.itemType;
        itemName.text = model.itemName ?? "";
        itemId.text = model.itemId == null ? "" : $"#{model.itemId}";
        itemAmount.text = $"Amount: {model.itemAmount}";
        button.onClick.AddListener(() => OpenNftModal(model));
        if (model.itemImage != null) lootboxItemImage.sprite = await GetSprite(model);
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
    /// Opens the NFT model to display data for a single item on a larger canvas.
    /// </summary>
    /// <param name="itemData">The item data to display.</param>
    private void OpenNftModal(ItemData itemData)
    {
        EventManager.OnToggleNftModal(itemData);
    }

    #endregion
}