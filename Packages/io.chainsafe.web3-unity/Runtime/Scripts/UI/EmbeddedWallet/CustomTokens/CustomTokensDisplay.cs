using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ChainSafe.Gaming.Evm.Contracts.BuiltIn;
using ChainSafe.Gaming.Web3;
using TMPro;
using Unity.Plastic.Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;
using AddressExtensions = ChainSafe.Gaming.Web3.Core.Debug.AddressExtensions;

namespace ChainSafe.Gaming
{
    public class CustomTokensDisplay : MonoBehaviour
    {
        [SerializeField] private Button addTokenButton;
        
        [SerializeField] private Transform emptyOverlayContainer;

        [Space]
        
        [SerializeField] private CustomTokenRowDisplay tokenRowPrefab;
        
        [SerializeField] private Transform tokenRowContainer;
        
        [SerializeField] private TMP_Dropdown tokensDropdown;

        private static string FilePath => Path.Combine(Application.persistentDataPath, "embedded-wallet-tokens.json");

        private List<CustomToken> _tokens;

        private Erc20Service _erc20;

        private AddCustomTokenDisplay _addCustomToken;
        
        public async void Initialize(Erc20Service erc20)
        {
            _erc20 = erc20;
            
            _tokens = LoadAllCustomTokens();

            emptyOverlayContainer.gameObject.SetActive(_tokens.Count == 0);

            foreach (var token in _tokens)
            {
                await AddToken(token.Address);
            }
            
            // Add token
            _addCustomToken = GetComponentInChildren<AddCustomTokenDisplay>(true);
            
            _addCustomToken.Initialize(AddToken);
            
            addTokenButton.onClick.AddListener(_addCustomToken.Open);
        }
        
        private async Task AddToken(string address)
        {
            AssertTokenAddress(address);

            var symbol = await _erc20.GetSymbol(address);
            
            var decimals = await _erc20.GetDecimals(address);
            
            CustomToken token = new CustomToken(address, symbol, decimals);
            
            // Add token row with balance
            var balance = await _erc20.GetBalanceOf(address);
            
            token.SetBalance(balance);
            
            // Add Token Row
            var tokenRow = Instantiate(tokenRowPrefab, tokenRowContainer);
            
            tokenRow.Attach(token);

            // Add Token to Dropdown
            tokensDropdown.options.Add(new TMP_Dropdown.OptionData(token.Symbol));
            
            // Save tokens persistently locally
            if (_tokens.All(t => t.Symbol != token.Symbol))
            {
                _tokens.Add(token);

                await File.WriteAllTextAsync(FilePath, JsonConvert.SerializeObject(_tokens, Formatting.Indented));

                if (emptyOverlayContainer.gameObject.activeSelf)
                {
                    emptyOverlayContainer.gameObject.SetActive(false);
                }
            }
        }

        private void AssertTokenAddress(string address)
        {
            if (!AddressExtensions.IsContractAddress(address))
            {
                throw new Web3Exception($"Invalid Contract Address {address}");
            }
        }

        private List<CustomToken> LoadAllCustomTokens()
        {
            if (!File.Exists(FilePath))
            {
                return new List<CustomToken>();
            }
            
            var json = File.ReadAllText(FilePath);
            
            return JsonConvert.DeserializeObject<List<CustomToken>>(json);
        }
    }
}