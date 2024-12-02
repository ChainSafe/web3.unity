using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
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

    private async void PopulateData(ItemData model)
    {
        typeText.text = model.itemType;
        nameText.text = model.itemName;
        idText.text = model.itemId;
        descriptionText.text = model.itemDescription;
        amountText.text = model.itemAmount;
        image.sprite = await GetSprite(model);
    }
    
    private async Task<Sprite> GetSprite(ItemData model)
    {
        Sprite sprite = null;
        string imageUrl = (string)model.itemImage;
        var unityWebRequest = UnityWebRequestTexture.GetTexture(imageUrl);
        await unityWebRequest.SendWebRequest();
        if (unityWebRequest.error != null)
        {
            Debug.LogError("There was an error getting the texture " + unityWebRequest.error);
            return null;
        }
        var myTexture = ((DownloadHandlerTexture)unityWebRequest.downloadHandler).texture;
        sprite = Sprite.Create(myTexture, new Rect(0, 0, myTexture.width, myTexture.height), Vector2.one * 0.5f);
        return sprite;
    }

    private void CloseNftModal()
    {
        gameObject.SetActive(false);
    }
}
