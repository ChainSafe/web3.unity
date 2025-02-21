using System;
using System.Threading.Tasks;
using ChainSafe.Gaming.UnityPackage.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ChainSafe.Gaming
{
    public class AddCustomTokenDisplay : MonoBehaviour
    {
        public delegate Task AddCustomToken(string address);
        
        [SerializeField] private TMP_InputField tokenAddressInput;
        
        [SerializeField] private TextMeshProUGUI errorText;
        
        [SerializeField] private Button addButton;
        
        [SerializeField] private Button closeButton;
        
        public void Initialize(AddCustomToken addToken)
        {
            addButton.onClick.AddListener(AddToken);
            
            closeButton.onClick.AddListener(Close);

            async void AddToken()
            {
                LoadingOverlay.ShowLoadingOverlay("Adding Custom Token");
                
                try
                {
                    await addToken.Invoke(tokenAddressInput.text);
                }
                catch (Exception e)
                {
                    errorText.text = $"{e.Message}";
                }
                finally
                {
                    LoadingOverlay.HideLoadingOverlay();
                }
            }
        }

        public void Open()
        {
            gameObject.SetActive(true);
        }
        
        private void Close()
        {
            gameObject.SetActive(false);
        }
    }
}