using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NftModal : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI typeText, nameText, idText, descriptionText, amountText;
    [SerializeField] private Image image;
    [SerializeField] private Button closeButton;

    private void Awake()
    {
        closeButton.onClick.AddListener(CloseNftModal);
    }
    
    private void OnEnable()
    {
        EventManager.ToggleNftModal += PopulateData;
    }

    private void OnDisable()
    {
        EventManager.ToggleNftModal -= PopulateData;
    }

    private void PopulateData(ItemData itemData)
    {
        typeText.text = itemData.itemType;
        nameText.text = itemData.itemName;
        idText.text = itemData.itemId;
        descriptionText.text = itemData.itemDescription;
        amountText.text = itemData.itemAmount;
        image = itemData.itemImage;
    }

    private void CloseNftModal()
    {
        gameObject.SetActive(false);
    }
}
