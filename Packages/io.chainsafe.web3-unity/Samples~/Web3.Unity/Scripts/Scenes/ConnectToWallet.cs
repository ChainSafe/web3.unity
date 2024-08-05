using ChainSafe.Gaming.UnityPackage.UI;
using UnityEngine;
using UnityEngine.UI;

public class ConnectToWallet : MonoBehaviour
{
    [SerializeField] private ConnectModal connectModalPrefab;
    [SerializeField] private RectTransform connectModalContainer;
    [SerializeField] private Button connectButton;

    private ConnectModal _connectModalInstance;
    
    private void Start()
    {
        connectButton.onClick.AddListener(PromptConnectModal);
    }

    private void PromptConnectModal()
    {
        if (_connectModalInstance != null)
        {
            _connectModalInstance.gameObject.SetActive(true);
            
            return;
        }
        
        _connectModalInstance = Instantiate(connectModalPrefab, connectModalContainer);
    }
}
