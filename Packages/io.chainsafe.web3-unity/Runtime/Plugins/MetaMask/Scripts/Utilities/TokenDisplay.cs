using MetaMask.Unity;
using MetaMask.Unity.Contracts;
using TMPro;
using UnityEngine;

public class TokenDisplay : MonoBehaviour
{
    private MetaMaskUnity _metaMask;
    private TextMeshProUGUI _balanceText;

    public ScriptableERC20 contract;
    
    // Start is called before the first frame update
    void Start()
    {
        _metaMask = MetaMaskUnity.Instance;
        _balanceText = GetComponent<TextMeshProUGUI>();

        if (_metaMask.Wallet.IsConnected)
        {
            DisplayBalance();
        }

        _metaMask.Wallet.Events.AccountChanged += (_, _) => DisplayBalance();
    }

    private async void DisplayBalance()
    {
        var address = _metaMask.Wallet.SelectedAddress;

        var tokenSymbol = await contract.Symbol();
        var tokenBalance = await contract.BalanceOf(address);

        _balanceText.text = $"{tokenSymbol}: {tokenBalance}";
    }
}
